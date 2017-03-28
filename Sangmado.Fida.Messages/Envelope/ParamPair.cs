using ProtoBuf;

namespace Sangmado.Fida.Messages
{
    [ProtoContract(UseProtoMembersOnly = true)]
    public class ParamPair
    {
        public ParamPair()
        {
        }

        public ParamPair(string key, string @value)
            : this()
        {
            Key = key;
            Value = @value;
        }

        [ProtoMember(1)]
        public string Key { get; set; }

        [ProtoMember(2)]
        public string Value { get; set; }
    }
}
