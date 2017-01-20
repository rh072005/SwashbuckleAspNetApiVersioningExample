using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace SwashbuckleAspNetApiVersioningExample
{
    public class Startup
    {
        private List<string> _versions;
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            _versions = GetSubClasses<Controller>().Select(x => x.Namespace.Split('.').Last().ToLower()).ToList();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc(c =>            
                c.Conventions.Add(new ApiExplorerGroupPerVersionConvention())                
            );

            services.AddApiVersioning();

            //Ammended from https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/244
            services.AddSwaggerGen(c =>
            {
                c.DocInclusionPredicate((version, apiDescription) =>
                {             
                    if(apiDescription.GroupName != version)
                    {
                        return false;
                    }

                    var values = apiDescription.RelativePath
                        .Split('/')
                        .Select(v => v.Replace("v{version}", version));

                    apiDescription.RelativePath = string.Join("/", values);

                    var versionParameter = apiDescription.ParameterDescriptions
                        .SingleOrDefault(p => p.Name == "version");

                    if (versionParameter != null)
                        apiDescription.ParameterDescriptions.Remove(versionParameter);

                    foreach (var parameter in apiDescription.ParameterDescriptions)
                        parameter.Name = char.ToLowerInvariant(parameter.Name[0]) + parameter.Name.Substring(1);

                    return true;
                });
                               
                
                foreach(var version in _versions)
                {
                    c.SwaggerDoc(version, new Info { Version = version, Title = string.Format($"API {version.ToUpper()}") });
                }               
                
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUi(c =>
            {   
                foreach(var version in _versions)
                {
                    c.SwaggerEndpoint(string.Format($"/swagger/{version}/swagger.json"), string.Format($"{version.ToUpper()} Docs"));
                }
            });
        }

        private static IEnumerable<Type> GetSubClasses<T>()
        {
            return Assembly.GetCallingAssembly().GetTypes().Where(type => type.IsSubclassOf(typeof(T))).ToList();
        }
    }
}
