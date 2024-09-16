using System.Security.Claims;
using CustomAuthorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Configure Authentication with a simple Cookie scheme (for example)
builder.Services.AddAuthentication("MyCookieAuthScheme")
    .AddCookie("MyCookieAuthScheme", options =>
    {
        options.Cookie.Name = "MyAuthCookie";
        /*options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";*/
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Define the policy using the custom requirement
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AtLeast18", policy =>
        policy.Requirements.Add(new MinimumAgeRequirement(18)));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
/*app.Use(async (context, next) =>
{
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, "John Doe"),
        new Claim(ClaimTypes.DateOfBirth, "2000-01-01")
    };
    
    var identity = new ClaimsIdentity(claims, "Test");
    context.User = new ClaimsPrincipal(identity);

    await next();
});*/

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();