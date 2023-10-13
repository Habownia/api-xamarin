using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using System.Net.Http;
using Newtonsoft.Json; // To trzeba pobrać z nuGeta
using System.Diagnostics;

namespace api_xamarin
{
    public class ExchangeRate
    {
        public string table { get; set; }
        public string no { get; set; }
        public DateTime effectiveDate { get; set; }
        public List<Rate> rates { get; set; }

    }

    public class Rate
    {
        public string currency { get; set; }
        public string code { get; set; }
        public decimal mid { get; set; }

    }

    public class ApiHelper
    {
        private readonly HttpClient _httpClient;
        private const string ApiBaseUrl = "https://api.nbp.pl/api/"; // Zmień na adres API, z którego chcesz pobrać dane

        public ApiHelper()
        {
            _httpClient = new HttpClient();
        }

        public async Task<ExchangeRate> GetApiDataAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync(ApiBaseUrl + "exchangerates/tables/A//?format=json"); // endpoint do api tabla A, a jest ich 3 - A,B,C
                if (!string.IsNullOrEmpty(response))
                {
                    // Deserializacja danych JSON, dlatego lista bo jest to w [] nie wiem dlaczego ale ok niech będzie
                    List<ExchangeRate> exchangeRates = JsonConvert.DeserializeObject<List<ExchangeRate>>(response);
                    return exchangeRates[0];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Błąd podczas pobierania danych z API: " + ex.Message);
            }

            return null;
        }
    }

    public partial class MainPage : ContentPage
    {
        private ApiHelper apiHelper;

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