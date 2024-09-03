using System;
using System.IO;

namespace Compilyator
{
    class LexicalAnalyzer
    {
        public IOModule iomodule;

        private string symbol;
        private Token token;

        static public TextPosition positionnow;

        public LexicalAnalyzer(StreamReader reader)
        {
            this.iomodule = new IOModule(reader);
        }

        private bool Check_If_KeyWord(string symbol)
        {
            return KeyWords.Check_If_KeyWord(symbol);
        }
        private bool Check_If_Constant(string symbol, ref TypeConst type)
        {
            if (symbol == "True" || symbol == "False"
                || symbol == "true" || symbol == "false")
            {
                type = TypeConst.boolean;
                return true;
            }
            else
            if (symbol[0] == '\'' && symbol[2] == '\'')
            {
                type = TypeConst.@char;
                return true;
            }
            else
            if (int.TryParse(symbol, out int a))
            {
                if (a <= 32767)
                {
                    type = TypeConst.integer;
                    return true;
                }
                else return false;
            }
            else
            {
                bool point = true;
                if (Convert.ToInt32(symbol[0]) - 48 >= 0 && Convert.ToInt32(symbol[0]) - 48 <= 9)
                {
                    for (int i = 1; i < symbol.Length; i++)
                    {
                        if (symbol[i] == '.')
                        {
                            if ((Convert.ToInt32(symbol[i + 1] - 48) < 0 || Convert.ToInt32(symbol[i + 1]) - 48 > 9) || !point)
                            {
                                return false;
                            }
                            else
                            {
                                i++;
                            }
                            point = false;
                        }
                        else if (Convert.ToInt32(symbol[i] - 48) < 0 || Convert.ToInt32(symbol[i]) - 48 > 9)
                        {
                            return false;
                        }
                    }
                    type = TypeConst.real;
                    return true;
                }
            }
            return false;
        }
        private bool Check_If_Identifier(string symbol)
        {         
            for (int i = 0; i < symbol.Length; i++)
            {
                int s = Convert.ToInt32(symbol[i]);
                if (!((s >= 48 && s <= 57 && i != 0) || (s >= 65 && s <= 90) || (s >= 97 && s <= 122) || (s == 95)))
                    return false;
            }
            return true;
        }

        public Token nextsym()
        {
            if (iomodule.is_end)
            {
                TypeConst type = TypeConst.real;
                symbol = iomodule.nextsym();

                token = null;

                if (Check_If_KeyWord(symbol))              
                    token = new KeyWordToken(KeyWords.GetKeyWord(symbol), positionnow);              
                else if (Check_If_Constant(symbol, ref type))            
                    token = new ConstToken(type, positionnow, symbol);
                else if (Check_If_Identifier(symbol))             
                    token = new IdentToken(symbol, positionnow);           
                else
                {
                    token = new UnexpectedToken(symbol, positionnow);
                    Error err = new Error(positionnow, "Лексическая ошибка", symbol);
                    iomodule.list_of_errors.Add(err);
                }

                if (token != null)             
                    Console.WriteLine($"[{symbol}] - {token.ToString()}");             
            }
            return token;
        }
    }
}

