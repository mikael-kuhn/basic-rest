using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Finance.DomainTest
{
    using Finance.Domain.Domain;
    using Finance.Domain.Repositories;
    using Finance.Domain;

    public sealed class DomainServiceCollectionExtensionsTest
    {
        private readonly Mock<IServiceCollection> serviceCollectionMock;

        public DomainServiceCollectionExtensionsTest()
        {
            serviceCollectionMock = new Mock<IServiceCollection>();
        }

        [Fact]
        public void AddDomain_InvoiceRepository_should_be_added()
        {
            // Act
            serviceCollectionMock.Object.AddDomain();

            // Assert
            serviceCollectionMock.Verify(sc =>
                sc.Add(
                    It.Is<ServiceDescriptor>(
                        sd =>
                            sd.ImplementationType == typeof(InvoiceRepository) &&
                            sd.ServiceType == typeof(IRepository<Invoice>) &&
                            sd.Lifetime == ServiceLifetime.Singleton)));
        }
    }
}