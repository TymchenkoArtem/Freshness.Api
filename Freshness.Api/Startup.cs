using Common.Settings;
using Freshness.Common.Constants;
using Freshness.Common.Enums;
using Freshness.Common.Extensions;
using Freshness.Common.Settings;
using Freshness.DAL;
using Freshness.DAL.Interfaces;
using Freshness.DAL.Repository;
using Freshness.DAL.UnitOfWork;
using Freshness.Domain.Entities;
using Freshness.Services.AutoMapper;
using Freshness.Services.Interfaces;
using Freshness.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Freshness
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            CallBotConfig = Configuration.GetSection("TelegramBotCallConfiguration").Get<BotConfiguration>();
            OrderBotConfig = Configuration.GetSection("TelegramBotOrderConfiguration").Get<BotConfiguration>();
        }

        public IConfiguration Configuration { get; }

        private BotConfiguration CallBotConfig { get; }

        private BotConfiguration OrderBotConfig { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            services.AddHostedService<ConfigureWebhook>();

            services.AddControllers()
                .AddNewtonsoftJson()
                .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

            services.AddMemoryCache();

            services.AddAutoMapper(typeof(MappingProfile));

            services.Configure<JwtBearerSettings>(Configuration.GetSection("JwtBearerSettings"));

            services.AddDbContext<FreshnessContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MSSql")));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAccessoryService, AccessoryService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<ICallService, CallService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IImageProcessor, ImageProcessor>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<IWorkerService, WorkerService>();
            services.AddScoped<ITelegramBotCallService, TelegramBotCallService>();
            services.AddScoped<ITelegramBotOrderService, TelegramBotOrderService>();

            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Freshness",
                    Version = "v1",
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Access token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[] { }
                    }
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                // c.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateActor = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration.GetSection("JwtBearerSettings")["Issuer"],
                        ValidAudience = Configuration.GetSection("JwtBearerSettings")["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("JwtBearerSettings")["Key"]))
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Freshness v1"));
            }

            app.ConfigureExceptionHandler(env);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
