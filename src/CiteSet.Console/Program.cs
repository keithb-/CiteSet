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
using CiteSet.Database;
using CiteSet.Web;

namespace CiteSet.Console
{
	class Program
	{
		private static IServiceHost _resourceService;
		private static IServiceHost _projectService;
		private static IDatabaseServer _databaseServer;

		static void Main(string[] args)
		{
			// Start database server.
			_databaseServer = DatabaseServer.CreateInstance();
			_databaseServer.Open();
			// Start RESTful resource service.
			_resourceService = ServiceHost.CreateInstance<ResourceService>();
			_resourceService.Open();
			// Start RESTful resource service.
			_projectService = ServiceHost.CreateInstance<ProjectService>();
			_projectService.Open();

			System.Console.WriteLine("Press enter to stop services...");
			System.Console.ReadLine();

			_projectService.Close();
			_resourceService.Close();
			_databaseServer.Close();
		}
	}
}
