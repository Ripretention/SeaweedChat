using System.Text;
using SeaweedChat.Infra;
using SeaweedChat.Infra.Repositories;
using SeaweedChat.Domain.Aggregates;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
                .AddSingleton<IPasswordEncoder>(new PasswordEncoder("test salt"))
                .AddControllers();

            configureAuthentication(services);
            configureRepositories(services);
        }
        private void configureAuthentication(IServiceCollection services)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => 
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidateLifetime = true,
                        ValidateAudience = true,
                        ValidAudience =  Configuration["Jwt:Issuer"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Jwt:Key"] ?? "the most secret key"))
                    };
                });
            
        }
        private void configureRepositories(IServiceCollection services) => services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IChatRepository, ChatRepository>()
            .AddScoped<IAccountRepository, AccountRepository>()
            .AddScoped<IMessageRepository, MessageRepository>();

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();
            app.UseAuthentication();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "API",
                    pattern: "api/{controller}/{action}/{id?}"
                );
            });
        }
    }
}
