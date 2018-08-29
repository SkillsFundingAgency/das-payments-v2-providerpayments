﻿using System;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using NServiceBus;
using NServiceBus.Features;
using NUnit.Framework;
using SFA.DAS.Payments.EarningEvents.Messages.Entities;
using SFA.DAS.Payments.EarningEvents.Messages.Events;

namespace SFA.DAS.Payments.RequiredPayments.AcceptanceTests.Tests
{
    //this is a temporary test just to get messaging working
    [TestFixture]
    public class RequiredPaymentsIntegrationTest
    {
        protected static IEndpointInstance Sender;
        protected IPayableEarningEvent Earning;

        [OneTimeSetUp]
        public static async Task SetUpMessaging()
        {
            Sender = await CreateMessageSender();
        }

        [SetUp]
        public void SetUp()
        {
            Earning = new PayableEarningEvent
            {
                Ukprn = 1,
                LearnRefNumber = "2",
                ContractType = ContractType.Act2,
                Learner = new LearnerEntity(),
                LearnAim = new LearnAimEntity
                {
                    ProgrammeType = 3,
                    FrameworkCode = 4,
                    PathwayCode = 5,
                    StandardCode = 6,
                    LearnAimRef = "7"
                },
                PriceEpisodes = new[]
                {
                    new PriceEpisodeEntity
                    {
                        StartDate = DateTime.Today.AddMonths(-1),
                        EndDate = DateTime.Today,
                        Periods = new byte[]
                        {
                            1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12
                        },
                        Price = 111
                    }
                }
            };

        }

        [Test]
        public async Task Processes_Earning_Event()
        {
            Console.WriteLine("Sending test earning event");
            await Sender.Send(Earning).ConfigureAwait(false);
            Console.WriteLine("Finished sending earning");
        }

        private static async Task<IEndpointInstance> CreateMessageSender()
        {
            var endpointConfiguration = new EndpointConfiguration("required-payments-test-sender");

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningMessagesAs(type => (type.Namespace?.StartsWith("SFA.DAS.Payments")??false) && (type.Namespace?.Contains(".Messages")??false));
            //conventions.DefiningEventsAs(type => (type.Namespace?.StartsWith("SFA.DAS.Payments")??false) && (type.Namespace?.Contains(".Messages.Commands")??false));
            //conventions.DefiningEventsAs(type => (type.Namespace?.StartsWith("SFA.DAS.Payments")??false) && (type.Namespace?.Contains(".Messages.Events")??false));

            endpointConfiguration.UsePersistence<AzureStoragePersistence>()
                .ConnectionString(TestConfiguration.StorageConnectionString);

            endpointConfiguration.DisableFeature<TimeoutManager>();
            endpointConfiguration.DisableFeature<MessageDrivenSubscriptions>();
            
            endpointConfiguration.UseTransport<AzureStorageQueueTransport>()
                .ConnectionString(TestConfiguration.StorageConnectionString)
                .Routing()
                .RouteToEndpoint(typeof(IEarningEvent).Assembly,EndpointNames.RequiredPayments);
            
            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
//            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UseContainer<AutofacBuilder>();
            endpointConfiguration.SendOnly();
            return await Endpoint.Start(endpointConfiguration);
        }

        [Test]
        public async Task Invoke_Require_Payments_Actor()
        {
            var actorId = new ActorId("12345");
//            var actor = new ActorProxyFactory().CreateActorProxy<SFA.DAS.Payments.RequiredPayments.RequiredPaymentsService.Interfaces.IRequiredPaymentsService>(new Uri("fabric:/SFA.DAS.Payments.RequiredPayments.ServiceFabric/RequiredPaymentsServiceActorService"), actorId);
            //var paymentsDue = await actor.HandlePayableEarning(Earning, CancellationToken.None).ConfigureAwait(false);

        }
    }
}