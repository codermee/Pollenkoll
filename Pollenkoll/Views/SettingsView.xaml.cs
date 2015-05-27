using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Navigation;
using ResourceLibrary;
using TileUtility = Pollenkoll.Classes.TileUtility;

namespace Pollenkoll.Views
{
	public partial class SettingsView
	{
		public SettingsView()
		{
			InitializeComponent();
		}

		#region Private methods

		private static void ToggleEnableLiveTileUpdates(bool enableLiveTileUpdates)
		{
			var settings = IsolatedStorageSettings.ApplicationSettings;
			if (settings.Contains(Globals.LiveTileSettingKey))
			{
				settings[Globals.LiveTileSettingKey] = enableLiveTileUpdates;
			}
			else
			{
				settings.Add(Globals.LiveTileSettingKey, enableLiveTileUpdates);
			}
		}

		#endregion

		#region Events

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			var isLowMemoryDevice = (bool)IsolatedStorageSettings.ApplicationSettings[Globals.IsLowMemoryDevice];
			if (isLowMemoryDevice)
			{
				EnableLiveTileUpdatesSwitch.IsEnabled = false;
				EnableLiveTileUpdatesSwitch.IsChecked = false;
			}
			else
			{
				EnableLiveTileUpdatesSwitch.IsChecked = TileUtility.Instance.GetLiveTileSetting();
			}
		}

		private void ToggleSwitch_Checked(object sender, RoutedEventArgs e)
		{
			ToggleEnableLiveTileUpdates(true);
			TileUtility.Instance.CreatePeriodicTask(Globals.LiveTileUpdaterPeriodicTaskName);
		}

		private void ToggleSwitch_Unchecked(object sender, RoutedEventArgs e)
		{
			ToggleEnableLiveTileUpdates(false);
			TileUtility.Instance.RemovePeriodicTask(Globals.LiveTileUpdaterPeriodicTaskName);
		}

		#endregion

	}
}