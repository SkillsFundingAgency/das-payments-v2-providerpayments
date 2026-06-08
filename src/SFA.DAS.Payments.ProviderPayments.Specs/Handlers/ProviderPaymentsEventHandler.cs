using System.Collections.Concurrent;
using SFA.DAS.Payments.ProviderPayments.Messages;

namespace SFA.DAS.Payments.ProviderPayments.Specs.Handlers;

public class ProviderPaymentsEventHandler: IHandleMessages<IProviderPaymentEvent>
{
    public ConcurrentBag<IProviderPaymentEvent> ReceivedEvents { get; } = new ConcurrentBag<IProviderPaymentEvent>();

    public async Task Handle(IProviderPaymentEvent message, IMessageHandlerContext context)
    {
        Console.WriteLine($"Received short course earnings event: {message.Ukprn}, uln: {message.Learner.Uln}, return: {message.CollectionPeriod.AcademicYear}-{message.CollectionPeriod.Period}, Course: {message.LearningAim.LearningType} - {message.LearningAim.CourseCode}");
        ReceivedEvents.Add(message);
    }
}