using AutoGenerator.Conditions;
using System.Reflection;

namespace LAHJAAPI.V1.Validators.Conditions
{
    public static class ConfigValidator
    {
        public static IServiceCollection AddAutoValidator(this IServiceCollection serviceCollection)
        {
            Assembly? assembly = Assembly.GetExecutingAssembly();
            serviceCollection.AddScoped<ITFactoryInjector, TFactoryInjector>();
            serviceCollection.AddScoped<IConditionChecker>(pro =>
            {
                var injctor = pro.GetRequiredService<ITFactoryInjector>();
                var checker = new ConditionChecker(injctor);
                BaseConfigValidator.Register(checker, assembly);
                return checker;
            });

            return serviceCollection;
        }
    }
}