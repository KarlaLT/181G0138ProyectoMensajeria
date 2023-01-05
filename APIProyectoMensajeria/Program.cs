using APIProyectoMensajeria.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

string _MyCors = "MyCors";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();

builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: _MyCors, builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

builder.Services.AddControllers();

builder.Services.AddDbContext<itesrcne_mensajeriakarlaContext>(optionsBuilder=> optionsBuilder.UseMySql("server=204.93.216.11;database=itesrcne_mensajeriakarla;user=itesrcne_karla;password=V4UvcrsVy4cm5g9", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.3.29-mariadb")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"])),
                        ValidateIssuerSigningKey = true
                    };
                }
                );

builder.Services.AddMvc();

var app = builder.Build();

app.UseCors(_MyCors);

app.UseFileServer();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
app.Run();