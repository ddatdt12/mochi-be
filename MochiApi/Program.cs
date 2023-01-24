using Microsoft.EntityFrameworkCore;
using MochiApi.Dtos.Settings;
using MochiApi.Extensions;
using MochiApi.Hubs;
using MochiApi.Middlewares;
using MochiApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(c =>
{
    c.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(5);
});
builder.Services.ConfigureCors();

// Add services to the container.
builder.Services.AddControllers().AddCustomOptions();

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
    options.HandshakeTimeout = TimeSpan.FromSeconds(30);
}).AddJsonProtocol(x => {
    x.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    x.PayloadSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});


builder.Services.AddSignalR();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.ConfigureSwaggerOptions();
    c.EnableAnnotations();
});
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.ConfigureAuthentication(builder.Configuration);

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("DbContext"));
}
);
builder.Services.ConfigureRepository();
builder.Services.AddAutoMapper(typeof(Program));
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.ConfigureExceptionHandler(app.Logger);
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();


app.UseMiddleware<JwtMiddleware>();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<NotiHub>("/hubs/noti");
});

app.Run();
