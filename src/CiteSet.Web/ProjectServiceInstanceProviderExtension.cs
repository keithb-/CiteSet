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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;

namespace CiteSet.Web
{
	/// <summary>
	/// Extends the service behaviors to define a service instance provider.
	/// </summary>
	/// <remarks>The <c>ProjectServiceInstanceProviderExtension</c> class is a form
	/// of dependency injection that allows the WCF service to be constructed with
	/// parameters. 
	/// <para>Without this extension and the instance provider the WCF services
	/// defined in this project would not have been able to read the connection string
	/// values from the app.config file.</para></remarks>
	public class ProjectServiceInstanceProviderExtension
		: BehaviorExtensionElement
	{
		/// <summary>
		/// Create a new instance of the ProjectServiceInstanceProvider.
		/// </summary>
		/// <returns></returns>
		protected override object CreateBehavior()
		{
			return new ProjectServiceInstanceProvider();
		}
		/// <summary>
		/// Return type of ProjectServiceInstanceProvider.
		/// </summary>
		public override Type BehaviorType
		{
			get { return typeof(ProjectServiceInstanceProvider); }
		}
	}
}
