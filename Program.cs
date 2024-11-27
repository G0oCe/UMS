using Microsoft.EntityFrameworkCore;
using UMS1._0;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UniversityDbContext>(options =>
	options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSwaggerGenConfig();
// Добавление сервисов

builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<JwtAuthenticationExtensions>();
builder.Services.AddScoped<HashingService>();

var jwtExtensions = new JwtAuthenticationExtensions(builder.Configuration);
jwtExtensions.AddJwtAuthentication(builder.Services);

// Add Authorization
builder.Services.AddAuthorization();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Построение приложения
var app = builder.Build();

app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication(); // Сначала используйте аутентификацию
app.UseAuthorization();
app.MapControllers();
app.Run();