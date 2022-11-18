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

builder.Services.AddMvc();
var app = builder.Build();

app.UseCors(_MyCors);
app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());



app.Run();