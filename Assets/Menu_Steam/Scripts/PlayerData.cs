using System.Collections.Generic;

namespace Menu_Steam.Scripts
{
    public struct PlayerData
    {
        public string PlayerName { get; private set; }
        public ulong ClientId { get; private set; }
        
        public int ChosenCharacter { get; private set; }
        
        public List<string> Abilities { get; private set; }

        public PlayerData(string playerName, ulong clientId, int chosen, List<string> abilities)
        {
            PlayerName = playerName;
            ClientId = clientId;
            Abilities = new List<string>();
            ChosenCharacter = chosen;
            Abilities = abilities;
        }
    }
}
