using System.Globalization;

namespace Calc
{
    public enum LexemType
    {
        Num,
        End,
        OpenBracket,
        Add,
        Sub,
        Mul,
        Div,
        Exp,
        CloseBracket,
        Unar,
        Error,
    }
    public enum UnarLexemType
    {
        exp, //экспонента
        log, //логарифм по основанию 10
        ln,  //логарифм по натуральному основанию
        tan, //тангенс
        ctg, //котангенс
        sin, //синус
        cos, //косинус
        atan,//арктангенс
        actg,//арккотангес
        asin,//арксинус
        acos,//арккосинус
        abs, //модуль
        sign,//знак числа
        sqrt,//квадратный корень
        cbrt,//кубический корень
        inv, //унарный минус
    }
    public struct Lexem
    {
        public LexemType Type;
        public double Value;
        public UnarLexemType UnarType;
        public Lexem(LexemType type)
        {
            Type = type;
        }
        public Lexem(double num)
        {
            Type = LexemType.Num;
            Value = num;
        }
        public Lexem(UnarLexemType unarType)
        {
            Type = LexemType.Unar;
            UnarType = unarType;
        }
        public override string ToString()
        {
            if (Type == LexemType.Num)
                return Value.ToString();
            if (Type == LexemType.Unar)
                return UnarType.ToString();
            return Type.ToString();
        }
    }
    internal class Lexer
    {
        static readonly Dictionary<char, LexemType> Operators = new Dictionary<char, LexemType>() {
            { '+', LexemType.Add },
            { '-', LexemType.Sub },
            { '*', LexemType.Mul },
            { '/', LexemType.Div },
            { ':', LexemType.Div },
            { '^', LexemType.Exp },
            { '(', LexemType.OpenBracket },
            { ')', LexemType.CloseBracket },};
        static readonly Dictionary<string, UnarLexemType> Unar = new Dictionary<string, UnarLexemType>() {
            { "cos", UnarLexemType.cos},
            { "tan", UnarLexemType.tan },
            { "tg", UnarLexemType.tan },
            { "ctg", UnarLexemType.ctg },
            { "sin", UnarLexemType.sin},

            { "acos", UnarLexemType.acos},
            { "atan", UnarLexemType.atan },
            { "atg", UnarLexemType.atan },
            { "actg", UnarLexemType.actg },
            { "asin", UnarLexemType.asin},

            { "arccos", UnarLexemType.acos},
            { "arctan", UnarLexemType.atan },
            { "arctg", UnarLexemType.atan },
            { "arcctg", UnarLexemType.actg },
            { "arcsin", UnarLexemType.asin},

            { "sqrt", UnarLexemType.sqrt },
            { "cbrt", UnarLexemType.cbrt },
            { "abs", UnarLexemType.abs },
            { "sign", UnarLexemType.sign },

            { "log", UnarLexemType.log },
            { "ln", UnarLexemType.ln},
            { "exp", UnarLexemType.exp },};
        static readonly Dictionary<string, double> Constants = new Dictionary<string, double>() {
            { "e", double.E},
            { "pi", double.Pi},
            { "tau", double.Tau},
            { "inf", double.PositiveInfinity},
            { "nan", double.NaN},};
        //Символы после которых может быть вставлено умножение
        static readonly HashSet<LexemType> HiddenMulBegin = 
            new HashSet<LexemType>() { LexemType.Num,LexemType.CloseBracket};
        //Символы перед которыми может быть вставлено умножение
        static readonly HashSet<LexemType> HiddenMulEnd =
            new HashSet<LexemType>() { LexemType.Num, LexemType.OpenBracket, LexemType.Unar };
        //Находит длину максимальной подстроки являющейся лексемой в словаре
        static public int GetLexemLength<T>(string input, int pos, Dictionary<string, T> LexemDictionary)
        {
            for (int i = 1; i < 7; i++)
            {
                if (pos + i == input.Length)
                    return 0;

                if (LexemDictionary.ContainsKey(input.Substring(pos, i)))
                    return i;
            }
            return 0;
        }
        //Находит длину максимальной подстроки являющейся числом в заданной строки начиная с позиции
        static public int FindNumLexemLength(string input, int it)
        {
            //Начало поиска чисел
            int BeginOfNum = it;
            if (input[it] == '-')
                it++;

            if (input[it] != '.')
                if (input[it] != '0')
                    if (char.IsDigit(input[it]))
                        while (char.IsDigit(input[it]))
                            it++;
                    else
                        return 0;
                else
                    it++;
            //Обработка точки
            if (input[it] == '.' || input[it] == ',')
            {
                it++;
                while (char.IsDigit(input[it]))
                    it++;
            }
            //Обработка экспоненты
            if (input[it] == 'e')
            {
                it++;
                if (input[it] == '+' || input[it] == '-')
                    it++;

                //Проверка того что экспонента была валидной 
                if (char.IsDigit(input[it])) 
                    //Сборка числа
                    while (char.IsDigit(input[it]))
                        it++;
                else
                {
                    it -= 1;
                    if (input[it] != 'e')
                        it -= 1;
                }
            }
            return it-BeginOfNum;
        }
        //Разбор строки на поток лексем
        static public List<Lexem> Analyze(string input)
        {
            input = input.ToLower();

            //Если нет символа конца строки, то вставляем его
            if (input[input.Length - 1] != '\n')
                input += '\n';

            List<Lexem> Result = new List<Lexem>();
            ListNode<Lexem> LexemNode = Result.PreBeginNode;
            int it = 0;
            while (it != input.Length)
            {
                while (input[it] == ' ')
                    it++;
                if (input[it] == '\n')
                {
                    LexemNode.AddAfter(new(LexemType.End));
                    break;
                }

                LexemType PrevType = LexemNode.IsPreBegin() ? LexemType.Error : LexemNode.Item.Type;
                var NextNode = LexemNode.AddAfter(AnalyzeLexem(input, ref it, PrevType));

                //Вставка умножения между скобками и числами (эта обработка должна быть в парсере)
                if (HiddenMulBegin.Contains(PrevType) && HiddenMulEnd.Contains(NextNode.Item.Type))
                    LexemNode.AddAfter(new(LexemType.Mul));
                LexemNode = NextNode;
            }

            return Result;
        }
        //Анализ очередной лексемы
        static private Lexem AnalyzeLexem(string input, ref int it, LexemType PrevLexem)
        {
            //Особая обработка **
            if (input[it] == '*' && input[it + 1] == '*')
            {
                it += 2;
                return new(LexemType.Exp);
            }

            //Особая обработка минуса (эта обработка должна быть в парсере)
            //Унарный минус не может быть после скобки или числа
            bool IsUnarMinus = input[it] == '-' && PrevLexem != LexemType.Num && PrevLexem != LexemType.CloseBracket;
            
            //Обработка бинарных операций
            if (!IsUnarMinus && Operators.ContainsKey(input[it]))
                return new(Operators[input[it++]]);

            //Поиск унарных операций
            if (GetLexemLength(input, it, Unar) != 0)
            {
                int Length = GetLexemLength(input, it, Unar);
                UnarLexemType type = Unar[input.Substring(it, Length)];
                it += Length;
                return new(type);
            }
            //Поиск констант
            if (GetLexemLength(input, input[it] == '-' ? it + 1 : it, Constants) != 0)
            {
                bool negative = false;
                if (input[it] == '-')
                {
                    negative = true;
                    it++;
                }

                int Length = GetLexemLength(input, it, Constants);
                double Number = Constants[input.Substring(it, Length)];
                it += Length;
                return new(negative ? Number : -Number);
            }
            //Обработка числа
            if (FindNumLexemLength(input, it) != 0)
            {
                int Length = FindNumLexemLength(input, it);
                double num = double.Parse(input.Substring(it, Length).Replace(',', '.'), NumberFormatInfo.InvariantInfo);
                it += Length;
                return new(num);
            }
            //Унарный минус если его не поймали ни константы ни числа
            if (input[it] == '-')
            {
                it++;
                return new(UnarLexemType.inv);
            }
            return new(LexemType.Error);
        }
    }
}
