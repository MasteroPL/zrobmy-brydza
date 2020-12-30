using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Models
{
    public class Contract
    {
        public ContractHeight ContractHeight { get; set; }
        public ContractColor ContractColor { get; set; }
        public bool XEnabled { get; set; }
        public bool XXEnabled { get; set; }

        public PlayerTag DeclaredBy;

        public Contract(ContractHeight ContractHeight, ContractColor ContractColor, bool XEnabled = false, bool XXEnabled = false)
        {
            this.ContractHeight = ContractHeight;
            this.ContractColor = ContractColor;
            this.XEnabled = XEnabled;
            this.XXEnabled = XXEnabled;
        }

        override
        public string ToString()
        {
            string contractHeight = (int)ContractHeight == -1 ? "" : ((int)ContractHeight).ToString();
            string contractColor = (int)ContractColor == -1 ? "" : ContractColor.ToString();
            string contractStr = contractHeight + contractColor;
            if (XXEnabled)
            {
                contractStr += "XX";
            } else if (XEnabled)
            {
                contractStr += "X";
            }
            return contractStr;
        }

        public bool Equals(Contract other)
        {
            if(other.ContractHeight.Equals(ContractHeight) 
                && other.ContractColor.Equals(ContractColor)
                && other.XEnabled == XEnabled 
                && other.XXEnabled == XXEnabled)
            {
                return true;
            }
            return false;
        }
    }

    public enum ContractColor
    {
        NONE = -1,
        C = 0,
        D = 1,
        H = 2,
        S = 3,
        NT = 4
    }

    public enum ContractHeight
    {
        NONE = -1,
        ONE = 1,
        TWO = 2,
        THREE = 3,
        FOUR = 4,
        FIVE = 5,
        SIX = 6,
        SEVEN = 7
    }
}
