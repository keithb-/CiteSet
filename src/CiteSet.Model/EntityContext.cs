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
using System.Security.Principal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;

namespace CiteSet.Model
{
	/// <summary>
	/// Provides a simple context for manipulating entities.
	/// </summary>
	/// <remarks>The EntityContext follows the singleton pattern and provides
	/// access to a collection of methods for creating and manipulating entities.
	/// </remarks>
	public class EntityContext 
		: IEntityContext, IDisposable
	{
		private Uri _connectionString;
		private IIdentity _user;
		private IObjectContainer _container;
		private bool _disposed;
		/// <summary>
		/// Creates an entity context singleton that is wrapped by the Enterprise Library
		/// policy injection.
		/// </summary>
		/// <param name="connectionStringName">A valid <see cref="System.Uri"/> instance 
		/// using the <c>db4o://</c> scheme.</param>
		/// <returns>IEntityContext</returns>
		public static IEntityContext CreateInstance(string connectionStringName)
		{
			return PolicyInjection.Wrap<IEntityContext>(new EntityContext(connectionStringName));
		}
		/// <summary>
		/// Creates an entity context using the specified connection string. 
		/// </summary>
		/// <param name="connectionStringName">A valid <see cref="System.Uri"/> instance 
		/// using the <c>db4o://</c> scheme.</param>
		/// <exception cref="System.ArgumentException">Throws when connection string is not valid.</exception>
		public EntityContext(string connectionStringName)
		{
			if (!Uri.TryCreate(
				ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString, 
				UriKind.Absolute, 
				out _connectionString))
			{
				throw new ArgumentException("Invalid connection string");
			}
			var userInfo = _connectionString.UserInfo.Split(new char[] {':'});
			_user = new GenericIdentity(userInfo[0]);
		}
		/// <summary>
		/// Open connection to the entity service.
		/// </summary>
		/// <exception cref="System.Exception">Throws when the singleton
		/// is already initialized.</exception>
		public void Open()
		{
			if (_container != null)
			{
				throw new Exception("Context is open");
			}
			var userInfo = _connectionString.UserInfo.Split(new char[] {':'});
			_container = Db4oClientServer.OpenClient(
				Db4oClientServer.NewClientConfiguration(),
				_connectionString.Host,
				_connectionString.Port,
				userInfo[0],
				userInfo[1]);
			
		}
		/// <summary>
		/// Close an open connection to the entity service.
		/// </summary>
		/// <exception cref="System.Exception">Throws when the singleton
		/// is not initialized.</exception>
		public void Close()
		{
			if (_container == null)
			{
				throw new Exception("Context is not open");
			}
			_container.Close();
			_container.Dispose();
			_container = null;
		}
		/// <summary>
		/// Identity of the active user.
		/// </summary>
		public IIdentity Identity
		{
			get
			{
				return _user;
			}
			set
			{
				_user = value;
			}
		}
		/// <summary>
		/// Store a entity using the entity service.
		/// </summary>
		/// <typeparam name="T">Represents an entity that inherits from the <c>EntityBase</c> class.</typeparam>
		/// <param name="input">Entity to be stored by the entity service.</param>
		/// <returns>Reference to the actual entity stored by the entity service and which may include
		/// properties that have been supplied by the entity service (e.g. unique identifier).
		/// Therefore the reference may differ from the <c>input</c> parameter.</returns>
		public T Save<T>(T input) where T : EntityBase
		{
			T result = input;
			((EntityBase)input).AuditUpdate(_user);
			_container.Store(result);
			_container.Commit();
			return result;
		}
		/// <summary>
		/// Delete an entity using a unique identifier.
		/// </summary>
		/// <typeparam name="T">Represents an entity that inherits from the <c>EntityBase</c> class.</typeparam>
		/// <param name="id">Unique identifier for the entity.</param>
		/// <returns>Reference to the actual entity that was removed from storage.</returns>
		public T Delete<T>(string id) where T : EntityBase
		{
			var proto = Activator.CreateInstance<T>();
			((EntityBase)proto).Id = id;
			var result = DeleteAll<T>(proto);
			if (result.Any())
				return result[0];
			return null;
		}
		/// <summary>
		/// Delete one or more entities using an entity prototype.
		/// </summary>
		/// <typeparam name="T">Represents an entity that inherits from the <c>EntityBase</c> class.</typeparam>
		/// <param name="proto">Entity prototype. Property values will be used as
		/// criteria for locating entities.</param>
		/// <returns>A collection of references to the actual entities that were removed
		/// from storage, if any.</returns>
		public IList<T> DeleteAll<T>(T proto) where T : EntityBase
		{
			var set = FindAll<T>(proto);
			if (!set.Any())
				return null;

			var result = new List<T>();
			foreach (T item in set)
			{
				result.Add(item);
				_container.Delete(item);
				_container.Commit();
			}
			return result;
		}
		//kbielaczyc.2012.07.28: This method is not used.
		//protected static IList<T> ToList<T>(IObjectSet result) where T : EntityBase
		//{
		//    var list = new List<T>();
		//    foreach (T item in result)
		//    {
		//        list.Add(item);
		//    }
		//    return list;
		//}
		/// <summary>
		/// Find all entities for a specific class type.
		/// </summary>
		/// <typeparam name="T">Represents an entity that inherits from the <c>EntityBase</c> class.</typeparam>
		/// <returns>A collection of references to the actual entities of the specified class type.</returns>
		public IList<T> FindAll<T>() where T : EntityBase
		{
			return _container.Query<T>();
		}
		/// <summary>
		/// Find all entities using an entity prototype.
		/// </summary>
		/// <typeparam name="T">Represents an entity that inherits from the <c>EntityBase</c> class.</typeparam>
		/// <param name="proto">Entity prototype. Property values will be used as
		/// criteria for locating entities.</param>
		/// <returns>A collection of references to the actual entities that match 
		/// the specified prototype, if any.</returns>
		public IList<T> FindAll<T>(T proto) where T : EntityBase
		{
			var results = _container.QueryByExample(proto);
			var temp = new List<T>();
			var loop = results.GetEnumerator();
			while (loop.MoveNext())
			{
				temp.Add((T)loop.Current);
			}
			return temp;
		}
		/// <summary>
		/// Find an entity using a unique identifier.
		/// </summary>
		/// <typeparam name="T">Represents an entity that inherits from the <c>EntityBase</c> class.</typeparam>
		/// <param name="id">Unique identifier for the entity.</param>
		/// <returns>Reference to the actual entity with the specified unique identifier.</returns>
		public T Find<T>(string id) where T : EntityBase
		{
			var result = _container.Query<T>(x => x.Id == id);
			if (result.Count == 1)
				return result[0];
			if (result.Count > 1)
				throw new Exception("Duplicate entities");
			return null;
		}
		/// <summary>
		/// Standard implementation of .NET dispose pattern. 
		/// <see href="http://msdn.microsoft.com/en-us/library/fs2xkftw.aspx">Implementing a Dispose Method</see>
		/// </summary>
		public void Dispose()
		{
			Dispose(true);

			// Use SupressFinalize in case a subclass
			// of this type implements a finalizer.
			GC.SuppressFinalize(this);
		}
		/// <summary>
		/// Standard implementation of .NET dispose pattern. 
		/// <see href="http://msdn.microsoft.com/en-us/library/fs2xkftw.aspx">Implementing a Dispose Method</see>
		/// </summary>
		protected virtual void Dispose(bool disposing)
		{
			// If you need thread safety, use a lock around these 
			// operations, as well as in your methods that use the resource.
			if (!_disposed)
			{
				if (disposing)
				{
					if (_container != null)
						_container.Close();
					_container.Dispose();
				}

				// Indicate that the instance has been disposed.
				_container = null;
				_disposed = true;
			}
		}
	}
}
