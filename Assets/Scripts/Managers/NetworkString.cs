using Unity.Netcode;
using Unity.Collections;

[System.Serializable]
public struct NetworkString : INetworkSerializable
{
    public string Value;

    public NetworkString(string value)
    {
        Value = value;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        // Serializa a string utilizando o método SerializeValue
        serializer.SerializeValue(ref Value);
    }

    // Operadores implícitos para facilitar o uso
    public static implicit operator string(NetworkString ns) => ns.Value;
    public static implicit operator NetworkString(string s) => new NetworkString(s);
}
