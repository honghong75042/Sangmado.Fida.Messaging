using ProtoBuf;

namespace Sangmado.Fida.ServiceModel
{
    [ProtoContract(SkipConstructor = false, UseProtoMembersOnly = true)]
    public class ActorLookupRequest
    {
        public ActorLookupRequest()
        {
        }
    }
}
