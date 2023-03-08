using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyUnitTestExperience.MiddelwareAndServiceRegister;
using System.Configuration;
using UntTest.Data.DbContexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();





//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

// add AutoMapper for mapping between entities and viewmodels
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//register-Services
builder.Services.RegisterBusinessService();
builder.Services.RegisterDataServices(builder.Configuration);

var app = builder.Build(); 


// custom middleware
app.UseMiddleware<EmployeeManagementSecurityHeadersMiddleware>();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=EmployeeOverview}/{action=Index}/{id?}");


app.Run();
