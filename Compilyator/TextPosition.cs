namespace Compilyator
{
    class TextPosition
    {
        public int linenumber { get; set; }
        public int charnumber { get; set; }

        public TextPosition(int ch = 0, int line = 1)
        {
            linenumber = line;
            charnumber = ch;
        }
        public TextPosition(TextPosition position)
        {
            linenumber = position.linenumber;
            charnumber = position.charnumber;
        }
        public override string ToString()
        {
            return "(" + linenumber + ", " + charnumber + ")";
        }
    }
}
