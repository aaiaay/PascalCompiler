using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilyator
{
    class Scope
    {
        private Dictionary<string, TypeConst> table = new Dictionary<string, TypeConst>();

        public bool Contains(string identifier)
        {
            return table.ContainsKey(identifier);
        }

        public TypeConst Get_Type(string identifier)
        {
            return table[identifier];
        }

        public TypeConst Type(Token token)
        {
            if (token is KeyWordToken keywordtoken)
                switch (keywordtoken.keyword)
                {
                    case Keyword.INTC:
                        return TypeConst.integer;

                    case Keyword.FLOATC:
                        return TypeConst.real;

                    case Keyword.CHARC:
                        return TypeConst.@char;

                    case Keyword.BOOLC:
                        return TypeConst.boolean;
                }
            else if (token is ConstToken constToken) return constToken.typeconst;
            else if (token is IdentToken identToken) return Get_Type(identToken.identifier);
            return TypeConst.integer;
        }

        public bool Add(List<string> identifier, TypeConst type)
        {
            bool a = false;
            foreach (var i in identifier)
                if (!Contains(i))
                {
                    table.Add(i, type);
                    a = true;
                }
            return a;
        }

        public bool Types(List<TypeConst> listTypes, TypeConst type)
        {
            if (type == TypeConst.real)
            {
                foreach (var i in listTypes)
                {
                    if (i != TypeConst.integer && i != TypeConst.real)
                    {
                        return false;
                    }
                }
            }
            else
            {
                foreach (var i in listTypes)
                {
                    if (i != type)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}

