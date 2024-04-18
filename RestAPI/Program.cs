using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestAPI.Configuration;
using RestAPI.Data;
using RestAPI.Models;
using RestAPI.Services.EmailService;
using RestAPI.Services.StudentService;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connString = builder.Configuration.GetConnectionString("restAPI_dbcs");

// Add services to the container.

builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connString);
});

builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount=true)
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme= JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

})
    .AddJwtBearer(jwt =>
    {
        var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Secret"]);
        jwt.SaveToken = true;
        jwt.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,

            IssuerSigningKey=new SymmetricSecurityKey(key),

            ValidateIssuer = false,
            ValidateAudience=false,
            RequireExpirationTime=false,
            ValidateLifetime=true,
        };

    });
builder.Services.AddCors(options =>
{
    options.AddPolicy("myPolicy",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//my services
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IStudentService, StudentService>();

var app = builder.Build(); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("myPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
