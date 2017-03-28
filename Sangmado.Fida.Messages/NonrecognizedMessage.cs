using ProtoBuf;

namespace Sangmado.Fida.Messages
{
    [ProtoContract(UseProtoMembersOnly = true)]
    public class NonrecognizedMessage
    {
        public NonrecognizedMessage() 
        { 
        }

        [ProtoMember(1)]
        public string MessageType { get; set; }
    }
}
