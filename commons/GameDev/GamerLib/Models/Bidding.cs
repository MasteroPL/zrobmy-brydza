using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameManagerLib.Exceptions;

namespace GameManagerLib.Models
{
	public class Bidding
	{
		public PlayerTag CurrentPlayer;
		public List<Contract> ContractList;
		public Contract HighestContract;
		public PlayerTag Dealer;
		public int PassCounter = 0;
		public bool End = false;
		public PlayerTag Declarer;

		// Tablice mówiące, kto jaki kolor jako pierwszy deklarował
		// 0 - C
		// 1 - D
		// 2 - H
		// 3 - S
		// 4 - NT
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
				if (IsTheSameTeam(Declarer, Contract.DeclaredBy) || HighestContract == null || HighestContract.ContractColor == ContractColor.NONE)
				{
					throw new WrongBidException();
				}
				else
				{
					HighestContract = new Contract(HighestContract.ContractHeight, HighestContract.ContractColor, HighestContract.DeclaredBy, true, false);
					ContractList.Add(Contract);
					this.PassCounter = 0;
					return true;
				}
			}

			if (XX == true)
			{
				PlayerTag Declarer = HighestContract.DeclaredBy;
				if (IsTheSameTeam(Declarer, Contract.DeclaredBy) == false || HighestContract == null || HighestContract.ContractColor == ContractColor.NONE || HighestContract.XEnabled == false)
				{
                    throw new WrongBidException();
				}
				else
				{
					HighestContract = new Contract(HighestContract.ContractHeight, HighestContract.ContractColor, HighestContract.DeclaredBy, false, true);
					ContractList.Add(Contract);
					this.PassCounter = 0;
					return true;
				}
			}

			if (HighestContract == null)
			{
				this.HighestContract = Contract;
				if(Contract.ContractColor != ContractColor.NONE)
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
				{	if (HighestContract.ContractColor != (ContractColor)(-1))
					{
						if ((int)HighestContract.DeclaredBy == 0 || (int)HighestContract.DeclaredBy == 2)
						{
							this.Declarer = NS[(int)HighestContract.ContractColor];

						}
						else
						{
							this.Declarer = WE[(int)HighestContract.ContractColor];
						}

						this.End = true;
					}
					else
                    {
						this.End = true;
					}
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

		public bool CheckAddBid(Contract Contract, bool X = false, bool XX = false)
		{
			if (Contract.DeclaredBy != CurrentPlayer)
			{
				return false;
			}

			if (X == true)
			{
				PlayerTag Declarer = HighestContract.DeclaredBy;
				if (IsTheSameTeam(Declarer, Contract.DeclaredBy) || HighestContract == null || HighestContract.ContractColor == ContractColor.NONE)
				{
					return false;
				}
				else
				{
					return true;
				}
			}

			if (XX == true)
			{
				PlayerTag Declarer = HighestContract.DeclaredBy;
				if (IsTheSameTeam(Declarer, Contract.DeclaredBy) == false || HighestContract == null || HighestContract.ContractColor == ContractColor.NONE || HighestContract.XEnabled == false)
				{
					return false;
				}
				else
				{
					return true;
				}
			}

			if (HighestContract == null)
			{
				return true;
			}

			if ((int)Contract.ContractColor == -1)
			{
				return true;
			}

			if (Contract.IsHigher(HighestContract))
			{
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

		private void SetColor(PlayerTag PlayerTag, ContractColor Color)
        {	
			if(PlayerTag == PlayerTag.N || PlayerTag == PlayerTag.S)
            {
				if((int)NS[(int)Color] == -1)
                {
					NS[(int)Color] = PlayerTag;
                }
            }
			if (PlayerTag == PlayerTag.E || PlayerTag == PlayerTag.W)
			{
				if ((int)WE[(int)Color] == -1)
				{
					WE[(int)Color] = PlayerTag;
				}
			}

        }

	}
}
