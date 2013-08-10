using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokerEvalEQ;

namespace BotLogic
{
	public class PreflopStrategy
	{
		static PreflopStrategy instance;

		public readonly string[] HandRangeKeys;
		Dictionary<string, Dictionary<string, HandRange>> handRangeGroups;

		public static PreflopStrategy Instance
		{
			get
			{
				if (instance == null)
					instance = new PreflopStrategy("Raise", "3-Bet", "All In");

				return instance;
			}
		}

		private PreflopStrategy()
		{
			throw new NotImplementedException();
		}

		private	PreflopStrategy(params string[] handGroupKeys)
		{
			HandRangeKeys = new string[] { "UTG", "MP", "CO", "BU", "SB", "BB" };

			handRangeGroups = new Dictionary<string, Dictionary<string, HandRange>>();

			foreach (string param in handGroupKeys)
			{
				Dictionary<string, HandRange> ranges = new Dictionary<string,HandRange>();

				foreach (string key in HandRangeKeys)
					ranges.Add(key, new HandRange());

				handRangeGroups.Add(param, ranges);
			}
		}

		public IEnumerable<string> HandRangeGroupKeys
		{
			get { return handRangeGroups.Keys; }
		}

		public void AssignRange(string handRangeGroupKey, string handRangeKey, string rangeString)
		{
			handRangeGroups[handRangeGroupKey][handRangeKey] = HandRange.Parse(rangeString);
		}
	}
}
