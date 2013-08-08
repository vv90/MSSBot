using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokerEvalEQ;

namespace OHConnectivity
{
	//struct holdem_player
	//{
	//	char            m_name[16]          ;	//player name if known
	//	double          m_balance           ;	//player balance 
	//	double          m_currentbet        ;	//player current bet
	//	unsigned char   m_cards[2]          ;	//player cards
	//
	//	unsigned char   m_name_known    : 1 ;	//0=no 1=yes
	//	unsigned char   m_balance_known : 1 ;	//0=no 1=yes
	//	unsigned char   m_fillerbits    : 6 ;	//filler bits
	//	unsigned char   m_fillerbyte        ;	//filler bytes
	//};

	public class HoldemPlayer
	{
		public string Name { get; private set; }
		public double Balance { get; private set; }
		public double CurrentBet { get; private set; }
		public CardMask Hand { get; private set; }
		public bool NameKnown { get; private set; }
		public bool BalanceKnown { get; private set; }

		public HoldemPlayer(byte[] data, int offset = 0)
		{
			//	0
			Name = Encoding.ASCII.GetString(data, offset, 16);

			//	16
			Balance = BitConverter.ToDouble(data, offset + 16);

			//	24
			CurrentBet = BitConverter.ToDouble(data, offset + 24);

			//	32
			///////////////////////////////////// 
			//card macros 
			//#define RANK(c)			((c>>4)&0x0f) 
			//#define SUIT(c)			((c>>0)&0x0f) 
			//#define ISCARDBACK(c)		(c==0xff) 
			//#define ISUNKNOWN(c)		(c==0) 
			///////////////////////////////////// 
			byte[] cards = new byte[5];
			for (int i = 0; i < 5; i++)
			{
				cards[i] = data[144 + sizeof(byte) * i];
				int rank = ((cards[i] >> 4) & 0x0F);
				int suit = ((cards[i] >> 0) & 0x0F);

				//	In pokerEval lib suits clubs and diamonds are swaped
				//	So we need to swap those suites in ovder to use PokerEval.Mask()
				if (suit == 1)
					suit = 3;
				else if (suit == 3)
					suit = 1;

				int index = (rank - 2) + 13 * (suit - 1);
				Hand |= PokerEval.Mask(index);
			}

			//	34
			//unsigned char   m_name_known    : 1 ;	//0=no 1=yes
			//unsigned char   m_balance_known : 1 ;	//0=no 1=yes
			//unsigned char   m_fillerbits    : 6 ;	//filler bits
			byte info = data[offset + 34];
			NameKnown = (info & 0x1) != 0;
			BalanceKnown = (info & 0x2) != 0;
		}
	}
}
