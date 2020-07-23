using Hangfire.Common;
using Hangfire.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.SpecifyQueue
{
    public class SpecifyQueueFilter : JobFilterAttribute, IElectStateFilter
    {
        public SpecifyQueueFilter()
        {
            Order = int.MaxValue;
        }

        public void OnStateElection(ElectStateContext context)
        {
            if (context.CandidateState is EnqueuedState enqueuedState)
            {
                var queue = context.GetJobParameter<string>(JobParam.Queue);
                if (!string.IsNullOrWhiteSpace(queue))
                {
                    enqueuedState.Queue = queue;
                }
            }
        }
    }

    public static class JobParam
    {
        public const string Queue = "QueueName";
    }
}
