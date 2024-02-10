using auth.jwt.refresh_token.Configs;
using auth.jwt.refresh_token.Implementations.Repositories.InMemory;
using auth.jwt.refresh_token.Implementations.Services.Jwt;
using auth.jwt.refresh_token.Options.Jwt;

var builder = WebApplication.CreateBuilder(args);

# region Services Registration

builder.Services.AddOptions<JwtOption>()
    .BindConfiguration(JwtOption.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwaggerGen();

builder.Services
    .AddAuthentication()
    .AddCustomJwtBearer(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddInMemoryTokenRepository();
builder.Services.AddJwtTokenGeneratorService();
builder.Services.AddJwtTokenRenewerService();

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
