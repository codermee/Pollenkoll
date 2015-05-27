using System;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using ResourceLibrary.Models;
using System.Net.Http;

namespace ResourceLibrary
{
	public class Common
	{
		public async Task<bool> IsUpdated()
		{
			var currentDate = DateTime.Now.ToShortDateString();
			var isUpdated = false;
			var preferredCity = GetPreferredCity();
			var settings = IsolatedStorageSettings.ApplicationSettings;

			if (!String.IsNullOrWhiteSpace(preferredCity) && settings != null)
			{
				if (settings.Contains(Globals.LatestReportString))
				{
					var item = settings[Globals.LatestReportString] as ReportModel;
					if (item != null)
					{
						if (item.Date == currentDate) // A report has already been downloaded for today
						{
							if (item.City == preferredCity)
							{
								// no need to assign isUpdated variable since it should stay 'false'
							}
							else // A new city has been chosen but no report has been downloaded
							{
								var result = await DownloadAndSave(preferredCity);
								if (result)
								{
									var newReport = settings[Globals.LatestReportString] as ReportModel;
									if (newReport.Date == currentDate)
									{
										isUpdated = true;
									}
								}
							}
						}
						else if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday) // It´s a weekday so download new report
						{
							var result = await DownloadAndSave(preferredCity);
							if (result)
							{
								var newReport = settings[Globals.LatestReportString] as ReportModel;
								if (newReport.Date == currentDate)
								{
									isUpdated = true;
								}
							}
						}
						else // It´s a weekend so use the latest report if...
						{
							if (item.City == preferredCity) // ...the report´s city is the preferred city...
							{
								// no need to assign isUpdated variable since it should stay 'false'
							}
							else // ...otherwise download what´s available
							{
								var result = await DownloadAndSave(preferredCity);
								if (result)
								{
									var newReport = settings[Globals.LatestReportString] as ReportModel;
									if (newReport.Date == currentDate)
									{
										isUpdated = true;
									}
								}
							}
						}
					}
				}
				else // No report has been downloaded yet so do that
				{
					var result = await DownloadAndSave(preferredCity);
					if (result)
					{
						var newReport = settings[Globals.LatestReportString] as ReportModel;
						if (newReport != null && newReport.Date == currentDate)
						{
							isUpdated = true;
						}
					}
				}
			}
			return isUpdated;
		}

		public string GetPreferredCity()
		{
			var preferredCity = String.Empty;
			var settings = IsolatedStorageSettings.ApplicationSettings;

			if (settings != null && settings.Contains(Globals.PreferredCityString))
			{
				preferredCity = IsolatedStorageSettings.ApplicationSettings[Globals.PreferredCityString].ToString();
			}
			return preferredCity;
		}

		private static async Task<string> Download(string rssFeed)
		{
			var uri = new Uri(rssFeed);
			var client = new HttpClient();
			return await client.GetStringAsync(uri);
		}

		public async Task<bool> DownloadAndSave(string preferredCity)
		{
			var rssFeed = GetRssFeedAddress(preferredCity);
			try
			{
				var feed = await Download(rssFeed);
				var report = ParseXML(feed);
				Save(report);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		private static void Archive()
		{
			var settings = IsolatedStorageSettings.ApplicationSettings;
			var report = settings[Globals.LatestReportString] as ReportModel;
			if (report != null)
			{
				var archiveKey = String.Format("{0}_{1}", report.City, report.Date);
				if (!settings.Contains(archiveKey))
				{
					settings.Add(archiveKey, report);
				}
			}
			settings.Save();
		}

		private static void Save(ReportModel report)
		{
			var settings = IsolatedStorageSettings.ApplicationSettings;
			if (settings.Contains(Globals.LatestReportString))
			{
				Archive();
				settings[Globals.LatestReportString] = report;
			}
			else
			{
				settings.Add(Globals.LatestReportString, report);
			}
			settings.Save();
		}
		
		private static string GetRssFeedAddress(string city)
		{
			string rssFeed;

			switch (city)
			{
				case Globals.BorlangeStation:
					rssFeed = Globals.BorlangeRss;
					break;
				case Globals.BrakneHobyStation:
					rssFeed = Globals.BrakneHobyRss;
					break;
				case Globals.BackeforsStation:
					rssFeed = Globals.BackeforsRss;
					break;
				case Globals.EskilstunaStation:
					rssFeed = Globals.EskilstunaRss;
					break;
				case Globals.ForshagaStation:
					rssFeed = Globals.ForshagaRss;
					break;
				case Globals.GavleStation:
					rssFeed = Globals.GavleRss;
					break;
				case Globals.GoteborgStation:
					rssFeed = Globals.GoteborgRss;
					break;
				case Globals.HassleholmStation:
					rssFeed = Globals.HassleholmRss;
					break;
				case Globals.JonkopingStation:
					rssFeed = Globals.JonkopingRss;
					break;
				case Globals.MalmoStation:
					rssFeed = Globals.MalmoRss;
					break;
				case Globals.NorrkopingStation:
					rssFeed = Globals.NorrkopingRss;
					break;
				case Globals.NassjoStation:
					rssFeed = Globals.NassjoRss;
					break;
				case Globals.KristianstadStation:
					rssFeed = Globals.KristianstadRss;
					break;
				case Globals.SkovdeStation:
					rssFeed = Globals.SkovdeRss;
					break;
				case Globals.StockholmStation:
					rssFeed = Globals.StockholmRss;
					break;
				case Globals.SundsvallStation:
					rssFeed = Globals.SundsvallRss;
					break;
				case Globals.UmeaStation:
					rssFeed = Globals.UmeaRss;
					break;
				case Globals.VisbyStation:
					rssFeed = Globals.VisbyRss;
					break;
				case Globals.VastervikStation:
					rssFeed = Globals.VastervikRss;
					break;
				case Globals.AlvsbynStation:
					rssFeed = Globals.AlvsbynRss;
					break;
				case Globals.OstersundStation:
					rssFeed = Globals.OstersundRss;
					break;
				default:
					rssFeed = String.Empty;
					break;
			}

			return rssFeed;
		}

		private static ReportModel ParseXML(string xml)
		{
			string city = String.Empty, title = String.Empty, link = String.Empty;
			var dateTime = new DateTime();

			var doc = XDocument.Parse(xml);
			var channel = doc.Descendants(XName.Get("channel")).FirstOrDefault();
			if (channel != null)
			{
				var titleElement = channel.Element("title");
				if (titleElement != null)
				{
					var titleValue = titleElement.Value.Encode();
					char[] sep = { ' ' };
					var splitTitle = titleValue.Split(sep);
					city = splitTitle[1];
				}

				var linkElement = channel.Element("link");
				if (linkElement != null)
				{
					link = linkElement.Value;
				}
			}

			var item = doc.Descendants(XName.Get("item")).FirstOrDefault();
			if (item != null)
			{
				var dateElement = item.Element("pubDate");
				if (dateElement != null)
				{
					char[] sep = { ',', ' ' };
					var date = dateElement.Value.Encode();
					var dateArray = date.Split(sep, StringSplitOptions.None);
					var day = Convert.ToInt32(dateArray[2]);
					var month = Helper.ConvertMonth(dateArray[3]);
					var year = Convert.ToInt32(dateArray[4]);
					dateTime = new DateTime(year, month, day);
				}

				var titleElement = item.Element("title");
				if (titleElement != null)
				{
					var diff = (titleElement.Value.Length - 1) - 14;
					var text = titleElement.Value.Substring(15, diff);
					title = text.Trim().Encode();
				}
			}

			return new ReportModel
			{
				City = city,
				Date = dateTime.ToShortDateString(),
				Title = title,
				Link = link
			};
		}
	}
}