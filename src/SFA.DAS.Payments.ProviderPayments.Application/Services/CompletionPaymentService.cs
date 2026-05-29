using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SFA.DAS.Payments.Application.Infrastructure.Logging;
using SFA.DAS.Payments.Model.Core.Entities;
using SFA.DAS.Payments.PeriodEnd.Messages.Events;
using SFA.DAS.Payments.ProviderPayments.Application.Repositories;
using SFA.DAS.Payments.ProviderPayments.Messages;
using SFA.DAS.Payments.ProviderPayments.Messages.Internal.Commands;

namespace SFA.DAS.Payments.ProviderPayments.Application.Services
{
    public interface ICompletionPaymentService
    {
        Task<IList<ICompletionEvent>> GetCompletionPaymentEvents(ProcessProviderMonthEndAct1CompletionPaymentCommand message);

        Task<List<ProcessProviderMonthEndAct1CompletionPaymentCommand>> GenerateProviderMonthEndAct1CompletionPaymentCommands(PeriodEndStoppedEvent collectionPeriod);
    }

    public class CompletionPaymentService : ICompletionPaymentService
    {
        private readonly IPaymentLogger logger;
        private readonly IMapper mapper;
        private readonly IProviderPaymentsRepository repository;

        public CompletionPaymentService(IPaymentLogger logger, IMapper mapper, IProviderPaymentsRepository repository)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IList<ICompletionEvent>> GetCompletionPaymentEvents(ProcessProviderMonthEndAct1CompletionPaymentCommand message)
        {
            logger.LogInfo($"now building Completion Payment Events for ukprn: {message.Ukprn} collection period: {message.CollectionPeriod.Period:00}-{message.CollectionPeriod.AcademicYear:0000}, job: {message.JobId}");

            var payments = await repository.GetMonthEndAct1CompletionPaymentsForProvider(message.Ukprn, message.CollectionPeriod).ConfigureAwait(false);

            var completionPaymentEvents = MapPaymentsToCompletionPaymentEvents(payments);

            logger.LogInfo($"Finished creating the provider period end events for collection period: {message.CollectionPeriod.Period:00}-{message.CollectionPeriod.AcademicYear:0000}, job: {message.JobId}");

            return completionPaymentEvents;
        }
        
        public async Task<List<ProcessProviderMonthEndAct1CompletionPaymentCommand>> GenerateProviderMonthEndAct1CompletionPaymentCommands(PeriodEndStoppedEvent message)
        {
            logger.LogInfo($"Building Completion Payment Events for collection period: {message.CollectionPeriod.Period:00}-{message.CollectionPeriod.AcademicYear:0000}, job: {message.JobId}");

            var providers = await repository.GetProvidersWithAct1CompletionPayments(message.CollectionPeriod).ConfigureAwait(false);

            var commands = providers
                .Select(ukprn => new ProcessProviderMonthEndAct1CompletionPaymentCommand
                {
                    Ukprn = ukprn,
                    CollectionPeriod = message.CollectionPeriod,
                    JobId = message.JobId
                }).ToList();

            logger.LogInfo($"Finished creating provider Completion Payment events for collection period: {message.CollectionPeriod.Period:00}-{message.CollectionPeriod.AcademicYear:0000}, job: {message.JobId}");

            return commands;
        }
        private IList<ICompletionEvent> MapPaymentsToCompletionPaymentEvents(List<PaymentModel> payments)
        {
            var completionPaymentEvents = new List<ICompletionEvent>();

            var shortCourseCompletionPayments = payments.Where(x => x.CourseType == CourseType.ShortCourse).ToList();
            completionPaymentEvents.AddRange(mapper.Map<IList<RecordedShortCourseCompletionPayment>>(shortCourseCompletionPayments));
            // exclude from further processing
            shortCourseCompletionPayments.ForEach(shortCoursePayment => payments.Remove(shortCoursePayment));

            completionPaymentEvents.AddRange(mapper.Map<IList<RecordedAct1CompletionPayment>>(payments));
            // return aggregated completion payment events
            return completionPaymentEvents;
        }
    }
}