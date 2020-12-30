using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Models
{
    public class Player
    {
        public PlayerTag Tag { get; set; }
        public string Name { get; set; }

        public Player(PlayerTag Tag, string Name)
        {
            this.Tag = Tag;
            this.Name = Name;
        }
        
        override
        public string ToString()
        {
            return Tag.ToString() +" "+ Name;
        }
    }
}
