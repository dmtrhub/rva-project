using System.ServiceModel;

namespace Client.Services
{
    public static class ServiceClientFactory
    {
        public static CruiseServiceProxy CreateClient(string baseAddress = "http://localhost:8080/CruiseService/")
        {
            var binding = new BasicHttpBinding();
            var endpointAddress = new EndpointAddress(baseAddress);

            return new CruiseServiceProxy(binding, endpointAddress);
        }
    }
}