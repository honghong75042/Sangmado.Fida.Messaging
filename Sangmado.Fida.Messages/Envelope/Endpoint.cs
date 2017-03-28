using ProtoBuf;

namespace Sangmado.Fida.Messages
{
    [ProtoContract(UseProtoMembersOnly = true)]
    public sealed class Endpoint
    {
        public const char Delimiter = '/';

        public Endpoint()
        {
        }

        public Endpoint(string address)
            : this()
        {
            this.Address = address;
        }

        [ProtoMember(1)]
        public string Address { get; set; }

        public Endpoint Clone()
        {
            return new Endpoint(this.Address);
        }

        public string TryAttachPath(string path)
        {
            return string.Format(@"{0}{1}{2}", path, Delimiter, this.Address);
        }

        public Endpoint AttachPath(string path)
        {
            this.Address = string.Format(@"{0}{1}{2}", path, Delimiter, this.Address);
            return this;
        }

        public string TryBackwardAttachPath(string path)
        {
            return string.Format(@"{0}{1}{2}{3}",
                this.Address.TrimEnd(Delimiter), Delimiter, path, Delimiter);
        }

        public Endpoint BackwardAttachPath(string path)
        {
            this.Address = string.Format(@"{0}{1}{2}{3}",
                this.Address.TrimEnd(Delimiter), Delimiter, path, Delimiter);
            return this;
        }

        public static string AttachPath(string path1, string path2)
        {
            return string.Format(@"{0}{1}{2}", path1, Delimiter, path2);
        }

        public string DetachPath()
        {
            if (!string.IsNullOrEmpty(this.Address))
            {
                var delimiterPosition = this.Address.IndexOf(Delimiter);
                if (delimiterPosition > 0)
                {
                    var path = this.Address.Substring(0, delimiterPosition);
                    this.Address = this.Address.Substring(delimiterPosition + 1);
                    return path;
                }
                else
                {
                    return this.Address;
                }
            }

            return string.Empty;
        }

        public string PeakPath()
        {
            if (!string.IsNullOrEmpty(this.Address))
            {
                var delimiterPosition = this.Address.IndexOf(Delimiter);
                if (delimiterPosition > 0)
                {
                    var path = this.Address.Substring(0, delimiterPosition);
                    return path;
                }
                else
                {
                    return this.Address;
                }
            }

            return string.Empty;
        }

        public bool StartsWith(string prefix)
        {
            if (!string.IsNullOrEmpty(this.Address))
            {
                if (this.Address.StartsWith(prefix))
                {
                    if (this.Address.Length == prefix.Length
                        || (this.Address.Length > prefix.Length
                            && this.Address[prefix.Length] == Delimiter))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override string ToString()
        {
            return string.Format("Address[{0}]", this.Address);
        }

        public static Endpoint CreateEndpoint()
        {
            return new Endpoint();
        }

        public static Endpoint CreateEndpoint(string address)
        {
            return new Endpoint(address);
        }
    }
}
