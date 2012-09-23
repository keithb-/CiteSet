using System;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;

namespace CiteSet.Web
{
	public class IndexerInstanceProviderServiceBehavior : IServiceBehavior
	{
		private string _connectionStringName;

		public IndexerInstanceProviderServiceBehavior(string connectionStringName)
		{
			_connectionStringName = connectionStringName;
		}

		public void ApplyDispatchBehavior(ServiceDescription serviceDescription, 
			ServiceHostBase serviceHostBase)
		{
			foreach (ChannelDispatcher cd in serviceHostBase.ChannelDispatchers)
			{
				foreach (EndpointDispatcher ed in cd.Endpoints)
				{
					if (!ed.IsSystemEndpoint)
					{
						ed.DispatchRuntime.InstanceProvider 
							= new IndexerInstanceProvider(_connectionStringName);
					}
				}
			}
		}

		public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
		{
			return;
		}

		public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
		{
			return;
		}
	}
}
