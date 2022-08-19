using Calculator.Calculator;

namespace Calculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DateTime dtNow = DateTime.Now;
            string a = Eval.Parse(" 2 + 2 + 2 * 3 / 6 + (1/3-(4+5+(5+9)/100*2134983))");
            Console.WriteLine($"Потрачено {(DateTime.Now - dtNow).TotalMilliseconds} миллисекунд");
            Console.WriteLine(a);
        }
    }
}