using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Payments.EarningEvents.Messages.Events;
using SFA.DAS.Payments.Model.Core;
using SFA.DAS.Payments.ProviderPayments.Application.Validators;

namespace SFA.DAS.Payments.ProviderPayments.Application.UnitTests.Validators
{
    [TestFixture]
    public class DASEarningsReceivedEventValidatorTests
    {
        private DASEarningsReceivedEventValidator sut;
        private DasEarningsReceivedEvent message;

        [SetUp]
        public void SetUp()
        {
            sut = new DASEarningsReceivedEventValidator();
            message = new DasEarningsReceivedEvent()
            {
                CourseCode = "ABC123",
                CollectionPeriod = new CollectionPeriod
                {
                    AcademicYear = 24526,
                    Period = 1
                },
                UKPRN = 10001234,
                ULN = 123456789,
                LearningAimReference = "1234567-aim-ref",
                EarningsId = Guid.NewGuid()
            };
        }


        [Test]
        public void Validate_throws_no_exceptions_for_a_valid_message()
        {
            var result = sut.Validate(message);

            result.Should().BeTrue();
        }

        [Test]
        public void Validate_rejects_empty_messages()
        {
            Action act = () => sut.Validate(null);

            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Message must not be null");
        }

        [Test]
        public void Validate_rejects_empty_course_code()
        {
            message.CourseCode = " ";

            Action act = () => sut.Validate(message);

            act.Should().Throw<ArgumentException>()
                .WithMessage("CourseCode must be provided");
        }

        [Test]
        public void Validate_rejects_empty_collection_period_object()
        {
            message.CollectionPeriod = null;

            Action act = () => sut.Validate(message);

            act.Should().Throw<ArgumentException>()
                .WithMessage("CollectionPeriod must be provided");
        }

        [Test]
        public void Validate_rejects_empty_academic_year()
        {
            message.CollectionPeriod.AcademicYear = 0;

            Action act = () => sut.Validate(message);

            act.Should().Throw<ArgumentException>()
                .WithMessage("AcademicYear must be provided");
        }

        [Test]
        public void Validate_rejects_empty_collection_period_period()
        {
            message.CollectionPeriod.Period = 0;

            Action act = () => sut.Validate(message);

            act.Should().Throw<ArgumentException>()
                .WithMessage("CollectionPeriod.Period must be provided");
        }

        [Test]
        public void Validate_rejects_out_of_bounds_collection_period_period()
        {
            message.CollectionPeriod.Period = 15;

            Action act = () => sut.Validate(message);

            act.Should().Throw<ArgumentException>()
                .WithMessage("CollectionPeriod Period is invalid");
        }

        [Test]
        public void Validate_rejects_empty_ukprn()
        {
            message.UKPRN = 0;

            Action act = () => sut.Validate(message);

            act.Should().Throw<ArgumentException>()
                .WithMessage("UKPRN must be provided");
        }

        [Test]
        public void Validate_rejects_empty_uln()
        {
            message.ULN = 0;

            Action act = () => sut.Validate(message);

            act.Should().Throw<ArgumentException>()
                .WithMessage("ULN must be provided");
        }

        [Test]
        public void Validate_rejects_empty_learning_aim_reference()
        {
            message.LearningAimReference = "";

            Action act = () => sut.Validate(message);

            act.Should().Throw<ArgumentException>()
                .WithMessage("LearningAimReference must be provided");
        }

        [Test]
        public void Validate_rejects_empty_earnings_id()
        {
            message.EarningsId = Guid.Empty;

            Action act = () => sut.Validate(message);

            act.Should().Throw<ArgumentException>()
                .WithMessage("EarningsId must be provided");
        }
    }
}
