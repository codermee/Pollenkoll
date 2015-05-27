using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading.Tasks;
using ResourceLibrary;
using ResourceLibrary.Models;

namespace Pollenkoll.ViewModels
{
	public class MainViewModel : INotifyPropertyChanged
	{
		#region Members

		private ReportModel _report;
		public ReportModel Report
		{
			get { return _report; }
			set
			{
				if (_report != value)
				{
					_report = value;
					NotifyPropertyChanged("Report");
				}
			}
		}

		private ObservableCollection<ReportModel> _historyList;
		public ObservableCollection<ReportModel> HistoryList
		{
			get { return _historyList; }
			set
			{
				if (_historyList != value)
				{
					_historyList = value;
					NotifyPropertyChanged("HistoryList");
				}
			}
		}

		private ObservableCollection<StationModel> _stations;
		public ObservableCollection<StationModel> Stations
		{
			get { return _stations; }
			set
			{
				if (_stations != value)
				{
					_stations = value;
					NotifyPropertyChanged("Stations");
				}
			}
		}

		#endregion

		public ObservableCollection<ReportModel> GetHistory()
		{
			var settings = IsolatedStorageSettings.ApplicationSettings;
			var archive = new ObservableCollection<ReportModel>();

			if (settings != null)
			{
				foreach (var item in settings)
				{
					if (item.Key.Contains("_"))
					{
						var archivedReport = item.Value as ReportModel;
						if (archivedReport != null)
						{
							archive.Add(archivedReport);
						}
					}
				}
			}
			return archive;
		}

		public void ClearHistory()
		{
			var settings = IsolatedStorageSettings.ApplicationSettings;
			if (settings != null)
			{
				var entriesToRemove = settings.Where(x => x.Key.Contains("_")).Select(x => x.Key).ToList();

				foreach (var key in entriesToRemove)
				{
					settings.Remove(key);
				}
			}
		}

		public async Task<ReportModel> GetReport(string preferredCity)
		{
			ReportModel reportModel = null;
			var common = new Common();
			var settings = IsolatedStorageSettings.ApplicationSettings;
			if (!String.IsNullOrWhiteSpace(preferredCity) && settings != null)
			{
				if (settings.Contains(Globals.LatestReportString))
				{
					var item = settings[Globals.LatestReportString] as ReportModel;
					if (item != null)
					{
						if (item.Date == DateTime.Now.ToShortDateString()) // A report has already been downloaded for today
						{
							if (item.City == preferredCity)
							{
								reportModel = item;
							}
							else // A new city has been chosen but no report has been downloaded
							{
								var result = await common.DownloadAndSave(preferredCity);
								if (result)
								{
									reportModel = settings[Globals.LatestReportString] as ReportModel;
								}
							}
						}
						else if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday) // It´s a weekday so download new report
						{
							var result = await common.DownloadAndSave(preferredCity);
							if (result)
							{
								reportModel = settings[Globals.LatestReportString] as ReportModel;
							}
						}
						else // It´s a weekend so use the latest report...
						{
							if (item.City == preferredCity) // ...if the report´s city is the preferred city...
							{
								reportModel = item;
							}
							else // ...otherwise download what´s available
							{
								var result = await common.DownloadAndSave(preferredCity);
								if (result)
								{
									reportModel = settings[Globals.LatestReportString] as ReportModel;
								}
							}
						}
					}
				}
				else // No report has been downloaded yet so do that
				{
					var result = await common.DownloadAndSave(preferredCity);
					if (result)
					{
						reportModel = settings[Globals.LatestReportString] as ReportModel;
					}
				}
			}
			return reportModel;
		}

		public void Save(string city)
		{
			var settings = IsolatedStorageSettings.ApplicationSettings;
			if (settings != null)
			{
				if (settings.Contains(Globals.PreferredCityString))
				{
					settings[Globals.PreferredCityString] = city;
				}
				else
				{
					settings.Add(Globals.PreferredCityString, city);
				}
				settings.Save();
			}
		}

		public ObservableCollection<StationModel> GetStationList()
		{
			var stationList = new ObservableCollection<StationModel>
				{
					new StationModel{ Name = Globals.BackeforsStation },
					new StationModel{ Name = Globals.BorlangeStation },
					new StationModel{ Name = Globals.BrakneHobyStation },
					new StationModel{ Name = Globals.EskilstunaStation },
					new StationModel{ Name = Globals.ForshagaStation },
					new StationModel{ Name = Globals.GoteborgStation },
					new StationModel{ Name = Globals.HassleholmStation },
					new StationModel{ Name = Globals.JonkopingStation },
					new StationModel{ Name = Globals.MalmoStation },
					new StationModel{ Name = Globals.NassjoStation },
					new StationModel{ Name = Globals.NorrkopingStation },
					new StationModel{ Name = Globals.OstersundStation },
					new StationModel{ Name = Globals.SkovdeStation },
					new StationModel{ Name = Globals.StockholmStation },
					new StationModel{ Name = Globals.SundsvallStation },
					new StationModel{ Name = Globals.UmeaStation },
					new StationModel{ Name = Globals.VastervikStation },
					new StationModel{ Name = Globals.AlvsbynStation },
					new StationModel{ Name = Globals.GavleStation },
					new StationModel{ Name = Globals.VisbyStation },
					new StationModel{ Name = Globals.KristianstadStation }
				};
			return stationList;
		}

		#region Eventhandlers

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(String propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion
	}
}