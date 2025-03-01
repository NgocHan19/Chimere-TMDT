var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Cấu hình route cho Admin (nếu có)
app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}",
    defaults: new { area = "Admin" });

// Route mặc định cho trang chủ
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Wishlist}/{action=WishlistEmpty}/{id?}");

app.Run();
