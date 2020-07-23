using Hangfire.Annotations;
using Hangfire.Common;
using Hangfire.States;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.SpecifyQueue
{
    /// <summary>
    /// Provides extension methods for the <see cref="IBackgroundJobClient"/>
    /// interface to simplify the creation of fire-and-forget jobs, delayed 
    /// jobs, continuations and other background jobs in well-known states.
    /// Also allows to re-queue and delete existing background jobs.
    /// </summary>
    public static class BackgroundJobClientExtensions
    {
        public static string Enqueue(
            [NotNull] this IBackgroundJobClient client,
            [NotNull] string queueName,
            [NotNull, InstantHandle] Expression<Action> methodCall)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            return client.Create(queueName, methodCall, new EnqueuedState());
        }

        public static string Enqueue(
            [NotNull] this IBackgroundJobClient client,
            [NotNull] string queueName,
            [NotNull, InstantHandle] Expression<Func<Task>> methodCall)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            return client.Create(queueName, methodCall, new EnqueuedState());
        }

        public static string Enqueue<T>(
            [NotNull] this IBackgroundJobClient client,
            [NotNull] string queueName,
            [NotNull, InstantHandle] Expression<Action<T>> methodCall)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            return client.Create(queueName, methodCall, new EnqueuedState());
        }

        public static string Enqueue<T>(
            [NotNull] this IBackgroundJobClient client,
            [NotNull] string queueName,
            [NotNull, InstantHandle] Expression<Func<T, Task>> methodCall)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            return client.Create(queueName, methodCall, new EnqueuedState());
        }

        public static string Schedule(
            [NotNull] this IBackgroundJobClient client,
            [NotNull] string queueName,
            [NotNull, InstantHandle] Expression<Action> methodCall,
            TimeSpan delay)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            return client.Create(queueName, methodCall, new ScheduledState(delay));
        }

        public static string Schedule(
            [NotNull] this IBackgroundJobClient client,
            [NotNull] string queueName,
            [NotNull, InstantHandle] Expression<Func<Task>> methodCall,
            TimeSpan delay)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            return client.Create(queueName, methodCall, new ScheduledState(delay));
        }

        public static string Schedule(
            [NotNull] this IBackgroundJobClient client,
            [NotNull] string queueName,
            [NotNull, InstantHandle] Expression<Action> methodCall,
            DateTimeOffset enqueueAt)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            return client.Create(queueName, methodCall, new ScheduledState(enqueueAt.UtcDateTime));
        }

        public static string Schedule(
            [NotNull] this IBackgroundJobClient client,
            [NotNull] string queueName,
            [NotNull, InstantHandle] Expression<Func<Task>> methodCall,
            DateTimeOffset enqueueAt)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            return client.Create(queueName, methodCall, new ScheduledState(enqueueAt.UtcDateTime));
        }

        public static string Schedule<T>(
            [NotNull] this IBackgroundJobClient client,
            [NotNull] string queueName,
            [NotNull, InstantHandle] Expression<Action<T>> methodCall,
            TimeSpan delay)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            return client.Create(queueName, methodCall, new ScheduledState(delay));
        }

        public static string Schedule<T>(
            [NotNull] this IBackgroundJobClient client,
            [NotNull] string queueName,
            [NotNull, InstantHandle] Expression<Func<T, Task>> methodCall,
            TimeSpan delay)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            return client.Create(queueName, methodCall, new ScheduledState(delay));
        }

        public static string Schedule<T>(
            [NotNull] this IBackgroundJobClient client,
            [NotNull] string queueName,
            [NotNull, InstantHandle] Expression<Action<T>> methodCall,
            DateTimeOffset enqueueAt)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            return client.Create(queueName, methodCall, new ScheduledState(enqueueAt.UtcDateTime));
        }

        public static string Schedule<T>(
            [NotNull] this IBackgroundJobClient client,
            [NotNull] string queueName,
            [NotNull, InstantHandle] Expression<Func<T, Task>> methodCall,
            DateTimeOffset enqueueAt)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            return client.Create(queueName, methodCall, new ScheduledState(enqueueAt.UtcDateTime));
        }

        public static string Create(
            [NotNull] this IBackgroundJobClient client,
            [NotNull] string queueName,
            [NotNull, InstantHandle] Expression<Action> methodCall,
            [NotNull] IState state)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            return client.Create(queueName, Job.FromExpression(methodCall), state);
        }

        public static string Create(
            [NotNull] this IBackgroundJobClient client,
            string queueName,
            [NotNull, InstantHandle] Expression<Func<Task>> methodCall,
            [NotNull] IState state)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            return client.Create(queueName, Job.FromExpression(methodCall), state);
        }

        public static string Create<T>(
            [NotNull] this IBackgroundJobClient client,
            string queueName,
            [NotNull, InstantHandle] Expression<Action<T>> methodCall,
            [NotNull] IState state)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            
            return client.Create(queueName, Job.FromExpression(methodCall), state);
        }

        public static string Create<T>(
            [NotNull] this IBackgroundJobClient client,
            string queueName,
            [NotNull, InstantHandle] Expression<Func<T, Task>> methodCall,
            [NotNull] IState state)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            return client.Create(queueName, Job.FromExpression(methodCall), state);
        }

        public static string Create(
            [NotNull] this IBackgroundJobClient client,
            [NotNull] string queueName,
            [NotNull] Job job,
            [NotNull] IState state)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            if (!string.IsNullOrWhiteSpace(queueName)
                && client is ISpecifiableClient specifiableClient)
            {
                return specifiableClient.Create(queueName, job, state);
            }
            else
            {
                return client.Create(job, state);
            }
        }

        public static string ContinueJobWith(
            [NotNull] this IBackgroundJobClient client,
            [NotNull] string parentId,
            [NotNull] string queueName,
            [NotNull, InstantHandle] Expression<Action> methodCall)
        {
            return ContinueJobWith(client, parentId, queueName, methodCall, new EnqueuedState());
        }

        public static string ContinueJobWith<T>(
            [NotNull] this IBackgroundJobClient client,
            [NotNull] string parentId,
            [NotNull] string queueName,
            [NotNull, InstantHandle] Expression<Action<T>> methodCall)
        {
            return ContinueJobWith(client, parentId, queueName, methodCall, new EnqueuedState());
        }

        public static string ContinueJobWith(
            [NotNull] this IBackgroundJobClient client,
            [NotNull] string parentId,
            [NotNull] string queueName,
            [NotNull, InstantHandle] Expression<Action> methodCall,
            [NotNull] IState nextState)
        {
            return ContinueJobWith(client, parentId, queueName, methodCall, nextState, JobContinuationOptions.OnlyOnSucceededState);
        }

        public static string ContinueJobWith<T>(
            [NotNull] this IBackgroundJobClient client,
            [NotNull] string parentId,
            [NotNull] string queueName,
            [NotNull, InstantHandle] Expression<Action<T>> methodCall,
            [NotNull] IState nextState)
        {
            return ContinueJobWith(client, parentId, queueName, methodCall, nextState, JobContinuationOptions.OnlyOnSucceededState);
        }

        public static string ContinueJobWith(
            [NotNull] this IBackgroundJobClient client,
            [NotNull] string parentId,
            [NotNull] string queueName,
            [NotNull, InstantHandle] Expression<Action> methodCall,
            JobContinuationOptions options)
        {
            return ContinueJobWith(client, parentId, queueName, methodCall, new EnqueuedState(), options);
        }

        public static string ContinueJobWith<T>(
            [NotNull] this IBackgroundJobClient client,
            [NotNull] string parentId,
            [NotNull] string queueName,
            [NotNull, InstantHandle] Expression<Action<T>> methodCall,
            JobContinuationOptions options)
        {
            return ContinueJobWith(client, parentId, queueName, methodCall, new EnqueuedState(), options);
        }

        public static string ContinueJobWith(
            [NotNull] this IBackgroundJobClient client,
            [NotNull] string parentId,
            [NotNull] string queueName,
            [InstantHandle] Expression<Action> methodCall,
            [NotNull] IState nextState,
            JobContinuationOptions options)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            var state = new AwaitingState(parentId, nextState, options);
            return client.Create(queueName, Job.FromExpression(methodCall), state);
        }

        public static string ContinueJobWith(
            [NotNull] this IBackgroundJobClient client,
            [NotNull] string parentId,
            [NotNull] string queueName,
            [InstantHandle] Expression<Func<Task>> methodCall,
            [CanBeNull] IState nextState = null,
            JobContinuationOptions options = JobContinuationOptions.OnlyOnSucceededState)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            var state = new AwaitingState(parentId, nextState ?? new EnqueuedState(), options);
            return client.Create(queueName, Job.FromExpression(methodCall), state);
        }

        public static string ContinueJobWith<T>(
            [NotNull] this IBackgroundJobClient client,
            [NotNull] string parentId,
            [NotNull] string queueName,
            [NotNull, InstantHandle] Expression<Action<T>> methodCall,
            [NotNull] IState nextState,
            JobContinuationOptions options)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            var state = new AwaitingState(parentId, nextState, options);
            return client.Create(queueName, Job.FromExpression(methodCall), state);
        }

        public static string ContinueJobWith<T>(
            [NotNull] this IBackgroundJobClient client,
            [NotNull] string parentId,
            [NotNull] string queueName,
            [NotNull, InstantHandle] Expression<Func<T, Task>> methodCall,
            [CanBeNull] IState nextState = null,
            JobContinuationOptions options = JobContinuationOptions.OnlyOnSucceededState)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            var state = new AwaitingState(parentId, nextState ?? new EnqueuedState(), options);
            return client.Create(queueName, Job.FromExpression(methodCall), state);
        }
    }
}
