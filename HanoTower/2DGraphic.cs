using SharpDX;
using SharpDX.Windows;
using SharpDX.DXGI;
using SharpDX.Direct3D11;
using SharpDX.Direct2D1;

using Device = SharpDX.Direct3D11.Device;
using Factory = SharpDX.DXGI.Factory;
using static HanoTower.HanoSolution;
using System.Windows.Forms;

namespace HanoTower
{
    class App2D
    {
        

        RenderForm window;

        Device device;
        SwapChain swapChain;
        SharpDX.Direct2D1.Factory d2dFactory = new SharpDX.Direct2D1.Factory();

        //Render things
        Texture2D backBuffer;
        RenderTargetView renderTargetView;
        Surface surface;
        RenderTarget d2dRenderTarget;
        SolidColorBrush solidColorBrush;

        System.Timers.Timer stopwatch;
        DateTime LastTime;

        List<List<int>> Towers;
        List<ScreenShot> Log;
        int index;
        int DiskCount;

        Vector2 EndPosition;
        Vector2 BeginPosition;
        int AnimDiskWidth;
        int AnimDiskDest;
        float AnimTime;
        float AnimScale;

        bool AutoPlay = false;

        bool Inversed = false;

        int WindowWidth;
        int WindowHeight;
        int ButtonWidth;

        public App2D(List<ScreenShot> log, int n)
        {
            Towers = [[], [], []];
            for (int i = 0; i < n; i++)
                Towers[0].Add(n - i);
            Log = log;
            index = 0;
            DiskCount = n;
            // Create the window to render to
            window = new RenderForm("Ханойские башни");


            // Create the device and swapchain
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
                            window.ClientSize.Width,
                            window.ClientSize.Height,
                            new Rational(60, 1),
                            Format.R8G8B8A8_UNorm
                        ),
                    SampleDescription = new SampleDescription(1, 0),
                    Usage = Usage.BackBuffer | Usage.RenderTargetOutput,
                    BufferCount = 3,
                    OutputHandle = window.Handle,
                    Flags = SwapChainFlags.None,
                    IsWindowed = true,
                    SwapEffect = SwapEffect.Discard,
                },
                out device, out swapChain
            );


            // Ignore all windows events
            Factory factory = swapChain.GetParent<Factory>();
            factory.MakeWindowAssociation(window.Handle, WindowAssociationFlags.IgnoreAll);

            AnimTime = 1;

            LastTime = DateTime.Now;

            Resize();
            window.Resize += (object? sender, EventArgs a) => Resize();
            window.MouseClick += Window_MouseClick;
            window.MouseDoubleClick += Window_MouseClick;

        }

        private void Window_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Location.Y < WindowHeight - 50)
                return;

            if (e.Location.X < ButtonWidth)
            {
                if (index != 0)
                    MoveWithAnimation(Log[--index].Inverted(), true);
                Inversed = true;
                AutoPlay = false;
                return;
            }
            if (e.Location.X > WindowWidth - ButtonWidth)
            {
                if(index != Log.Count)
                    MoveWithAnimation(Log[index++]);
                Inversed = false;
                AutoPlay = false;
                return;
            }
            if (Math.Abs(e.Location.X- WindowWidth/2) < ButtonWidth/2)
            {

                AutoPlay = !AutoPlay;
                Inversed = false;
                if (AutoPlay && index != Log.Count)
                    MoveWithAnimation(Log[index++]);
                return;
            }
        }
        public void Run()
        {
            RenderLoop.Run(window, RenderCallback);
        }
        public void Resize()
        {
            WindowWidth = window.ClientSize.Width;
            WindowHeight = window.ClientSize.Height;
            ButtonWidth = Math.Min(WindowHeight / 10 * 3, 300);

            Utilities.Dispose(ref backBuffer);
            Utilities.Dispose(ref renderTargetView);
            Utilities.Dispose(ref surface);
            Utilities.Dispose(ref d2dRenderTarget);
            Utilities.Dispose(ref solidColorBrush);

            swapChain.ResizeBuffers(3, WindowWidth, WindowHeight,
                Format.Unknown, SwapChainFlags.None);

            backBuffer = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);
            renderTargetView = new RenderTargetView(device, backBuffer);

            surface = backBuffer.QueryInterface<Surface>();

            d2dRenderTarget = new RenderTarget(d2dFactory, surface,
                new RenderTargetProperties(new
                PixelFormat(Format.Unknown, SharpDX.Direct2D1.AlphaMode.Premultiplied)));

            solidColorBrush = new SolidColorBrush(d2dRenderTarget, Color.White);
        }
        public void RenderCallback()
        {
            d2dRenderTarget.BeginDraw();
            d2dRenderTarget.Clear(Color.Black);
            solidColorBrush.Color = new Color4(1, 1, 1, 1);

            for (int i = 0; i < 3; i++)
                Draw(Towers[i], i);

            DateTime dateTime = DateTime.Now;
            float DeltaTime = (float)(dateTime - LastTime).TotalSeconds;
            LastTime = dateTime;

            if (DeltaTime > 0.2f)
                DeltaTime = 0.2f;

            UpdateAnimation(DeltaTime);
            solidColorBrush.Color = new Color4(1, 1, 1, 1);
            
            d2dRenderTarget.FillRectangle(new RectangleF(0, WindowHeight - 40, ButtonWidth, 40), solidColorBrush);
            d2dRenderTarget.FillRectangle(new RectangleF((WindowWidth - ButtonWidth) / 2, WindowHeight - (AutoPlay ? 20 : 40), ButtonWidth, 40), solidColorBrush);
            d2dRenderTarget.FillRectangle(new RectangleF(WindowWidth - ButtonWidth, WindowHeight - 40, ButtonWidth, 40), solidColorBrush);
            
            d2dRenderTarget.EndDraw();
            swapChain.Present(0, PresentFlags.None);
        }
        public void Draw(RoundedRectangle Geometry, int Width)
        {
            solidColorBrush.Color = RainbowColor((float)(Width - 1) * 0.85f / DiskCount);
            d2dRenderTarget.FillRoundedRectangle(Geometry, solidColorBrush);
        }
        public void Draw(List<int> stack, int id)
        {
            for (int i = 0; i < stack.Count; i++)
                Draw(GetDiskGeometry(id, i, stack[i]), stack[i]);
        }
        public void MoveWithAnimation(ScreenShot Movement, bool inversed = false)
        {
            if (AnimTime < 1)
                Towers[AnimDiskDest].Add(AnimDiskWidth);

            var temp = Towers[Movement.Begin].Last();
            Towers[Movement.Begin].Remove(temp);

            BeginPosition = GetDiskLocation(Movement.Begin, Towers[Movement.Begin].Count);
            EndPosition = GetDiskLocation(Movement.End, Towers[Movement.End].Count);

            if (inversed != Inversed)
                AnimTime = 1 - AnimTime;
            else
                AnimTime = 0;

            AnimScale = (Math.Abs(Movement.Begin - Movement.End) == 1) ? 1 : 0.75f;
            AnimDiskWidth = temp;
            AnimDiskDest = Movement.End;
        }
        public RoundedRectangle GetDiskGeometry(Vector2 Pos, int Width)
        {
            float TotalWidth = Width * 25;
            float Height = 20;

            return new RoundedRectangle()
            {
                RadiusX = 7,
                RadiusY = 7,
                Rect = new RectangleF(Pos.X - TotalWidth / 2, Pos.Y,
                TotalWidth, Height)
            };
        }
        public RoundedRectangle GetDiskGeometry(int Stack, int Pos, int Width)
        {
            return GetDiskGeometry(GetDiskLocation(Stack, Pos), Width);
        }
        public Vector2 GetDiskLocation(int Stack, int Pos)
        {
            return new Vector2(
                    WindowWidth * 0.25f * (1 + Stack),
                WindowHeight * 0.65f - 21 * Pos);
        }
        void UpdateAnimation(float DeltaTime)
        {
            if (AnimTime > 1)
                return;

            AnimTime += DeltaTime * 3.5f * AnimScale;

            Vector2 Intermediate = (BeginPosition + EndPosition + new Vector2(0, -2 * (BeginPosition - EndPosition).Length())) / 2;

            float TimeSmoothed = 2 * AnimTime * AnimTime;
            if (TimeSmoothed > 0.5f)
                TimeSmoothed = -2.0f * (AnimTime - 1f) * (AnimTime - 1f) + 1f;

            Vector2 Position = Vector2.Lerp(Vector2.Lerp(BeginPosition, Intermediate, TimeSmoothed),
                Vector2.Lerp(Intermediate, EndPosition, TimeSmoothed), TimeSmoothed);

            if (AnimTime > 1)
            {
                Towers[AnimDiskDest].Add(AnimDiskWidth);
                Draw(GetDiskGeometry(EndPosition, AnimDiskWidth), AnimDiskWidth);
                if (AutoPlay && index != Log.Count)
                    MoveWithAnimation(Log[index++]);
            }
            else
                Draw(GetDiskGeometry(Position, AnimDiskWidth), AnimDiskWidth);


        }

        float hue2rgb(float t, float min = 0.2f, float max = 1.0f)
        {
            t = (t + 1) % 1;
            if (t < 1f / 6)
                return min + (max - min) * 6 * t;
            if (t < 1f / 2)
                return max;
            if (t < 2f / 3)
                return min + (max - min) * (2f / 3 - t) * 6;
            return min;

        }
        Color4 RainbowColor(float h)
        {
            Color4 result = new Color4();

            result.Red = hue2rgb(h + 1f / 3);
            result.Green = hue2rgb(h);
            result.Blue = hue2rgb(h - 1f / 3);
            result.Alpha = 1;

            return result;
        }

    }
}
