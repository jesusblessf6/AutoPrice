using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using DataModel;

namespace AutoPrice.Crawlers.Base
{
	public class CrawlerManager
	{
		public static List<CrawlTask> CrawlTasks { get; set; }
		private DispatcherTimer _timer;
		private bool _isTimerRunning;
		public bool IsTimerRunning{
			get { return _isTimerRunning; }
		}

		static CrawlerManager ()
		{
			CrawlTasks = new List<CrawlTask>();
		}

		public void Begin()
		{
			if (_timer == null)
			{
				_timer = new DispatcherTimer();
			}

			if (!_isTimerRunning)
			{
				_timer.Tick += RunPlanedCrawler;
				_timer.Interval = TimeSpan.FromSeconds(15);
				_timer.Start();
				_isTimerRunning = true;
			}
			
		}

		public void End()
		{
			if (_timer != null && _isTimerRunning)
			{
				_timer.Stop();
				_isTimerRunning = false;
			}
		}

		private void RunPlanedCrawler(object sender, EventArgs e)
		{
			//
			try
			{
				using (var ctx = new AutoNewsEntities())
				{
					//get the markets to run crawlers according to the start time
					var nowTime = TimeSpan.Parse(DateTime.Now.ToShortTimeString());
					var crawlers2Run = ctx.Markets.Where(o => o.Auto && o.StartTime <= nowTime && (o.EndTime == null || o.EndTime > nowTime)).ToList();

					//if the market is not in the list, then put it into the list;
					foreach (var market in crawlers2Run)
					{
						if (CrawlTasks.All(o => o.Market.Id != market.Id))
						{
							CrawlTasks.Add(new CrawlTask
							{
								Market = market,
								Status = CrawlTaskStatus.Pending,
								IsContinueing = true,
								TheTask = null
							});
						}
					}

					//Handle the finish and aborted: if now is within end time, then set it to pending
					var finishedTasks = CrawlTasks.Where(o => o.Status == CrawlTaskStatus.Finished || o.Status == CrawlTaskStatus.Aborted);
					foreach (var finishedTask in finishedTasks)
					{
						if ((finishedTask.Market.EndTime == null || finishedTask.Market.EndTime.Value > nowTime) && finishedTask.Market.StartTime <= nowTime)
						{
							finishedTask.Status = CrawlTaskStatus.Pending;
						}
					}

					//run the pending tasks
					var markets2Crawl = CrawlTasks.Where(o => o.Status == CrawlTaskStatus.Pending).ToList();
					foreach (var crawlTask in markets2Crawl)
					{
						CrawlTask ctask = crawlTask;
						ctask.Status = CrawlTaskStatus.Running;
						var t = new Task(() => RunMarketCrawler(ctask));
						ctask.TheTask = t;
						t.Start();
					}

					//remove the timeout task
					var timeoutTasksCount = CrawlTasks.RemoveAll(o => ((o.Market.EndTime != null && o.Market.EndTime < nowTime) || (o.Market.StartTime != null && o.Market.StartTime > nowTime)) && o.Status != CrawlTaskStatus.Pending && o.Status != CrawlTaskStatus.Running);

					//todo ? : restart the failed task the push back the end time by 5 minutes

				}
			}
			catch (Exception)
			{
			}
			
		}

		private void RunMarketCrawler(CrawlTask ctask)
		{
			try
			{
				var crawlers = CrawlerFactory.GetInstance().GetCrawlers(ctask.Market.Id);
				foreach (var crawler in crawlers)
				{
					crawler.Run();
				}
				ctask.Status = CrawlTaskStatus.Finished;
			}
			catch (Exception)
			{
				ctask.Status = CrawlTaskStatus.Aborted;
			}
			
		}
	}

	public class CrawlTask : DispatcherObject
	{
		// the market must include the crawler
		public Market Market { get; set; }

		public CrawlTaskStatus Status { get; set; }

		public Task TheTask { get; set; }

		public bool IsContinueing { get; set; }
	}

	public enum CrawlTaskStatus
	{
		Pending,
		Running,
		Aborted, //there is error during executing
		Failed,  //not the finish the task; Currently not to handle.
		Finished, //finish crawling for one time
		Succeeded
	}
}
