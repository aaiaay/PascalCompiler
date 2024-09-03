using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Compilyator
{
    class IOModule
    {
        private StreamReader reader;

        public List<Error> list_of_errors = new List<Error>();
        public TextPosition positionnow = new TextPosition();

        private string linenow;
        private char charnow;

        public bool is_end = true;
        public string symbol;
   
        private char lit;
        private char[] operators = {':', '=', '>', '<', ';', '(', ')', ',', '{', '}', '.', '+', '-', '*', '/'};  

        public IOModule(StreamReader reader)
        {
            this.reader = reader;
            linenow = reader.ReadLine();
            positionnow.linenumber = 1;
            positionnow.charnumber = 0;
        }

        private char nextch()
        {
            char ch;

            if (positionnow.charnumber == linenow.Length)
            {
                linenow = reader.ReadLine();
                positionnow.linenumber++;
                positionnow.charnumber = 0;
                if (linenow == null)               
                    is_end = false;
                
                return ' ';
            }
            ch = linenow[positionnow.charnumber];
            positionnow.charnumber++;
            return ch;
        }

        public string nextsym()
        {
            charnow = nextch();

            while (charnow == ' ' && is_end)
            {
                charnow = nextch();
            }

            LexicalAnalyzer.positionnow = new TextPosition(positionnow.charnumber - 1, positionnow.linenumber);

            if (Operator())           
                return symbol;

            if (symbol == "//")
            {
                charnow = nextch();
                while (charnow == ' ' && is_end) charnow = nextch();
            }

            symbol = "";

            while (charnow != ' ')
            {
                symbol = symbol + charnow;
                if (CheckNonspace()) break;
                charnow = nextch();
            }
            return symbol;
        }

        public void PrintError()
        {
            foreach (var error in list_of_errors)
            {
                Console.WriteLine(error);
            }
        }

        private bool CheckNonspace()
        {
            char nextsym = ' ';
            if (linenow != null)
            {
                if (positionnow.charnumber < linenow.Length)
                {
                    lit = linenow[positionnow.charnumber];
                    if (positionnow.charnumber + 1 < linenow.Length)                   
                        nextsym = linenow[positionnow.charnumber + 1];                  

                    if (lit == '.' && (int.TryParse(nextsym.ToString(), out int a)
                        || int.TryParse(charnow.ToString(), out int b)))                    
                        return false;
                    
                    foreach (var op in operators)
                    {
                        if (lit == op)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool Operator()
        {
            char nextlit;

            if (linenow == null)           
                return false;

            if (positionnow.charnumber < linenow.Length)
                nextlit = linenow[positionnow.charnumber];    
            else
                nextlit = ' ';

            foreach (var op in operators)
            {
                if (charnow == op)
                {
                    symbol = "" + charnow;
                    if (((symbol == ":" || symbol == ">" || symbol == "<") && nextlit == '=')
                            || (symbol == "<" && nextlit == '>') || (symbol == "/" && nextlit == '/'))
                    {
                        nextlit = nextch();
                        symbol = symbol + nextlit;
                        if (symbol == "//")
                        {
                            linenow = reader.ReadLine();
                            positionnow.linenumber++;
                            positionnow.charnumber = 0;
                            if (linenow == null)               
                                is_end = false;                          
                            return false;
                        }
                    }  
                    return true;
                }
            }
            return false;
        }
    }
}
