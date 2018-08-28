﻿using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Payments.RequiredPayments.AcceptanceTests.Data
{
    public class ContractTypeEarnings
    {
        public ContractTypeEarnings(short contractType, short fromPeriod, short toPeriod, string academicYear)
        {
            ContractType = contractType;

            FromPeriod = fromPeriod;

            ToPeriod = toPeriod;

            AcademicYear = academicYear;
        }

        public ContractTypeEarnings(short contractType, short fromPeriod, short toPeriod, string academicYear, IEnumerable<OnProgrammeEarning> rawEarnings)
        :this(contractType, fromPeriod, toPeriod, academicYear)
        {
            OnProgrammeEarnings = rawEarnings.ToList();
        }

        public ContractTypeEarnings(short contractType, short fromPeriod, short toPeriod, string academicYear, IEnumerable<IncentiveEarning> rawEarnings)
            : this(contractType, fromPeriod, toPeriod, academicYear)
        {
            IncentiveEarnings = rawEarnings.ToList();
        }

        public void AddOnProgrammeEarnings(short period, IEnumerable<OnProgrammeEarning> earning)
        {
            ToPeriod = period;

            OnProgrammeEarnings.AddRange(earning);
        }

        public short ToPeriod { get; private set; }

        public short FromPeriod { get;  }

        public short ContractType { get; } 

        public string AcademicYear { get; }

        public List<OnProgrammeEarning> OnProgrammeEarnings { get; private set; } 

        public List<IncentiveEarning> IncentiveEarnings { get; }
    }
}