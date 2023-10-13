using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace api_xamarin
{
    public partial class MainPage : ContentPage
    {
        private readonly ApiHelper apiHelper;

        public MainPage()
        {
            InitializeComponent();
            apiHelper = new ApiHelper();
        }

        private async void OnGetApiDataClicked(object sender, EventArgs e)
        {

            var exchangeRate = await apiHelper.GetApiDataAsync();

            if (exchangeRate != null)
            {
                // Wyświetl dane z API na etykiecie
                string dataText = $"No: {exchangeRate.no}\n";
                dataText += $"Effective Date: {exchangeRate.effectiveDate}\n";
                dataText += "Rates:\n";

                foreach (var rate in exchangeRate.rates)
                {
                    dataText += $"{rate.currency} ({rate.code}): {rate.mid}\n";
                }

                apiDataLabel.Text = dataText;
            }
            else
            { 
                apiDataLabel.Text = "Błąd podczas pobierania danych z API.";
            }
        }
    }
}