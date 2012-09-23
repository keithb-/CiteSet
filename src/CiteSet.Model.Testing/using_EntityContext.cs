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
using CiteSet.Database;
using CiteSet.Model;

namespace CiteSet.Model.Testing
{
	[TestClass]
	public abstract class using_EntityContext
	{
		protected const string ConnectionStringName = "Default";
		public IDatabaseServer Server { get; protected set; }
		public IEntityContext Context { get; protected set; }
		public bool Result { get; protected set; }

		[TestInitialize]
		public virtual void Arrange()
		{
			Server = new DatabaseServer(ConnectionStringName);
			Server.Open();
			Context = new EntityContext(ConnectionStringName);
		}

		[TestCleanup]
		public virtual void Cleanup()
		{
			Context.Close();
			Server.Close();
		}
	}

	[TestClass]
	public class when_opening_context
		: using_EntityContext
	{
		[TestInitialize]
		public void Act()
		{
			Context.Open();
			Result = true;
		}

		[TestMethod]
		public void then_result_is_true()
		{
			Assert.IsTrue(Result);
		}
	}

	[TestClass]
	public abstract class using_open_EntityContext
		: using_EntityContext
	{
		[TestInitialize]
		public override void Arrange()
		{
			base.Arrange();
			Context.Open();
		}

		[TestCleanup]
		public override void Cleanup()
		{
			base.Cleanup();
		}
	}

	[TestClass]
	public class when_adding_new_resource
		: using_open_EntityContext
	{
		[TestInitialize]
		public void Act()
		{
			var newResource = new Resource(Context.Identity)
			{
				Title = "Test"
			};
			Context.Save<Resource>(newResource);
			Result = true;
		}

		[TestMethod]
		public void then_result_is_true()
		{
			Assert.IsTrue(Result);
		}
	}

	[TestClass]
	public class when_querying_by_type
		: using_open_EntityContext
	{
		public IList<Resource> List { get; protected set; }

		[TestInitialize]
		public void Act()
		{
			List = Context.FindAll<Resource>();
		}

		[TestMethod]
		public void then_list_is_not_null()
		{
			Assert.IsNotNull(List);
		}

		[TestMethod]
		public void then_list_is_not_empty()
		{
			Assert.IsTrue(List.Count > 0);
		}
	}
}
