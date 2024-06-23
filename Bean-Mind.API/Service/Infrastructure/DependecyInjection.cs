using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bean_Mind_Business.Infrastructure
{
    public static class DependecyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddQuartz(options =>
            {
                options.UseMicrosoftDependencyInjectionJobFactory();

                var jobKey = JobKey.Create(nameof(BackupJob));

                options
                    .AddJob<BackupJob>(jobKey)
                    .AddTrigger(trigger => trigger
                                .ForJob(jobKey)
                                .WithSimpleSchedule(schedule =>
                                    schedule.WithIntervalInHours(720).RepeatForever()));
            });

            services.AddQuartzHostedService();
        }
    }
}
