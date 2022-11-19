using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

string _MyCors = "MyCors";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: _MyCors, builder =>
                {
                    builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
                    .AllowAnyHeader().AllowAnyMethod();
                });
            });

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
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
app.UseAuthentication();

app.Run();