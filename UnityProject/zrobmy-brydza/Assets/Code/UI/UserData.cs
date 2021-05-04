using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameManagerLib.Models;

namespace Assets.Code.UI
{
    public class UserData
    {
        public static PlayerTag position = PlayerTag.NOBODY;
        public static PlayerTag positionStart = PlayerTag.NOBODY; // non-changing position (just for development)
        public static bool IsAdmin = false;
        public static bool Sitting = false;
        public static bool LoggedIn = false;
        public static ServerSocket.Actions.GetTableInfo.ResponseSerializer TableData = null;
    }
}
