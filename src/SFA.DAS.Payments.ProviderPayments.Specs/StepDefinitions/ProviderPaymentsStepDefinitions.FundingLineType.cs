using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Reqnroll;
using SFA.DAS.Payments.Model.Core;
using SFA.DAS.Payments.Model.Core.Entities;
using FundingSourceEvents = SFA.DAS.Payments.FundingSource.Messages.Events;

namespace SFA.DAS.Payments.ProviderPayments.Specs.StepDefinitions
{
    public partial class ProviderPaymentsStepDefinitions
    {
        private const decimal FundingLineTypePaymentAmount = 300m;

        private ApprenticeshipEmployerType employerType = ApprenticeshipEmployerType.Levy;
        private CourseType? courseTypeOnFundingSourceEvent;
        private decimal expectedAmount;

        [Given("the employer is Levy funded")]
        public void GivenTheEmployerIsLevyFunded()
        {
            employerType = ApprenticeshipEmployerType.Levy;
        }

        [Given("the employer is Non-Levy funded")]
        public void GivenTheEmployerIsNonLevyFunded()
        {
            employerType = ApprenticeshipEmployerType.NonLevy;
        }

        [Given("the learning is funded under the {string} funding line")]
        public void GivenTheLearningIsFundedUnderTheFundingLine(string fundingLineType)
        {
            testSession.Learner.Course.FundingLineType = fundingLineType;
        }

        [Given("the funding source event does not carry the course type")]
        public void GivenTheFundingSourceEventDoesNotCarryTheCourseType()
        {
            courseTypeOnFundingSourceEvent = 0;
        }

        [When("the funding source payment is received for the milestone")]
        public async Task WhenTheFundingSourcePaymentIsReceivedForTheMilestone()
        {
            expectedAmount = FundingLineTypePaymentAmount;
            await SendFundingSourceEvent(expectedAmount, null);
        }

        [When("the funding source clawback is received for the milestone")]
        public async Task WhenTheFundingSourceClawbackIsReceivedForTheMilestone()
        {
            expectedAmount = -FundingLineTypePaymentAmount;
            await SendFundingSourceEvent(expectedAmount, Guid.NewGuid());
        }

        [Then("the payment is recorded with funding line type {string}")]
        public async Task ThenThePaymentIsRecordedWithFundingLineType(string expectedFundingLineType)
        {
            await FindPaymentForMilestone();

            Assert.That(Payments.Single().ReportingAimFundingLineType, Is.EqualTo(expectedFundingLineType));
        }

        [Then("the funding line type is not an unknown value")]
        public void ThenTheFundingLineTypeIsNotAnUnknownValue()
        {
            Assert.That(Payments.Single().ReportingAimFundingLineType, Does.Not.StartWith("None"));
        }

        [Then("the payment keeps the short course details")]
        public void ThenThePaymentKeepsTheShortCourseDetails()
        {
            var payment = Payments.Single();

            Assert.That(payment.CourseType, Is.EqualTo(CourseType.ShortCourse));
            Assert.That(payment.LearningType, Is.EqualTo(LearningType.ApprenticeshipUnit));
            Assert.That(payment.CourseCode, Is.EqualTo(testSession.Learner.Course.CourseCode));
        }

        private async Task FindPaymentForMilestone()
        {
            await testSession.WaitForIt(async () =>
            {
                Payments = await testSession.DataContext.Payment.Where(p =>
                        p.LearnerUln == testSession.Learner.Uln &&
                        p.JobId == testSession.JobId &&
                        p.TransactionType == transactionType &&
                        p.Amount == expectedAmount &&
                        p.EarningEventId == testSession.CurrentEarningsId &&
                        p.CollectionPeriod.Period == testSession.CurrentPeriod.Period &&
                        p.CollectionPeriod.AcademicYear == testSession.CurrentPeriod.AcademicYear)
                    .ToListAsync();

                return Payments.Any();
            }, "Failed to find the expected milestone payment");
        }

        private async Task SendFundingSourceEvent(decimal amountDue, Guid? clawbackSourcePaymentEventId)
        {
            FundingSourceEvents.FundingSourcePaymentEvent fundingSourceEvent;

            if (employerType == ApprenticeshipEmployerType.Levy)
            {
                fundingSourceEvent = new FundingSourceEvents.LevyFundingSourcePaymentEvent
                {
                    AgreementId = Guid.NewGuid().ToString("N"),
                    FundingSourceType = FundingSourceType.Levy
                };
            }
            else
            {
                fundingSourceEvent = new FundingSourceEvents.SfaCoInvestedFundingSourcePaymentEvent
                {
                    FundingSourceType = FundingSourceType.CoInvestedSfa
                };
            }

            fundingSourceEvent.Learner = new Learner
            {
                ReferenceNumber = testSession.Learner.LearnRefNumber,
                Uln = testSession.Learner.Uln
            };
            fundingSourceEvent.CourseType = courseTypeOnFundingSourceEvent ?? testSession.Learner.Course.CourseType;
            fundingSourceEvent.AccountId = 1;
            fundingSourceEvent.ActualEndDate = null;
            fundingSourceEvent.AgeAtStartOfLearning = testSession.Learner.Age;
            fundingSourceEvent.AmountDue = amountDue;
            fundingSourceEvent.ApprenticeshipEmployerType = employerType;
            fundingSourceEvent.ApprenticeshipId = 1234567;
            fundingSourceEvent.CollectionPeriod = testSession.CurrentPeriod;
            fundingSourceEvent.CompletionAmount = 700;
            fundingSourceEvent.CompletionStatus = 1;
            fundingSourceEvent.ContractType = ContractType.Act1;
            fundingSourceEvent.DeliveryPeriod = 1;
            fundingSourceEvent.EarningEventId = testSession.CurrentEarningsId;
            fundingSourceEvent.EventId = Guid.NewGuid();
            fundingSourceEvent.EventTime = DateTime.UtcNow;
            fundingSourceEvent.FundingPlatformType = FundingPlatformType.DigitalApprenticeshipService;
            fundingSourceEvent.InstalmentAmount = 300;
            fundingSourceEvent.JobId = testSession.JobId;
            fundingSourceEvent.LearningAim = new LearningAim
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
            };
            fundingSourceEvent.StartDate = DateTime.Today.AddMonths(-1);
            fundingSourceEvent.LearningStartDate = DateTime.Today.AddMonths(-1);
            fundingSourceEvent.NumberOfInstalments = 1;
            fundingSourceEvent.PlannedEndDate = DateTime.Today.AddMonths(1);
            fundingSourceEvent.RequiredPaymentEventId = Guid.NewGuid();
            fundingSourceEvent.ClawbackSourcePaymentEventId = clawbackSourcePaymentEventId;
            fundingSourceEvent.TransactionType = transactionType;
            fundingSourceEvent.SfaContributionPercentage = .95m;
            fundingSourceEvent.TransferSenderAccountId = 0;
            fundingSourceEvent.Ukprn = testSession.Provider.Ukprn;
            fundingSourceEvent.PriceEpisodeIdentifier = string.Empty;
            fundingSourceEvent.IlrSubmissionDateTime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;

            Console.WriteLine($"Sending the funding source event.  Event: {fundingSourceEvent.EventId}, Type: {fundingSourceEvent.GetType().Name}, Amount: {amountDue}");
            await testSession.Pv2MessageContext.Send(fundingSourceEvent);
        }
    }
}
