using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyHosting.Models.Client;
using GameManagerLib.Models;

namespace Assets.Code.UI
{
    public static class UserData
    {
        public static bool DataAssigned = false;
        public static PlayerTag Position = PlayerTag.NOBODY;
        public static PlayerTag PositionStart = PlayerTag.NOBODY; // non-changing position (just for development)
        public static bool IsAdmin = false;
        public static bool Sitting = false;
        public static bool LoggedIn = false;
        public static ServerSocket.Actions.GetTableInfo.ResponseSerializer TableData = null;
        public static string Username = null;
        public static ClientSocket ClientConnection = null;
    }
}
