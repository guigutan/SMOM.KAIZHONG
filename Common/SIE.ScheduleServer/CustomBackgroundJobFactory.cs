using Hangfire;
using Hangfire.Client;
using Hangfire.Server;
using Hangfire.States;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SIE.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SIE.ScheduleServer
{
    internal class CustomBackgroundJobFactory : IBackgroundJobFactory
    {
        private readonly IBackgroundJobFactory _inner;

        public CustomBackgroundJobFactory(IBackgroundJobFactory inner)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        public IStateMachine StateMachine => _inner.StateMachine;

        public BackgroundJob Create(CreateContext context)
        {
            Console.WriteLine($"Create: {context.Job.Type.FullName}.{context.Job.Method.Name} in {context.InitialState?.Name} state");
            return _inner.Create(context);
        }
    }


    internal class CustomBackgroundJobPerformer : IBackgroundJobPerformer
    {
        private readonly IBackgroundJobPerformer _inner;

        public CustomBackgroundJobPerformer(IBackgroundJobPerformer inner)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        public object Perform(PerformContext context)
        {
            DistributionContext.Clear();
            try
            {
                Console.WriteLine($"Perform {context.BackgroundJob.Id} ({context.BackgroundJob.Job.Type.FullName}.{context.BackgroundJob.Job.Method.Name})");
                return _inner.Perform(context);
            }
            finally
            {
                DistributionContext.Clear();
            }
        }
    }

    internal class CustomBackgroundJobStateChanger : IBackgroundJobStateChanger
    {
        private readonly IBackgroundJobStateChanger _inner;

        public CustomBackgroundJobStateChanger(IBackgroundJobStateChanger inner)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        public IState ChangeState(StateChangeContext context)
        {
            Console.WriteLine($"ChangeState {context.BackgroundJobId} to {context.NewState}");
            return _inner.ChangeState(context);
        }
    }

    internal class RecurringJobsService : BackgroundService
    {
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly IRecurringJobManager _recurringJobs;
        private readonly ILogger<RecurringJobScheduler> _logger;

        public RecurringJobsService(
            IBackgroundJobClient backgroundJobs,
            IRecurringJobManager recurringJobs,
            ILogger<RecurringJobScheduler> logger)
        {
            _backgroundJobs = backgroundJobs ?? throw new ArgumentNullException(nameof(backgroundJobs));
            _recurringJobs = recurringJobs ?? throw new ArgumentNullException(nameof(recurringJobs));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
            }
            catch (Exception e)
            {
                _logger.LogError("An exception occurred while creating recurring jobs.", e);
            }

            return Task.CompletedTask;
        }
    }
}
