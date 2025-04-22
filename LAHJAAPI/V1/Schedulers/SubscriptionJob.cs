using ApiCore.Helper;
using ApiCore.Validators;
using AutoGenerator.Notifications;
using AutoGenerator.Schedulers;
using AutoNotificationService;
using AutoNotificationService.Services.Email;
using AutoNotificationService.Services.Sms;
using LAHJAAPI.Models;
using LAHJAAPI.V1.Validators.Conditions;
using Microsoft.EntityFrameworkCore;
using System;

namespace ApiCore.Schedulers
{
    public class SubscriptionJob : BaseJob
    {
        private readonly IConditionChecker _checker;
        public SubscriptionJob(IConditionChecker checker) : base()
        {
            _checker = checker;


        }
        static bool isons = false;

        static string confirmationLink = "https://example.com/confirm?token=123456";
        public static bool IsExpiringSoon(Subscription subscription, int daysBeforeEnd=3 )
        {


            var ischeeck= subscription.Status.ToLower() == "active" &&
                   (subscription.CurrentPeriodEnd - DateTime.UtcNow).TotalDays <= 10 &&
                   subscription.CurrentPeriodEnd > DateTime.UtcNow;

            return ischeeck;
        }


         List<Subscription> GetSubscriptions()
        {

            var subscriptions =  _checker.Injector.Context.Set<Subscription>().Include(p=>p.User).Where(IsExpiringSoon).AsParallel().ToList();//.Where(x => IsExpiringSoon(x, 3)).ToListAsync();
            if (subscriptions.Count == 0)
            {
                Console.WriteLine("No subscriptions expiring soon.");
            }
            else
            {
                Console.WriteLine($"Found {subscriptions.Count} subscriptions expiring soon.");
            }
            return subscriptions;
        }


        public override async Task Execute(JobEventArgs context)
        {   // „À«· 
            if (!isons)
            {

                var subscriptions =  GetSubscriptions();

                foreach (var sub in subscriptions)
                {


                    //await _checker.Injector.Notifier.NotifyAsyn(new EmailModel()
                    //{
                    //    Body = TemplateTagEmail.SubscriptionExpiringSoonTemplate(sub.User.UserName, sub.CurrentPeriodEnd),
                    //    Subject = "LAHJA-API",
                    //    ToEmail = sub.User.Email,
                    //});


                  

                  
                    isons = true;
                }


            }

            Console.WriteLine($"Executing job: {_options.JobName} with cron: {_options.Cron}");

        }

        protected override void InitializeJobOptions()
        {
            // _options.
            _options.JobName = "Subscription1";
            _options.Cron = CronSchedule.EveryMinute;


        }
    }
}