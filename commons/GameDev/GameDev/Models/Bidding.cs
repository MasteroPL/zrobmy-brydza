using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	class Bidding
	{
		public PlayerTag CurrentPlayer;
		public List<Contract> ContractList;
		public Contract HighestContract;
		public PlayerTag Dealer;
		private int PassCounter = 0;
		private bool End = false;
		public Bidding(PlayerTag Dealer)
		{
			this.Dealer = Dealer;
			this.ContractList = new List<Contract>();
			this.CurrentPlayer = NextPlayer(this.Dealer);
			this.HighestContract = null;
		}

		private PlayerTag NextPlayer(PlayerTag CurrentPlayer)
		{
			int ID = (int)(CurrentPlayer);
			if (ID == 3)
			{
				return (PlayerTag)(0);
			}
			else
			{
				return (PlayerTag)(ID + 1);
			}
		}

		public bool AddBid(Contract Contract, bool X = false, bool XX = false)
		{

			if (X == true)
			{
				PlayerTag Declarer = HighestContract.DeclaredBy;
				if (IsTheSameTeam(Declarer, Contract.DeclaredBy) || HighestContract == null || (int)HighestContract.ContractColor == -1)
				{
					return false;
				}
				else
				{
					HighestContract.XEnabled = true;
					ContractList.Add(Contract);
					this.PassCounter = 0;
					return true;
				}
			}

			if (XX == true)
			{
				PlayerTag Declarer = HighestContract.DeclaredBy;
				if (IsTheSameTeam(Declarer, Contract.DeclaredBy) == false || HighestContract == null || (int)HighestContract.ContractColor == -1 || HighestContract.XEnabled == false)
				{
					return false;
				}
				else
				{
					HighestContract.XXEnabled = true;
					ContractList.Add(Contract);
					this.PassCounter = 0;
					return true;
				}
			}

			if (HighestContract == null)
			{
				this.HighestContract = Contract;
				this.ContractList.Add(Contract);
				return true;
			}

			if ((int)Contract.ContractColor == -1)
			{
				ContractList.Add(Contract);
				this.PassCounter++;
				if (this.PassCounter == 3)
				{
					this.End = true;
				}
				return true;
			}

			if (Contract.IsHigher(HighestContract))
			{
				this.HighestContract = Contract;
				this.ContractList.Add(Contract);
				this.PassCounter = 0;
				return true;
			}

			return false;
		}

		public bool IsEnd()
		{
			return this.End;
		}

		private bool IsTheSameTeam(PlayerTag Player1, PlayerTag Player2)
		{
			if (Player1 == Player2)
			{
				return true;
			}
			else
			{
				Player1 = NextPlayer(NextPlayer(Player1));
				if (Player1 == Player2)
				{
					return true;
				}
				else
				{
					return false;
				}
			}

		}

	}
}
