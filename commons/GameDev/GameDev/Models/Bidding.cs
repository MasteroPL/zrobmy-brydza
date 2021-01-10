using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameManagerLib.Exceptions;

namespace GameManagerLib.Models
{
	class Bidding
	{
		public PlayerTag CurrentPlayer;
		public List<Contract> ContractList;
		public Contract HighestContract;
		public PlayerTag Dealer;
		private int PassCounter = 0;
		private bool End = false;
		public PlayerTag Declarer;

		private PlayerTag[] NS = new PlayerTag[5];
		private PlayerTag[] WE = new PlayerTag[5];

		public Bidding(PlayerTag Dealer)
		{
			this.Dealer = Dealer;
			this.ContractList = new List<Contract>();
			this.CurrentPlayer = NextPlayer(this.Dealer);
			this.HighestContract = null;
			for( int i = 0; i <5; i++)
            {
				NS[i] = (PlayerTag)(-1);
				WE[i] = (PlayerTag)(-1);
			}
		}

		public PlayerTag NextPlayer(PlayerTag CurrentPlayer)
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
			if (Contract.DeclaredBy != CurrentPlayer)
			{
                throw new WrongPlayerException();
			}

			if (X == true)
			{
				PlayerTag Declarer = HighestContract.DeclaredBy;
				if (IsTheSameTeam(Declarer, Contract.DeclaredBy) || HighestContract == null || (int)HighestContract.ContractColor == -1)
				{
					throw new WrongBidException();
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
                    throw new WrongBidException();
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
				if((int)Contract.ContractColor != -1)
                {
					SetColor(HighestContract.DeclaredBy, HighestContract.ContractColor);

				}
				this.ContractList.Add(Contract);
				return true;
			}

			if ((int)Contract.ContractColor == -1)
			{
				ContractList.Add(Contract);
				this.PassCounter++;
				if (this.PassCounter == 3)
				{	
					if((int)HighestContract.DeclaredBy == 0 || (int)HighestContract.DeclaredBy == 2)
					{
						this.Declarer = NS[(int)HighestContract.ContractColor];

					}
                    else
                    {
						this.Declarer = WE[(int)HighestContract.ContractColor];
					}
					
					this.End = true;
				}
				return true;
			}

			if (Contract.IsHigher(HighestContract))
			{
				this.HighestContract = Contract;
				this.ContractList.Add(Contract);
				this.PassCounter = 0;
				SetColor(HighestContract.DeclaredBy, HighestContract.ContractColor);
				return true;
			}

            throw new UnexpectedFunctionEndException();
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

		private void SetColor(PlayerTag PlayerTag, ContractColor Color)
        {	
			if((int)PlayerTag == 0 || (int)PlayerTag == 2)
            {
				if((int)NS[(int)Color] == -1)
                {
					NS[(int)Color] = PlayerTag;
                }
            }
			if ((int)PlayerTag == 1 || (int)PlayerTag == 3)
			{
				if ((int)WE[(int)Color] == -1)
				{
					WE[(int)Color] = PlayerTag;
				}
			}

        }

	}
}
