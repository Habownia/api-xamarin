using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
                //var response = await _httpClient.GetStringAsync(ApiBaseUrl + "exchangerates/tables/A//?format=json"); // endpoint do api tabla A, a jest ich 3 - A,B,C
                #region DUMMY RESPONSE
                var response = "[{\"table\":\"A\",\"no\":\"199/A/NBP/2023\",\"effectiveDate\":\"2023-10-13\",\"rates\":[{\"currency\":\"bat (Tajlandia)\",\"code\":\"THB\",\"mid\":0.1183},{\"currency\":\"dolar amerykański\",\"code\":\"USD\",\"mid\":4.3033},{\"currency\":\"dolar australijski\",\"code\":\"AUD\",\"mid\":2.7206},{\"currency\":\"dolar Hongkongu\",\"code\":\"HKD\",\"mid\":0.5501},{\"currency\":\"dolar kanadyjski\",\"code\":\"CAD\",\"mid\":3.1483},{\"currency\":\"dolar nowozelandzki\",\"code\":\"NZD\",\"mid\":2.5454},{\"currency\":\"dolar singapurski\",\"code\":\"SGD\",\"mid\":3.1448},{\"currency\":\"euro\",\"code\":\"EUR\",\"mid\":4.5417},{\"currency\":\"forint (Węgry)\",\"code\":\"HUF\",\"mid\":0.011732},{\"currency\":\"frank szwajcarski\",\"code\":\"CHF\",\"mid\":4.7470},{\"currency\":\"funt szterling\",\"code\":\"GBP\",\"mid\":5.2563},{\"currency\":\"hrywna (Ukraina)\",\"code\":\"UAH\",\"mid\":0.1183},{\"currency\":\"jen (Japonia)\",\"code\":\"JPY\",\"mid\":0.028746},{\"currency\":\"korona czeska\",\"code\":\"CZK\",\"mid\":0.1843},{\"currency\":\"korona duńska\",\"code\":\"DKK\",\"mid\":0.6090},{\"currency\":\"korona islandzka\",\"code\":\"ISK\",\"mid\":0.031001},{\"currency\":\"korona norweska\",\"code\":\"NOK\",\"mid\":0.3935},{\"currency\":\"korona szwedzka\",\"code\":\"SEK\",\"mid\":0.3934},{\"currency\":\"lej rumuński\",\"code\":\"RON\",\"mid\":0.9147},{\"currency\":\"lew (Bułgaria)\",\"code\":\"BGN\",\"mid\":2.3221},{\"currency\":\"lira turecka\",\"code\":\"TRY\",\"mid\":0.1548},{\"currency\":\"nowy izraelski szekel\",\"code\":\"ILS\",\"mid\":1.0837},{\"currency\":\"peso chilijskie\",\"code\":\"CLP\",\"mid\":0.004586},{\"currency\":\"peso filipińskie\",\"code\":\"PHP\",\"mid\":0.0758},{\"currency\":\"peso meksykańskie\",\"code\":\"MXN\",\"mid\":0.2400},{\"currency\":\"rand (Republika Południowej Afryki)\",\"code\":\"ZAR\",\"mid\":0.2266},{\"currency\":\"real (Brazylia)\",\"code\":\"BRL\",\"mid\":0.8526},{\"currency\":\"ringgit (Malezja)\",\"code\":\"MYR\",\"mid\":0.9100},{\"currency\":\"rupia indonezyjska\",\"code\":\"IDR\",\"mid\":0.0002744},{\"currency\":\"rupia indyjska\",\"code\":\"INR\",\"mid\":0.051698},{\"currency\":\"won południowokoreański\",\"code\":\"KRW\",\"mid\":0.003188},{\"currency\":\"yuan renminbi (Chiny)\",\"code\":\"CNY\",\"mid\":0.5890},{\"currency\":\"SDR (MFW)\",\"code\":\"XDR\",\"mid\":5.6257}]}]\r\n";
#endregion
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
}
