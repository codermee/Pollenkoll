using System;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Navigation;
using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Pollenkoll.Classes;
using Pollenkoll.ViewModels;
using System.Windows;
using ResourceLibrary;
using ResourceLibrary.Models;
using TileUtility = Pollenkoll.Classes.TileUtility;

namespace Pollenkoll
{
	public partial class MainPage
	{
		private readonly MainViewModel _viewModel;
		private readonly Common _common;

		public MainPage()
		{
			InitializeComponent();

			_viewModel = new MainViewModel();
			_common = new Common();
			_viewModel.Stations = _viewModel.GetStationList();
			_viewModel.HistoryList = _viewModel.GetHistory();

		}

		#region Private methods

		private void LoadAndShowReport()
		{
			DataContext = _viewModel.Report;
			CityPanoramaItem.Header = _viewModel.Report.City;
			SetReportPanelVisible();
		}

		private void SortAndLoadStationList()
		{
			var source = AlphaKeyGroup<StationModel>.CreateGroups(_viewModel.Stations, Thread.CurrentThread.CurrentUICulture, s => s.Name, true);
			StationList.ItemsSource = source;
		}

		private void LoadHistoryList()
		{
			HistoryListBox.DataContext = _viewModel.HistoryList.Reverse();
		}

		private static void SetBackgroundAgent()
		{
			var isLowMemoryDevice = (bool)IsolatedStorageSettings.ApplicationSettings[Globals.IsLowMemoryDevice];
			if (!isLowMemoryDevice)
			{
				TileUtility.Instance.CreatePeriodicTask(Globals.LiveTileUpdaterPeriodicTaskName);
			}
		}

		private static void SetProgressIndicator(bool value)
		{
			SystemTray.IsVisible = value;
			if (SystemTray.ProgressIndicator == null)
			{
				SystemTray.ProgressIndicator = new ProgressIndicator
					{
						IsIndeterminate = value,
						IsVisible = value
					};
			}
			else
			{
				SystemTray.ProgressIndicator.IsIndeterminate = value;
				SystemTray.ProgressIndicator.IsVisible = value;
			}
		}

		private async void GetReport()
		{
			var preferredCity = _common.GetPreferredCity();

			if (_viewModel.Report == null && !String.IsNullOrWhiteSpace(preferredCity))
			{
				SetProgressIndicator(true);
				var newReport = await _viewModel.GetReport(preferredCity);
				_viewModel.HistoryList = _viewModel.GetHistory();
				SetProgressIndicator(false);

				if (newReport != null)
				{
					_viewModel.Report = newReport;
					LoadAndShowReport();
				}
				else
				{
					var toast = new ToastPrompt
						{
							Title = "Oops! Något gick snett.",
							Message = "Det gick inte att hämta pollenrapport just nu.",
							TextWrapping = TextWrapping.Wrap,
							MillisecondsUntilHidden = 3000,
							Height = 150
						};
					toast.Show();
				}
			}
			else if (_viewModel.Report != null)
			{
				if (_viewModel.Report.City == preferredCity)
				{
					SetProgressIndicator(true);
					LoadAndShowReport();
					SetProgressIndicator(false);
				}
				else
				{
					SetProgressIndicator(true);
					var newReport = await _viewModel.GetReport(preferredCity);
					_viewModel.HistoryList = _viewModel.GetHistory();
					SetProgressIndicator(false);

					if (newReport != null)
					{
						_viewModel.Report = newReport;
						SetProgressIndicator(true);
						LoadAndShowReport();
						SetProgressIndicator(false);
					}
					else
					{
						var toast = new ToastPrompt
						{
							Title = "Oops! Något gick snett.",
							Message = "Det gick inte att hämta pollenrapport just nu.",
							TextWrapping = TextWrapping.Wrap,
							MillisecondsUntilHidden = 3000,
							Height = 150
						};
						toast.Show();
					}
				}
			}
			else
			{
				SetEmptyPanelVisible();
			}
		}

		private void SetReportPanelVisible()
		{
			ReportStackPanel.Visibility = Visibility.Visible;
			EmptyStationStackPanel.Visibility = Visibility.Collapsed;
		}

		private void SetEmptyPanelVisible()
		{
			EmptyStationStackPanel.Visibility = Visibility.Visible;
			ReportStackPanel.Visibility = Visibility.Collapsed;
		}

		private static void NavigateToUrl(string url)
		{
			var uri = new Uri(url, UriKind.Relative);
			((PhoneApplicationFrame)Application.Current.RootVisual).Navigate(uri);
		}

		#endregion

		#region Events

		protected override void OnBackKeyPress(CancelEventArgs e)
		{
			base.OnBackKeyPress(e);

			if (!NavigationService.CanGoBack)
			{
				var tile = (from t in ShellTile.ActiveTiles select t).FirstOrDefault();
				if (tile != null)
				{
					var newTile = new IconicTileData
						{
							Count = 0
						};
					tile.Update(newTile);
				}
			}
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			while (NavigationService.CanGoBack)
			{
				NavigationService.RemoveBackEntry();
			}

			SetBackgroundAgent();
			SortAndLoadStationList();
			LoadHistoryList();
			GetReport();
		}

		private void OnApplicationBarSettingsClick(object sender, EventArgs e)
		{
			NavigateToUrl(Globals.SettingsPageUri);
		}

		private void OnApplicationBarAboutClick(object sender, EventArgs e)
		{
			NavigateToUrl(Globals.AboutPageUri);
		}

		private void OnStationListSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var item = StationList.SelectedItem as StationModel;
			if (item != null)
			{
				_viewModel.Save(item.Name);
				var toast = new ToastPrompt
					{
						Title = "Tack! Mätstationen är sparad.",
						Message = "Svep tillbaka för att se aktuell rapport.",
						TextWrapping = TextWrapping.Wrap,
						MillisecondsUntilHidden = 3000,
						Height = 150
					};
				toast.Show();
			}
		}

		private void OnPanoramaSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (MainPanorama.SelectedIndex == 0)
			{
				GetReport();
			}
			else if (MainPanorama.SelectedIndex == 2)
			{
				LoadHistoryList();
			}
		}

		private void OnHyperlinkButtonClick(object sender, RoutedEventArgs e)
		{
			var webBrowserTask = new WebBrowserTask
				{
					Uri = new Uri(_viewModel.Report.Link)
				};
			webBrowserTask.Show();
		}

		private void OnHistoryListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			// If selected index is not -1 (no selection)
			if (HistoryListBox.SelectedIndex != -1)
			{
				// Navigate to the new page
				var item = HistoryListBox.SelectedItem as ReportModel;
				if (item != null)
				{
					var detailsUri = String.Format(Globals.DetailsPageUri, item.City, item.Date);
					NavigateToUrl(detailsUri);
				}
			}

			// Reset selected index to -1 (no selection)
			HistoryListBox.SelectedIndex = -1;
		}

		private void OnApplicationBarClearHistoryMenuItemClick(object sender, EventArgs e)
		{
			_viewModel.ClearHistory();
			_viewModel.HistoryList = _viewModel.GetHistory();
			LoadHistoryList();
		}

		#endregion
	}
}