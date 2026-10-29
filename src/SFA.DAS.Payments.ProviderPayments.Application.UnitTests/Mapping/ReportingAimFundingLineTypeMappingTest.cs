using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Payments.FundingSource.Messages.Events;
using SFA.DAS.Payments.Model.Core;
using SFA.DAS.Payments.Model.Core.Entities;
using SFA.DAS.Payments.ProviderPayments.Application.Mapping;
using SFA.DAS.Payments.ProviderPayments.Model;

namespace SFA.DAS.Payments.ProviderPayments.Application.UnitTests.Mapping
{
    [TestFixture]
    public class ReportingAimFundingLineTypeMappingTest
    {
        private IMapper mapper;

        [OneTimeSetUp]
        public void SetUp()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ProviderPaymentsProfile>());
            config.AssertConfigurationIsValid();
            mapper = new Mapper(config);
        }

        [Test]
        [TestCase(ContractType.Act1, ApprenticeshipEmployerType.Levy, "16-18 Apprenticeship (From May 2017) Levy Contract", "16-18 Apprenticeship (Employer on App Service) Levy funding")]
        [TestCase(ContractType.Act1, ApprenticeshipEmployerType.NonLevy, "16-18 Apprenticeship (From May 2017) Levy Contract", "16-18 Apprenticeship (Employer on App Service) Non-Levy funding")]
        [TestCase(ContractType.Act1, ApprenticeshipEmployerType.Levy, "16-18 Apprenticeship (Employer on App Service)", "16-18 Apprenticeship (Employer on App Service) Levy funding")]
        [TestCase(ContractType.Act1, ApprenticeshipEmployerType.NonLevy, "16-18 Apprenticeship (Employer on App Service)", "16-18 Apprenticeship (Employer on App Service) Non-Levy funding")]
        [TestCase(ContractType.Act1, ApprenticeshipEmployerType.Levy, "19+ Apprenticeship (From May 2017) Levy Contract", "19+ Apprenticeship (Employer on App Service) Levy funding")]
        [TestCase(ContractType.Act1, ApprenticeshipEmployerType.NonLevy, "19+ Apprenticeship (From May 2017) Levy Contract", "19+ Apprenticeship (Employer on App Service) Non-Levy funding")]
        [TestCase(ContractType.Act1, ApprenticeshipEmployerType.Levy, "19+ Apprenticeship (Employer on App Service)", "19+ Apprenticeship (Employer on App Service) Levy funding")]
        [TestCase(ContractType.Act1, ApprenticeshipEmployerType.NonLevy, "19+ Apprenticeship (Employer on App Service)", "19+ Apprenticeship (Employer on App Service) Non-Levy funding")]

        [TestCase(ContractType.Act2, ApprenticeshipEmployerType.NonLevy, "16-18 Apprenticeship (From May 2017) Non-Levy Contract (non-procured)", "16-18 Apprenticeship (From May 2017) Non-Levy Contract (non-procured)")]
        [TestCase(ContractType.Act2, ApprenticeshipEmployerType.NonLevy, "16-18 Apprenticeship Non-Levy Contract (procured)", "16-18 Apprenticeship Non-Levy Contract (procured)")]
        [TestCase(ContractType.Act2, ApprenticeshipEmployerType.NonLevy, "19+ Apprenticeship (From May 2017) Non-Levy Contract (non-procured)", "19+ Apprenticeship (From May 2017) Non-Levy Contract (non-procured)")]
        [TestCase(ContractType.Act2, ApprenticeshipEmployerType.NonLevy, "19+ Apprenticeship Non-Levy Contract (procured)", "19+ Apprenticeship Non-Levy Contract (procured)")]

        [TestCase(ContractType.Act2, ApprenticeshipEmployerType.NonLevy, "Alice in wonderland", "None")]

        public void SetsReportingAimFundingLineType(ContractType contractType, ApprenticeshipEmployerType employerType, string fundingLineType, string expected)
        {
            var fundingSourceEvent = new EmployerCoInvestedFundingSourcePaymentEvent
            {
                ContractType = contractType,
                LearningAim = new LearningAim {FundingLineType = fundingLineType},
                ApprenticeshipEmployerType = employerType
            };

            var providerPayment = mapper.Map<ProviderPaymentEventModel>(fundingSourceEvent);

            providerPayment.ReportingAimFundingLineType.Should().StartWith(expected);
        }

        [Test]
        [TestCase( "GSO Short Courses (Apprenticeship Units) Levy")]
        [TestCase( "GSO Short Courses (Apprenticeship Units) Non-Levy")]
        public void ShouldSetReportingAimFundingLineTypeToFundingLineTypeIfShortCourse(string fundingLineType)
        {
            var fundingSourceEvent = new EmployerCoInvestedFundingSourcePaymentEvent
            {
                ContractType = ContractType.Act1, //Short courses can only be Act 1
                LearningAim = new LearningAim { FundingLineType = fundingLineType },
                CourseType = CourseType.ShortCourse
            };

            var providerPayment = mapper.Map<ProviderPaymentEventModel>(fundingSourceEvent);

            providerPayment.ReportingAimFundingLineType.Should().Be(fundingLineType);
        }

        [Test]
        [TestCase((CourseType)0, ApprenticeshipEmployerType.NonLevy, "GSO Short Courses (Apprenticeship Units) Non-Levy")]
        [TestCase((CourseType)0, ApprenticeshipEmployerType.Levy, "GSO Short Courses (Apprenticeship Units) Levy")]
        [TestCase(CourseType.Apprenticeship, ApprenticeshipEmployerType.NonLevy, "GSO Short Courses (Apprenticeship Units) Non-Levy")]
        [TestCase(CourseType.Apprenticeship, ApprenticeshipEmployerType.Levy, "GSO Short Courses (Apprenticeship Units) Levy")]
        [TestCase(CourseType.ShortCourse, ApprenticeshipEmployerType.NonLevy, "GSO Short Courses (Apprenticeship Units) Non-Levy")]
        [TestCase(CourseType.ShortCourse, ApprenticeshipEmployerType.Levy, "GSO Short Courses (Apprenticeship Units) Levy")]
        public void ShouldSetReportingAimFundingLineTypeToFundingLineTypeIfApprenticeshipUnitRegardlessOfCourseType(CourseType courseType, ApprenticeshipEmployerType employerType, string fundingLineType)
        {
            var fundingSourceEvent = new SfaCoInvestedFundingSourcePaymentEvent
            {
                ContractType = ContractType.Act1,
                ApprenticeshipEmployerType = employerType,
                CourseType = courseType,
                LearningAim = new LearningAim { FundingLineType = fundingLineType, LearningType = LearningType.ApprenticeshipUnit }
            };

            var providerPayment = mapper.Map<ProviderPaymentEventModel>(fundingSourceEvent);

            providerPayment.ReportingAimFundingLineType.Should().Be(fundingLineType);
        }

        [Test]
        public void ShouldNotPassThroughShortCourseFundingLineTypeForNonApprenticeshipUnitLearning()
        {
            var fundingSourceEvent = new SfaCoInvestedFundingSourcePaymentEvent
            {
                ContractType = ContractType.Act1,
                ApprenticeshipEmployerType = ApprenticeshipEmployerType.NonLevy,
                CourseType = (CourseType)0,
                LearningAim = new LearningAim { FundingLineType = "GSO Short Courses (Apprenticeship Units) Non-Levy", LearningType = LearningType.Apprenticeship }
            };

            var providerPayment = mapper.Map<ProviderPaymentEventModel>(fundingSourceEvent);

            providerPayment.ReportingAimFundingLineType.Should().StartWith("None");
        }
    }
}
