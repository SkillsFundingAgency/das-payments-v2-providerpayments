using Autofac;
using Autofac.Extras.Moq;
using FluentAssertions;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.Payments.Application.Repositories;
using SFA.DAS.Payments.EarningEvents.Messages.Events;
using SFA.DAS.Payments.Model.Core.Entities;
using SFA.DAS.Payments.Monitoring.Jobs.Client;
using SFA.DAS.Payments.Monitoring.Jobs.Client.Infrastructure.Ioc;
using SFA.DAS.Payments.ProviderPayments.Application.Repositories;
using SFA.DAS.Payments.ProviderPayments.Application.Services;
using SFA.DAS.Payments.ProviderPayments.Domain;
using SFA.DAS.Payments.ProviderPayments.Domain.Models;
using SFA.DAS.Payments.ProviderPayments.ProviderPaymentsService.Infrastructure;
using SFA.DAS.Payments.ProviderPayments.ProviderPaymentsService.Infrastructure.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Payments.ProviderPayments.ProviderPaymentsService.UnitTests.Services
{
    [TestFixture]
    public class AutofacRegistrationTests
    {

        [Test]
        public async Task ShouldResolveJobMessageClientFactoryAsync()
        {
            // Arrange
            var builder = new ContainerBuilder();

            builder.RegisterModule<ProviderPaymentsInstaller>();

            var endpointConfiguration = new EndpointConfiguration("TestEndpoint");
            builder.RegisterInstance(endpointConfiguration).As<EndpointConfiguration>();

            var container = builder.Build();

            // Act
            var factory = container.Resolve<IJobMessageClientFactory>();
            var instance1 = container.Resolve<IJobMessageClientFactory>();
            var instance2 = container.Resolve<IJobMessageClientFactory>();


            // Assert
            factory.Should().NotBe(null);
            Assert.That(factory, Is.Not.Null, "IJobMessageClientFactory should be registered and resolvable.");
            instance1.Should().BeOfType<JobMessageClientFactory>();
            instance2.Should().BeSameAs(instance1);
        }
    }

    [TestFixture]
    public class JobStatusClientModuleTests
    {
        [Test]
        public void Should_register_IJobMessageClientFactory_As_Single_Instance()
        {
            // Arrange
            var builder = new ContainerBuilder();
            builder.RegisterModule<JobStatusClientModule>();
            var container = builder.Build();

            // Act
            var instance1 = container.Resolve<IJobMessageClientFactory>();
            var instance2 = container.Resolve<IJobMessageClientFactory>();

            // Assert
            instance1.Should().BeOfType<JobMessageClientFactory>();
            instance2.Should().BeSameAs(instance1);
        }
    }
}
