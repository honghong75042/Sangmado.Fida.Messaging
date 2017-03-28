using System.Collections.Generic;
using ProtoBuf;

namespace Sangmado.Fida.ServiceModel
{
    [ProtoContract]
    public class ActorDescriptionCollection
    {
        public ActorDescriptionCollection()
        {
            Items = new List<ActorDescription>();
        }

        [ProtoMember(10)]
        public List<ActorDescription> Items { get; set; }
    }
}
