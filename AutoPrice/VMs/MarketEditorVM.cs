using System;
using System.Linq;
using AutoPrice.EntityHelper;
using DataModel;

namespace AutoPrice.VMs
{
	public class MarketEditorVM : BaseVM
	{
		public int MarketId { get; set; }

		public string Title { get; set; }

		public string Name
		{
			get { return _name; }
			set
			{
				if (_name != value)
				{
					_name = value;
					Notify("Name");
				}
			}
		}
		private string _name;

		public int CrmId
		{
			get { return _crmId; }
			set
			{
				if (_crmId != value)
				{
					_crmId = value;
					Notify("CrmId");
				}
			}
		}
		private int _crmId;

		public bool Auto
		{
			get { return _auto; }
			set
			{
				if (_auto != value)
				{
					_auto = value;
					Notify("Auto");
				}
			}
		}
		private bool _auto;

		public TimeSpan? StartTime
		{
			get { return _startTime; }
			set
			{
				if (_startTime != value)
				{
					_startTime = value;
					Notify("StartTime");
				}
			}
		}
		private TimeSpan? _startTime;

		public TimeSpan? EndTime
		{
			get { return _endTime; }
			set
			{
				if (_endTime != value)
				{
					_endTime = value;
					Notify("EndTime");
				}
			}
		}
		private TimeSpan? _endTime;

		public int MarketTypeId { get; set; }


		public MarketEditorVM(int marketTypeId, int marketId = 0)
		{
			MarketId = marketId;
			MarketTypeId = marketTypeId;
			var ctx = new AutoNewsEntities();
			var marketType = ctx.MarketTypes.Single(o => o.Id == marketTypeId);

			if (MarketId > 0)
			{
				Title = "编辑市场";
				
				var m = ctx.Markets.Single(o => o.Id == MarketId);
				_name = m.Name;
				_crmId = m.MarketCrmId;
				_auto = m.Auto;
				_startTime = m.StartTime;
				_endTime = m.EndTime;
			}
			else
			{
				Title = "新增市场";
				_auto = false;
			}
			
			Title += "-" + marketType.Name;

		}

		public void Save()
		{
			Validate();

			var m = new Market
				{
					Id = MarketId,
					MarketTypeId = MarketTypeId,
					Auto = Auto,
					StartTime = StartTime,
					EndTime = EndTime,
					Name = Name,
					MarketCrmId = CrmId
				};
			
			MarketHelper.Save(m);
		}

		private void Validate()
		{
			if (string.IsNullOrWhiteSpace(Name))
			{
				throw new Exception(("市场名称不能为空"));
			}

			if (Auto && StartTime == null)
			{
				throw new Exception(("自动模式下开始时间不能为空"));
			}

			if (Auto && EndTime != null && EndTime <= StartTime)
			{
				throw new Exception(("结束时间必须晚于开始时间"));
			}
		}
	}
}
