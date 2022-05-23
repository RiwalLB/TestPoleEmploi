using RestSharp;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace TestPoleEmploi
{
    internal class ApiRequest
    {
        private const string clientId = "PAR_testapi_9aadcc540f91dd0e9beb863b516be0171a6e6bd9316e89de1093ac820487ddc5";
        private const string secretKey = "44d910c9a3720afc154c23bdc629e22c760b675989cc4c26efaa3cabc220983f";

        private const string baseUrl = "https://entreprise.pole-emploi.fr";
        private const string authUrl = "/connexion/oauth2/access_token?realm=%2Fpartenaire";
        private const string jobBaseUrl = "https://api.emploi-store.fr/partenaire/offresdemploi/v2";
        private const string cityUrl = "/referentiel/communes";
        private const string jobSearchUrl = "/offres/search";
        private const string acc_tok = "access_token";

        private const int page = 50;
        private const int maxRange = 100;

        private string token = string.Empty;
        private readonly RestClient authClient = new RestClient(baseUrl);
        private readonly RestClient jobClient = new RestClient(jobBaseUrl);

        private async Task<string> GetAccessToken()
        {
            var request = new RestRequest(authUrl);

            request.AddHeader("Content_type", "application/x-www-form-urlencoded");
            request.AddParameter("realm", "%2Fpartenaire");
            request.AddParameter("grant_type", "client_credentials");
            request.AddParameter("client_id", clientId);
            request.AddParameter("client_secret", secretKey);
            request.AddParameter("scope", "api_offresdemploiv2 o2dsoffre");

            Console.WriteLine("Generating access token");

            var restResponse = await authClient.PostAsync(request);
            var responseString = restResponse.Content;

            return responseString;
        }

        private async Task<string> GetCityReferential()
        {
            var request = new RestRequest(cityUrl);

            request.AddHeader("Authorization", "Bearer " + token);

            Console.WriteLine("Fetching city referential");

            var restResponse = await jobClient.GetAsync(request);
            var responseString = restResponse.Content;

            return responseString;
        }

        private HashSet<string> GetCityCodes(List<string> cities)
        {
            string cityResponseString = GetCityReferential().Result;

            List<City> referential = JsonSerializer.Deserialize<List<City>>(cityResponseString);
            HashSet<string> codes = new HashSet<string>();

            Console.WriteLine("Getting city codes");

            foreach (string city in cities)
            {
                codes.UnionWith(referential
                    .Where(x => x.libelle != null
                    && Utils.TrimTrailingNumbers(x.libelle.ToLower()) == city.ToLower())
                    .Select(x => x.code));
            }

            return codes;
        }

        private async Task<string> GetOffersFromCity(string cityCode, int range)
        {
            var request = new RestRequest(jobSearchUrl);

            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("commune", cityCode);
            request.AddParameter("range", range.ToString() + "-" + (range + page - 1).ToString());

            var restResponse = await jobClient.GetAsync(request);
            var responseString = restResponse.Content;

            return responseString;
        }

        private List<JobOffer> GetOffers(HashSet<string> cityCodes)
        {
            List<JobOffer> jobOffers = new List<JobOffer>();

            foreach (string code in cityCodes)
            {
                int range = 0;
                while (range < maxRange)
                {
                    try
                    {
                        string jobOfferResponseString = GetOffersFromCity(code, range).Result;
                        var json = JObject.Parse(jobOfferResponseString);
                        string results = json["resultats"].ToString();

                        Console.WriteLine("Fetching offers for city " + code);

                        jobOffers.AddRange(JsonSerializer.Deserialize<List<JobOffer>>(results));
                        range += page;
                    }
                    catch (AggregateException)
                    {
                        // c'est pas beau mais j'ai pas le temps
                        range = maxRange;
                    }
                    catch (HttpRequestException)
                    {
                        // c'est pas beau mais j'ai pas le temps
                        range = maxRange;
                    }
                }
            }

            return jobOffers;
        }

        public List<JobOffer> ContactApi()
        {
            // Get Token
            string tokenResponseString = GetAccessToken().Result;
            var json = JObject.Parse(tokenResponseString);
            token = json[acc_tok].ToString();

            // Get city codes
            List<string> cities = new List<string> { "Rennes", "Bordeaux", "Paris" };
            HashSet<string> cityCodes = GetCityCodes(cities);

            // Get job offers
            List<JobOffer> jobOffers = GetOffers(cityCodes);

            return jobOffers;
        }
    }
}
