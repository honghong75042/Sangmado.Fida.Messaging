using ProtoBuf;

namespace Sangmado.Fida.Messages
{
    [ProtoContract(UseProtoMembersOnly = true)]
    public class UnauthorizedMessage
    {
        public UnauthorizedMessage() 
        { 
        }

        [ProtoMember(1)]
        public string MessageType { get; set; }

        [ProtoMember(10)]
        public string Reason { get; set; }
    }
}
