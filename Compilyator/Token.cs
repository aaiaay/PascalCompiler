using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilyator
{
    public enum TokenType
    {
        KEYWORD,
        IDENTIFIER,
        CONST,
        UNEXPECTED
    }


    public enum TypeConst
    {
        integer,
        real,
        boolean,
        @char
    }

    abstract class Token
    {
        public TextPosition position { get; }
        public static TokenType tokentype;
        protected Token(TextPosition pos, TokenType tokentype)
        {
            position = new TextPosition(pos);
            tokentype = TokenType.UNEXPECTED;
        }

        public TokenType Get_Type()
        {
            return tokentype;
        }
    }
    class KeyWordToken : Token
    {
        public Keyword keyword { get; }

        public KeyWordToken(Keyword keyword, TextPosition position) : base(position, tokentype)
        {
            this.keyword = keyword;
            tokentype = TokenType.KEYWORD;
        }

        public override string ToString()
        {
            return Convert.ToString(tokentype);
        }
    }

    class IdentToken : Token
    {
        public string identifier;
        public IdentToken(string identifier, TextPosition position) : base(position, tokentype)
        {
            this.identifier = identifier;
            tokentype = TokenType.IDENTIFIER;
        }
        public override string ToString()
        {
            return Convert.ToString(tokentype);
        }
    }

    class ConstToken : Token
    {
        public TypeConst typeconst { get; }
        public string value;
        public ConstToken(TypeConst typeconst, TextPosition position, string value) : base(position, tokentype)
        {
            this.typeconst = typeconst;
            tokentype = TokenType.CONST;
            this.value = value;
        }
        public override string ToString()
        {
            return Convert.ToString(tokentype) + '-' + Convert.ToString(typeconst);
        }
    }

    class UnexpectedToken : Token
    {
        public string unexpected;
        public UnexpectedToken(string unexpected, TextPosition position) : base(position, tokentype)
        {
            tokentype = TokenType.UNEXPECTED;
            this.unexpected = unexpected;
        }
        public override string ToString()
        {
            return Convert.ToString(tokentype);
        }
    }
}
