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
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CiteSet.Database.Testing
{
	[TestClass]
	public abstract class using_DatabaseServer
	{
		public IDatabaseServer Server { get; protected set; }
		public bool Result { get; protected set; }

		[TestInitialize]
		public void Arrange()
		{
			//Server = new DatabaseServer();
			Server = DatabaseServer.CreateInstance();
		}
	}

	[TestClass]
	public class when_opening_databaseServer
		: using_DatabaseServer
	{
		[TestInitialize]
		public void Act()
		{
			Server.Open();
			Result = true;
		}

		[TestCleanup]
		public void Cleanup()
		{
			Server.Close();
		}
		
		[TestMethod]
		public void then_result_is_true()
		{
			Assert.IsTrue(Result);
		}
	}

	[TestClass]
	public class when_closing_databaseServer
		: using_DatabaseServer
	{
		[TestInitialize]
		public void Act()
		{
			Server.Open();
			Server.Close();
			Result = true;
		}

		[TestMethod]
		public void then_result_is_true()
		{
			Assert.IsTrue(Result);
		}
	}
}
