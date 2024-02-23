using System;
using СurrenciesApi.Abstractions;
using СurrenciesApi.Filter;
using СurrenciesApi.Helpers;
using СurrenciesApi.Model;

namespace СurrenciesApi.Services
{
    public class CurrencieService
    {
        private static HttpClient? _client;
        private  IUriService _uriService;
        public CurrencieService(HttpClient httpClient , IUriService uriService)
        {
            _client = httpClient;
            _uriService = uriService;
        }

        private static async Task<Currencies?> Currencies()
        {
            HttpResponseMessage response = await _client!.GetAsync("https://www.cbr-xml-daily.ru/daily_json.js");
            if (response.IsSuccessStatusCode)
            {
                var valutes = await response.Content.ReadFromJsonAsync<Currencies>();
                return valutes;
            }
            return null;
        }

        public async Task<IResult> GetCurrencies(PaginationFilter filter, string route)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
        
            var currencies = await Currencies();
            if (currencies is not null)
            {
                var pagedData = currencies.Valute
                 .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                 .Take(validFilter.PageSize);

                var totalRecords = currencies.Valute.Count;
                var pagedReponse = PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, _uriService, route);

                return TypedResults.Ok( pagedReponse);
            }
            return TypedResults.BadRequest();

        }
        public async Task<IResult> GetCurrency(string currencyName)
        {
            var currencies = await Currencies() ;
            var currency = currencies?
                .Valute
                .FirstOrDefault(v => v.Key.Equals(currencyName));

            if (currency is not null)
                return TypedResults.Ok(currency);
            return TypedResults.BadRequest();
        }
    }
}
