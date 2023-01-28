using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace AddressBookUnitTest
{
    public class DependencySetupFixture
    {
        public DependencySetupFixture()
        {
            var serviceCollection = new ServiceCollection();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger>();    
        }
        public ILogger logger { get; set; }
    }
}
