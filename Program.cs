using CodeQuest.Data;
using CodeQuest.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// ── Services ─────────────────────────────────────────────────────────
builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var rawConnString = builder.Configuration["SUPABASE_CONNECTION_STRING"] 
    ?? builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=codequest.db";

var connectionString = rawConnString;

if (rawConnString.StartsWith("postgres://") || rawConnString.StartsWith("postgresql://"))
{
    var uri = new Uri(rawConnString);
    var userInfo = uri.UserInfo.Split(':');
    var password = userInfo.Length > 1 ? userInfo[1] : "";
    connectionString = $"Host={uri.Host};Port={(uri.Port > 0 ? uri.Port : 5432)};Database={uri.LocalPath.TrimStart('/')};Username={userInfo[0]};Password={password};Ssl Mode=Require;Trust Server Certificate=true;Pooling=true;";
}

builder.Services.AddDbContextPool<AppDbContext>(options =>
{
    if (connectionString.Contains("codequest.db"))
    {
        options.UseSqlite(connectionString);
    }
    else
    {
        options.UseNpgsql(connectionString);
    }
});

builder.Services.AddScoped<IChapterService, ChapterService>();
builder.Services.AddScoped<IGameSessionService, GameSessionService>();
builder.Services.AddScoped<ILeaderboardService, LeaderboardService>();

var app = builder.Build();

var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    app.Urls.Add($"http://*:{port}");
}

// ── Middleware ────────────────────────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

// ── Seed database ─────────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (db.Database.ProviderName != null && db.Database.ProviderName.Contains("Npgsql"))
    {
        try
        {
            var creator = db.Database.GetService<Microsoft.EntityFrameworkCore.Storage.IRelationalDatabaseCreator>();
            try
            {
                if (!creator.Exists())
                {
                    creator.Create();
                }
            }
            catch { /* Ignore database creation failures */ }

            var connection = db.Database.GetDbConnection();
            bool hasTables = false;
            try
            {
                connection.Open();
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = 'public' AND table_name = 'Chapters';";
                var count = Convert.ToInt64(command.ExecuteScalar());
                hasTables = count > 0;
            }
            finally
            {
                connection.Close();
            }

            if (!hasTables)
            {
                creator.CreateTables();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing db: {ex.Message}");
        }
    }
    else
    {
        db.Database.EnsureCreated();
    }

    try
    {
        DbSeeder.Seed(db);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding db: {ex.Message}");
    }
}

// ── Routes ────────────────────────────────────────────────────────────
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
