using System;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using ResourceLibrary.Models;

namespace Pollenkoll.ViewModels
{
	public class DetailsViewModel : INotifyPropertyChanged
	{
		private ReportModel report;
		public ReportModel Report
		{
			get { return report; }
			set
			{
				if (report != value)
				{
					report = value;
					NotifyPropertyChanged("Report");
				}
			}
		}

		public ReportModel GetArchivedReport(string key)
		{
			var archivedReport = new ReportModel();
			var settings = IsolatedStorageSettings.ApplicationSettings;
			if (settings != null && settings.Contains(key))
			{
				archivedReport = settings[key] as ReportModel;
			}
			return archivedReport;
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