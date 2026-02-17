using Azure.Storage.Blobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient(); // Register HttpClient for dependency injection
//builder.Services.AddHttpClient("TangyWebAPI", options =>
//{
//    options.BaseAddress = new Uri(builder.Configuration["TangyWebAPI:BaseUrl"]);
//    // You can also set default headers here if needed
//    // options.DefaultRequestHeaders.Add("Authorization", "Bearer YOUR_TOKEN");
//});

builder.Services.AddSingleton(u => new BlobServiceClient(builder.Configuration.GetValue<string>("StorageConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
