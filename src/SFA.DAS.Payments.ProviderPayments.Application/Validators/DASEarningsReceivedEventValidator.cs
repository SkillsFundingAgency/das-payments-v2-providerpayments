using System;
using SFA.DAS.Payments.EarningEvents.Messages.Events;

namespace SFA.DAS.Payments.ProviderPayments.Application.Validators;

public interface IDASEarningsReceivedEventValidator
{
    bool Validate(DasEarningsReceivedEvent message);
}

public class DASEarningsReceivedEventValidator : IDASEarningsReceivedEventValidator
{
    public bool Validate(DasEarningsReceivedEvent message)
    {
        if (message == null) throw new ArgumentNullException(null, "Message must not be null");

        if (string.IsNullOrWhiteSpace(message.CourseCode))
            throw new ArgumentException("CourseCode must be provided");

        if (message.CollectionPeriod == null)
            throw new ArgumentException("CollectionPeriod must be provided");

        if (message.CollectionPeriod.AcademicYear == 0)
            throw new ArgumentException("AcademicYear must be provided");

        if (message.CollectionPeriod.Period == 0)
            throw new ArgumentException("CollectionPeriod.Period must be provided");

        if (message.CollectionPeriod.Period > 14)
            throw new ArgumentException("CollectionPeriod Period is invalid");

        if (message.UKPRN == 0)
            throw new ArgumentException("UKPRN must be provided");

        if (message.ULN == 0)
            throw new ArgumentException("ULN must be provided");

        if (string.IsNullOrWhiteSpace(message.LearningAimReference))
            throw new ArgumentException("LearningAimReference must be provided");

        if (message.EarningsId == Guid.Empty)
            throw new ArgumentException("EarningsId must be provided");

        return true;
    }
}