using System;
using System.Reflection;
using Sangmado.Fida.Messages;
using Sangmado.Fida.ServiceModel;
using Sangmado.Inka.Logging;

namespace Sangmado.Fida.Messaging
{
    public static class MessageEnvelopeExtensions
    {
        private static ILog _log = Logger.Get<MessageEnvelope>();

        public static MessageEnvelope<T> Instantiate<T>(this MessageEnvelope envelope, IMessageDecoder decoder)
        {
            var message = envelope.ConvertTo<T>();
            message.Message = decoder.DecodeMessage<T>(envelope.Body);
            return message;
        }

        public static MessageEnvelope Marshal<T>(this MessageEnvelope<T> envelope, IMessageEncoder encoder)
        {
            var message = envelope.ConvertToNonGeneric();
            message.Body = encoder.EncodeMessage(envelope.Message);
            return message;
        }

        public static byte[] ToBytes(this MessageEnvelope envelope, IMessageEncoder encoder)
        {
            return encoder.EncodeMessage(envelope);
        }

        public static byte[] ToBytes<T>(this MessageEnvelope<T> envelope, IMessageEncoder encoder)
        {
            return ToBytes(envelope.Marshal(encoder), encoder);
        }

        public static void HandledBy(this MessageEnvelope envelope, object handlerFrom, Type messageType, IMessageDecoder decoder)
        {
            HandledBy(envelope, handlerFrom, messageType, decoder, (string s) => { return @"On" + s; });
        }

        public static void HandledBy(this MessageEnvelope envelope, object handlerFrom, Type messageType, IMessageDecoder decoder, Func<string, string> getHandlerName)
        {
            HandledBy(envelope, handlerFrom, messageType, decoder,
                (object o) =>
                {
                    return o.GetType().GetMethod(getHandlerName(envelope.MessageType), BindingFlags.NonPublic | BindingFlags.Instance);
                });
        }

        public static void HandledBy(this MessageEnvelope envelope, object handlerFrom, Type messageType, IMessageDecoder decoder, Func<object, MethodInfo> getHandlerMethod)
        {
            var instantiateMethod = typeof(MessageEnvelopeExtensions)
                .GetMethod("Instantiate", new Type[] { typeof(MessageEnvelope), typeof(IMessageDecoder) })
                .MakeGenericMethod(messageType);
            var instantiatedEnvelope = instantiateMethod.Invoke(null, new object[] { envelope, decoder });

            try
            {
                var messageHandlerMethod = getHandlerMethod(handlerFrom);
                messageHandlerMethod.Invoke(handlerFrom, new object[] { instantiatedEnvelope });
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("HandledBy, MessageType[{0}], ErrorMessage[{1}].", messageType.Name, ex);
                throw;
            }
        }

        public static void HandledBy<T>(this MessageEnvelope envelope, object handlerFrom, Type messageType, IMessageDecoder decoder, T client) where T : class
        {
            HandledBy<T>(envelope, handlerFrom, messageType, decoder, client, (string s) => { return @"On" + s; });
        }

        public static void HandledBy<T>(this MessageEnvelope envelope, object handlerFrom, Type messageType, IMessageDecoder decoder, T client, Func<string, string> getHandlerName) where T : class
        {
            HandledBy<T>(envelope, handlerFrom, messageType, decoder, client,
                (object o) =>
                {
                    return o.GetType().GetMethod(getHandlerName(envelope.MessageType), BindingFlags.NonPublic | BindingFlags.Instance);
                });
        }

        public static void HandledBy<T>(this MessageEnvelope envelope, object handlerFrom, Type messageType, IMessageDecoder decoder, T client, Func<object, MethodInfo> getHandlerMethod) where T : class
        {
            var instantiateMethod = typeof(MessageEnvelopeExtensions)
                .GetMethod("Instantiate", new Type[] { typeof(MessageEnvelope), typeof(IMessageDecoder) })
                .MakeGenericMethod(messageType);
            var instantiatedEnvelope = instantiateMethod.Invoke(null, new object[] { envelope, decoder });

            try
            {
                var messageHandlerMethod = getHandlerMethod(handlerFrom);
                messageHandlerMethod.Invoke(handlerFrom, new object[] { client, instantiatedEnvelope });
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("HandledBy, MessageType[{0}], ErrorMessage[{1}].", messageType.Name, ex);
                throw;
            }
        }

        public static MessageEnvelope<T> Instantiate<T>(this MessageEnvelope envelope, IActorMessageDecoder decoder)
        {
            var message = envelope.ConvertTo<T>();
            message.Message = decoder.DecodeMessage<T>(envelope.Body, 0, envelope.Body.Length);
            return message;
        }

        public static MessageEnvelope Marshal<T>(this MessageEnvelope<T> envelope, IActorMessageEncoder encoder)
        {
            var message = envelope.ConvertToNonGeneric();
            message.Body = encoder.EncodeMessage(envelope.Message);
            return message;
        }

        public static byte[] ToBytes(this MessageEnvelope envelope, IActorMessageEncoder encoder)
        {
            return encoder.Encode(envelope);
        }

        public static byte[] ToBytes<T>(this MessageEnvelope<T> envelope, IActorMessageEncoder encoder)
        {
            return ToBytes(envelope.Marshal(encoder), encoder);
        }

        public static void HandledBy(this MessageEnvelope envelope, object handlerFrom, Type messageType, IActorMessageDecoder decoder)
        {
            HandledBy(envelope, handlerFrom, messageType, decoder, (string s) => { return @"On" + s; });
        }

        public static void HandledBy(this MessageEnvelope envelope, object handlerFrom, Type messageType, IActorMessageDecoder decoder, Func<string, string> getHandlerName)
        {
            HandledBy(envelope, handlerFrom, messageType, decoder,
                (object o) =>
                {
                    return o.GetType().GetMethod(getHandlerName(envelope.MessageType), BindingFlags.NonPublic | BindingFlags.Instance);
                });
        }

        public static void HandledBy(this MessageEnvelope envelope, object handlerFrom, Type messageType, IActorMessageDecoder decoder, Func<object, MethodInfo> getHandlerMethod)
        {
            var instantiateMethod = typeof(MessageEnvelopeExtensions)
                .GetMethod("Instantiate", new Type[] { typeof(MessageEnvelope), typeof(IActorMessageDecoder) })
                .MakeGenericMethod(messageType);
            var instantiatedEnvelope = instantiateMethod.Invoke(null, new object[] { envelope, decoder });

            try
            {
                var messageHandlerMethod = getHandlerMethod(handlerFrom);
                messageHandlerMethod.Invoke(handlerFrom, new object[] { instantiatedEnvelope });
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("HandledBy, MessageType[{0}], ErrorMessage[{1}].", messageType.Name, ex);
                throw;
            }
        }

        public static void HandledBy<T>(this MessageEnvelope envelope, object handlerFrom, Type messageType, IActorMessageDecoder decoder, T state) where T : class
        {
            HandledBy<T>(envelope, handlerFrom, messageType, decoder, state, (string s) => { return @"On" + s; });
        }

        public static void HandledBy<T>(this MessageEnvelope envelope, object handlerFrom, Type messageType, IActorMessageDecoder decoder, T state, Func<string, string> getHandlerName) where T : class
        {
            HandledBy<T>(envelope, handlerFrom, messageType, decoder, state,
                (object o) =>
                {
                    return o.GetType().GetMethod(getHandlerName(envelope.MessageType), BindingFlags.NonPublic | BindingFlags.Instance);
                });
        }

        public static void HandledBy<T>(this MessageEnvelope envelope, object handlerFrom, Type messageType, IActorMessageDecoder decoder, T state, Func<object, MethodInfo> getHandlerMethod) where T : class
        {
            var instantiateMethod = typeof(MessageEnvelopeExtensions)
                .GetMethod("Instantiate", new Type[] { typeof(MessageEnvelope), typeof(IActorMessageDecoder) })
                .MakeGenericMethod(messageType);
            var instantiatedEnvelope = instantiateMethod.Invoke(null, new object[] { envelope, decoder });

            try
            {
                var messageHandlerMethod = getHandlerMethod(handlerFrom);
                messageHandlerMethod.Invoke(handlerFrom, new object[] { state, instantiatedEnvelope });
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("HandledBy, MessageType[{0}], ErrorMessage[{1}].", messageType.Name, ex);
                throw;
            }
        }
    }
}
