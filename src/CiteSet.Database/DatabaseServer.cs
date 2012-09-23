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
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;

namespace CiteSet.Database
{
	/// <summary>
	/// Provides a simple implementation of a Db4o database server.
	/// </summary>
	/// <remarks>The DatabaseServer follows the singleton pattern and provides
	/// access to a single database file as defined in the connection string.
	/// </remarks>
	public class DatabaseServer
		: IDatabaseServer, IDisposable
	{
		private Uri _connectionString;
		private IIdentity _user;
		private IServerConfiguration _config;
		private IObjectServer _databaseServer;
		private bool _disposed;
		/// <summary>
		/// Creates a Db4o database server singleton that is wrapped by the Enterprise Library
		/// policy injection.
		/// </summary>
		/// <returns>IDatabaseServer</returns>
		public static IDatabaseServer CreateInstance()
		{
			return PolicyInjection.Wrap<IDatabaseServer>(new DatabaseServer());
		}
		/// <summary>
		/// Createa a DatabaseServer instance using the configuration settings from
		/// the app.config file.
		/// </summary>
		public DatabaseServer()
		{
			var config = ConfigurationManager.GetSection("citeSet/database")
				as CiteSet.Database.Configuration;
			ValidateConnectionString(config.ConnectionString.Name);
		}
		/// <summary>
		/// Creates a DatabaseServer instance using the connection string parameter.
		/// </summary>
		/// <param name="connectionStringName">A valid <see cref="System.Uri"/> instance 
		/// using the <c>db4o://</c> scheme.
		/// </param>
		public DatabaseServer(string connectionStringName)
		{
			ValidateConnectionString(connectionStringName);
		}
		/// <summary>
		/// Validates the <see cref="System.Uri"/> instance.
		/// </summary>
		/// <param name="connectionStringName">A valid <see cref="System.Uri"/> instance 
		/// using the <c>db4o://</c> scheme.</param>
		private void ValidateConnectionString(string connectionStringName)
		{
			if (!Uri.TryCreate(
				ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString,
				UriKind.Absolute,
				out _connectionString))
			{
				throw new ArgumentException("Invalid connection string");
			}
		}
		/// <summary>
		/// Opens a connection to the database file.
		/// </summary>
		/// <remarks>The <c>Open</c> method assumes that the connection string will
		/// contain user credentials which are used when calling the 
		/// <see cref="Db4objects.Db4o.CS.Db4oClientServer"/> <c>GrantAccess</c> method.</remarks>
		/// <exception cref="System.Exception">Throws when the singleton
		/// is already initialized.</exception>
		public void Open()
		{
			if (_databaseServer != null)
			{
				throw new Exception("Server is running");
			}
			var userInfo = _connectionString.UserInfo.Split(new char[] { ':' });
			_user = new GenericIdentity(userInfo[0]);
			_config = Db4oClientServer.NewServerConfiguration();
			_databaseServer = Db4oClientServer.OpenServer(_connectionString.AbsolutePath,
				_connectionString.Port);
			_databaseServer.GrantAccess(userInfo[0], userInfo[1]);
		}
		/// <summary>
		/// Closes the open database file connection.
		/// </summary>
		public void Close()
		{
			if (_databaseServer == null)
			{
				throw new Exception("Server is not running");
			}
			_databaseServer.Close();
			_databaseServer.Dispose();
			_databaseServer = null;
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
					if (_databaseServer != null)
						_databaseServer.Close();
					_databaseServer.Dispose();
				}

				// Indicate that the instance has been disposed.
				_databaseServer = null;
				_disposed = true;
			}
		}
	}
}
