using System;
using System.Windows.Navigation;
using Pollenkoll.ViewModels;

namespace Pollenkoll.Views
{
	public partial class DetailsView
	{
		private readonly DetailsViewModel _viewModel;

		public DetailsView()
		{
			InitializeComponent();
			_viewModel = new DetailsViewModel();
		}

		private string GetFromQuerystring()
		{
			string querystring;
			NavigationContext.QueryString.TryGetValue("key", out querystring);
			return querystring;
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			var key = GetFromQuerystring();

			if (!String.IsNullOrEmpty(key))
			{
				_viewModel.Report = _viewModel.GetArchivedReport(key);
				DataContext = _viewModel.Report;
			}
		}
	}
}