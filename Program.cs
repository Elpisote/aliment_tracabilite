using aliment_backend;
using aliment_backend.Entities;
using aliment_backend.Interfaces;
using aliment_backend.Mail;
using aliment_backend.Service;
using aliment_backend.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

namespace Aliment
{
    /// <summary>
    /// Classe principale contenant la méthode d'entrée de l'application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Méthode principale d'entrée de l'application.
        /// </summary>
        /// <param name="args">Arguments de ligne de commande.</param>
        public static Task Main(string[] args)
        {
            DotNetEnv.Env.Load();
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Configuration de la sérialisation JSON pour ignorer les cycles d'objets
            builder.Services.AddControllers().AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            // Configuration de Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                // Configuration de la documentation Swagger
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "JWTToken_Auth_API",
                    Version = "v1"
                });
                // Configuration de la sécurité JWT
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
                });
            });

            // Configuration de la base de données
            _ = builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                MySqlServerVersion serverVersion = new(new Version(10, 4, 10));

                string? server = Environment.GetEnvironmentVariable("DB_SERVER");
                string? user = Environment.GetEnvironmentVariable("DB_USER");
                string? password = Environment.GetEnvironmentVariable("DB_PASSWORD");
                string? database = Environment.GetEnvironmentVariable("DB_DATABASE_NAME");

                string connectionString = $"server={server};user id={user};password={password};database={database}";

                options.UseMySql(connectionString, serverVersion,
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));              
            });

            // Configuration de l'identité
            builder.Services
                .AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.SignIn.RequireConfirmedEmail = true;
            });

            //token provider life span
            builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromHours(2));

            // Configuration de l'authentification et JwtBearer
            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;

                    string? issuer = Environment.GetEnvironmentVariable("JWT_VALID_ISSUER");
                    string? audience = Environment.GetEnvironmentVariable("JWT_VALID_AUDIENCE");
                    string? key = Environment.GetEnvironmentVariable("JWT_KEY");

                    if (issuer == null || audience == null || key == null)
                    {
                        throw new InvalidOperationException("JWT environment variables are not set properly.");
                    }

                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            // Injection des dépendance
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddTransient<ITokenService, TokenService>();
            builder.Services.AddTransient<IUnitOfWork, UnitOfWorkClass>();
            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            // Configuration CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", builder => builder.WithOrigins("http://localhost:4200", "https://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            // Configuration de l'envoi d'e-mail
            // Configuration de l'envoi d'e-mail
            EmailConfiguration emailConfig = new()
            {
                From = Environment.GetEnvironmentVariable("EMAIL_FROM") ?? throw new InvalidOperationException("EMAIL_FROM not set"),
                SmtpServer = Environment.GetEnvironmentVariable("EMAIL_SMTP_SERVER") ?? throw new InvalidOperationException("EMAIL_SMTP_SERVER not set"),
                Port = int.Parse(Environment.GetEnvironmentVariable("EMAIL_PORT") ?? throw new InvalidOperationException("EMAIL_PORT not set")),
                Username = Environment.GetEnvironmentVariable("EMAIL_USERNAME") ?? throw new InvalidOperationException("EMAIL_USERNAME not set"),
                Password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD") ?? throw new InvalidOperationException("EMAIL_PASSWORD not set")
            };

            builder.Services.AddSingleton(emailConfig);


            builder.Services.AddScoped<IEmailSender, EmailSender>();

            WebApplication app = builder.Build();

            // Configuration du pipeline de requête HTTP
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();


            app.UseCors("MyPolicy");


            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();


            // Récupérer le service IHostApplicationLifetime pour gérer le cycle de vie de l'application
            var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();

            // Ajouter un gestionnaire d'événement pour l'événement ApplicationStarted
            lifetime.ApplicationStarted.Register(async () =>
            {
                // Création des rôles et de l'utilisateur admin
                using (IServiceScope scope = app.Services.CreateScope())
                {
                    RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    string[] roles = new[] { "Admin", "User" };

                    foreach (var role in roles)
                    {
                        try
                        {
                            // Vérifier si le rôle existe, sinon le créer
                            if (!await roleManager.RoleExistsAsync(role))
                            {
                                await roleManager.CreateAsync(new IdentityRole(role));
                                IdentityRole accountRole = await roleManager.FindByNameAsync(role);
                                await roleManager.AddClaimAsync(accountRole, new Claim(ClaimTypes.Role, role));
                            }
                        }
                        catch (Exception ex)
                        {
                            // Journalisez l'exception pour voir son message d'erreur
                            Console.WriteLine("Une exception s'est produite : " + ex.Message);
                        }
                    }
                }

                // Création de l'utilisateur admin s'il n'existe pas
                using (IServiceScope scope = app.Services.CreateScope())
                {
                    UserManager<User> userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                    string email = "admin@gmail.com";
                    string password = "Admin123!";

                    try
                    {
                        // Vérifier si l'utilisateur admin existe, sinon le créer
                        if (await userManager.FindByEmailAsync(email) == null)
                        {
                            User user = new()
                            {
                                Email = email,
                                Firstname = "Test",
                                Lastname = "AdminTest",
                                UserName = "Administrator"
                            };

                            await userManager.CreateAsync(user, password);
                            await userManager.AddToRoleAsync(user, "Admin");
                            await userManager.AddClaimAsync(user, new Claim("Username", user.UserName));
                            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, user.Email));
                            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, StaticUserRole.ADMIN));
                        }
                    }
                    catch (Exception ex)
                    {
                        // Journalisez l'exception pour voir son message d'erreur
                        Console.WriteLine("Une exception s'est produite : " + ex.Message);
                    }
                }
            });

            app.Run();
            return Task.CompletedTask;
        }
    }
}