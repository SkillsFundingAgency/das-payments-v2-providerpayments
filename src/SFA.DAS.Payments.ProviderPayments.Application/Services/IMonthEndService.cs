﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Payments.Model.Core.Entities;

namespace SFA.DAS.Payments.ProviderPayments.Application.Services
{
    public interface IMonthEndService
    {
        Task<List<PaymentModel>> GetMonthEndPayments(string collectionPeriodName, long ukprn,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}