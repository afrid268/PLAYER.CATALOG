using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Catalog.Service.Repositories;
using Play.Catalog.Service.Settings;

namespace Play.Catalog.Service
{
    public class Startup
    {
        private ServiceSettings serviceSettings;
        private MongoDBSettings MongoDBSettings;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            serviceSettings = Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();//constrtuct service

            services.AddSingleton(serviceProvider =>
            {
                var mongoDbSettings = Configuration.GetSection(nameof(MongoDBSettings)).Get<MongoDBSettings>();
                var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
                return mongoClient.GetDatabase(serviceSettings.ServiceName);//helps us inject the service and mongo connection
            });//allows only one instance throughout by singleton

            services.AddSingleton<IItemRepository, ItemRepository>();//declare the rejistration type to the service and construct when needed

            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String)); // solves mongo db serialisation of guid,date and decimal and its representation
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DecimalSerializer(BsonType.String));

            services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false; //at run time asp .net core will not remove async suffix to methods
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Play.Catalog.Service", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Play.Catalog.Service v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
