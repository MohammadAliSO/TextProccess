using Asynchronous_TextProcessing.Auth.API;
using Asynchronous_TextProcessing.Auth.Services;
using Asynchronous_TextProcessing.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Reflection;

//parse appsettings.json  add to Models
var baseAddress = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
var myJsonString = File.ReadAllText(baseAddress + @"\appsettings.json");
var myJObject = JObject.Parse(myJsonString);
Config.All = myJObject.ToObject<ConfigModels>();

var builder = WebApplication.CreateBuilder(args);

//Possibility windows service
builder.Host.UseWindowsService();

//Scaffold-DbContext "Server=DESKTOP-TQD5Q14\SQL2019;Initial Catalog=Tasks_DB;MultipleActiveResultSets=true;User ID=sa;Password=22816321;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
builder.Services.AddDbContext<DBContext>(options => options.UseSqlServer(Config.All.ConnectionStrings.DefaultConnection));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//nuget Swashbuckle.AspNetCore.Newtonsoft
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
});

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authorization header using the Bearer scheme."
    });


    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
              new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "basic"
                    }
                },
                new string[] {}
        }
    });


    var fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var filePath = Path.Combine(AppContext.BaseDirectory, fileName);
    options.IncludeXmlComments(filePath);
});

builder.Services.AddSwaggerGenNewtonsoftSupport();

builder.Services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

builder.Services.AddScoped<IUserService, UserService>();
var app = builder.Build();


app.Use(async (context, next) =>
{
    try
    {
        switch (context.Request.Path)
        {
            //case "/api/Task/NewRequest":

            //    break;
            //case "/api/Task/CheckRequest":

            //    break;
            //case "/api/Task/Result":

            //    break;
            case "/":
                context.Response.Redirect("/swagger/index.html");
                break;
            default:
                break;
        }
        await next().ConfigureAwait(false);
    }
    catch (Exception e)
    {
        throw;
    }
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();