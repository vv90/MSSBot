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
			TableTitle = Encoding.ASCII.GetString(data, 0, 64).TrimEnd('\0');

			Pots = new double[10];
			for (int i = 0; i < 10; i++)
				Pots[i] = BitConverter.ToDouble(data, 64 + sizeof(double) * i);



		}
	}
}
