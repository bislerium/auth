using auth.jwt.non_refresh_token.Configs;
using auth.jwt.non_refresh_token.Options;
using auth.jwt.non_refresh_token.Services;

var builder = WebApplication.CreateBuilder(args);

# region Services Registration

builder.Services.Configure<JwtOption>(
    builder.Configuration.GetRequiredSection(JwtOption.SectionName));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwaggerGen();

builder.Services
    .AddAuthentication()
    .AddCustomJwtBearer(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddSingleton<JwtService>();

# endregion

var app = builder.Build();

# region Middlewares Registration
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

# endregion

app.Run();
