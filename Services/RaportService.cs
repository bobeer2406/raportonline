using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace RaportOnline.Services
{
    public class RaportService
    {
        private readonly HttpClient _http;

        public RaportService(HttpClient http)
        {
            _http = http;
        }

        public class Rachunek
        {
            public string data { get; set; }
            public string kategoria { get; set; }
            public double kwota { get; set; }
            public string opis { get; set; }
        }

        private class Root
        {
            public List<Rachunek> Rachunki { get; set; }
        }

        public async Task<List<Rachunek>> PobierzRachunkiAsync(string url)
        {
            try
            {
                var json = await _http.GetStringAsync(url);
                var dane = JsonSerializer.Deserialize<Root>(json);
                return dane?.Rachunki ?? new List<Rachunek>();
            }
            catch
            {
                return new List<Rachunek>(); // Możesz dodać logowanie błędu
            }
        }

        public List<string> PobierzKategorie(List<Rachunek> rachunki)
        {
            return rachunki
                .Select(r => r.kategoria)
                .Distinct()
                .OrderBy(k => k)
                .ToList();
        }

        public List<Rachunek> FiltrujPoKategorii(List<Rachunek> rachunki, string kategoria)
        {
            if (string.IsNullOrEmpty(kategoria))
                return rachunki;

            return rachunki
                .Where(r => r.kategoria == kategoria)
                .ToList();
        }
    }
}
