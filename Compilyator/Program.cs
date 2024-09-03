using System;

namespace Compilyator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var compiler = new Compiler(@"C:\Users\alexa\Desktop\text.txt");
            compiler.Compile();
        }
    }
}
