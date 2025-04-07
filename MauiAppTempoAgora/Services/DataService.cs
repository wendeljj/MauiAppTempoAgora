using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {

        public static async Task<Tempo?> GetPrevisao(string cidade) 
        {
             Tempo? t = null;

            string chave = "32474987193e7965713a079ab932f2e1";

            string url = $"https://api.openweathermap.org/data/2.5/weather?" +
                $"q={cidade}&units=metric&appid={chave}";

            using (HttpClient client = new HttpClient())
            {
                try {
                    HttpResponseMessage resp = await client.GetAsync(url);

                    if (resp.IsSuccessStatusCode)
                    {
                        string json = await resp.Content.ReadAsStringAsync();

                        var rascuho = JObject.Parse(json);

                        DateTime time = new();

                        DateTime sunrise = time.AddSeconds((double)rascuho["sys"]["sunrise"]).ToLocalTime();
                        DateTime sunset = time.AddSeconds((double)rascuho["sys"]["sunset"]).ToLocalTime();

                        t = new()
                        {
                            lat = (double)rascuho["coord"]["lat"],
                            lon = (double)rascuho["coord"]["lon"],
                            description = (string)rascuho["weather"][0]["description"],
                            main = (string)rascuho["weather"][0]["main"],
                            temp_min = (double)rascuho["main"]["temp_min"],
                            temp_max = (double)rascuho["main"]["temp_max"],
                            speed = (double)rascuho["wind"]["speed"],
                            visibility = (int)rascuho["visibility"],
                            sunrise = sunrise.ToString(),
                            sunset = sunset.ToString()
                        }; //Fecha objeto do Tempo
                    } //Fecha if se o status do servidor for sucesso
                    else if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        throw new Exception("Cidade não encontrada, tente outro nome");
                    }

                } catch (HttpRequestException) 
                {
                    throw new Exception("Sem conexão com a Internet!");
                }
            } //Fecha o laço using

            return t;
        }

    }
}
