﻿using Autofac.Extras.Moq;
using Moq;
using NUnit.Framework;
using SFA.DAS.Payments.Application.Repositories;
using SFA.DAS.Payments.EarningEvents.Messages.Events;
using SFA.DAS.Payments.Model.Core;
using SFA.DAS.Payments.Model.Core.Entities;
using SFA.DAS.Payments.ProviderPayments.Application.Repositories;
using SFA.DAS.Payments.ProviderPayments.Application.Services;
using SFA.DAS.Payments.ProviderPayments.Domain;
using SFA.DAS.Payments.ProviderPayments.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Payments.Audit.Application.PaymentsEventModelCache;
using SFA.DAS.Payments.ProviderPayments.Model;

namespace SFA.DAS.Payments.ProviderPayments.Application.UnitTests.Services
{
    [TestFixture]
    public class ProviderPaymentsHandlerServiceTest
    {
        private AutoMock mocker;
        private ProviderPaymentsService providerPaymentsService;

        private Mock<IProviderPaymentsRepository> providerPaymentsRepository;
        private Mock<IDataCache<IlrSubmittedEvent>> ilrSubmittedEventCache;
        private Mock<IValidateIlrSubmission> validateIlrSubmission;


        private long ukprn = 10000;
        private long jobId = 10000;
        private List<ProviderPaymentEventModel> payments;
        private IlrSubmittedEvent ilrSubmittedEvent;

        [SetUp]
        public void SetUp()
        {
            mocker = AutoMock.GetLoose();

            payments = new List<ProviderPaymentEventModel>
            {
                new ProviderPaymentEventModel
                {
                    Ukprn = ukprn,
                    FundingSource = FundingSourceType.CoInvestedEmployer,
                    SfaContributionPercentage = 0.9m,
                    JobId = 1,
                    DeliveryPeriod = 7,
                    DeliveryPeriodName= "1819-R07",
                    DeliveryPeriodYear = 2018,
                    DeliveryPeriodMonth = 2,
                    
                    CollectionPeriod = 8,
                    CollectionPeriodYear = 2018,
                    CollectionPeriodName = "1819-R08",
                    CollectionPeriodMonth = 3,
                    IlrSubmissionDateTime = DateTime.UtcNow,
                    ContractType = ContractType.Act1,
                    PriceEpisodeIdentifier = "P-1",
                    LearnerReferenceNumber = "100500",
                    EventId = Guid.NewGuid(),
                    LearningAimFundingLineType = "16-18",
                    LearningAimPathwayCode = 1,
                    LearningAimReference = "1",
                    LearningAimFrameworkCode = 1,
                    TransactionType = TransactionType.Learning,
                    LearnerUln = 1000,
                    LearningAimProgrammeType = 1,
                    LearningAimStandardCode = 1,
                    Amount = 2000.00m
                }
            };

            providerPaymentsRepository = mocker.Mock<IProviderPaymentsRepository>();
            providerPaymentsRepository
                .Setup(t => t.SavePayment(It.IsAny<PaymentModel>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            ilrSubmittedEvent = new IlrSubmittedEvent
            {
                Ukprn = ukprn,
                JobId = jobId,
                IlrSubmissionDateTime = DateTime.MaxValue,
                CollectionPeriod = new CalendarPeriod(2018, 2)
            };

            ilrSubmittedEventCache = mocker.Mock<IDataCache<IlrSubmittedEvent>>();
            ilrSubmittedEventCache
                .Setup(o => o.TryGet(ukprn.ToString(), default(CancellationToken)))
                .Returns(Task.FromResult(new ConditionalValue<IlrSubmittedEvent>(true, ilrSubmittedEvent)));

            ilrSubmittedEventCache
                .Setup(o => o.Clear(ukprn.ToString(), default(CancellationToken)))
                .Returns(Task.CompletedTask);

            ilrSubmittedEventCache
                .Setup(o => o.Add(ukprn.ToString(), It.IsAny<IlrSubmittedEvent>(), default(CancellationToken)))
                .Returns(Task.CompletedTask);

            validateIlrSubmission = mocker.Mock<IValidateIlrSubmission>();
            validateIlrSubmission
                .Setup(o => o.IsLatestIlrPayment(It.IsAny<IlrSubmissionValidationRequest>()))
                .Returns(true);

            mocker.Mock<IPaymentsEventModelCache<ProviderPaymentEventModel>>()
                .Setup(x => x.AddPayment(It.IsAny<ProviderPaymentEventModel>()))
                .Returns(Task.CompletedTask);

            providerPaymentsService = mocker.Create<ProviderPaymentsService>();


        }

        [Test]
        public async Task ProcessEventShouldCallRequiredServices()
        {
            await providerPaymentsService.ProcessPayment(payments.First(), default(CancellationToken));
            
            mocker.Mock<IPaymentsEventModelCache<ProviderPaymentEventModel>>()
                .Verify(x => x.AddPayment(It.IsAny<ProviderPaymentEventModel>()),Times.Once);
        }
    }
}
