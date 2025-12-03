using Microsoft.EntityFrameworkCore;
using Monster_University_GR2.CapaDatos; // Asegúrate de que este namespace sea correcto

var builder = WebApplication.CreateBuilder(args);

// =========================================================
// PARTE 1: AGREGAR SERVICIOS (Antes del builder.Build)
// =========================================================

builder.Services.AddControllersWithViews();

// 1. Agregar la cadena de conexión (Ya lo tenías)
var connectionString = builder.Configuration.GetConnectionString("CadenaSQL");
builder.Services.AddDbContext<MonsterContext>(options =>
    options.UseSqlServer(connectionString));

// 2. IMPORTANTE: Habilitar la Caché en memoria (La sesión la necesita para guardar datos)
builder.Services.AddDistributedMemoryCache();

// 3. IMPORTANTE: Configurar el servicio de Sesión
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // La sesión muere en 20 mins
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// =========================================================

var app = builder.Build();

// Configuración del pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// =========================================================
// PARTE 2: ACTIVAR EL MOTOR DE SESIÓN (Antes de MapControllerRoute)
// =========================================================
app.UseSession();
// ^^^ ESTA ES LA LÍNEA QUE TE FALTA O ESTÁ FUERA DE LUGAR
// =========================================================

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Access}/{action=Login}/{id?}");

app.Run();