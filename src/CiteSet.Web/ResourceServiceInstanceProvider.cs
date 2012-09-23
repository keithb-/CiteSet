/*
Copyright (c) 2012 by Keith R. Bielaczyc. All Right Reserved.

Unless required by applicable law or agreed to in writing,
software distributed under the License is distributed on an
"AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
KIND, either express or implied.  See the License for the
specific language governing permissions and limitations
under the License.
*/
using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using CiteSet.Model;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;

namespace CiteSet.Web
{
	/// <summary>
	/// Provides a new instance of the RESTful resource service.
	/// </summary>
	/// <remarks>The <c>ResourceServiceInstanceProvider</c> class is a form
	/// of dependency injection that allows the WCF service to be constructed with
	/// parameters. 
	/// <para>Without this instance provider and the behavior extension the WCF services
	/// defined in this project would not have been able to read the connection string
	/// values from the app.config file.</para></remarks>
	public class ResourceServiceInstanceProvider
		: IInstanceProvider, IServiceBehavior
	{
		private string _connectionStringName;
		/// <summary>
		/// Create a new instance of the ResourceServiceInstanceProvider using connection string value from the app.config file.
		/// </summary>
		public ResourceServiceInstanceProvider()
		{
			var config = ConfigurationManager.GetSection("citeSet/resourceService")
				as CiteSet.Web.Configuration;
			_connectionStringName = config.ConnectionString.Name;
		}
		/// <summary>
		/// Create a new instance of the ResourceServiceInstanceProvider using the connection string argument.
		/// </summary>
		/// <param name="connectionStringName"></param>
		public ResourceServiceInstanceProvider(string connectionStringName)
		{
			_connectionStringName = connectionStringName;
		}
		/// <summary>
		/// GetInstance is used to create an instance of ResourceService wrapped by the PolicyInjection framework.
		/// </summary>
		/// <param name="instanceContext"></param>
		/// <param name="message">Not used.</param>
		/// <returns>Contract.IResourceService</returns>
		public object GetInstance(System.ServiceModel.InstanceContext instanceContext, System.ServiceModel.Channels.Message message)
		{
			// Start client connection to database.
			var context = EntityContext.CreateInstance(_connectionStringName);
			context.Open();
			// Return indexer service.
			return PolicyInjection.Wrap<Contract.IResourceService>(new ResourceService(
				context,
				new PageAdapter<Resource>(
					new ResourceBridge(context.Identity))));
		}
		/// <summary>
		/// GetInstance is used to create an instance of ResourceService wrapped by the PolicyInjection framework.
		/// </summary>
		/// <param name="instanceContext"></param>
		/// <returns>Contract.IResourceService</returns>
		public object GetInstance(System.ServiceModel.InstanceContext instanceContext)
		{
			return GetInstance(instanceContext, null);
		}
		/// <summary>
		/// Not used.
		/// </summary>
		/// <param name="instanceContext"></param>
		/// <param name="instance"></param>
		public void ReleaseInstance(System.ServiceModel.InstanceContext instanceContext, object instance)
		{
			return;
		}
		/// <summary>
		/// ApplyDispatchBehavior creates an instance of the ResourceServiceInstanceProvider for each defined end point.
		/// </summary>
		/// <param name="serviceDescription"></param>
		/// <param name="serviceHostBase"></param>
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
							= new ResourceServiceInstanceProvider(_connectionStringName);
					}
				}
			}
		}
		/// <summary>
		/// Not used.
		/// </summary>
		/// <param name="serviceDescription"></param>
		/// <param name="serviceHostBase"></param>
		/// <param name="endpoints"></param>
		/// <param name="bindingParameters"></param>
		public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
		{
			return;
		}
		/// <summary>
		/// Not used.
		/// </summary>
		/// <param name="serviceDescription"></param>
		/// <param name="serviceHostBase"></param>
		public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
		{
			return;
		}
	}
}
