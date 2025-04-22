using Quartz;
using ApiCore.Validators;
using AutoGenerator.Schedulers;
using System.Reflection;
using System;
using LAHJAAPI.V1.Validators.Conditions;

namespace ApiCore.Schedulers
{
    public static class ConfigMScheduler
    {
        public static IServiceCollection AddAutoConfigScheduler(this IServiceCollection serviceCollection)
        {
            Assembly? assembly = Assembly.GetExecutingAssembly();
            serviceCollection.AddHostedService<JobScheduler>(pro =>
            {
                var jober = pro.GetRequiredService<ISchedulerFactory>();
                var checker = new ConditionChecker(null);
                var jobs = ConfigScheduler.getJobOptions(checker, assembly);
                return new JobScheduler(jober, jobs);
            });
            return serviceCollection;
        }
    }
}