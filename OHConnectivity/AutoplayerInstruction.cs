using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OHConnectivity
{
	public enum AutoplayerAction
	{
		Wait = 0,
		Fold = 1,
		Call = 2,
		Raise = 3,
		Allin = 4
	}

	public class AutoplayerInstruction
	{
		public AutoplayerAction Action;
		public int Play;
		public double Swag;

		public AutoplayerInstruction() : this(AutoplayerAction.Wait, -1, 0)
		{}

		public AutoplayerInstruction(AutoplayerAction action, int play, double swag)
		{
			Action = action;
			Play = play;
			Swag = swag;
		}

		public byte[] Pack()
		{
			byte[] result = new byte[16];

			Buffer.BlockCopy(BitConverter.GetBytes((int)Action), 0, result, 0, 4);
			Buffer.BlockCopy(BitConverter.GetBytes(Play), 0, result, 4, 4);
			Buffer.BlockCopy(BitConverter.GetBytes(Swag), 0, result, 8, 8);

			return result;
		}
	}
}
