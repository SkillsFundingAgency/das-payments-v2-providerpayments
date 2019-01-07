﻿using SFA.DAS.Payments.ProviderPayments.Application.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Payments.ProviderPayments.Application.Services
{
    public class MonthEndEventHandlerService : IMonthEndEventHandlerService
    {
        private readonly IProviderPaymentsRepository providerPaymentsRepository;

        public MonthEndEventHandlerService(IProviderPaymentsRepository providerPaymentsRepository)
        {
            this.providerPaymentsRepository = providerPaymentsRepository;
        }

        public Task<List<long>> GetMonthEndUkprns(int collectionYear, int collectionMonth, CancellationToken cancellationToken = default(CancellationToken))
        {
            return providerPaymentsRepository.GetMonthEndUkprns(collectionYear, collectionMonth, cancellationToken);
        }
    }
}
