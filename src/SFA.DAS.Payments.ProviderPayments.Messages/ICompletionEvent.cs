using SFA.DAS.Payments.Messages.Common.Events;
using SFA.DAS.Payments.Model.Core;
using SFA.DAS.Payments.Model.Core.Entities;
using System;

namespace SFA.DAS.Payments.ProviderPayments.Messages
{
    public interface ICompletionEvent : IPaymentsEvent
    {
        long JobId { get; }
        Guid EventId { get; set; }
        DateTimeOffset EventTime { get; set; }
        long Ukprn { get; set; }
        Learner Learner { get; set; }
        LearningAim LearningAim { get; set; }
        DateTime IlrSubmissionDateTime { get; set; }
        CollectionPeriod CollectionPeriod { get; set; }
        decimal AmountDue { get; set; }
        byte DeliveryPeriod { get; set; }
        long? AccountId { get; set; }
        long? TransferSenderAccountId { get; set; }
        ContractType ContractType { get; set; }
        TransactionType TransactionType { get; set; }
        decimal SfaContributionPercentage { get; set; }
        long? ApprenticeshipId { get; set; }
        ApprenticeshipEmployerType ApprenticeshipEmployerType { get; set; }
        string ReportingAimFundingLineType { get; set; }
        FundingSourceType FundingSource { get; set; }
        EarningDetails EarningDetails { get; set; }
        CourseType CourseType { get; set; }
        LearningType LearningType { get; set; }
        string CourseCode { get; set; }
    }
}
