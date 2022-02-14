using Microsoft.Extensions.Configuration;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Xml;
namespace OneConnector.Services.Utils
{
    public class OneSoap<T>
    {
        public IConfiguration Configuration { get; }

        public ChannelFactory<T> Factory { get; }

        public T Client
        {
            get
            {
                return Factory.CreateChannel();
            }
        }

        public DisposableWrapper<T> DisposableClient
        {
            get
            {
                T client = Client;
                System.ServiceModel.IClientChannel clientChannel = (System.ServiceModel.IClientChannel)client;
                return new DisposableWrapper<T>(
                    client,
                    (client) => clientChannel.Close()
                ); 
            }
        }

        public string User { get; }

        public string Password { get; }

        public string EndPoint { get; }

        public OneSoap(IConfiguration configuration)
        {
            Configuration = configuration;

            string fullTypeName = typeof(T).FullName;
            string shortTypeName = fullTypeName.Substring(fullTypeName.LastIndexOf('.') + 1);

            User = Configuration[$"{shortTypeName}:User"];
            Password = Configuration[$"{shortTypeName}:Password"];
            EndPoint = Configuration[$"{shortTypeName}:EndPoint"];

            BasicHttpBinding binding = new()
            {
                SendTimeout = TimeSpan.FromSeconds(100),
                MaxBufferSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
                AllowCookies = true,
                ReaderQuotas = XmlDictionaryReaderQuotas.Max
            };

            binding.Security.Mode = (BasicHttpSecurityMode)Configuration.GetValue<int>($"{shortTypeName}:BasicHttpSecurityMode");
            EndpointAddress address = new EndpointAddress(EndPoint);
            Factory = new ChannelFactory<T>(binding, address);

            if (binding.Security.Mode == BasicHttpSecurityMode.TransportCredentialOnly)
            {
                if (!Factory.Endpoint.EndpointBehaviors.TryGetValue(typeof(ClientCredentials), out IEndpointBehavior behaviour))
                {
                    throw new Exception("Endpoint, could not obtain ClientCredentials");
                }
                ClientCredentials credentials = (ClientCredentials)behaviour;
                credentials.UserName.UserName = User;
                credentials.UserName.Password = Password;
            }
        }
    }
}
