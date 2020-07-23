using Hangfire.Common;
using Hangfire.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.SpecifyQueue
{
    public interface ISpecifiableClient
    {
        string Create(string queueName, Job job, IState state);
    }
}
