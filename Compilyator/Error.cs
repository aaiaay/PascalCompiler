using System;

namespace Compilyator
{
    class Error
    {
        private TextPosition error = new TextPosition();
        private string reason;
        private string identifier;

        public string Identifier { get; }

        public Error(TextPosition position, string reason)
        {
            this.error = position;
            this.reason = reason;
        }

        public Error(TextPosition position, string reason, string identifier)
        {
            this.error = position;
            this.reason = reason;
            this.identifier = identifier;
        }

        public override string ToString()
        {
            return reason + ' ' + error;
        }
    }
}
