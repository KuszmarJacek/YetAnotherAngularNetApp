global using YetAnotherAngularNetApp.Server;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHealthChecks()
    .AddCheck("ICMP_01", new ICMPHealthCheck("www.wp.pl", 50))
    .AddCheck("ICMP_02", new ICMPHealthCheck("onet.pl", 10))
    .AddCheck("ICMP_03", new ICMPHealthCheck($"www.{Guid.NewGuid():N}.com", 100));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseHealthChecks(new PathString("/api/health"), new CustomHealthCheckOptions());
app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
