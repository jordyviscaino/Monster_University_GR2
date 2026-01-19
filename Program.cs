using Microsoft.Extensions.Options;
using Monster_University_GR2.CapaDatos;      // Datos
using Monster_University_GR2.Colecciones;    // Repositorios
using Monster_University_GR2.CapaNegocio;    // <--- IMPORTANTE: FALTABA ESTE USING

var builder = WebApplication.CreateBuilder(args);

// =========================================================
// 1. CONFIGURACIÓN DE SESIÓN
// =========================================================
builder.Services.AddDistributedMemoryCache(); // Necesario para la sesión
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// =========================================================
// 2. INYECCIÓN DE DEPENDENCIAS (El núcleo del sistema)
// =========================================================

// A. Singleton: Una sola conexión para toda la vida de la app
builder.Services.AddSingleton<ContextoMongo>();

// B. Scoped: Se crean una vez por cada petición HTTP
builder.Services.AddScoped<UsuariosCollection>();

// C. Servicios de Negocio (LO QUE TE FALTABA)
// Sin esto, el AccessController falla porque no encuentra quien haga la lógica
builder.Services.AddScoped<ServicioAcceso>();
builder.Services.AddScoped<ServicioCorreo>();
builder.Services.AddScoped<ServicioUsuarios>();
// =========================================================
// 3. SERVICIOS MVC
// =========================================================
builder.Services.AddControllersWithViews();

var app = builder.Build();

// =========================================================
// 4. PIPELINE HTTP
// =========================================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// IMPORTANTE: El orden es vital aquí
app.UseSession();        // 1. Primero activamos sesión
app.UseAuthentication(); // 2. (Opcional si usas Identity, pero bueno tenerlo)
app.UseAuthorization();  // 3. Luego autorización

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Access}/{action=Login}/{id?}"); // Arrancamos en Login

app.Run();