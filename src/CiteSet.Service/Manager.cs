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
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using CiteSet.Database;
using CiteSet.Model;
using CiteSet.Web;
using CiteSet.Web.Contract;

namespace CiteSet.Service
{
	/// <summary>
	/// Windows service that hosts both the database server and the
	/// WCF services defined in the app.config.
	/// </summary>
	public partial class Manager : ServiceBase
	{
		private IServiceHost _resourceService;
		private IServiceHost _projectService;
		private IDatabaseServer _databaseServer;
		/// <summary>
		/// Create an instance of the Manager.
		/// </summary>
		public Manager()
		{
			InitializeComponent();
		}
		/// <summary>
		/// Method to execute when the service starts.
		/// </summary>
		/// <param name="args"></param>
		protected override void OnStart(string[] args)
		{
			// Start database server.
			_databaseServer = DatabaseServer.CreateInstance();
			_databaseServer.Open();
			// Start RESTful resource service.
			_resourceService = ServiceHost.CreateInstance<ResourceService>();
			_resourceService.Open();
			// Start RESTful project service.
			_projectService = ServiceHost.CreateInstance<ProjectService>();
			_projectService.Open();
		}
		/// <summary>
		/// Method to execute when the service stops.
		/// </summary>
		protected override void OnStop()
		{
			_projectService.Close();
			_resourceService.Close();
			_databaseServer.Close();
		}
	}
}
