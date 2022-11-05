var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
var app = builder.Build();
app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());

app.Run();