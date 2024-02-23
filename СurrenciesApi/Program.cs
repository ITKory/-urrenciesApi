using ÑurrenciesApi.Abstractions;
using ÑurrenciesApi.Services;

var builder = WebApplication.CreateBuilder(args);

 
builder.Services.AddTransient<CurrencieService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IUriService>(o =>
{
    var accessor = o.GetRequiredService<IHttpContextAccessor>();
    var request = accessor.HttpContext.Request;
    var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
    return new UriService(uri);
});

builder.Services.AddControllers();

builder.Services.AddHttpClient();
 
var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
