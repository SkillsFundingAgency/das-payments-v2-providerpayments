using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Payments.Application.Infrastructure.Logging;
using SFA.DAS.Payments.EarningEvents.Messages.Events;
using SFA.DAS.Payments.ProviderPayments.Application.Repositories;
using SFA.DAS.Payments.ProviderPayments.Application.Validators;
using UUIDNext.Tools;

namespace SFA.DAS.Payments.ProviderPayments.Application.Services;

public interface IDASEarningsReceivedEventService
{
    Task RemovePreviousEarningsInCurrentCollection(DasEarningsReceivedEvent message,
        CancellationToken cancellationToken);
}

public class DASEarningsReceivedEventService : IDASEarningsReceivedEventService
{
    private readonly IPaymentLogger _paymentLogger;
    private readonly IProviderPaymentsRepository _providerPaymentsRepository;
    private readonly IDASEarningsReceivedEventValidator _dasEarningsReceivedEventValidator;

    public DASEarningsReceivedEventService(
        IProviderPaymentsRepository providerPaymentsRepository,
        IPaymentLogger paymentLogger,
        IDASEarningsReceivedEventValidator dasEarningsReceivedEventValidator)
    {
        _providerPaymentsRepository = providerPaymentsRepository;
        _paymentLogger = paymentLogger;
        _dasEarningsReceivedEventValidator = dasEarningsReceivedEventValidator;
    }

    public async Task RemovePreviousEarningsInCurrentCollection(DasEarningsReceivedEvent message,
        CancellationToken cancellationToken)
    {
        _dasEarningsReceivedEventValidator.Validate(message);
        var courseCode = message.CourseCode;
        var academicYear = message.CollectionPeriod.AcademicYear;
        var period = message.CollectionPeriod.Period;
        var ukprn = message.UKPRN;
        var uln = message.ULN;
        var learningAimReference = message.LearningAimReference;

        var logContext =
            $"CourseCode: {courseCode}, AcademicYear: {academicYear}, CollectionPeriod: {period}, UKPRN: {ukprn}, ULN: {uln}, LearningAimReference: {learningAimReference}";
        _paymentLogger.LogInfo($"Searching DASPayments payments table for {logContext}");

        try
        {
            var paymentTransactions = await _providerPaymentsRepository
                .GetPayments(courseCode, academicYear, period, ukprn, uln, learningAimReference, cancellationToken).ConfigureAwait(false);

            if (paymentTransactions == null || paymentTransactions.Count == 0)
            {
                _paymentLogger.LogInfo($"No payment transactions found for {logContext}");
                return;
            }

            foreach (var payment in paymentTransactions)
            {
                if (IsIncomingEarningsIdNewer(message.EarningsId, payment.EarningEventId))
                {
                    await _providerPaymentsRepository.DeletePayment(payment, cancellationToken).ConfigureAwait(false);
                    _paymentLogger.LogInfo($"Deleted older payment transaction for {logContext}. Existing EarningEventId: {payment.EarningEventId}, Message EarningsId: {message.EarningsId}");
                }
                else
                {
                    _paymentLogger.LogInfo($"Existing payment transaction is newer or equal for {logContext}. Existing EarningEventId: {payment.EarningEventId}, Message EarningsId: {message.EarningsId}");
                }
            }
        }
        catch (Exception e)
        {
            _paymentLogger.LogError($"Error while getting or deleting payment transactions for {logContext}", e);
            throw;
        }
    }

    private static bool IsIncomingEarningsIdNewer(Guid incomingEarningsId, Guid existingEarningEventId)
    {
        var canIncomingEarningsIdBeDecoded = UuidDecoder.TryDecodeTimestamp(incomingEarningsId, out var incomingEarningsIdTimestamp);
        var canExistingEarningEventIdBeDecoded = UuidDecoder.TryDecodeTimestamp(existingEarningEventId, out var existingEarningEventIdTimestamp);

        if (canIncomingEarningsIdBeDecoded && canExistingEarningEventIdBeDecoded)
        {
            return incomingEarningsIdTimestamp > existingEarningEventIdTimestamp;
        }

        return false;
    }
}