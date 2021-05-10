using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameManagerLib.Models;

namespace Assets.Code.Models
{
    public class LobbyUserData
    {
        public string Username;
        public bool IsSitted;
        public PlayerTag PlayerTag;

        public LobbyUserData(string Username, bool IsSitted, PlayerTag Position = PlayerTag.NOBODY)
        {
            this.Username = Username;
            this.IsSitted = IsSitted;
            PlayerTag = Position;
        }
    }
}
