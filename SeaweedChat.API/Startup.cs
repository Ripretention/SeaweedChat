using SeaweedChat.Infra;
using Microsoft.OpenApi.Models;
using SeaweedChat.Infra.Repositories;
using SeaweedChat.Domain.Aggregates;
using SeaweedChat.API.Security;
using Microsoft.EntityFrameworkCore;

namespace SeaweedChat
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<ApplicationContext>(options =>
                    options.UseSqlite("Filename=test.db")
                )
                .AddSingleton<IAuthentication, JwtAuthentication>(s => 
                {
                    var auth = new JwtAuthentication(Configuration);
                    auth.Configure(s.GetRequiredService<IServiceCollection>());
                    return auth;
                })
                .AddSingleton<IPasswordEncoder>(new PasswordEncoder(Configuration["PasswordSalt"] ?? "test-salt"))
                .AddControllers();

            configureSwagger(services);
            configureRepositories(services);
        }
        private void configureSwagger(IServiceCollection services) => services
            .AddSwaggerGen(swagger =>
            { 
                swagger.SwaggerDoc("v1", new OpenApiInfo 
                {
                    Title = "SeaweedChat API",
                    Version = "v1"
                });
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \n\n Enter 'Bearer' [space] and then your token in the text input below.\n\nExample: \"Bearer 12345abcdef\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
        private void configureRepositories(IServiceCollection services) => services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IChatRepository, ChatRepository>()
            .AddScoped<IAccountRepository, AccountRepository>()
            .AddScoped<IMessageRepository, MessageRepository>();

        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env        
        )
        {
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "SeaweedChat API");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapSwagger();
                endpoints.MapControllerRoute(
                    name: "API",
                    pattern: "api/{controller}s/{action}/{id?}"
                );
            });
        }
    }
}
