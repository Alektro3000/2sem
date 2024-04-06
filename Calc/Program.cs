using Microsoft.VisualBasic;

namespace Calc
{
    internal class Program
    {
        static public double Eval(double a, UnarLexemType Operator)
        {
            double value;
            switch (Operator)
            {
                case UnarLexemType.sin:
                    value = Math.Sin(a);
                    break;
                case UnarLexemType.cos:
                    value = Math.Cos(a);
                    break;
                case UnarLexemType.tan:
                    value = Math.Tan(a);
                    break;
                case UnarLexemType.ctg:
                    value = Math.Tan(double.Pi / 2 - a);
                    break;
                case UnarLexemType.asin:
                    value = Math.Asin(a);
                    break;
                case UnarLexemType.acos:
                    value = Math.Acos(a);
                    break;
                case UnarLexemType.atan:
                    value = Math.Atan(a);
                    break;
                case UnarLexemType.actg:
                    value = double.Pi/2 - Math.Atan(a);
                    break;
                case UnarLexemType.exp:
                    value = Math.Exp(a);
                    break;
                case UnarLexemType.sqrt:
                    value = Math.Sqrt(a);
                    break;
                case UnarLexemType.cbrt:
                    value = Math.Cbrt(a);
                    break;
                case UnarLexemType.sign:
                    value = Math.Sign(a);
                    break;
                case UnarLexemType.abs:
                    value = Math.Abs(a);
                    break;
                case UnarLexemType.ln:
                    value = Math.Log(a);
                    break;
                case UnarLexemType.log:
                    value = Math.Log10(a);
                    break;
                case UnarLexemType.inv:
                    value = -a;
                    break;
                default:
                    throw new ArgumentException();
            }
            return value;
        }
        //Версия Eval в которой операнды имеют обратный порядок
        static public double EvalInv(double b, double a, LexemType Operator) => Eval(a, b, Operator);
        static public double Eval(double a, double b, LexemType Operator)
        {
            double value;
            switch (Operator)
            {
                case LexemType.Add:
                    value = a + b;
                    break;
                case LexemType.Sub:
                    value = a - b;
                    break;
                case LexemType.Mul:
                    value = a * b;
                    break;
                case LexemType.Div:
                    value = a / b;
                    break;
                case LexemType.Exp:
                    value = Math.Pow(a, b);
                    break;
                default:
                    throw new ArgumentException();
            }

            return value;
        }

        static void Main(string[] args)
        {
            string[] ParserTestData = {"2","2+2","2*3","2-3", "3/2", "2 + 3 * 2", "-5/2",  "0.1 + 0,1*2",
            "pi", "-inf", "inf", "e", "tau*0.5", "nan + 2",
            "-1e3+1e-2",
            "abs -1",
            "tan pi/4 + sin pi/2",
            "(ctg pi/3) * tg pi/3",
            "sqrt 4 * 0.5",
            "exp ln 0.5",
            "log 10^3",
            "log cbrt 1000",
            "cos arcsin 0.5",
            "(2)2(2+2)(3)",
            "2--2","2+-2", "2*-2",
            "-(2+2)",
            "-2pi1/-tau",
            "1/sinpi",
            "2^4*2+2",
            "2+2*2^4",
            "2+2*2**4",
            "9-(2-11)/10", "(5+10,5-24)*2"};
            string[] SimpleParserTestData = { "0", "(2+2)","(2*3)","(2-3)", "(3/2)", "(2 + (3 * 2))", "(-5/2)",
            "(0.1 + (0,1*2))",
            "(2^4)",
            "(2**4)",
            "pi", "-inf", "inf", "e", "(tau*0.5)", "(nan + 2)",
            "(-1e3+1e-2)",
            "(cbrt 8)",
            "(abs -1)",
            "((tan (pi/4)) + (sin(pi/2)))",
            "(exp (ln 0.5))",
            "(log (cbrt 1000))",
            "(cos (asin 0.5))",
            "(0)",
            "(2pi)",
            "((2pi)(3/pi))",
            "((3(2pi))/pi)",
            "((2-11)/(10-8))", "(11-((5+10,5)*2))",};

            foreach (var Equation in ParserTestData)
                Console.WriteLine(Equation + " = " + Parser.Evaluate(Lexer.Analyze(Equation)));

            foreach (var Equation in SimpleParserTestData)
            {
                var a = Lexer.Analyze(Equation);
                Console.WriteLine(Equation + " = " + SimpleParser.Evaluate(a));
            }
        }
    }
}
