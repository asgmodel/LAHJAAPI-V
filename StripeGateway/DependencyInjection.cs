using Microsoft.Extensions.Configuration;
using Stripe;
using StripeGateway;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IStripePrice, StripePrice>();
        services.AddScoped<IStripeProduct, StripeProduct>();
        services.AddScoped<IStripeCheckout, StripeCheckout>();
        services.AddScoped<IStripeWebhook, StripeWebhook>();
        services.AddScoped<IStripeCustomer, StripeCustomer>();
        services.AddScoped<IStripeSession, StripeSession>();
        services.AddScoped<IStripeSubscription, StripeSubscription>();
        services.AddScoped<IStripeInvoice, StripeInvoice>();
        services.AddScoped<IStripePaymentIntent, StripePaymentIntent>();
        services.AddScoped<IStripePaymentMethod, StripePaymentMethod>();
        services.AddScoped<IStripeSetupIntent, StripeSetupIntent>();


        services.AddAutoMapper(typeof(StripeMappingConfig));
        return services;
    }
    public static IServiceCollection AddStripeGateway(this IServiceCollection services,
        StripeInfo appInfo,
        IConfiguration configuration,
        string stripeOptions = "stripe:options")
    {
        StripeConfiguration.AppInfo = appInfo;

        services.AddOptions<StripeOptions>().BindConfiguration(stripeOptions);
        StripeConfiguration.ApiKey = configuration["stripe:options:SecretKey"];
        return services.AddServices();
    }


    public static IServiceCollection AddStripeGateway(this IServiceCollection services,
        IConfiguration configuration,
        string stripeOptions = "stripe:options", string stripeInfo = "stripe:info")
    {


        //StripeOptions settings = new StripeOptions();      
        //builder.Configuration.Bind(settings);


        services.AddOptions<StripeOptions>().BindConfiguration(stripeOptions);
        var appInfo = configuration.GetSection(stripeInfo).Get<StripeInfo>();
        StripeConfiguration.AppInfo = appInfo;
        StripeConfiguration.ApiKey = configuration["stripe:options:SecretKey"];
        return services.AddServices();
    }
}
