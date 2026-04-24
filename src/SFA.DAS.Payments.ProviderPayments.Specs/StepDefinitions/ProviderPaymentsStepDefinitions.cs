using Bogus.DataSets;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework.Interfaces;
using Reqnroll;
using SFA.DAS.Payments.ProviderPayments.Specs.Handlers;
using SFA.DAS.Payments.ProviderPayments.Specs.StepDefinitions;
using SFA.DAS.Payments.Model.Core;
using SFA.DAS.Payments.Model.Core.Entities;
using Database = UUIDNext.Database;

namespace SFA.DAS.Payments.ProviderPayments.Specs.StepDefinitions
{
    [Binding]
    public class ProviderPaymentsStepDefinitions
    {
        private readonly ScenarioContext scenarioContext;
        private readonly MessagingContext messagingContext;
        private TestSession testSession;
        private Guid previousIdentifier;
        private TransactionType transactionType;
        public List< PaymentModel> Payments { get; set; }

        public ProviderPaymentsStepDefinitions(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }

        protected void SetCurrentCollectionYear()
        {
            testSession.CurrentPeriod = new CollectionPeriodBuilder().WithDate(DateTime.Today).Build();
        }

        [BeforeScenario]
        public async Task BeforeScenario()
        {
            testSession = new TestSession();
            await testSession.DataContext.ClearCollectionPeriodsData();
            SetCurrentCollectionYear();
            Console.WriteLine($"UKPRN : {testSession.Provider.Ukprn}, ULN: {testSession.Learner.Uln}, collection year: {testSession.CurrentPeriod.AcademicYear}");
        }

        [AfterScenario]
        public void AfterScenario()
        {
        }

        [Given("the collection period has opened recently")]
        [Given("that the collection period has opened recently")]
        public async Task GivenThatTheCollectionPeriodHasOpenedRecently()
        {
            testSession.CurrentPeriod = new CollectionPeriodBuilder().WithDate(DateTime.Today).Build();
            testSession.DataContext.CollectionPeriods.Add(new CollectionPeriodModel
            {
                AcademicYear = testSession.CurrentPeriod.AcademicYear,
                CalendarMonth = (byte)DateTime.Today.Month,
                CalendarYear = (byte)DateTime.Today.Year,
                CompletionDate = DateTime.Today,
                EndDateTime = null,
                Period = testSession.CurrentPeriod.Period,
                ReferenceDataValidationDate = null,
                StartDateTime = DateTime.Today,
                Status = CollectionPeriodStatus.Open
            });
            await testSession.DataContext.SaveChangesAsync();
        }


        private bool IsLaterThan(Guid previousEventId, Guid newEventId)
        {
            var comparer = new UUIDNext.Tools.GuidComparer();
            Console.WriteLine($"Comparing previous guid: {previousEventId} to new guid: {newEventId}");
            return comparer.Compare(newEventId, previousEventId) > 0;
        }

        [Given("the learner is undertaking a short course")]
        [Given("the learning is undertaking an Apprenticeship Unit short course")]
        public void GivenTheLearningIsUndertakingAnApprenticeshipUnitShortCourse()
        {
            testSession.Learner.Course.CourseType = CourseType.ShortCourse;
            testSession.Learner.Course.CourseCode = "ZSC00001";
            testSession.Learner.Course.LearningType = LearningType.ApprenticeshipUnit;
        }

        [Given("the provider has delivered the first milestone of the short course")]
        public void GivenTheProviderHasDeliveredTheFirstMilestoneOfTheShortCourse()
        {
            transactionType = TransactionType.Milestone1;
        }

        [Given("the provider has recorded the completion of the short course")]
        public void GivenTheProviderHasRecordedTheCompletionOfTheShortCourse()
        {
            transactionType = TransactionType.Completion;
        }

        [Given("the initial earnings have been processed by the payments system")]
        public async Task GivenTheInitialEarningsHaveBeenProcessedByThePaymentsSystem()
        {
            await testSession.DataContext.Payment.AddAsync(new PaymentModel
            {
                EarningEventId = testSession.PreviousEarningsId,
                AccountId = 12345,
                ActualEndDate = null,
                AgeAtStartOfLearning = testSession.Learner.Age,
                ApprenticeshipEmployerType = testSession.Learner.IsLevyLearner ? ApprenticeshipEmployerType.Levy : ApprenticeshipEmployerType.NonLevy,
                Amount = 1000,
                TransactionType = transactionType,
                CollectionPeriod = testSession.CurrentPeriod,
                ApprenticeshipId = 123,
                CompletionAmount = 700,
                CompletionStatus = 1,
                ContractType = ContractType.Act1,
                DeliveryPeriod = 1,
                CourseCode = testSession.Learner.Course.CourseCode,
                CourseType = testSession.Learner.Course.CourseType,
                LearningAimFrameworkCode = testSession.Learner.Course.FrameworkCode,
                LearningAimFundingLineType = testSession.Learner.Course.FundingLineType,
                LearningAimStandardCode = testSession.Learner.Course.StandardCode,
                LearningAimProgrammeType = testSession.Learner.Course.ProgrammeType,
                LearningAimPathwayCode = testSession.Learner.Course.PathwayCode,
                LearningAimReference = testSession.Learner.Course.LearnAimRef,
                LearningType = testSession.Learner.Course.LearningType,
                EventId = Guid.NewGuid(),
                FundingSource = FundingSourceType.Levy,
                FundingSourceEventId = Guid.NewGuid(),
                FundingPlatformType = FundingPlatformType.DigitalApprenticeshipService,
                InstalmentAmount = 300,
                LearnerReferenceNumber = testSession.Learner.LearnRefNumber,
                LearnerUln = testSession.Learner.Uln,
                LearningStartDate = testSession.Learner.Course.LearningStartDate,
                NumberOfInstalments = 1,
                PlannedEndDate = testSession.Learner.Course.LearningPlannedEndDate,
                ReportingAimFundingLineType = testSession.Learner.Course.FundingLineType,
                SfaContributionPercentage = .95m,
                StartDate = testSession.Learner.Course.LearningStartDate,
                Ukprn = testSession.Provider.Ukprn
            });
            await testSession.DataContext.SaveChangesAsync();
        }

        [When("the employers Levy account is used to fund the milestone payment at period end")]
        public async Task WhenTheEmployersLevyAccountIsUsedToFundTheMilestonePaymentAtPeriodEnd()
        {
            var fundingSourceEvent = new SFA.DAS.Payments.FundingSource.Messages.Events.LevyFundingSourcePaymentEvent
            {
                AgreementId = Guid.NewGuid().ToString("N"),
                Learner = new Learner
                {
                    ReferenceNumber = testSession.Learner.LearnRefNumber,
                    Uln = testSession.Learner.Uln
                },
                CourseType = testSession.Learner.Course.CourseType,
                AccountId = 1,
                ActualEndDate = null,
                AgeAtStartOfLearning = testSession.Learner.Age,
                AmountDue = 300,
                ApprenticeshipEmployerType = ApprenticeshipEmployerType.Levy,
                ApprenticeshipId = 1234567,
                CollectionPeriod = testSession.CurrentPeriod,
                CompletionAmount = 700,
                CompletionStatus = 1,
                ContractType = ContractType.Act1,
                DeliveryPeriod = 1,
                EarningEventId = testSession.CurrentEarningsId,
                EventId = Guid.NewGuid(),
                EventTime = DateTime.UtcNow,
                FundingPlatformType = FundingPlatformType.DigitalApprenticeshipService,
                FundingSourceType = FundingSourceType.Levy,
                InstalmentAmount = 300,
                JobId = testSession.JobId,
                LearningAim = new LearningAim
                {
                    CourseCode = testSession.Learner.Course.CourseCode,
                    LearningType = testSession.Learner.Course.LearningType,
                    StandardCode = testSession.Learner.Course.StandardCode,
                    FundingLineType = testSession.Learner.Course.FundingLineType,
                    FrameworkCode = testSession.Learner.Course.FrameworkCode,
                    PathwayCode = testSession.Learner.Course.PathwayCode,
                    ProgrammeType = testSession.Learner.Course.ProgrammeType,
                    Reference = testSession.Learner.Course.Reference,
                    StartDate = DateTime.Today.AddMonths(-1)
                },
                StartDate = DateTime.Today.AddMonths(-1),
                LearningStartDate = DateTime.Today.AddMonths(-1),
                NumberOfInstalments = 1,
                PlannedEndDate = DateTime.Today.AddMonths(1),
                RequiredPaymentEventId = Guid.NewGuid(),
                TransactionType = transactionType,
                SfaContributionPercentage = .95m,
                TransferSenderAccountId = 0,
                Ukprn = testSession.Provider.Ukprn
            };
            Console.WriteLine($"Sending the funding source event for the milestone payment.  Event: {fundingSourceEvent.EventId}");
            await testSession.Pv2MessageContext.Send(fundingSourceEvent);
        }

        [When("the employers Levy account is used to fund the completion payment at period end")]
        public async Task WhenTheEmployersLevyAccountIsUsedToFundTheCompletionPaymentAtPeriodEnd()
        {
            var fundingSourceEvent = new SFA.DAS.Payments.FundingSource.Messages.Events.LevyFundingSourcePaymentEvent
            {
                AgreementId = Guid.NewGuid().ToString("N"),
                Learner = new Learner
                {
                    ReferenceNumber = testSession.Learner.LearnRefNumber,
                    Uln = testSession.Learner.Uln
                },
                CourseType = testSession.Learner.Course.CourseType,
                AccountId = 1,
                ActualEndDate = testSession.Learner.Course.LearningStartDate.AddMonths(1),
                AgeAtStartOfLearning = testSession.Learner.Age,
                AmountDue = 700,
                ApprenticeshipEmployerType = ApprenticeshipEmployerType.Levy,
                ApprenticeshipId = 1234567,
                CollectionPeriod = testSession.CurrentPeriod,
                CompletionAmount = 700,
                CompletionStatus = 2,
                ContractType = ContractType.Act1,
                DeliveryPeriod = 1,
                EarningEventId = testSession.CurrentEarningsId,
                EventId = Guid.NewGuid(),
                EventTime = DateTime.UtcNow,
                FundingPlatformType = FundingPlatformType.DigitalApprenticeshipService,
                FundingSourceType = FundingSourceType.Levy,
                InstalmentAmount = 300,
                JobId = testSession.JobId,
                LearningAim = new LearningAim
                {
                    CourseCode = testSession.Learner.Course.CourseCode,
                    LearningType = testSession.Learner.Course.LearningType,
                    StandardCode = testSession.Learner.Course.StandardCode,
                    FundingLineType = testSession.Learner.Course.FundingLineType,
                    FrameworkCode = testSession.Learner.Course.FrameworkCode,
                    PathwayCode = testSession.Learner.Course.PathwayCode,
                    ProgrammeType = testSession.Learner.Course.ProgrammeType,
                    Reference = testSession.Learner.Course.Reference,
                    StartDate = DateTime.Today.AddMonths(-1)
                },
                StartDate = DateTime.Today.AddMonths(-1),
                LearningStartDate = DateTime.Today.AddMonths(-1),
                NumberOfInstalments = 1,
                PlannedEndDate = DateTime.Today.AddMonths(1),
                RequiredPaymentEventId = Guid.NewGuid(),
                TransactionType = transactionType,
                SfaContributionPercentage = .95m,
                TransferSenderAccountId = 0,
                Ukprn = testSession.Provider.Ukprn
            };
            Console.WriteLine($"Sending the funding source event for the milestone payment.  Event: {fundingSourceEvent.EventId}");
            await testSession.Pv2MessageContext.Send(fundingSourceEvent);
        }

        [When("there is a change to the earnings and a new set of earnings have been received")]
        public async Task WhenThereIsAChangeToTheEarningsAndANewSetOfEarningsHaveBeenReceived()
        {
            var fundingSourceEvent = new SFA.DAS.Payments.FundingSource.Messages.Events.LevyFundingSourcePaymentEvent
            {
                AgreementId = Guid.NewGuid().ToString("N"),
                Learner = new Learner
                {
                    ReferenceNumber = testSession.Learner.LearnRefNumber,
                    Uln = testSession.Learner.Uln
                },
                CourseType = testSession.Learner.Course.CourseType,
                AccountId = 1,
                ActualEndDate = testSession.Learner.Course.LearningStartDate.AddMonths(1),
                AgeAtStartOfLearning = testSession.Learner.Age,
                AmountDue = 700,
                ApprenticeshipEmployerType = ApprenticeshipEmployerType.Levy,
                ApprenticeshipId = 1234567,
                CollectionPeriod = testSession.CurrentPeriod,
                CompletionAmount = 700,
                CompletionStatus = 2,
                ContractType = ContractType.Act1,
                DeliveryPeriod = 1,
                EarningEventId = testSession.CurrentEarningsId,
                EventId = Guid.NewGuid(),
                EventTime = DateTime.UtcNow,
                FundingPlatformType = FundingPlatformType.DigitalApprenticeshipService,
                FundingSourceType = FundingSourceType.Levy,
                InstalmentAmount = 300,
                JobId = testSession.JobId,
                LearningAim = new LearningAim
                {
                    CourseCode = testSession.Learner.Course.CourseCode,
                    LearningType = testSession.Learner.Course.LearningType,
                    StandardCode = testSession.Learner.Course.StandardCode,
                    FundingLineType = testSession.Learner.Course.FundingLineType,
                    FrameworkCode = testSession.Learner.Course.FrameworkCode,
                    PathwayCode = testSession.Learner.Course.PathwayCode,
                    ProgrammeType = testSession.Learner.Course.ProgrammeType,
                    Reference = testSession.Learner.Course.Reference,
                    StartDate = DateTime.Today.AddMonths(-1)
                },
                StartDate = DateTime.Today.AddMonths(-1),
                LearningStartDate = DateTime.Today.AddMonths(-1),
                NumberOfInstalments = 1,
                PlannedEndDate = DateTime.Today.AddMonths(1),
                RequiredPaymentEventId = Guid.NewGuid(),
                TransactionType = transactionType,
                SfaContributionPercentage = .95m,
                TransferSenderAccountId = 0,
                Ukprn = testSession.Provider.Ukprn
            };
            Console.WriteLine($"Sending the funding source event for the milestone payment.  Event: {fundingSourceEvent.EventId}");
            await testSession.Pv2MessageContext.Send(fundingSourceEvent);
        }

        [Then("the payments generated for the previous earnings should be reversed")]
        public async Task ThenThePaymentsGeneratedForThePreviousEarningsShouldBeReversed()
        {
            await testSession.WaitForIt(async () =>
            {
                var foundPreviousPayments = await testSession.DataContext.Payment.AnyAsync(p =>
                        p.LearnerUln == testSession.Learner.Uln &&
                        p.JobId == testSession.JobId &&
                        p.CourseCode == testSession.Learner.Course.CourseCode &&
                        p.CourseType == testSession.Learner.Course.CourseType &&
                        p.LearningType == testSession.Learner.Course.LearningType &&
                        p.EarningEventId == testSession.PreviousEarningsId &&
                        p.CollectionPeriod.Period == testSession.CurrentPeriod.Period &&
                        p.CollectionPeriod.AcademicYear == testSession.CurrentPeriod.AcademicYear
                    //TODO: Added bare minimum checks, Add missing properties to check
                );
                return !foundPreviousPayments;
            }, "Expected the previous payments to have been deleted but they were still found");
        }

        [Then("the levy funded Milestone payment should be recorded")]
        public async Task ThenTheMilestonePaymentShouldBeRecorded()
        {
            await testSession.WaitForIt(async () =>
            {
                Payments = await testSession.DataContext.Payment.Where(p =>
                            p.LearnerUln == testSession.Learner.Uln &&
                            p.JobId == testSession.JobId &&
                            p.CourseCode == testSession.Learner.Course.CourseCode &&
                            p.CourseType == testSession.Learner.Course.CourseType &&
                            p.LearningType == testSession.Learner.Course.LearningType &&
                            p.TransactionType == transactionType &&
                            p.FundingSource == FundingSourceType.Levy &&
                            p.EarningEventId == testSession.CurrentEarningsId &&
                            p.CollectionPeriod.Period == testSession.CurrentPeriod.Period &&
                            p.CollectionPeriod.AcademicYear == testSession.CurrentPeriod.AcademicYear
                        //TODO: Added bare minimum checks, Add missing properties to check
                    )
                    .ToListAsync();

                return Payments.Any();
            }, "Failed to find the expected milestone payment(s)");

        }


        [Then("the levy funded completion payment should be recorded")]
        public async Task ThenTheCompletionPaymentShouldBeRecorded()
        {
            await testSession.WaitForIt(async () =>
            {
                Payments = await testSession.DataContext.Payment.Where(p =>
                            p.LearnerUln == testSession.Learner.Uln &&
                            p.JobId == testSession.JobId &&
                            p.CourseCode == testSession.Learner.Course.CourseCode &&
                            p.CourseType == testSession.Learner.Course.CourseType &&
                            p.LearningType == testSession.Learner.Course.LearningType &&
                            p.TransactionType == transactionType &&
                            p.FundingSource == FundingSourceType.Levy &&
                            p.EarningEventId == testSession.CurrentEarningsId &&
                            p.CollectionPeriod.Period == testSession.CurrentPeriod.Period &&
                            p.CollectionPeriod.AcademicYear == testSession.CurrentPeriod.AcademicYear
                        //TODO: Added bare minimum checks, Add missing properties to check
                    )
                    .ToListAsync();

                return Payments.Any();
            }, "Failed to find the expected completion payment(s)");
        }

    }
}