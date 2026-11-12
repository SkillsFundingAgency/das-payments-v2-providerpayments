using System;
using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Payments.FundingSource.Messages.Events;
using SFA.DAS.Payments.Model.Core;
using SFA.DAS.Payments.Model.Core.Entities;
using SFA.DAS.Payments.ProviderPayments.Application.Mapping;
using SFA.DAS.Payments.ProviderPayments.Messages;
using SFA.DAS.Payments.ProviderPayments.Model;

namespace SFA.DAS.Payments.ProviderPayments.Application.UnitTests.Mapping
{
    [TestFixture]
    public class ApprenticeshipUnitFundingLineTypeMappingTests
    {
        private const string LevyFundingLineType = "GSO Short Courses (Apprenticeship Units) Levy";
        private const string NonLevyFundingLineType = "GSO Short Courses (Apprenticeship Units) Non-Levy";
        private const string CourseCode = "ZSC0001";
        private const CourseType MissingCourseType = 0;

        private IMapper mapper;

        [OneTimeSetUp]
        public void SetUp()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ProviderPaymentsProfile>());
            config.AssertConfigurationIsValid();
            mapper = new Mapper(config);
        }

        [Test]
        public void Levy_Payment_Gets_Levy_Funding_Line_Type()
        {
            var fundingSourceEvent = CreateEvent<LevyFundingSourcePaymentEvent>(300m, ApprenticeshipEmployerType.Levy, CourseType.ShortCourse);

            var payment = mapper.Map<ProviderPaymentEventModel>(fundingSourceEvent);

            payment.ReportingAimFundingLineType.Should().Be(LevyFundingLineType);
        }

        [Test]
        public void Levy_Clawback_Gets_Levy_Funding_Line_Type()
        {
            var fundingSourceEvent = CreateEvent<LevyFundingSourcePaymentEvent>(-300m, ApprenticeshipEmployerType.Levy, CourseType.ShortCourse);

            var payment = mapper.Map<ProviderPaymentEventModel>(fundingSourceEvent);

            payment.ReportingAimFundingLineType.Should().Be(LevyFundingLineType);
        }

        [Test]
        public void Non_Levy_Payment_Gets_Non_Levy_Funding_Line_Type()
        {
            var fundingSourceEvent = CreateEvent<SfaCoInvestedFundingSourcePaymentEvent>(300m, ApprenticeshipEmployerType.NonLevy, CourseType.ShortCourse);

            var payment = mapper.Map<ProviderPaymentEventModel>(fundingSourceEvent);

            payment.ReportingAimFundingLineType.Should().Be(NonLevyFundingLineType);
        }

        [Test]
        public void Non_Levy_Clawback_Gets_Non_Levy_Funding_Line_Type()
        {
            var fundingSourceEvent = CreateEvent<SfaCoInvestedFundingSourcePaymentEvent>(-300m, ApprenticeshipEmployerType.NonLevy, CourseType.ShortCourse);

            var payment = mapper.Map<ProviderPaymentEventModel>(fundingSourceEvent);

            payment.ReportingAimFundingLineType.Should().Be(NonLevyFundingLineType);
        }

        [Test]
        public void Levy_Clawback_Without_Course_Type_Gets_Levy_Funding_Line_Type()
        {
            var fundingSourceEvent = CreateEvent<LevyFundingSourcePaymentEvent>(-300m, ApprenticeshipEmployerType.Levy, MissingCourseType);

            var payment = mapper.Map<ProviderPaymentEventModel>(fundingSourceEvent);

            payment.ReportingAimFundingLineType.Should().Be(LevyFundingLineType);
        }

        [Test]
        public void Non_Levy_Clawback_Without_Course_Type_Gets_Non_Levy_Funding_Line_Type()
        {
            var fundingSourceEvent = CreateEvent<SfaCoInvestedFundingSourcePaymentEvent>(-300m, ApprenticeshipEmployerType.NonLevy, MissingCourseType);

            var payment = mapper.Map<ProviderPaymentEventModel>(fundingSourceEvent);

            payment.ReportingAimFundingLineType.Should().Be(NonLevyFundingLineType);
        }

        [Test]
        public void Funding_Line_Type_Is_Never_Unknown_For_Apprenticeship_Unit_Clawback_Without_Course_Type()
        {
            var fundingSourceEvent = CreateEvent<SfaCoInvestedFundingSourcePaymentEvent>(-300m, ApprenticeshipEmployerType.NonLevy, MissingCourseType);

            var payment = mapper.Map<ProviderPaymentEventModel>(fundingSourceEvent);

            payment.ReportingAimFundingLineType.Should().NotStartWith("None");
        }

        [TestCase(ApprenticeshipEmployerType.Levy)]
        [TestCase(ApprenticeshipEmployerType.NonLevy)]
        public void Employer_Co_Invested_Event_Gets_Funding_Line_Type(ApprenticeshipEmployerType employerType)
        {
            var fundingSourceEvent = CreateEvent<EmployerCoInvestedFundingSourcePaymentEvent>(-300m, employerType, MissingCourseType);

            var payment = mapper.Map<ProviderPaymentEventModel>(fundingSourceEvent);

            payment.ReportingAimFundingLineType.Should().Be(FundingLineTypeFor(employerType));
        }

        [TestCase(ApprenticeshipEmployerType.Levy)]
        [TestCase(ApprenticeshipEmployerType.NonLevy)]
        public void Sfa_Fully_Funded_Event_Gets_Funding_Line_Type(ApprenticeshipEmployerType employerType)
        {
            var fundingSourceEvent = CreateEvent<SfaFullyFundedFundingSourcePaymentEvent>(-300m, employerType, MissingCourseType);

            var payment = mapper.Map<ProviderPaymentEventModel>(fundingSourceEvent);

            payment.ReportingAimFundingLineType.Should().Be(FundingLineTypeFor(employerType));
        }

        [TestCase(ApprenticeshipEmployerType.Levy)]
        [TestCase(ApprenticeshipEmployerType.NonLevy)]
        public void Transfer_Event_Gets_Funding_Line_Type(ApprenticeshipEmployerType employerType)
        {
            var fundingSourceEvent = CreateEvent<TransferFundingSourcePaymentEvent>(-300m, employerType, MissingCourseType);

            var payment = mapper.Map<ProviderPaymentEventModel>(fundingSourceEvent);

            payment.ReportingAimFundingLineType.Should().Be(FundingLineTypeFor(employerType));
        }

        [TestCase(CourseType.ShortCourse)]
        [TestCase(MissingCourseType)]
        public void Payment_Keeps_Course_Details(CourseType courseTypeOnEvent)
        {
            var fundingSourceEvent = CreateEvent<SfaCoInvestedFundingSourcePaymentEvent>(300m, ApprenticeshipEmployerType.NonLevy, courseTypeOnEvent);

            var payment = mapper.Map<ProviderPaymentEventModel>(fundingSourceEvent);

            payment.CourseType.Should().Be(CourseType.ShortCourse);
            payment.LearningType.Should().Be(LearningType.ApprenticeshipUnit);
            payment.CourseCode.Should().Be(CourseCode);
        }

        [Test]
        public void Clawback_Keeps_Negative_Amount_And_Clawback_Source_Payment_Event_Id()
        {
            var fundingSourceEvent = CreateEvent<SfaCoInvestedFundingSourcePaymentEvent>(-300m, ApprenticeshipEmployerType.NonLevy, CourseType.ShortCourse);

            var payment = mapper.Map<ProviderPaymentEventModel>(fundingSourceEvent);

            payment.Amount.Should().Be(-300m);
            payment.ClawbackSourcePaymentEventId.Should().Be(fundingSourceEvent.ClawbackSourcePaymentEventId);
            payment.ClawbackSourcePaymentEventId.Should().NotBeNull();
        }

        [TestCase(300)]
        [TestCase(-300)]
        public void Published_Provider_Payment_Event_Carries_Course_Details_For_Levy_Event(int amount)
        {
            var fundingSourceEvent = CreateEvent<LevyFundingSourcePaymentEvent>(amount, ApprenticeshipEmployerType.Levy, CourseType.ShortCourse);

            var providerPaymentEvent = mapper.Map<ProviderPaymentEvent>(fundingSourceEvent);

            AssertCourseDetails(providerPaymentEvent, amount);
        }

        [TestCase(300)]
        [TestCase(-300)]
        public void Published_Provider_Payment_Event_Carries_Course_Details_For_Transfer_Event(int amount)
        {
            var fundingSourceEvent = CreateEvent<TransferFundingSourcePaymentEvent>(amount, ApprenticeshipEmployerType.Levy, CourseType.ShortCourse);

            var providerPaymentEvent = mapper.Map<ProviderPaymentEvent>(fundingSourceEvent);

            AssertCourseDetails(providerPaymentEvent, amount);
        }

        [TestCase(300)]
        [TestCase(-300)]
        public void Published_Provider_Payment_Event_Carries_Course_Details_For_Sfa_Co_Invested_Event(int amount)
        {
            var fundingSourceEvent = CreateEvent<SfaCoInvestedFundingSourcePaymentEvent>(amount, ApprenticeshipEmployerType.NonLevy, CourseType.ShortCourse);

            var providerPaymentEvent = mapper.Map<ProviderPaymentEvent>(fundingSourceEvent);

            AssertCourseDetails(providerPaymentEvent, amount);
        }

        [TestCase(300)]
        [TestCase(-300)]
        public void Published_Provider_Payment_Event_Carries_Course_Details_For_Employer_Co_Invested_Event(int amount)
        {
            var fundingSourceEvent = CreateEvent<EmployerCoInvestedFundingSourcePaymentEvent>(amount, ApprenticeshipEmployerType.NonLevy, CourseType.ShortCourse);

            var providerPaymentEvent = mapper.Map<ProviderPaymentEvent>(fundingSourceEvent);

            AssertCourseDetails(providerPaymentEvent, amount);
        }

        [TestCase(300)]
        [TestCase(-300)]
        public void Published_Provider_Payment_Event_Carries_Course_Details_For_Sfa_Fully_Funded_Event(int amount)
        {
            var fundingSourceEvent = CreateEvent<SfaFullyFundedFundingSourcePaymentEvent>(amount, ApprenticeshipEmployerType.NonLevy, CourseType.ShortCourse);

            var providerPaymentEvent = mapper.Map<ProviderPaymentEvent>(fundingSourceEvent);

            AssertCourseDetails(providerPaymentEvent, amount);
        }

        private static void AssertCourseDetails(ProviderPaymentEvent providerPaymentEvent, int amount)
        {
            providerPaymentEvent.CourseType.Should().Be(CourseType.ShortCourse);
            providerPaymentEvent.LearningType.Should().Be(LearningType.ApprenticeshipUnit);
            providerPaymentEvent.CourseCode.Should().Be(CourseCode);
            providerPaymentEvent.AmountDue.Should().Be(amount);
        }

        private static string FundingLineTypeFor(ApprenticeshipEmployerType employerType)
        {
            return employerType == ApprenticeshipEmployerType.Levy ? LevyFundingLineType : NonLevyFundingLineType;
        }

        private static T CreateEvent<T>(decimal amount, ApprenticeshipEmployerType employerType, CourseType courseType)
            where T : FundingSourcePaymentEvent, new()
        {
            return new T
            {
                EventId = Guid.NewGuid(),
                CollectionPeriod = new CollectionPeriod { AcademicYear = 2627, Period = 2 },
                DeliveryPeriod = 1,
                Learner = new Learner { ReferenceNumber = "LEARNER1", Uln = 1000000001 },
                Ukprn = 10000001,
                ContractType = ContractType.Act1,
                ApprenticeshipEmployerType = employerType,
                AmountDue = amount,
                TransactionType = TransactionType.Milestone1,
                FundingPlatformType = FundingPlatformType.DigitalApprenticeshipService,
                CourseType = courseType,
                RequiredPaymentEventId = Guid.NewGuid(),
                ClawbackSourcePaymentEventId = amount < 0 ? Guid.NewGuid() : (Guid?)null,
                LearningAim = new LearningAim
                {
                    Reference = CourseCode,
                    CourseCode = CourseCode,
                    LearningType = LearningType.ApprenticeshipUnit,
                    FundingLineType = FundingLineTypeFor(employerType)
                }
            };
        }
    }
}
