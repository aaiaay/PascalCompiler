using System.Collections.Generic;
using System.IO;

namespace Compilyator
{
    class SyntaxAnalyzer
    {
        public LexicalAnalyzer lexicalanalyzer;

        private Token tokennow;
        private List<string> identifiernow = new List<string>();

        private List<TypeConst> list_of_types_now = new List<TypeConst>();
        private List<Keyword> starters = new List<Keyword>();
        private List<Keyword> followers = new List<Keyword>();

        private Scope scope = new Scope();

        private TypeConst typenow = TypeConst.integer;
        public SyntaxAnalyzer(StreamReader reader)
        {
            this.lexicalanalyzer = new LexicalAnalyzer(reader);
        }
        public void begin()
        {
            nextsym();
            program();
        }

        private void nextsym()
        {
            if (lexicalanalyzer.iomodule.is_end)
                tokennow = lexicalanalyzer.nextsym();
            else
                tokennow = null;
        }

        private void program()
        {
            starters = new List<Keyword> { Keyword.PROGRAM };
            if (belong(starters))
            {
                if (accept(Keyword.PROGRAM))
                    nextsym();

                if (accept(TokenType.IDENTIFIER))
                    nextsym();

                if (accept(Keyword.SEMICOLON))
                    nextsym();

                block();
            }
            else lexicalanalyzer.iomodule.list_of_errors.Add(new Error(tokennow.position, "Синтаксическая ошибка", tokennow.ToString()));
        }

        private void block()
        {
            starters = new List<Keyword> { Keyword.CONSTC, Keyword.VAR, Keyword.BEGIN };
            followers.Add(Keyword.POINT);
            if (belong(starters))
            {
                if (waiting(Keyword.CONSTC))
                    constpart();

                if (waiting(Keyword.VAR))
                    varpart();

                compoundstatemen();
            }
            else
            {
                lexicalanalyzer.iomodule.list_of_errors.Add(new Error(tokennow.position, "Синтаксическая ошибка", tokennow.ToString()));
                skip();
            }
            accept(Keyword.POINT);
        }

        private void constpart()
        {
            nextsym();

            while (waiting(TokenType.IDENTIFIER))
            {
                checkident();
                nextsym();
                if (accept(Keyword.EQUAL))
                {
                    nextsym();
                }
                if (waiting(Keyword.MINUS))
                {
                    nextsym();
                }
                if (accept(TokenType.CONST))
                {
                    add_into_scope();
                    nextsym();
                }
                if (accept(Keyword.SEMICOLON))
                    nextsym();
            }
        }

        private void varpart()
        {
            nextsym();

            while (waiting(TokenType.IDENTIFIER))
            {
                checkident();
                nextsym();

                while (waiting(Keyword.COMMA))
                {
                    nextsym();
                    checkident();
                    accept(TokenType.IDENTIFIER);
                    nextsym();
                }

                if (accept(Keyword.COLON))
                    nextsym();

                accept(tokennow);
                add_into_scope();
                nextsym();

                if (accept(Keyword.SEMICOLON))
                    nextsym();
            }
        }

        private void compoundstatemen()
        {
            starters = new List<Keyword> { Keyword.BEGIN };
            if (belong(starters))
            {
                if (accept(Keyword.BEGIN))
                    nextsym();

                statementpart();

                while (waiting(Keyword.SEMICOLON))
                {
                    statementpart();
                }

                if (accept(Keyword.END))
                    nextsym();
            }
            else
            {
                lexicalanalyzer.iomodule.list_of_errors.Add(new Error(tokennow.position, "Синтаксическая ошибка", tokennow.ToString()));
                skip();
            }
        }

        private void statementpart()
        {
            if (waiting(Keyword.BEGIN))
            {
                compoundstatemen();
            }
            else
            {
                while (waiting(TokenType.IDENTIFIER))
                {
                    list_of_types_now.Clear();
                    get_type();
                    nextsym();
                    if (accept(Keyword.ASSIGN))
                        nextsym();

                    expression();

                    if (!scope.Types(list_of_types_now, typenow))
                        lexicalanalyzer.iomodule.list_of_errors.Add(new Error(tokennow.position, "Семантическая ошибка"));

                    if (accept(Keyword.SEMICOLON))
                        nextsym();
                }
            }
        }

        private void expression()
        {
            if (waiting(Keyword.MINUS))           
                nextsym();
            
            term();

            starters = new List<Keyword> { Keyword.MINUS, Keyword.PLUS, Keyword.SEMICOLON };
            followers = new List<Keyword> { Keyword.SEMICOLON };
            if (belong(starters) || tokennow is IdentToken)
            {
                while (waiting(Keyword.PLUS) || waiting(Keyword.MINUS))
                {
                    nextsym();
                    term();
                }
            }
            else
            {
                lexicalanalyzer.iomodule.list_of_errors.Add(new Error(tokennow.position, "Синтаксическая ошибка", tokennow.ToString()));
                skip();
            }

        }

        private void term()
        {
            starters = new List<Keyword> { Keyword.STAR, Keyword.SLASH };
            multiplier();
            {
                while (waiting(Keyword.STAR) || waiting(Keyword.SLASH))
                {
                    nextsym();
                    multiplier();
                }
            }
        }

        private void multiplier()
        {
            starters = new List<Keyword> { Keyword.LEFTPAR };
            if (belong(starters) || tokennow is ConstToken || tokennow is IdentToken)
            {
                if (waiting(Keyword.LEFTPAR))
                {
                    nextsym();
                    expression();

                    if (accept(Keyword.RIGHTPAR))
                        nextsym();
                }
                else if (waiting(TokenType.IDENTIFIER))
                {
                    search();
                    nextsym();
                }
                else if (waiting(TokenType.CONST))
                {
                    search();
                    nextsym();
                }
            }
            else
            {
                lexicalanalyzer.iomodule.list_of_errors.Add(new Error(tokennow.position, "Синтаксическая ошибка"));
                skip();
            }
        }

        private void search()
        {
            if (tokennow is IdentToken curT)
            {
                if (scope.Contains(curT.identifier))
                    list_of_types_now.Add(scope.Type(tokennow));
                else
                    lexicalanalyzer.iomodule.list_of_errors.Add(new Error(curT.position, "Семантическая ошибка", curT.identifier));
            }
            else
                list_of_types_now.Add(scope.Type(tokennow));

        }

        private bool belong(List<Keyword> keywordtoken)
        {          
            return (tokennow is KeyWordToken kwt && keywordtoken.Contains(kwt.keyword));
        }

        private void skip()
        {
            while (tokennow != null && !belong(starters) && !belong(followers))
            {
                nextsym();
            }
        }

        private void checkident()
        {
            if (tokennow is IdentToken curT)
            {
                if (!scope.Contains(curT.identifier)) identifiernow.Add(curT.identifier);
                else lexicalanalyzer.iomodule.list_of_errors.Add(new Error(curT.position, "Семантическая ошибка", curT.identifier));
            }
        }

        private void add_into_scope()
        {
            if (tokennow is ConstToken curT)
            {
                if (!scope.Add(identifiernow, curT.typeconst))               
                    lexicalanalyzer.iomodule.list_of_errors.Add(new Error(curT.position, "Семантическая ошибка"));
            }
            else
            {
                if (tokennow is KeyWordToken curT2)
                {
                    TypeConst tc = scope.Type(tokennow);
                    if (!scope.Add(identifiernow, tc))
                    {
                        lexicalanalyzer.iomodule.list_of_errors.Add(new Error(curT2.position, "Семантическая ошибка"));
                    }
                }
            }
            identifiernow.Clear();
        }

        public void get_type()
        {
            if (tokennow is IdentToken curT)
            {
                if (scope.Contains(curT.identifier))                
                    typenow = scope.Get_Type(curT.identifier);                
                else               
                    lexicalanalyzer.iomodule.list_of_errors.Add(new Error(curT.position, "Семантическая ошибка", curT.identifier));          
            }
        }

        private bool waiting(Keyword kw)
        {
            return (tokennow is KeyWordToken curT && curT.keyword == kw);
        }

        private bool accept(Keyword kw)
        {
            if (!waiting(kw))
            {
                lexicalanalyzer.iomodule.list_of_errors.Add(new Error(tokennow.position, "Синтаксическая ошибка", tokennow.ToString()));
                return false;
            }
            else       
                return true;        
        }

        private bool waiting(TokenType type)
        {
            return (tokennow.Get_Type() == type && tokennow != null);
        }

        private bool accept(TokenType type)
        {
            if (!waiting(type))
            {
                lexicalanalyzer.iomodule.list_of_errors.Add(new Error(tokennow.position, "Синтаксическая ошибка", tokennow.ToString()));
                return false;
            }
            else
                return true;       
        }

        private void accept(Token kwtoken)
        {
            if (!waiting(Keyword.BOOLC) && !waiting(Keyword.INTC) && !waiting(Keyword.FLOATC) && !waiting(Keyword.CHARC))          
                lexicalanalyzer.iomodule.list_of_errors.Add(new Error(tokennow.position, "Синтаксическая ошибка", tokennow.ToString()));           
        }
      
    }
}

