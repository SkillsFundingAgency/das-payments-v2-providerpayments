﻿using System;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Payments.Application.Infrastructure.Logging;
using SFA.DAS.Payments.Model.Core.Entities;
using SFA.DAS.Payments.Monitoring.Jobs.Model;
using SFA.DAS.Payments.ProviderPayments.Application.Data;

namespace SFA.DAS.Payments.ProviderPayments.Application.Services
{
    public class CollectionPeriodStorageService : ICollectionPeriodStorageService
    {
        private readonly IProviderPaymentsDataContext context;
        private readonly IPaymentLogger logger;

        public CollectionPeriodStorageService(IProviderPaymentsDataContext context, IPaymentLogger logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task StoreCollectionPeriod(short academicYear, byte period, DateTime completionDateTime)
        {
            if(context.CollectionPeriod.Any(x => x.AcademicYear == academicYear && x.Period == period))
                return;

            var referenceDataValidationDate = GetReferenceDataValidationDate(academicYear, period);
            if(referenceDataValidationDate == null)
                logger.LogWarning($"Failed to find successful PeriodEndSubmissionWindowValidationJob for academic year: {academicYear} and period: {period} with an EndTime set");

            await context.CollectionPeriod.AddAsync(new CollectionPeriodModel
            {
                AcademicYear = academicYear,
                Period = period,
                CompletionDate = completionDateTime,
                ReferenceDataValidationDate = referenceDataValidationDate
            });
            await context.SaveChanges();
        }

        private DateTime? GetReferenceDataValidationDate(short academicYear, byte period)
        {
            return context.Job.Where(x => x.JobType == JobType.PeriodEndSubmissionWindowValidationJob
                                    && x.AcademicYear == academicYear
                                    && x.CollectionPeriod == period
                                    && x.EndTime != null
                                    && x.Status == JobStatus.Completed)
                .OrderByDescending(x => x.EndTime)
                .FirstOrDefault()?.EndTime?.DateTime;
        }
    }
}