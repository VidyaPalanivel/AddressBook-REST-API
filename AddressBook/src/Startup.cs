using System.Reflection;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using AddressBook.Entities.Models;
using AddressBook.Contracts;
using AddressBook.Service;
using AddressBook.Logger;
using AddressBook.Logger.IManager;

namespace AddressBook
{
    public class Startup
    {
        
        public IConfiguration _configuration { get; }
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            _configuration = configuration;
        }
        
        public virtual void ConfigureServices(IServiceCollection services)
        {
            
            services.AddControllers();
            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddMvcCore();
            services.AddDbContext<AddressBookContext>(opt =>
            opt.UseNpgsql("User ID = postgres; Password=acies123; Server=localhost;Port=5432;Database=AddressBook1;Integrated Security=true;Pooling=true;"));
            services.AddHttpContextAccessor();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAdminDetailsService, AdminService>();
            
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("some_big_key_value_here_secret")),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero //the default for this setting is 5 minutes
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AddressBook", Version = "v1", });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "JWT Authorization header using the Bearer scheme.",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                       {
                         new OpenApiSecurityScheme
                         {
                           Reference = new OpenApiReference
                           {
                             Type = ReferenceType.SecurityScheme,
                             Id = "Bearer"
                           }
                          },
                          Array.Empty<string>()
                        }
                      });
                    c.EnableAnnotations();
            });
            // var mappingConfig = new AutoMapper.MapperConfiguration(mc =>
            // {
            //     mc.AddProfile(new Service.MappingProfile());
            // });
            // IMapper mapper = mappingConfig.CreateMapper();
            // services.AddSingleton(mapper);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "AddressBook"); c.RoutePrefix = string.Empty; });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

