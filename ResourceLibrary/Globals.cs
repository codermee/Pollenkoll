namespace ResourceLibrary
{
	public class Globals
	{
		public static string PreferredCityString = "PreferredCity";
		public static string LatestReportString = "LatestReport";
		public static string SettingsPageUri = "/Views/SettingsView.xaml";
		public static string AboutPageUri = "/Views/AboutView.xaml";
		public static string DetailsPageUri = "/Views/DetailsView.xaml?key={0}_{1}";
		public static string LiveTileSettingKey = "LiveTileSetting";
		public static string IsLowMemoryDevice = "IsLowMemoryDevice";
		public static string LiveTileUpdaterPeriodicTaskName = "LiveTileUpdaterPollenkoll";
		
		// Stations
		public const string BorlangeStation = "Borlänge";
		public const string BrakneHobyStation = "Bräkne-Hoby";
		public const string BackeforsStation = "Bäckefors";
		public const string EskilstunaStation = "Eskilstuna";
		public const string ForshagaStation = "Forshaga";
		public const string GavleStation = "Gävle";
		public const string GoteborgStation = "Göteborg";
		public const string HassleholmStation = "Hässleholm";
		public const string JonkopingStation = "Jönköping";
		public const string MalmoStation = "Malmö";
		public const string NorrkopingStation = "Norrköping";
		public const string NassjoStation = "Nässjö";
		public const string PiteaStation = "Piteå";
		public const string SkovdeStation = "Skövde";
		public const string StockholmStation = "Stockholm";
		public const string SundsvallStation = "Sundsvall";
		public const string UmeaStation = "Umeå";
		public const string VisbyStation = "Visby";
		public const string VastervikStation = "Västervik";
		public const string AlvsbynStation = "Älvsbyn";
		public const string OstersundStation = "Östersund";
		public const string KristianstadStation = "Kristianstad";
		
		// Rss Urls
		public static string BorlangeRss = "http://pollenrapporten.se/4.5dae555f13d5eaab600ff/12.314e02dd13d69872ec023.portlet?state=rss&sv.contenttype=text/xml;charset=UTF-8";
		public static string BrakneHobyRss = "http://pollenrapporten.se/4.314e02dd13d69872ec0245/12.314e02dd13d69872ec024f.portlet?state=rss&sv.contenttype=text/xml;charset=UTF-8";
		public static string BackeforsRss = "http://pollenrapporten.se/4.67f7c5a013d827ecb4cf5/12.67f7c5a013d827ecb4cff.portlet?state=rss&sv.contenttype=text/xml;charset=UTF-8";
		public static string EskilstunaRss = "http://pollenrapporten.se/4.67f7c5a013d827ecb4c155/12.67f7c5a013d827ecb4c15f.portlet?state=rss&sv.contenttype=text/xml;charset=UTF-8";
		public static string ForshagaRss = "http://pollenrapporten.se/4.67f7c5a013d827ecb4c2c9/12.67f7c5a013d827ecb4c2d3.portlet?state=rss&sv.contenttype=text/xml;charset=UTF-8";
		public static string GavleRss = "http://pollenrapporten.se/4.4a349ce514c29d2b75128433/12.4a349ce514c29d2b7512843f.portlet?state=rss&sv.contenttype=text/xml;charset=UTF-8";
		public static string GoteborgRss = "http://pollenrapporten.se/4.67f7c5a013d827ecb4c349/12.67f7c5a013d827ecb4c353.portlet?state=rss&sv.contenttype=text/xml;charset=UTF-8";
		public static string HassleholmRss = "http://pollenrapporten.se/4.549d670913d8d81d158193/12.549d670913d8d81d15819d.portlet?state=rss&sv.contenttype=text/xml;charset=UTF-8";
		public static string JonkopingRss = "http://pollenrapporten.se/4.549d670913d8d81d1581b9/12.549d670913d8d81d1581c3.portlet?state=rss&sv.contenttype=text/xml;charset=UTF-8";
		public static string MalmoRss = "http://pollenrapporten.se/4.549d670913d8d81d15827f/12.549d670913d8d81d158289.portlet?state=rss&sv.contenttype=text/xml;charset=UTF-8";
		public static string NorrkopingRss = "http://pollenrapporten.se/4.549d670913d8d81d1582a3/12.549d670913d8d81d1582ad.portlet?state=rss&sv.contenttype=text/xml;charset=UTF-8";
		public static string NassjoRss = "http://pollenrapporten.se/4.549d670913d8d81d1582f5/12.549d670913d8d81d1582ff.portlet?state=rss&sv.contenttype=text/xml;charset=UTF-8";
		public static string SkovdeRss = "http://pollenrapporten.se/4.3688e97313f58cd83202e91/12.3688e97313f58cd83203c17.portlet?state=rss&sv.contenttype=text/xml;charset=UTF-8";
		public static string StockholmRss = "http://pollenrapporten.se/4.549d670913d8d81d158347/12.549d670913d8d81d158351.portlet?state=rss&sv.contenttype=text/xml;charset=UTF-8";
		public static string SundsvallRss = "http://pollenrapporten.se/4.549d670913d8d81d15839f/12.549d670913d8d81d1583a9.portlet?state=rss&sv.contenttype=text/xml;charset=UTF-8";
		public static string UmeaRss = "http://pollenrapporten.se/4.549d670913d8d81d1583f6/12.549d670913d8d81d158400.portlet?state=rss&sv.contenttype=text/xml;charset=UTF-8";
		public static string VastervikRss = "http://pollenrapporten.se/4.549d670913d8d81d158447/12.549d670913d8d81d158451.portlet?state=rss&sv.contenttype=text/xml;charset=UTF-8";
		public static string VisbyRss = "http://pollenrapporten.se/4.4a349ce514c29d2b75128385/12.4a349ce514c29d2b75128391.portlet?state=rss&sv.contenttype=text/xml;charset=UTF-8";
		public static string AlvsbynRss = "http://pollenrapporten.se/4.549d670913d8d81d15820e/12.549d670913d8d81d158218.portlet?state=rss&sv.contenttype=text/xml;charset=UTF-8";
		public static string OstersundRss = "http://pollenrapporten.se/4.549d670913d8d81d15849a/12.549d670913d8d81d1584a4.portlet?state=rss&sv.contenttype=text/xml;charset=UTF-8";
		public static string KristianstadRss = "http://pollenrapporten.se/4.1cb760b014a762ca80194b9b/12.1cb760b014a762ca80194ba7.portlet?state=rss&sv.contenttype=text/xml;charset=UTF-8";
	}
}