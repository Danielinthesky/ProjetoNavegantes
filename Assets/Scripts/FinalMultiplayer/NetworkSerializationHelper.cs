using Unity.Netcode;
using UnityEngine;

[GenerateSerializationForType(typeof(string))]
public class DummyStringSerializer : NetworkBehaviour
{
    // Essa classe existe apenas para forçar o Netcode a gerar a serialização para System.String.
    // Você NÃO precisa usar nenhum código aqui.
}
