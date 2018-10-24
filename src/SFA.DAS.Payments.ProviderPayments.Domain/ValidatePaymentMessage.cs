﻿using SFA.DAS.Payments.ProviderPayments.Domain.Models;

namespace SFA.DAS.Payments.ProviderPayments.Domain
{
    public class ValidatePaymentMessage : IValidatePaymentMessage
    {
        public bool IsLatestIlrPayment(PaymentMessageValidationRequest request)
        {
            return request.CurrentIlr == null ||
                   (request.IncomingPaymentJobId == request.CurrentIlr.JobId &&
                    request.IncomingPaymentUkprn == request.CurrentIlr.Ukprn) &&
                    request.IncomingPaymentSubmissionDate.CompareTo(request.CurrentIlr.IlrSubmissionDateTime) == 0;
        }

    }
}
