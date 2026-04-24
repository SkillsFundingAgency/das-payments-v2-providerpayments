using System;
using System.Data;
using SFA.DAS.Payments.Audit.Application.Data;
using SFA.DAS.Payments.ProviderPayments.Model;

namespace SFA.DAS.Payments.ProviderPayments.Application.Data
{
    public class ProviderPaymentDataTable : PeriodisedPaymentsEventModelDataTable<ProviderPaymentEventModel>
    {
        public ProviderPaymentDataTable()
        {
            DataTable.Columns.AddRange(new[]
            {
                new DataColumn("FundingSourceEventId", typeof(Guid)),
                new DataColumn("RequiredPaymentEventId", typeof(Guid)),
                new DataColumn("ClawbackSourcePaymentEventId", typeof(Guid)),
                new DataColumn("FundingSource"),
                new DataColumn("ApprenticeshipEmployerType", typeof(byte)),
                new DataColumn("ReportingAimFundingLineType", typeof(string)),
                new DataColumn("LearningAimSequenceNumber", typeof(long)),
                new DataColumn("AgeAtStartOfLearning", typeof(byte)),
                new DataColumn("FundingPlatformType", typeof(byte)),
                new DataColumn("CourseType", typeof(byte)),
                new DataColumn("LearningType", typeof(byte)),
                new DataColumn("CourseCode", typeof(string)),
            });
        }

        protected override DataRow CreateDataRow(ProviderPaymentEventModel eventModel)
        {
            var dataRow = base.CreateDataRow(eventModel);
            dataRow["FundingSourceEventId"] = eventModel.FundingSourceId;
            dataRow["RequiredPaymentEventId"] = eventModel.RequiredPaymentEventId;
            dataRow["FundingSource"] = (byte)eventModel.FundingSource;

            if (!eventModel.ClawbackSourcePaymentEventId.HasValue)
            {
                dataRow["ClawbackSourcePaymentEventId"] = Guid.Empty;
            }
            else
            {
                dataRow["ClawbackSourcePaymentEventId"] = eventModel.ClawbackSourcePaymentEventId.Value;
            }

            if (!eventModel.ApprenticeshipId.HasValue)
            {
                dataRow["ApprenticeshipId"] = DBNull.Value;
            }
            else
            {
                dataRow["ApprenticeshipId"] = eventModel.ApprenticeshipId.Value;
            }

            if (!eventModel.ApprenticeshipPriceEpisodeId.HasValue)
            {
                dataRow["ApprenticeshipPriceEpisodeId"] = DBNull.Value;
            }
            else
            {
                dataRow["ApprenticeshipPriceEpisodeId"] = eventModel.ApprenticeshipPriceEpisodeId.Value;
            }

            dataRow["ApprenticeshipEmployerType"] = eventModel.ApprenticeshipEmployerType;
            dataRow["ReportingAimFundingLineType"] = eventModel.ReportingAimFundingLineType;

            if (!eventModel.LearningAimSequenceNumber.HasValue)
            {
                dataRow["LearningAimSequenceNumber"] = DBNull.Value;
            }
            else
            {
                dataRow["LearningAimSequenceNumber"] = eventModel.LearningAimSequenceNumber.Value;
            }

            if (!eventModel.AgeAtStartOfLearning.HasValue)
            {
                dataRow["AgeAtStartOfLearning"] = DBNull.Value;
            }
            else
            {
                dataRow["AgeAtStartOfLearning"] = eventModel.AgeAtStartOfLearning;
            }
            
            dataRow["FundingPlatformType"] = (byte)eventModel.FundingPlatformType;

            dataRow["CourseType"] = eventModel.CourseType.HasValue ? (byte)eventModel.CourseType.Value : DBNull.Value;
            dataRow["LearningType"] = eventModel.LearningType.HasValue ? (byte)eventModel.LearningType.Value : DBNull.Value;
            dataRow["CourseCode"] = eventModel.CourseCode;

            return dataRow;
        }

        public override string TableName => "Payments2.Payment";
    }
}