using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.SpecifyQueue
{
    using Hangfire;
    using Hangfire.Annotations;
    using Hangfire.Client;
    using Hangfire.Common;
    using Hangfire.States;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class SpecifyQueueBackgroundJobClient : ISpecifiableClient, IBackgroundJobClient
    {
        private readonly JobStorage _storage;
        private readonly IBackgroundJobFactory _factory;
        private readonly IBackgroundJobStateChanger _stateChanger;

        public SpecifyQueueBackgroundJobClient()
            : this(JobStorage.Current)
        {
        }

        public SpecifyQueueBackgroundJobClient([NotNull] JobStorage storage)
            : this(storage, JobFilterProviders.Providers)
        {
        }

        public SpecifyQueueBackgroundJobClient([NotNull] JobStorage storage, [NotNull] IJobFilterProvider filterProvider)
            : this(storage, new BackgroundJobFactory(filterProvider), new BackgroundJobStateChanger(filterProvider))
        {
        }

        public SpecifyQueueBackgroundJobClient(
            [NotNull] JobStorage storage,
            [NotNull] IBackgroundJobFactory factory,
            [NotNull] IBackgroundJobStateChanger stateChanger)
        {
            if (storage == null) throw new ArgumentNullException(nameof(storage));
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (stateChanger == null) throw new ArgumentNullException(nameof(stateChanger));

            _storage = storage;
            _stateChanger = stateChanger;
            _factory = factory;
        }

        public bool ChangeState(string jobId, IState state, string expectedState)
        {
            if (jobId == null) throw new ArgumentNullException(nameof(jobId));
            if (state == null) throw new ArgumentNullException(nameof(state));

            try
            {
                using (var connection = _storage.GetConnection())
                {
                    var appliedState = _stateChanger.ChangeState(new StateChangeContext(
                        _storage,
                        connection,
                        jobId,
                        state,
                        expectedState != null ? new[] { expectedState } : null));

                    return appliedState != null && appliedState.Name.Equals(state.Name, StringComparison.OrdinalIgnoreCase);
                }
            }
            catch (Exception ex)
            {
                throw new BackgroundJobClientException("State change of a background job failed. See inner exception for details", ex);
            }
        }

        public string Create(Job job, IState state)
        {
            if (job == null) throw new ArgumentNullException(nameof(job));
            if (state == null) throw new ArgumentNullException(nameof(state));

            try
            {
                using (var connection = _storage.GetConnection())
                {
                    var context = new CreateContext(_storage, connection, job, state);
                    var backroundJob = _factory.Create(context);

                    return backroundJob?.Id;
                }
            }
            catch (Exception ex)
            {
                throw new BackgroundJobClientException("Background job creation failed. See inner exception for details.", ex);
            }
        }

        public string Create(string queueName, Job job, IState state)
        {
            if (job == null) throw new ArgumentNullException(nameof(job));
            if (state == null) throw new ArgumentNullException(nameof(state));

            try
            {
                using (var connection = _storage.GetConnection())
                {
                    var context = new CreateContext(_storage, connection, job, state);
                    context.Parameters.Add(JobParam.Queue, queueName);
                    var backroundJob = _factory.Create(context);

                    return backroundJob?.Id;
                }
            }
            catch (Exception ex)
            {
                throw new BackgroundJobClientException("Background job creation failed. See inner exception for details.", ex);
            }
        }
    }
}
