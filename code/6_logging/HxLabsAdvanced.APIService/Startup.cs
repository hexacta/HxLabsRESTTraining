using FluentValidation.AspNetCore;
using HxLabsAdvanced.APIService.Data;
using HxLabsAdvanced.APIService.Helpers;
using HxLabsAdvanced.APIService.Helpers.Extensions;
using HxLabsAdvanced.APIService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace HxLabsAdvanced.APIService
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc(setupAction =>
            {
                //REF 1 - XML Output and Input Format
                setupAction.ReturnHttpNotAcceptable = true;
                setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                setupAction.InputFormatters.Add(new XmlDataContractSerializerInputFormatter());
            });

            //REF 8 Herramienta de validaciones
            //Automatically register validators with the container
            services.AddMvc().AddFluentValidation(fvc =>
                fvc.RegisterValidatorsFromAssemblyContaining<Startup>());

            // register the DbContext on the container, getting the connection string from
            // appSettings (note: use this during development; in a production environment,
            // it's better to store the connection string in an environment variable)
            var connectionString = Configuration["connectionStrings:cinemaDBConnectionString"];

            services.AddDbContext<CinemaContext>(o => o.UseSqlServer(connectionString));

            // register the repository
            //REF 2 - Dependency Injection (ready)
            services.AddScoped<ICinemaService, CinemaService>();

            //REF 10 Paginación
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            //REF 10 Paginación
            services.AddScoped<IUrlHelper>(implementationFactory =>
            {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;

                return new UrlHelper(actionContext);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, CinemaContext cinemaContext)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            loggerFactory.AddDebug(LogLevel.Debug);

            loggerFactory.AddNLog();

            env.IsDevelopment().Is<IApplicationBuilder>(
                () => app.UseDeveloperExceptionPage(),
                () => app.UseExceptionHandler(appBuilder =>
                {
                    //REF 3 – Response error
                    appBuilder.Run(async context =>
                    {
                        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

                        if (exceptionHandlerFeature != null)
                        {
                            var logger = loggerFactory.CreateLogger("Global exception logger");

                            logger.LogError(500, exceptionHandlerFeature.Error, exceptionHandlerFeature.Error.Message);
                        }

                        context.Response.StatusCode = 500;

                        await context.Response.WriteAsync("An unexpected error happened. Please try again later.");
                    });
                }));


            //REF 5 - Mapeo de modelo – entidad
            this.MapperModels();
            
            //clear database and add new cinema information
            new CinemaData(cinemaContext).Seed();

            app.UseMvc();
        }

        private void MapperModels()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.Movie, Models.MovieDto>()
                    .ForMember(dest => dest.Director, opt => opt.MapFrom(src => $"{src.DirectorName} {src.DirectorLastName}"))
                    .ForMember(dest => dest.PublishYear, opt => opt.MapFrom(src => src.Publish.Year));

                cfg.CreateMap<Entities.Actor, Models.ActorDto>()
                    .ForMember(dest => dest.CompleteName, opt => opt.MapFrom(src => $"{src.Name} {src.LastName}"));

                cfg.CreateMap<Models.MovieForCreateDto, Entities.Movie>();

                cfg.CreateMap<Models.ActorForCreateDto, Entities.Actor>();

                cfg.CreateMap<Entities.Actor, Models.ActorForUpdateDto>();

                cfg.CreateMap<Models.ActorForUpdateDto, Entities.Actor>();
            });
        }
    }
}
