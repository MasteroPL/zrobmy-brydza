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
        public PlayerTag position = PlayerTag.NOBODY;
        public PlayerTag positionStart = PlayerTag.NOBODY; // non-changing position (just for development)
        public bool IsAdmin = true;
        public bool Sitting = false;
    }
}
