using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;
using ECommerce.Application.src.Utils;
using ECommerce.Domain.src.Enums;
using ECommerce.Domain.src.Models.PaymentSession;
using ECommerce.Domain.src.Utils;
using ECommerce.Infrastructure.src.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit.Abstractions;

namespace ECommerce.Infrastructure.Tests.Integration
{
    public class PaymentSessionServiceTest : IAsyncLifetime
    {
        private readonly IFutureDockerImage _image;
        private IContainer _container;
        private readonly ITestOutputHelper _output;

        public PaymentSessionServiceTest(ITestOutputHelper output)
        {
            _output = output;

            _image = new ImageFromDockerfileBuilder()
                .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), "../PaymentService")
                .WithDockerfile("Dockerfile")
                .Build();

            _image.CreateAsync().Wait();

            DotNetEnv.Env.TraversePath().Load();

            var cfg = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            string? stripeApiKey = Environment.GetEnvironmentVariable("Stripe__ApiKey");
            string? stripeWebhookSecret = Environment.GetEnvironmentVariable("Stripe__WebhookSecret");

            ArgumentNullException.ThrowIfNullOrWhiteSpace(stripeApiKey, "Stripe__ApiKey");
            ArgumentNullException.ThrowIfNullOrWhiteSpace(stripeWebhookSecret, "Stripe__WebhookSecret");

            _container = new ContainerBuilder().WithImage(_image)
                .WithPortBinding(8080, true)
                .WithEnvironment(new Dictionary<string, string>()
                {
                    { "Stripe__ApiKey", stripeApiKey },
                    { "Stripe__WebhookSecret", stripeWebhookSecret },
                })
                .Build();
        }
        public async Task InitializeAsync()
        {
            await _container.StartAsync();
        }

        public async Task DisposeAsync()
        {
            var logs = await _container.GetLogsAsync();
            _output.WriteLine(logs.Stderr);
            _output.WriteLine(logs.Stdout);
            await _container.DisposeAsync();
        }

        [Fact]
        public async Task PaymentService_HealthCheck()
        {
            var httpClient = new HttpClient();

            HttpResponseMessage res = await httpClient.GetAsync($"http://localhost:{_container.GetMappedPublicPort(8080)}/api/healthcheck");
            Assert.True(res.IsSuccessStatusCode);
        }

        [Fact]
        public async Task PaymentSessionService_ShouldBeAbleToCreateAndGetAPaymentSession()
        {
            string userId = Guid.NewGuid().ToString();

            var cfg = new MicroserviceNetworkConfig
            {
                PaymentServiceUrl = $"http://localhost:{_container.GetMappedPublicPort(8080)}/api"
            };

            var service = new PaymentSessionService(
                new HttpClient(),
                Options.Create(cfg),
                new Mock<ILogger<PaymentSessionService>>().Object
            );

            var sessionOptions = new Domain.src.Interfaces.Services.GeneratePaymentSessionOptions
            {
                UserId = userId,
                PaymentProvider = PaymentProvider.STRIPE,
                PriceCents = 500,
            };
            Result<PaymentProviderSession> paymentSessionResult = await service.GeneratePaymentSessionAsync(sessionOptions);

            // Assertions
            Assert.Empty(paymentSessionResult.Errors);
            PaymentProviderSession session = paymentSessionResult.GetValue();

            Assert.Equal(userId, session.UserId);

            // Getting payment service

            Result<PaymentProviderSession?> retrievedSessionResult = await service.GetPaymentSessionAsync(new Guid(userId));

            // Assertions
            Assert.Empty(retrievedSessionResult.Errors);

            PaymentProviderSession? retrievedSession = retrievedSessionResult.GetValue();

            Assert.NotNull(retrievedSession);
            Assert.Equal(session.SessionId, retrievedSession.SessionId);
            Assert.Equal(session.UserId, retrievedSession.UserId);
            Assert.Equal(session.Provider, retrievedSession.Provider);
        }
    }
}
