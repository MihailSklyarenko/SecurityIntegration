using Microsoft.EntityFrameworkCore;
using SecurityIntegration.Database.IdentityContext;
using SecurityIntegration.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<IdentityContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.IntegrateIdentity();
builder.IntegrateJWT();
builder.IntegrateSwaggerWithBearerAuth("Identity.Api", "v1");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();