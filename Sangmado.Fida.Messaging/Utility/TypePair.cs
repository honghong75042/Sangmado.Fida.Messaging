using System;

namespace Sangmado.Fida.Messaging
{
    public class TypePair
    {
        public TypePair() { }

        public TypePair(Type request, Type response)
        {
            this.Request = request;
            this.Response = response;
        }

        public Type Request { get; set; }
        public Type Response { get; set; }
    }
}
