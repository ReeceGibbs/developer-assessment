using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TodoList.Api.Filters;
using TodoList.Api.Middleware;
using TodoList.Api.Swashbuckle;
using TodoList.Common.Mappings;
using TodoList.Common.Models.TodoItem;
using TodoList.Infrastructure.Data.Contexts;
using TodoList.Service.Services;

namespace TodoList.Api
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
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                                 .AllowAnyHeader()
                                 .AllowAnyMethod();
                      });
            });

            // We use custom operation filters to cater for the custom model binders when using swagger
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.OperationFilter<TodoItemOperationFilter>();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TodoList.Api", Version = "v1" });
            });

            services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoItemsDB"));
            services.AddScoped<ITodoContext, TodoContext>();
            services.AddScoped<ITodoItemsService, TodoItemsService>();

            services.AddControllers(options =>
            {
                options.Filters.Add<ApiExceptionFilterAttribute>();
                options.Filters.Add<ApiAuthFilterAttribute>();
            })
            .AddFluentValidation(options =>
                options.RegisterValidatorsFromAssemblyContaining<TodoItemRequestDtoValidator>());

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new TodoItemMappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.DefaultModelsExpandDepth(-1);
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoList.Api v1");
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowAllHeaders");

            app.UseAuthorization();

            app.UseMiddleware<RequestLoggingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
