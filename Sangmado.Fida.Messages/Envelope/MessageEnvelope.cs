using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;
using ProtoBuf;

namespace Sangmado.Fida.Messages
{
    [ProtoContract(SkipConstructor = false, UseProtoMembersOnly = true)]
    [Serializable]
    [XmlType(TypeName = "Message")]
    public class MessageEnvelope
    {
        #region Constructors

        private static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public MessageEnvelope()
        {
            this.MessageID = Guid.NewGuid().ToString();
            this.CreatedTime = DateTime.UtcNow;
        }

        #endregion

        #region Header

        [ProtoMember(1)]
        public string MessageID { get; set; }
        [ProtoMember(2)]
        public string MessageType { get; set; }
        [ProtoMember(5, Name = @"CreatedTime")]
        public long SerializedCreatedTime { get; set; }
        [ProtoIgnore]
        public DateTime CreatedTime
        {
            get
            {
                return _unixEpoch.AddTicks(this.SerializedCreatedTime);
            }
            set
            {
                this.SerializedCreatedTime = value.Subtract(_unixEpoch).Ticks;
            }
        }

        [ProtoMember(10)]
        public string CorrelationID { get; set; }
        [ProtoMember(11, Name = @"CorrelationTime")]
        public long SerializedCorrelationTime { get; set; }
        [ProtoIgnore]
        public DateTime CorrelationTime
        {
            get
            {
                return _unixEpoch.AddTicks(this.SerializedCorrelationTime);
            }
            set
            {
                this.SerializedCorrelationTime = value.Subtract(_unixEpoch).Ticks;
            }
        }

        [ProtoMember(20)]
        public string SourceToken { get; set; }
        [ProtoMember(21)]
        public string PassToken { get; set; }

        [ProtoMember(30)]
        public Endpoint Source { get; set; }
        [ProtoMember(31)]
        public Endpoint Target { get; set; }

        [ProtoMember(40)]
        public string RoutingKey { get; set; }
        [ProtoMember(50, OverwriteList = false)]
        public List<ParamPair> Params { get; set; }

        [ProtoMember(70, OverwriteList = false)]
        public List<string> Recipients { get; set; }

        #endregion

        #region Payload

        [ProtoMember(80)]
        public byte[] Body { get; set; }

        #endregion

        #region Methods

        public MessageEnvelope<T> ConvertTo<T>()
        {
            var envelope = new MessageEnvelope<T>();
            envelope.CopyFrom(this);
            return envelope;
        }

        public static MessageEnvelope NewFrom<T>(MessageEnvelope<T> source)
        {
            var envelope = new MessageEnvelope();
            envelope.CopyFrom(source);
            return envelope;
        }

        public void CopyFrom<T>(MessageEnvelope<T> source)
        {
            this.MessageID = source.MessageID;
            this.CreatedTime = source.CreatedTime;

            this.MessageType = source.MessageType;
            this.CorrelationID = source.CorrelationID;
            this.CorrelationTime = source.CorrelationTime;

            this.SourceToken = source.SourceToken;
            this.PassToken = source.PassToken;

            this.Source = source.Source == null ? null : source.Source.Clone();
            this.Target = source.Target == null ? null : source.Target.Clone();

            this.RoutingKey = source.RoutingKey;
            this.Params = source.Params == null ? null : new List<ParamPair>(source.Params);

            this.Recipients = source.Recipients == null ? null : new List<string>(source.Recipients);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture,
                "MessageType[{0}], SourceToken[{1}], PassToken[{2}], "
                + "Source[{3}], Target[{4}], RoutingKey[{5}], BodySize[{6}], "
                + "MessageID[{7}], CorrelationID[{8}], CorrelationTime[{9}], CreatedTime[{10}]",
                this.MessageType,
                this.SourceToken,
                this.PassToken,
                this.Source,
                this.Target,
                this.RoutingKey,
                Body == null ? 0 : Body.Length,
                this.MessageID,
                this.CorrelationID,
                this.CorrelationTime.ToString(@"yyyy-MM-dd HH:mm:ss.fffffff"),
                this.CreatedTime.ToString(@"yyyy-MM-dd HH:mm:ss.fffffff"));
        }

        #endregion
    }

    [Serializable]
    [XmlType(TypeName = "Message")]
    public sealed class MessageEnvelope<T>
    {
        #region Constructors

        private static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public MessageEnvelope()
        {
            this.MessageID = Guid.NewGuid().ToString();
            this.CreatedTime = DateTime.UtcNow;
            this.MessageType = typeof(T).Name;
        }

        #endregion

        #region Header

        public string MessageID { get; set; }
        public string MessageType { get; set; }
        public DateTime CreatedTime { get; set; }

        public string CorrelationID { get; set; }
        public DateTime CorrelationTime { get; set; }

        public string SourceToken { get; set; }
        public string PassToken { get; set; }

        public Endpoint Source { get; set; }
        public Endpoint Target { get; set; }

        public string RoutingKey { get; set; }
        public List<ParamPair> Params { get; set; }

        public List<string> Recipients { get; set; }

        #endregion

        #region Payload

        [XmlIgnore]
        public T Message { get; set; }

        #endregion

        #region Methods

        public MessageEnvelope ConvertToNonGeneric()
        {
            var envelope = new MessageEnvelope();
            envelope.CopyFrom(this);
            return envelope;
        }

        public static MessageEnvelope<T> NewFrom(MessageEnvelope source)
        {
            var envelope = new MessageEnvelope<T>();
            envelope.CopyFrom(source);
            return envelope;
        }

        public void CopyFrom(MessageEnvelope source)
        {
            this.MessageID = source.MessageID;
            this.CreatedTime = source.CreatedTime;

            this.MessageType = source.MessageType;
            this.CorrelationID = source.CorrelationID;
            this.CorrelationTime = source.CorrelationTime;

            this.SourceToken = source.SourceToken;
            this.PassToken = source.PassToken;

            this.Source = source.Source == null ? null : source.Source.Clone();
            this.Target = source.Target == null ? null : source.Target.Clone();

            this.RoutingKey = source.RoutingKey;
            this.Params = source.Params == null ? null : new List<ParamPair>(source.Params);

            this.Recipients = source.Recipients == null ? null : new List<string>(source.Recipients);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture,
                "MessageType[{0}], SourceToken[{1}], PassToken[{2}], "
                + "Source[{3}], Target[{4}], RoutingKey[{5}], "
                + "MessageID[{6}], CorrelationID[{7}], CorrelationTime[{8}], CreatedTime[{9}]",
                this.MessageType,
                this.SourceToken,
                this.PassToken,
                this.Source,
                this.Target,
                this.RoutingKey,
                this.MessageID,
                this.CorrelationID,
                this.CorrelationTime.ToString(@"yyyy-MM-dd HH:mm:ss.fffffff"),
                this.CreatedTime.ToString(@"yyyy-MM-dd HH:mm:ss.fffffff"));
        }

        #endregion
    }
}
