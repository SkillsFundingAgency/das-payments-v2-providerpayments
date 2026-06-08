using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using UUIDNext;
using NUnit.Framework;
using SFA.DAS.Payments.Application.Infrastructure.Logging;
using SFA.DAS.Payments.EarningEvents.Messages.Events;
using SFA.DAS.Payments.Model.Core;
using SFA.DAS.Payments.Model.Core.Entities;
using SFA.DAS.Payments.ProviderPayments.Application.Repositories;
using SFA.DAS.Payments.ProviderPayments.Application.Services;

namespace SFA.DAS.Payments.ProviderPayments.Application.UnitTests.Services
{
    [TestFixture]
    public class DASEarningsReceivedEventServiceTests
    {
        private Mock<IProviderPaymentsRepository> providerPaymentsRepository;
        private Mock<IPaymentLogger> paymentLogger;
        private DASEarningsReceivedEventService service;

        private DasEarningsReceivedEvent message;

        private const string CourseCode = "123456";
        private const short AcademicYear = 2526;
        private const byte Period = 1;
        private const long Ukprn = 12345;
        private const long Uln = 123456;
        private const string LearningAimReference = "1234567-aim-ref";
        private readonly Guid oldExistingGuid = Uuid.NewDatabaseFriendly(Database.SqlServer);
        private readonly Guid messageGuid = Uuid.NewDatabaseFriendly(Database.SqlServer);
        private readonly Guid newExistingGuid = Uuid.NewDatabaseFriendly(Database.SqlServer);

        private PaymentModel olderExistingPayment;
        private PaymentModel matchingExistingPayment;
        private PaymentModel newerExistingPayment;

        [SetUp]
        public void SetUp()
        {
            providerPaymentsRepository = new Mock<IProviderPaymentsRepository>();
            paymentLogger = new Mock<IPaymentLogger>();

            message = new DasEarningsReceivedEvent
            {
                CourseCode = CourseCode,
                CollectionPeriod = new CollectionPeriod
                {
                    AcademicYear = AcademicYear,
                    Period = Period
                },
                UKPRN = Ukprn,
                ULN = Uln,
                LearningAimReference = LearningAimReference,
                EarningsId = messageGuid
            };

            olderExistingPayment = new PaymentModel { EarningEventId = oldExistingGuid };
            matchingExistingPayment = new PaymentModel { EarningEventId = messageGuid };
            newerExistingPayment = new PaymentModel { EarningEventId = newExistingGuid };

            service = new DASEarningsReceivedEventService(providerPaymentsRepository.Object, paymentLogger.Object);
        }

        [Test]
        public async Task RemovePreviousEarningsInCurrentCollection_When_Payment_List_Is_Null_DeletePayment_Is_Not_Called()
        {
            //Arrange
            providerPaymentsRepository
                .Setup(x => x.GetPayments(CourseCode, AcademicYear, Period, Ukprn, Uln, LearningAimReference, It.IsAny<CancellationToken>()))
                .ReturnsAsync((List<PaymentModel>)null);

            //Act
            await service.RemovePreviousEarningsInCurrentCollection(message, CancellationToken.None);

            //Assert
            providerPaymentsRepository.Verify(
                x => x.DeletePayment(It.IsAny<PaymentModel>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Test]
        public async Task RemovePreviousEarningsInCurrentCollection_When_Payment_List_Is_Empty_DeletePayment_Is_Not_Called()
        {
            //Arrange
            providerPaymentsRepository
                .Setup(x => x.GetPayments(CourseCode, AcademicYear, Period, Ukprn, Uln, LearningAimReference, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PaymentModel>());

            //Act
            await service.RemovePreviousEarningsInCurrentCollection(message, CancellationToken.None);

            //Assert
            providerPaymentsRepository.Verify(
                x => x.DeletePayment(It.IsAny<PaymentModel>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Test]
        public async Task RemovePreviousEarningsInCurrentCollection_When_Existing_EarningEventId_Is_Older_It_Is_Deleted()
        {
            //Arrange
            providerPaymentsRepository
                .Setup(x => x.GetPayments(CourseCode, AcademicYear, Period, Ukprn, Uln, LearningAimReference, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PaymentModel> { olderExistingPayment });

            //Act
            await service.RemovePreviousEarningsInCurrentCollection(message, CancellationToken.None);

            //Assert
            providerPaymentsRepository.Verify(
                x => x.DeletePayment(olderExistingPayment, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task RemovePreviousEarningsInCurrentCollection_When_Existing_EarningEventId_Is_Newer_It_Is_Not_Deleted()
        {
            //Arrange
            providerPaymentsRepository
                .Setup(x => x.GetPayments(CourseCode, AcademicYear, Period, Ukprn, Uln, LearningAimReference, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PaymentModel> { newerExistingPayment });

            //Act
            await service.RemovePreviousEarningsInCurrentCollection(message, CancellationToken.None);

            //Assert
            providerPaymentsRepository.Verify(
                x => x.DeletePayment(It.IsAny<PaymentModel>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Test]
        public async Task RemovePreviousEarningsInCurrentCollection_When_Existing_EarningEventId_Is_Equal_It_Is_Not_Deleted()
        {
            //Arrange
            providerPaymentsRepository
                .Setup(x => x.GetPayments(CourseCode, AcademicYear, Period, Ukprn, Uln, LearningAimReference, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PaymentModel> { matchingExistingPayment });

            //Act
            await service.RemovePreviousEarningsInCurrentCollection(message, CancellationToken.None);

            //Assert
            providerPaymentsRepository.Verify(
                x => x.DeletePayment(It.IsAny<PaymentModel>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Test]
        public async Task RemovePreviousEarningsInCurrentCollection_When_Receiving_Mixed_Payments_Only_Older_Ones_Are_Deleted()
        {
            //Arrange
            providerPaymentsRepository
                .Setup(x => x.GetPayments(CourseCode, AcademicYear, Period, Ukprn, Uln, LearningAimReference, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PaymentModel> { olderExistingPayment, newerExistingPayment });

            //Act
            await service.RemovePreviousEarningsInCurrentCollection(message, CancellationToken.None);

            //Assert
            providerPaymentsRepository.Verify(
                x => x.DeletePayment(olderExistingPayment, It.IsAny<CancellationToken>()),
                Times.Once);
            providerPaymentsRepository.Verify(
                x => x.DeletePayment(newerExistingPayment, It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Test]
        public async Task RemovePreviousEarningsInCurrentCollection_When_Repository_Throws_Exception_It_Should_Be_Propagated()
        {
            //Arrange
            var expected = new InvalidOperationException();
            providerPaymentsRepository
                .Setup(x => x.GetPayments(CourseCode, AcademicYear, Period, Ukprn, Uln, LearningAimReference, It.IsAny<CancellationToken>()))
                .ThrowsAsync(expected);

            //Act
            Func<Task> act = () => service.RemovePreviousEarningsInCurrentCollection(message, CancellationToken.None);

            //Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}
