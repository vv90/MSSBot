using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVVMBase;
using BotLogic;

namespace MSSBot.ViewModels
{
	public class RangeStringInputViewModel : ViewModelBase
	{
		PreflopStrategy strategy;

		public List<RangeGroup> RangeGroups { get; private set; }

		public RangeStringInputViewModel(PreflopStrategy strategy)
		{
			this.strategy = strategy;

			RangeGroups = new List<RangeGroup>();

			foreach (string groupKey in strategy.HandRangeGroupKeys)
			{
				RangeGroup group = new RangeGroup() { Key = groupKey, Ranges = new List<Range>() };

				foreach (string rangeKey in strategy.HandRangeKeys)
					group.Ranges.Add(new Range() { Key = rangeKey, Value = string.Empty });

				RangeGroups.Add(group);
			}
		}

		public void SaveCommandExecute()
		{
			foreach (RangeGroup group in RangeGroups)
				foreach (Range range in group.Ranges)
					strategy.AssignRange(group.Key, range.Key, range.Value);
		}

		public class RangeGroup
		{
			public string Key { get; set; }
			public List<Range> Ranges { get; set; }
		}

		public class Range
		{
			public string Key { get; set; }
			public string Value { get; set; }
		}
	}
}
