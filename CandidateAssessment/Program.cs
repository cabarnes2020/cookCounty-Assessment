using CandidateAssessment.Models;
using CandidateAssessment.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<CandidateAssessmentContext>(options =>
              options.UseSqlServer(builder.Configuration.GetConnectionString("MainDbConnection")),
              ServiceLifetime.Scoped);

builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<SchoolService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Records}/{action=Students}/{id?}");

app.MapControllerRoute(
    name: "schools_students",
    pattern: "{controller=Records}/{action=SchoolsStudents}/{id}");


app.Run();
