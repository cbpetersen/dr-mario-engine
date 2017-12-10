using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using ApiClient.Entities;
using ApiClient.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ApiClient
{
    public class ApiClient
    {
        private readonly HttpClient httpClient = new HttpClient();
        private readonly JsonSerializerSettings settings = new JsonSerializerSettings
        {
            DateParseHandling = DateParseHandling.DateTimeOffset,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.None,
            NullValueHandling = NullValueHandling.Ignore
        };

        public ApiClient(Uri baseUri)
        {
            this.httpClient.BaseAddress = baseUri;
            this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public bool PostStats(IGameResult gameResult)
        {
            var result = this.httpClient.PostAsync($"/evolutions/{gameResult.AlgorithmId}/result", this.ConverToContent(gameResult)).Result;
            return result.IsSuccessStatusCode;
        }

        public AlgorithmSetting<TWeights> GetAlgorithmSettings<TAlgorithm, TWeights>(IAlgorithmT<TWeights> algorithmT) where TWeights : class, new()
        {
			IAlgorithm Algorithm = this.GetAlgorithm<TAlgorithm>();
            if (Algorithm == null)
            {
                return this.CreateAlgorithm(new AlgorithmT<TWeights>
                    {
                        Name = typeof(TAlgorithm).Name,
                        Weights = this.GetInstance<TWeights>()
                    });
            }

            var result = this.httpClient.GetStringAsync($"/evolutions/{Algorithm.AlgorithmId}/settings").Result;
            return JsonConvert.DeserializeObject<AlgorithmSetting<TWeights>>(result);
        }

        protected AlgorithmSetting<TWeights> CreateAlgorithm<TWeights>(IAlgorithmT<TWeights> algorithmT)
        {
            var result = this.httpClient.PostAsync("/evolutions", this.ConverToContent(algorithmT)).Result;
            return JsonConvert.DeserializeObject<AlgorithmSetting<TWeights>>(result.Content.ReadAsStringAsync().Result);
        }

        protected Algorithm[] GetAlgorithms()
        {
            var result = this.httpClient.GetStringAsync("/evolutions").Result;
            return JsonConvert.DeserializeObject<Algorithm[]>(result);
        }

        private IAlgorithm GetAlgorithm<TAlgorithm>()
        {
            var name = typeof(TAlgorithm).Name;
            return this.GetAlgorithms().FirstOrDefault(x => name == x.Name);
        }

        private StringContent ConverToContent(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj, this.settings), Encoding.UTF8, "application/json");
        }

        private T GetInstance<T>() where T : class, new()
        {
            var t = typeof(T);

            return (T)Activator.CreateInstance(t);
        }
    }
}
