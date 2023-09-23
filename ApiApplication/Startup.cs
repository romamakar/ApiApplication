using ApiApplication.Commands.Seat;
using ApiApplication.Commands.Seat.Validators;
using ApiApplication.Commands.Showtime;
using ApiApplication.Commands.Showtime.Validators;
using ApiApplication.Database;
using ApiApplication.Database.Repositories;
using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Extensions;
using ApiApplication.Pipelines;
using ApiApplication.Services;
using ApiApplication.Settings;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace ApiApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IShowtimesRepository, ShowtimesRepository>();
            services.AddTransient<ITicketsRepository, TicketsRepository>();
            services.AddTransient<IAuditoriumsRepository, AuditoriumsRepository>();
            services.AddTransient<IMoviesRepository, MoviesRepository>();
            services.AddTransient<IProviderApiService, ProviderApiService>();
            services.AddSwaggerGen();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Startup>());
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddDbContext<CinemaContext>(options =>
            {
                options.UseInMemoryDatabase("CinemaDb")
                    .EnableSensitiveDataLogging()
                    .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
            services.AddControllers();
            services.AddHttpClient();
            services.AddStackExchangeRedisCache(options => {
                options.Configuration = "localhost";                
                options.InstanceName = "local";
            });

            services.AddScoped<IValidator<BuySeatCommand>, BuySeatValidator>();
            services.AddScoped<IValidator<ConfirmSeatCommand>, ConfirmSeatValidator>();
            services.AddScoped<IValidator<GetSeatByIdRequest>, GetByIdSeatValidator>();
            services.AddScoped<IValidator<ReserveSeatCommand>, ReserveSeatValidator>();
            services.AddScoped<IValidator<CreateShowtimeCommand>, CreateShowtimeValidator>();
            services.AddScoped<IValidator<GetShowtimeByIdRequest>, GetShowtimByIdValidator>();


            var identitySettingsSection = Configuration.GetSection("ProviderApi");
            services.Configure<ProviderAPISettings>(identitySettingsSection);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.ConfigureExceptionHandler(logger);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI();

            SampleData.Initialize(app);
        }      
    }
}
