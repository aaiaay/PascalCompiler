using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilyator
{
    public enum Keyword
    {
        STAR,               /* * */
        SLASH,              /* / */
        EQUAL,              /* = */
        COMMA,              /* , */
        SEMICOLON,          /* ; */
        COLON,              /* : */
        POINT,              /* . */
        RIGHTPAR,           /* ) */
        LEFTPAR,            /* ( */
        LBRACKET,           /* [ */
        RBRACKET,           /* ] */
        LATER,              /* < */
        GREATER,            /* > */
        LATEREQUAL,         /* <= */
        GREATEREQUAL,       /* >= */
        LATERGREATER,       /* <> */
        ASSIGN,             /* := */
        PLUS,               /* + */
        MINUS,              /* - */

        PROGRAM,
        VAR,
        BEGIN,
        END,

        INTC,               /* Целочисленная константа */
        FLOATC,             /* Вещественная константа */
        CHARC,              /* Символьная константа */
        BOOLC,              /* Логическая константа */
        CONSTC              /* Константа */
    }
    class KeyWords
    {
        private static Dictionary<string, Keyword> list_of_keywords = new Dictionary<string, Keyword> 
            {
            {"*", Keyword.STAR},
            {"/", Keyword.SLASH},
            {"=", Keyword.EQUAL},
            {",", Keyword.COMMA},
            {";", Keyword.SEMICOLON},
            {":", Keyword.COLON},
            {".", Keyword.POINT},
            {")", Keyword.RIGHTPAR},
            {"(", Keyword.LEFTPAR},
            {"]", Keyword.RBRACKET},
            {"[", Keyword.LBRACKET},
            {"<", Keyword.LATER},
            {">", Keyword.GREATER},
            {"<=", Keyword.LATEREQUAL},
            {">=", Keyword.GREATEREQUAL},
            {"<>", Keyword.LATERGREATER},
            {":=" , Keyword.ASSIGN},
            {"+", Keyword.PLUS},
            {"-",Keyword.MINUS},

            {"program", Keyword.PROGRAM},
            {"var", Keyword.VAR},
            {"begin", Keyword.BEGIN},
            {"end", Keyword.END},


            {"integer", Keyword.INTC},
            {"real", Keyword.FLOATC},
            {"char", Keyword.CHARC},
            {"boolean", Keyword.BOOLC},
            {"const", Keyword.CONSTC }
        };

        public static bool Check_If_KeyWord(string key)
        {
            return list_of_keywords.ContainsKey(key);
        }

        public static Keyword GetKeyWord(string key)
        {
            return list_of_keywords[key];
        }
    }
}
