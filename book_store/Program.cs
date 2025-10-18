using book_store.Areas.Admin.Services;
using book_store.Areas.Admin.Services.IServices;
using book_store.DataAccess.Data;
using book_store.DataAccess.Repository;
using book_store.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Identity;
using book_store.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using book_store.Areas.Customer.Services.IServices;
using book_store.Areas.Customer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = $"/Identity/Account/Login"; // เมื่อผู้ใช้พยายามเข้าถึงหน้าที่ต้องล็อกอินก่อน ระบบจะ redirect ไปที่
    options.LogoutPath = $"/Identity/Account/Logout"; // หน้าที่ใช้สำหรับออกจากระบบ
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied"; // เมื่อผู้ใช้ล็อกอินแล้วแต่ไม่มีสิทธิ์เข้าถึงหน้าใดหน้าหนึ่ง จะถูกส่งไปที่หน้านี้
});

builder.Services.AddRazorPages();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IHomeService, HomeService>();
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
app.UseAuthorization();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
