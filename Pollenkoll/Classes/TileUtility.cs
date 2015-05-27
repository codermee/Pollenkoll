using System;
using System.IO.IsolatedStorage;
using System.Windows;
using Microsoft.Phone.Scheduler;
using ResourceLibrary;

namespace Pollenkoll.Classes
{
	public class TileUtility
	{

		#region Singleton

		private static TileUtility instance;
		public static TileUtility Instance
		{
			get { return instance ?? (instance = new TileUtility()); }
		}

		#endregion

		#region Constructor

		private TileUtility() { }

		#endregion

		#region Public methods

		public bool GetLiveTileSetting()
		{
			bool isEnabled;
			var settings = IsolatedStorageSettings.ApplicationSettings;

			if (settings != null && settings.Contains(Globals.LiveTileSettingKey))
			{
				isEnabled = (bool)settings[Globals.LiveTileSettingKey];
			}
			else
			{
				isEnabled = true;
			}

			return isEnabled;
		}

		public void CreatePeriodicTask(string name)
		{
			var task = new PeriodicTask(name)
				{
					Description = "Live tile updater for Pollenkoll",
					ExpirationTime = DateTime.Now.AddDays(14)
				};

			// If the agent is already registered, remove it and then add it again (further down)
			RemovePeriodicTask(task.Name);

			// Not supported in current version
			//task.BeginTime = DateTime.Now.AddSeconds(10);

			try
			{
				// Can only be called when application is running in foreground
				ScheduledActionService.Add(task);
			}
			catch (InvalidOperationException exception)
			{
				if (exception.Message.Contains("BNS Error: The action is disabled"))
				{
					MessageBox.Show("Bakgrundsagenter har blivit inaktiverat av användaren. Gå till Inställningar i telefonen.");
				}
				if (exception.Message.Contains("BNS Error: The maximum number of ScheduledActions of this type have already been added."))
				{
					// No user action required.
					// The system prompts the user when the hard limit of periodic tasks has been reached.
				}
			}

			#if DEBUG
			ScheduledActionService.LaunchForTest(task.Name, TimeSpan.FromSeconds(60));
			#endif
		}

		public void RemovePeriodicTask(string name)
		{
			if (ScheduledActionService.Find(name) != null)
			{
				ScheduledActionService.Remove(name);
			}
		}

		#endregion

	}
}