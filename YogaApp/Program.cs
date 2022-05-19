using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using YogaApp.Data;
using YogaApp.Services;
using Azure.Identity;
using YogaApp.Areas.Identity.Data;
using YogaApp.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<UserDbContext>(options => options.UseSqlServer(connectionString)); ;
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<YogaSystemContext>(options => options.UseSqlServer(connectionString));



builder.Services.AddDefaultIdentity<YogaAppUser>(options => options.SignIn.RequireConfirmedAccount=true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<UserDbContext>(); ;

//allow any email address without verification
//builder.Services.AddDefaultIdentity<YogaAppUser>(options => options.SignIn.RequireConfirmedEmail = false)
//    .AddRoles<IdentityRole>()
//    .AddEntityFrameworkStores<UserDbContext>(); ;




//builder.Services.AddAuthorization(options => {
//    options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
//});



builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);
builder.Services.AddHttpClient();


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();





app.Run();
