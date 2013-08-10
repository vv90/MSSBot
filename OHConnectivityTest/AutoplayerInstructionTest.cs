using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OHConnectivity;

namespace OHConnectivityTest
{
	[TestClass]
	public class AutoplayerInstructionTest
	{
		[TestMethod]
		public void TestPack()
		{
			AutoplayerInstruction target = new AutoplayerInstruction(AutoplayerAction.Raise, 1, 1.4);
			byte[] packed;
			AutoplayerAction action;
			int play;
			double swag;

			packed = target.Pack();
			action = (AutoplayerAction)BitConverter.ToInt32(packed, 0);
			play = BitConverter.ToInt32(packed, 4);
			swag = BitConverter.ToDouble(packed, 8);

			Assert.AreEqual(target.Action, action, "Action");
			Assert.AreEqual(target.Play, play, "Play");
			Assert.AreEqual(target.Swag, swag, "Swag");
		}
	}
}
