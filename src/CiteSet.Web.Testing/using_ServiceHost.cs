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
using System.Text;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CiteSet.Web;
using CiteSet.Web.Contract;
using CiteSet.Model;
using CiteSet.Database;

namespace CiteSet.Web.Testing
{
	[TestClass]
	public abstract class using_ServiceHost
	{
		public IServiceHost Service { get; protected set; }
		public IDatabaseServer Server { get; protected set; }
		public bool Result { get; protected set; }

		[TestInitialize]
		public void Arrange()
		{
			Server = new DatabaseServer();
			Server.Open();
			Service = new ServiceHost<ResourceService>();
		}

		[TestCleanup]
		public void Cleanup()
		{
			Service.Close();
			Server.Close();
		}
	}

	[TestClass]
	public class when_opening_service
		: using_ServiceHost
	{
		[TestInitialize]
		public void Act()
		{
			Service.Open();
			Result = true;
		}

		[TestCleanup]
		public new void Cleanup()
		{
			Service.Close();
		}

		[TestMethod]
		public void then_result_is_true()
		{
			Assert.IsTrue(Result);
		}
	}

	[TestClass]
	public class when_calling_service
		: using_ServiceHost
	{
		[TestInitialize]
		public void Act()
		{
			Service.Open();
			try
			{
				var request = WebRequest.Create("http://localhost:5001/resource/") as HttpWebRequest;
				using (var response = request.GetResponse())
				{
					using (var reader = new StreamReader(response.GetResponseStream()))
					{
						System.Diagnostics.Debug.Write(reader.ReadToEnd());
					}
				}
				Result = true;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.ToString());
				Result = false;
			}
		}

		[TestCleanup]
		public new void Cleanup()
		{
			Service.Close();
		}

		[TestMethod]
		public void then_result_is_true()
		{
			Assert.IsTrue(Result);
		}
	}
}
