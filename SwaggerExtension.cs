using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.SwaggerGeneration.Processors.Security;

namespace KTSwagger
{
    /// <summary>
    /// Swagger Extension
    /// </summary>
    public static class SwaggerExtension
    {
        public class KTSwaggerOptionSetting
        {
            public string Title { get; set; } = "Services API";
            public string Description { get; set; } = "Services API";
            public string TermsOfService { get; set; } = "Services API";
            public Contact Contact { get; set; }
        }

        public class Contact
        {
            public string Name { get; set; }
            public string Email { get; set; }
        }


        /// <summary>
        /// add Swagger
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddKTSwagger(this IServiceCollection services, KTSwaggerOptionSetting optionSetting)
        {
            var provider = services.BuildServiceProvider()
                          .GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var apiVersionDescription in provider.ApiVersionDescriptions)
            {
                services.AddSwaggerDocument(document =>
                {
                    document.DocumentName = apiVersionDescription.GroupName;
                    document.ApiGroupNames = new[] { apiVersionDescription.GroupName };

                    document.PostProcess = d =>
                    {
                        d.Schemes = new[] { SwaggerSchema.Https, SwaggerSchema.Http };
                        d.Info.Version = apiVersionDescription.GroupName;
                        d.Info.Title = $"{optionSetting.Title} {apiVersionDescription.GroupName}";
                        d.Info.Description = $"{optionSetting.Description}";
                        d.Info.TermsOfService = $"{optionSetting.TermsOfService}";

                        if (optionSetting.Contact != null)
                        {
                            d.Info.Contact = new SwaggerContact
                            {
                                Email = optionSetting.Contact.Email,
                                Name = optionSetting.Contact.Name
                            };
                        }
                    };

                    document.DocumentProcessors.Add(
                        new SecurityDefinitionAppender("JWT Token", new SwaggerSecurityScheme
                        {
                            Type = SwaggerSecuritySchemeType.ApiKey,
                            Name = "Authorization",
                            In = SwaggerSecurityApiKeyLocation.Header,
                            Description = "Copy 'Bearer ' + valid JWT token into field"
                        }));
                    document.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT Token"));
                });
            }

            return services;
        }

        /// <summary>
        /// UseKTSwagger
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseKTSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUi3(config => config.TransformToExternalPath = (internalUiRoute, request) =>
            {
                if (internalUiRoute.StartsWith("/") == true && internalUiRoute.StartsWith(request.PathBase) == false)
                {
                    return request.PathBase + internalUiRoute;
                }
                else
                {
                    return internalUiRoute;
                }
            });

            return app;
        }
    }
}
