using Hangfire.Annotations;
using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.States;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.SpecifyQueue
{
    public static class HangfireServiceCollectionExtensions
    {
        public static IServiceCollection AddHangfireSpecifyQueue(
            [NotNull] this IServiceCollection services)
        {
            GlobalJobFilters.Filters.Add(new SpecifyQueueFilter());

            services.AddSingleton<IBackgroundJobClient>(x =>
            {
                if (GetInternalServices(x, out var factory, out var stateChanger, out _))
                {
                    return new SpecifyQueueBackgroundJobClient(x.GetRequiredService<JobStorage>(), factory, stateChanger);
                }

                return new SpecifyQueueBackgroundJobClient(
                    x.GetRequiredService<JobStorage>(),
                    x.GetRequiredService<IJobFilterProvider>());
            });

            return services;
        }

        internal static bool GetInternalServices(
            IServiceProvider provider,
            out IBackgroundJobFactory factory,
            out IBackgroundJobStateChanger stateChanger,
            out IBackgroundJobPerformer performer)
        {
            factory = provider.GetService<IBackgroundJobFactory>();
            performer = provider.GetService<IBackgroundJobPerformer>();
            stateChanger = provider.GetService<IBackgroundJobStateChanger>();

            if (factory != null && performer != null && stateChanger != null)
            {
                return true;
            }

            factory = null;
            performer = null;
            stateChanger = null;

            return false;
        }
    }
}
