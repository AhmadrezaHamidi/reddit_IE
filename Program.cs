using Microsoft.EntityFrameworkCore;
using Authentication.Models;
using Microsoft.AspNetCore.Identity;
using Authentication.Configurations;
using System.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Authentication.Models.Dto.Requests;
using System.Security.Claims;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.


var key = Encoding.ASCII.GetBytes("ijurkbdlhmklqacwqzdxmkkhvqowlyqa");

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
 })
 .AddJwtBearer(jwt => {
    jwt.RequireHttpsMetadata = false;
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, // this will validate the 3rd part of the jwt token using the secret that we added in the appsettings and verify we have generated the jwt token
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateAudience = false,
        RequireExpirationTime = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
 });



// builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//                 .AddEntityFrameworkStores<Context>();

/*builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserPolicy",
        policy => policy.RequireClaim("UserId"));
});*/





var connectionString = builder.Configuration.GetConnectionString("CS");

builder.Services.AddDbContext<Context>(options => options.UseSqlServer(connectionString));
//*/





builder.Services.AddAuthorization(config =>  
            {  
                config.AddPolicy("Policy", policyBuilder =>  
                {  
                    policyBuilder.RequireClaim(ClaimTypes.Surname);  
                    policyBuilder.RequireClaim(ClaimTypes.Email);  
                }); 
                config.AddPolicy(Roles.Admin , Roles.AdminPolicy());
                config.AddPolicy(Roles.User , Roles.UserPolicy());

            });


builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<Context>()
                .AddRoles<IdentityRole>()
                .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
        {
           
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@/";
            
        });

        


// var key = Encoding.ASCII.GetBytes("MY_BIG_SECRET_KEY_LKSHDJFLSDKFW@#($)(#)32234");




builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});



builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder => {
    builder
    .WithMethods("GET", "POST")
    .AllowAnyHeader()
    .AllowAnyOrigin();
}));

builder.Services.AddAuthorization(builder => builder.AddPolicy("Policy", options=>options.RequireRole("User","Admin")));
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

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

app.Run();

