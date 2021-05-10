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

        public LobbyUserData(string username)
        {
            this.Username = username;
        }
    }
}
