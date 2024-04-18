using SharpDX;
using SharpDX.Windows;
using SharpDX.DXGI;
using SharpDX.Direct3D11;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;

using Factory = SharpDX.DXGI.Factory;
using Device = SharpDX.Direct3D11.Device;
using Color = SharpDX.Color;
using RectangleF = SharpDX.RectangleF;
using Game2048;
using System.Timers;

namespace Game2048
{
    public struct PinInfo
    {
        public Vector2 Position;
        public float Score;
        public Vector2 Size;
        public PinInfo(Vector2 position, float score)
        {
            Position = position;
            Score = score;
            Size = new Vector2(1);
        }
        public static PinInfo Lerp(PinInfo a, PinInfo b, float blend)
        {
            return new PinInfo(Vector2.Lerp(a.Position, b.Position, blend), (int)(a.Score + (b.Score - a.Score) * blend));
        }
    }
    public struct AnimationInfo
    {
        PinInfo Begin;
        PinInfo End;
        public PinInfo Current;
        float TimeNormalized = 1;
        float AnimScale;
        Vector2 Movement;

        public AnimationInfo(PinInfo begin, PinInfo end, float delay = 0)
        {
            Begin = begin;
            End = end;
            TimeNormalized = -delay;
            AnimScale = 3;
        }
        public AnimationInfo(PinInfo begin, PinInfo end, Vector2 movement)
        {
            Begin = begin;
            End = end;
            TimeNormalized = 0;
            AnimScale = 1;
            Movement = movement;
        }

        public void Flip()
        {
            var temp = Begin;
            Begin = End;
            End = temp;
        }

        public void Tick(float DeltaTime)
        {
            if (AnimScale == 0 || TimeNormalized >= 1)
                return;


            TimeNormalized += DeltaTime * 4f * AnimScale;

            if (TimeNormalized < 0)
                return;

            if (TimeNormalized > 1)
                TimeNormalized = 1;

            float TimeSmoothed = 2 * TimeNormalized * TimeNormalized;
            if (TimeSmoothed > 0.5f)
                TimeSmoothed = -2.0f * (TimeNormalized - 1f) * (TimeNormalized - 1f) + 1f;

            Current = PinInfo.Lerp(Begin, End, TimeSmoothed);

            if (TimeNormalized < 0.5f)
                return;

            if (Begin.Score != End.Score)
            {
                float sign = 1;
                if (Movement.X != 0)
                    sign = -1;

                const float a = -2.5f;
                const float b = 3.75f;
                const float c = -1.25f;

                var Scretch = a * TimeNormalized * TimeNormalized + b * TimeNormalized + c;
                Current.Size.X = (1 + Math.Max(0, sign * Scretch));
                Current.Size.Y = (1 + Math.Max(0, -sign * Scretch));
            }
        }
    }
    class GameScreen : Control
    {

        static readonly Color4[] Colors = [
            new(1f, 1f, 1f, 0f),
            new(0.93f, 0.89f, 0.85f, 1f),
            new(0.93f, 0.88f, 0.79f, 1f),
            new(0.95f, 0.70f, 0.48f, 1f),
            new(0.96f, 0.59f, 0.39f, 1f),
            new(0.97f, 0.49f, 0.37f, 1f),
            new(0.97f, 0.37f, 0.23f, 1f),
            new(0.93f, 0.82f, 0.45f, 1f),
            new(0.93f, 0.82f, 0.45f, 1f),
            new(0.93f, 0.80f, 0.38f, 1f),
            new(0.93f, 0.79f, 0.31f, 1f),
            new(0.93f, 0.77f, 0.25f, 1f),
            new(0.93f, 0.76f, 0.18f, 1f),/*
            new(0.70f, 0.95f, 0.48f, 1f),
            new(0.59f, 0.96f, 0.39f, 1f),
            new(0.49f, 0.97f, 0.37f, 1f),
            new(0.37f, 0.97f, 0.23f, 1f),
            new(0.31f, 0.97f, 0.17f, 1f),*/
            new(0.24f, 0.23f, 0.20f, 1f),
            new(0.24f, 0.23f, 0.20f, 1f)];

        //Всякое для рендера
        Device device;
        SwapChain swapChain;
        SharpDX.Direct2D1.Factory d2dFactory = new SharpDX.Direct2D1.Factory();

        //Всякое для рендера
        Texture2D backBuffer;
        RenderTargetView renderTargetView;
        Surface surface;
        RenderTarget d2dRenderTarget;
        SolidColorBrush solidColorBrush;

        //Timers
        System.Windows.Forms.Timer stopwatch;
        DateTime LastTime;

        //Движущиеся ячейки
        AnimationInfo[] Animations;

        //Геометрия окна
        int WindowWidth;
        int WindowHeight;
        int MinDimension;

        public TextFormat TextFormat { get; private set; }
        SharpDX.DirectWrite.Factory FactoryDWrite;

        //Количество ячеек (по одной из осей)
        int CellCount;
        //расстояние между ячейками в процентах от ширины окна(или высоты, если оно меньше)
        float CellPerc;
        protected override void Dispose(bool Disposing)
        {
            Utilities.Dispose(ref backBuffer);
            Utilities.Dispose(ref renderTargetView);
            Utilities.Dispose(ref surface);
            Utilities.Dispose(ref d2dRenderTarget);
            Utilities.Dispose(ref solidColorBrush);
            Utilities.Dispose(ref swapChain);
            Utilities.Dispose(ref d2dFactory);
            Utilities.Dispose(ref device);

        }

        public GameScreen()
        {
            //Создаём формат текста
            FactoryDWrite = new SharpDX.DirectWrite.Factory();
            TextFormat = new TextFormat(FactoryDWrite, "Times New Roman", 40) { TextAlignment = TextAlignment.Center, ParagraphAlignment = ParagraphAlignment.Center };
            

            //Создаём Device и SwapChain 
            Device.CreateWithSwapChain(
                SharpDX.Direct3D.DriverType.Hardware,
                DeviceCreationFlags.BgraSupport,
                new[] {
                    SharpDX.Direct3D.FeatureLevel.Level_10_0,
                },
                new SwapChainDescription()
                {
                    ModeDescription =
                        new ModeDescription(
                            ClientSize.Width,
                            ClientSize.Height,
                            new Rational(60, 1),
                            Format.R8G8B8A8_UNorm
                        ),
                    SampleDescription = new SampleDescription(1, 0),
                    Usage = Usage.BackBuffer | Usage.RenderTargetOutput,
                    BufferCount = 3,
                    OutputHandle = Handle,
                    Flags = SwapChainFlags.None,
                    IsWindowed = true,
                    SwapEffect = SwapEffect.Discard,
                },
                out device, out swapChain
            );
            //Обновляем таймер
            LastTime = DateTime.Now;

            //Добавляем обработку изменения размера
            OnResize();
            Resize += (object? sender, EventArgs a) => OnResize();
            
            //Добавляем таймер для автоматического рендера
            stopwatch = new System.Windows.Forms.Timer();
            stopwatch.Interval = 1;
            stopwatch.Tick += (object? sender, EventArgs e) => Render();
            stopwatch.Start();
        }
        //Обновление количества ячеек в игре
        public void UpdateGrid(int NewCellCount)
        {
            CellCount = NewCellCount;
            CellPerc = (1f - 0.03f) / CellCount;
            Animations = new AnimationInfo[CellCount * CellCount + 1];
        }

        //Обновление буферов при изменении размеров окна
        public void OnResize()
        {
            //Обновляем геометрию ячеек
            WindowWidth = ClientSize.Width;
            WindowHeight = ClientSize.Height;
            MinDimension = Math.Min(WindowWidth, WindowHeight);
            
            //Удаляем все ссылки на swapChain
            Utilities.Dispose(ref backBuffer);
            Utilities.Dispose(ref renderTargetView);
            Utilities.Dispose(ref surface);
            Utilities.Dispose(ref d2dRenderTarget);
            Utilities.Dispose(ref solidColorBrush);

            //Обновляем swapChain
            swapChain.ResizeBuffers(3, WindowWidth, WindowHeight,
                Format.Unknown, SwapChainFlags.None);

            //Создаём текстуру, которая будет целью отрисовки
            backBuffer = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);
            renderTargetView = new RenderTargetView(device, backBuffer);

            //Создаём холст для 2D графики
            surface = backBuffer.QueryInterface<Surface>();
            d2dRenderTarget = new RenderTarget(d2dFactory, surface,
                new RenderTargetProperties(new
                PixelFormat(Format.Unknown, SharpDX.Direct2D1.AlphaMode.Premultiplied)));

            //Создаём кисть
            solidColorBrush = new SolidColorBrush(d2dRenderTarget, Color.White);
        }
        public Vector2 ToVector(int pos)
        {
            return new Vector2(pos % CellCount, pos / CellCount);
        }
        public void Render()
        {
            //Подсчёт относительно точного времени кадра
            DateTime dateTime = DateTime.Now;
            float DeltaTime = (float)(dateTime - LastTime).TotalSeconds;
            LastTime = dateTime;

            //Обновляем анимации
            for (int i = 0; i < Animations.Count(); i++)
                Animations[i].Tick(DeltaTime);

            //Если окно скрыто то не рендерим
            if (MinDimension == 0)
                return;

            //Начало отрисовки
            d2dRenderTarget.BeginDraw();
            //Заливаем стол приятным цветом
            d2dRenderTarget.Clear(new Color4(0.733f, 0.678f, 0.627f, 1));
            
            //Рисуем ячейки
            solidColorBrush.Color = new Color4(0.93f, 0.89f, 0.85f, 1);
            for (int i = 0; i < CellCount * CellCount; i++)
                d2dRenderTarget.FillRoundedRectangle(MakeRect(ToPixelPosition(ToVector(i)), ToPixelSize()), solidColorBrush);

            //Рисуем двигающиеся блоки
            foreach (var a in Animations)
                if (a.Current.Score > 0)
                    Draw(a.Current);

            //Заканчиваем отрисовку
            d2dRenderTarget.EndDraw();
            swapChain.Present(0, PresentFlags.None);
        }
        public RoundedRectangle MakeRect(Vector2 Pos, Vector2 Size)
        {
            return new RoundedRectangle()
            {
                RadiusX = Size.X/10,
                RadiusY = Size.Y/10,
                Rect = new RectangleF(Pos.X - Size.X / 2, Pos.Y - Size.Y / 2,
                Size.X, Size.Y)
            };
        }
        //Отрисовка одной ячейки
        public void Draw(PinInfo Pin)
        {
            double NormalizedScore = Math.Log2(Pin.Score);
            if (NormalizedScore >= Colors.Length - 2)
                NormalizedScore = Colors.Length - 2;

            //Линейная интерполяция между двух цветов
            solidColorBrush.Color = Color4.Lerp(Colors[(int)NormalizedScore],
                Colors[(int)NormalizedScore + 1], (float)NormalizedScore % 1);

            //Отрисовка самой ячейки
            var sz = ToPixelSize(Pin.Size);
            d2dRenderTarget.FillRoundedRectangle(MakeRect(ToPixelPosition(Pin.Position), sz), solidColorBrush);

            //Собираем текст
            var Text = new TextLayout(FactoryDWrite, Pin.Score.ToString(), TextFormat, sz.X,sz.Y);
            
            //Для значений меньше 6(2,4) цвет чёрный, для остального он белый
            if (Pin.Score < 6)
                solidColorBrush.Color = new Color4(0, 0, 0, 1);
            else
                solidColorBrush.Color = new Color4(1, 1, 1, 1);

            //Устанавливаем размер шрифта в зависимости от размеров окна и количества ячеек
            float BaseFontSize = MinDimension * 0.4f / CellCount;
            Text.SetFontSize(BaseFontSize, new TextRange(0, 6));

            //Изменение размеров, если число слишком большое
            if (Pin.Score >= 10000)
                if (Pin.Score >= 100000)
                    Text.SetFontSize(BaseFontSize*0.675f, new TextRange(0, 6));
                else
                    Text.SetFontSize(BaseFontSize * 0.8f, new TextRange(0, 6));


            //Отрисовка номера
            d2dRenderTarget.DrawTextLayout(ToPixelPosition(Pin.Position) - sz/2, Text, solidColorBrush);

            //Очистка ресурсов
            Utilities.Dispose(ref Text);
        }
        public void NextAnimation(int[] Values, int[] MovedTo, int[] ResultValues, Vector2 movementDirection, bool Inversed = false)
        {
            for (int i = 0; i < CellCount*CellCount; i++)
                if (Values[i] != 0)
                    Animations[i] = new(new(ToVector(i), Values[i]), new(ToVector(MovedTo[i]), ResultValues[MovedTo[i]]), movementDirection);
                else
                    Animations[i] = new(new(ToVector(i), 0), new(ToVector(i), 0));
        }
        public void NewPinAnimation(int Values, int Pos, bool NullDelay = false)
        {
            Animations[CellCount* CellCount] = new(new(ToVector(Pos), 0), new(ToVector(Pos), Values), NullDelay ? 0 : 2f);
        }
        public void InverseAnimation()
        {
            for (int i = 0; i < Animations.Length; i++)
                Animations[i].Flip();
        }
        public Vector2 ToPixelPosition(Vector2 pos)
        {
            return new Vector2((pos.X - CellCount * 0.5f + 0.5f) * MinDimension * CellPerc + WindowWidth * 0.5f,
                (pos.Y - CellCount * 0.5f + 0.5f) * MinDimension * CellPerc + WindowHeight * 0.5f);
        }
        public Vector2 ToPixelSize()
        {
            return ToPixelSize(new Vector2(1));
        }
        public Vector2 ToPixelSize(Vector2 pos)
        {
            return pos * new Vector2(CellPerc * MinDimension * 0.94f);
        }
    }
}
