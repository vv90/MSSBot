using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokerEvalEQ;

namespace OHConnectivity
{
	//struct holdem_state
	//{
	//	char            m_title[64]         ;	//table title
	//	double          m_pot[10]           ;	//total in each pot
	//
	//	unsigned char   m_cards[5]          ;	//common cards
	//
	//	unsigned char   m_is_playing    : 1 ;	//0=sitting-out, 1=sitting-in
	//	unsigned char   m_is_posting    : 1 ;	//0=autopost-off, 1=autopost-on
	//	unsigned char   m_fillerbits    : 6 ;	//filler bits
	//
	//	unsigned char   m_fillerbyte        ;	//filler byte
	//	unsigned char   m_dealer_chair      ;	//0-9
	//
	//	holdem_player   m_player[10]        ;	//player records
	//};

	public class HoldemState
	{
		public string TableTitle { get; private set; }
		public double[] Pots { get; private set; }
		public CardMask CommonCards { get; private set; }
		public bool IsPlaing { get; private set; }
		public bool IsPosting { get; private set; }
		public int DealerChair { get; private set; }
		public HoldemPlayer[] Players { get; private set; }

		public double Pot
		{
			get { return Pots.Sum(); }
		}

		public HoldemState(byte[] data)
		{
			//	0
			TableTitle = Encoding.ASCII.GetString(data, 0, 64).TrimEnd('\0');

			//	64
			Pots = new double[10];
			for (int i = 0; i < 10; i++)
				Pots[i] = BitConverter.ToDouble(data, 64 + sizeof(double) * i);

			//	144
			///////////////////////////////////// 
			//card macros 
			//#define RANK(c)			((c>>4)&0x0f) 
			//#define SUIT(c)			((c>>0)&0x0f) 
			//#define ISCARDBACK(c)		(c==0xff) 
			//#define ISUNKNOWN(c)		(c==0) 
			///////////////////////////////////// 
			byte[] commonCards = new byte[5];
			for (int i = 0; i < 5; i++)
			{
				commonCards[i] = data[144 + sizeof(byte) * i];
				int rank = ((commonCards[i] >> 4) & 0x0F);
				int suit = ((commonCards[i] >> 0) & 0x0F);

				//	In pokerEval lib suits clubs and diamonds are swaped
				//	So we need to swap those suites in ovder to use PokerEval.Mask()
				if (suit == 1)
					suit = 3;
				else if (suit == 3)
					suit = 1;

				int index = (rank - 2) + 13 * (suit - 1);
				CommonCards |= PokerEval.Mask(index);
			}

			//	149
			//	unsigned char   m_is_playing    : 1 ;	//0=sitting-out, 1=sitting-in
			//	unsigned char   m_is_posting    : 1 ;	//0=autopost-off, 1=autopost-on
			//	unsigned char   m_fillerbits    : 6 ;	//filler bits
			byte info = data[149];
			IsPlaing = (info & 0x1) != 0;
			IsPosting = (info & 0x2) != 0;

			//	151
			DealerChair = BitConverter.ToInt32(data, 151);

			//	152
			//	each holdem_player struct is 40 bytes long
			Players = new HoldemPlayer[10];
			for (int i = 0; i < 10; i++)
				Players[i] = new HoldemPlayer(data, 152 + 40 * i);
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(string.Format("\"{0}\" ", TableTitle));
			sb.Append(string.Format("DC:{0} ", DealerChair));
			sb.Append("[");
			foreach (double pot in Pots)
				sb.Append(string.Format("{0} ", pot));
			sb.Append(string.Format("] [{0}] {1} {2}", 
									CommonCards, 
									IsPlaing ? '*' : '_',
									IsPosting ? '*' : '_'));
			return sb.ToString();
		}
	}
}
