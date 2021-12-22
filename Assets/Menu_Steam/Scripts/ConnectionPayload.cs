using System;

namespace Menu_Steam.Scripts
{
    [Serializable]
    public class ConnectionPayload
    {
        public string clientGUID;
        public int clientScene = -1;
        public string playerName;
        public bool isAdmin;
    }
}
