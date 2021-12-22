using System.Collections.Generic;
using MLAPI.Serialization;

namespace Lobby.Scripts
{
    public struct LobbyPlayerState : INetworkSerializable
    {
        public ulong ClientId;
        public string PlayerName;
        public bool IsReady;
        public int AbilityCount;
        public string Ability1;
        public string Ability2;
        public int ChosenCharacter;

        public LobbyPlayerState(ulong clientId, string playerName, bool isReady, string ability1, string ability2, int abilityCount, int chosen)
        {
            ClientId = clientId;
            PlayerName = playerName;
            IsReady = isReady;
            Ability1 = ability1;
            Ability2 = ability2;
            AbilityCount = abilityCount;
            ChosenCharacter = chosen;
        }

        public void NetworkSerialize(NetworkSerializer serializer)
        {
            serializer.Serialize(ref ClientId);
            serializer.Serialize(ref PlayerName);
            serializer.Serialize(ref IsReady);
            serializer.Serialize(ref AbilityCount);
            serializer.Serialize(ref Ability1);
            serializer.Serialize(ref Ability2);
            serializer.Serialize(ref ChosenCharacter);
        }
    }
}
