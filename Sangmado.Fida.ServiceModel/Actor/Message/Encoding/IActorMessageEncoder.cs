namespace Sangmado.Fida.ServiceModel
{
    public interface IActorMessageEncoder
    {
        byte[] EncodeMessage<T>(T messageData);
        byte[] Encode<T>(T messageData);
    }
}
