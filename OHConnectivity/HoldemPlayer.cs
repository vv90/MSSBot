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

		}
	}
}
