using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Compilyator
{
    class Compiler
    {
        public string file_name;

        public Compiler(string file_name)
        {
            this.file_name = file_name;
        }

        public void Compile()
        {
            using (StreamReader reader = new StreamReader(file_name))
            {
                SyntaxAnalyzer syntaxanalayzer = new SyntaxAnalyzer(reader);
                syntaxanalayzer.begin();
                syntaxanalayzer.lexicalanalyzer.iomodule.PrintError();
            }

        }
    }
}
