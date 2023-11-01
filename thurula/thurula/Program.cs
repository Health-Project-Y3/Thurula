using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using thurula.Hubs;
using thurula.Models;
using thurula.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication(
    options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }
).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration.GetSection("AppSettings:Token").Value!)),
    };
});
builder.Services.Configure<AtlasDbSettings>(
    builder.Configuration.GetSection(nameof(AtlasDbSettings)));
builder.Services.AddSingleton<IAtlasDbSettings>(sp =>
    sp.GetRequiredService<IOptions<AtlasDbSettings>>().Value);
builder.Services.AddSingleton<IMongoClient>(_ =>
    new MongoClient(builder.Configuration.GetValue<string>("AtlasDbSettings:ConnectionString")));

//Custom built Services
builder.Services.AddScoped<IAuthUserService, AuthUserService>();
builder.Services.AddScoped<IBabyService, BabyService>();
builder.Services.AddScoped<IBabyLengthChartService, BabyLengthChartService>();
builder.Services.AddScoped<IBabyWeightChartService, BabyWeightChartService>();
builder.Services.AddScoped<INapService, NapService>();
builder.Services.AddScoped<IChecklistService, ChecklistService>();
builder.Services.AddScoped<IDiaperService, DiaperService>();
builder.Services.AddScoped<IFeedingService, FeedingService>();
builder.Services.AddScoped<IVaccineAppointmentService, VaccineAppointmentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBabyNameService, BabyNameService>();
builder.Services.AddScoped<IForumService, ForumService>();
builder.Services.AddScoped<IUserExerciseService, UserExerciseService>();
builder.Services.AddScoped<IUserWeightService, UserWeightService>();
builder.Services.AddScoped<IUserDrinkingService, UserDrinkingService>();
builder.Services.AddScoped<IUserBpService, UserBpService>();
builder.Services.AddScoped<IBmiService, BmiService>();
builder.Services.AddScoped<IEyeCheckupService, EyeCheckupService>();


builder.Services.AddSignalR(hubOptions => {
        hubOptions.EnableDetailedErrors = true;
        hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(1);
        hubOptions.ClientTimeoutInterval = TimeSpan.FromMinutes(2);
        hubOptions.HandshakeTimeout = TimeSpan.FromMinutes(1);
    });

builder.Services.AddControllers(option => { option.ReturnHttpNotAcceptable = false; }).AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    // .WriteTo.File("logs/myLogs-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapHub<ForumHub>("forum");
app.MapControllers();

app.Run();