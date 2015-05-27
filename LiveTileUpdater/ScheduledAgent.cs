using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using ResourceLibrary;

namespace LiveTileUpdater
{
	public class ScheduledAgent : ScheduledTaskAgent
	{
		/// <remarks>
		/// ScheduledAgent constructor, initializes the UnhandledException handler
		/// </remarks>
		static ScheduledAgent()
		{
			// Subscribe to the managed exception handler
			Deployment.Current.Dispatcher.BeginInvoke(delegate
			{
				Application.Current.UnhandledException += UnhandledException;
			});
		}

		/// Code to execute on Unhandled Exceptions
		private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
		{
			if (Debugger.IsAttached)
			{
				// An unhandled exception has occurred; break into the debugger
				Debugger.Break();
			}
		}

		/// <summary>
		/// Agent that runs a scheduled task
		/// </summary>
		/// <param name="task">
		/// The invoked task
		/// </param>
		/// <remarks>
		/// This method is called when a periodic or resource intensive task is invoked
		/// </remarks>
		protected override void OnInvoke(ScheduledTask task)
		{
			var tile = (from t in ShellTile.ActiveTiles select t).FirstOrDefault();
			var common = new Common();
			var startHour = new DateTime(2014, 05, 13, 08, 50, 00).Hour;
			var startMinute = new DateTime(2014, 05, 13, 08, 50, 00).Minute;
			var currentHour = DateTime.Now.Hour;
			var currentMinute = DateTime.Now.Minute;
			var callReport = (currentHour == startHour && currentMinute >= startMinute) || currentHour >= startHour;
			if (callReport)
			{
				var isUpdated = common.IsUpdated();

				if (tile != null && isUpdated.Result)
				{
					var newTile = new IconicTileData
						{
							Count = 1
						};
					tile.Update(newTile);
				}
			}

			// Call NotifyComplete to let the system know the agent is done working.
			NotifyComplete();
		}
	}
}