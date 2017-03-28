
namespace Sangmado.Fida.Messaging
{
    public class Subscription
    {
        public Subscription()
        {
        }

        public Subscription(string subscriber, string resource)
            : this()
        {
            this.Subscriber = subscriber;
            this.Resource = resource;
        }

        public Subscription(string subscriber, string resource, string owner)
            : this(subscriber, resource)
        {
            this.Owner = owner;
        }

        public string Subscriber { get; set; }
        public string Resource { get; set; }
        public string Owner { get; set; }
        public int Count { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == (object)this)
                return true;

            if (obj == null)
                return false;

            Subscription other = obj as Subscription;
            if (other == null)
                return false;

            return this.Subscriber == other.Subscriber
                && this.Resource == other.Resource
                && this.Owner == other.Owner;
        }

        public override int GetHashCode()
        {
            return string.Format("{0}#{1}#{2}", Subscriber, Resource, Owner).GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Subscriber[{0}], Resource[{1}], Owner[{2}], Count[{3}]", Subscriber, Resource, Owner, Count);
        }
    }
}
