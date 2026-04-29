using System;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.Payments.Application.Infrastructure.Logging;
using SFA.DAS.Payments.EarningEvents.Messages.Events;
using SFA.DAS.Payments.ProviderPayments.Application.Services;
using SFA.DAS.Payments.ProviderPayments.Application.Validators;

namespace SFA.DAS.Payments.ProviderPayments.ProviderPaymentsService.Handlers;

public class DASEarningsReceivedEventHandler : IHandleMessages<DasEarningsReceivedEvent>
{
    private readonly IDASEarningsReceivedEventService dasEarningsReceivedEventService;
    private readonly IDASEarningsReceivedEventValidator dasEarningsReceivedEventValidator;
    private readonly IPaymentLogger paymentLogger;

    public DASEarningsReceivedEventHandler(
        IPaymentLogger paymentLogger,
        IDASEarningsReceivedEventService dasEarningsReceivedEventService,
        IDASEarningsReceivedEventValidator dasEarningsReceivedEventValidator)
    {
        this.paymentLogger = paymentLogger ?? throw new ArgumentNullException(nameof(paymentLogger));
        this.dasEarningsReceivedEventService = dasEarningsReceivedEventService ??
                                               throw new ArgumentNullException(nameof(dasEarningsReceivedEventService));
    }

    public async Task Handle(DasEarningsReceivedEvent message, IMessageHandlerContext context)
    {
        paymentLogger.LogInfo($"Processing DAS Earnings Received Event for Message Id : {context.MessageId}");
        dasEarningsReceivedEventValidator.Validate(message);
        await dasEarningsReceivedEventService.RemovePreviousEarningsInCurrentCollection(message, CancellationToken.None).ConfigureAwait(false);
        paymentLogger.LogInfo($"Finished processing DAS Earnings Received Event for Message Id :{context.MessageId}");
    }
}