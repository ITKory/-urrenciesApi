using Microsoft.AspNetCore.Mvc;
using System.IO;
using СurrenciesApi.Filter;
using СurrenciesApi.Model;
using СurrenciesApi.Services;

namespace СurrenciesApi.Controllers;
 
[ApiController]
[Route("[controller]")]
public class CurrencieController(CurrencieService currencieService) : ControllerBase
{
    private readonly CurrencieService _currencieService = currencieService;

    [HttpGet("currencies")]
    public async Task<IResult> GetCurrencies ([FromQuery] PaginationFilter filter)
    => await _currencieService.GetCurrencies(filter, Request.Path.Value);

    [HttpGet("currency")]
    public async Task<IResult> GetCurrency(string currencyName)
     => await _currencieService.GetCurrency(currencyName);     
}
