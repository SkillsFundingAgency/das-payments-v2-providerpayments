using SFA.DAS.Payments.Model.Core;
using SFA.DAS.Payments.Model.Core.Entities;
using System;

namespace SFA.DAS.Payments.ProviderPayments.Messages
{
    public class RecordedShortCourseCompletionPayment : ICompletionEvent
    {
        public long JobId { get; }
        public Guid EventId { get; set; }
        public DateTimeOffset EventTime { get; set; }
        public long Ukprn { get; set; }
        public Learner Learner { get; set; }
        public LearningAim LearningAim { get; set; }
        public DateTime IlrSubmissionDateTime { get; set; }
        public CollectionPeriod CollectionPeriod { get; set; }
        public decimal AmountDue { get; set; }
        public byte DeliveryPeriod { get; set; }
        public long? AccountId { get; set; }
        public long? TransferSenderAccountId { get; set; }
        public ContractType ContractType { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal SfaContributionPercentage { get; set; }
        public long? ApprenticeshipId { get; set; }
        public ApprenticeshipEmployerType ApprenticeshipEmployerType { get; set; }
        public string ReportingAimFundingLineType { get; set; }
        public FundingSourceType FundingSource { get; set; }
        public EarningDetails EarningDetails { get; set; }
        public CourseType CourseType { get; set; }
        public LearningType LearningType { get; set; }
        public string CourseCode { get; set; }
    }
}
