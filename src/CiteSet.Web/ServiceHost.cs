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
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using CiteSet.Model;
using CiteSet.Web.Contract;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;

namespace CiteSet.Web
{
	/// <summary>
	/// Provides a simple WCF entity service.
	/// </summary>
	public sealed class ServiceHost
	{
		/// <summary>
		/// Creates a WCF entity service that is wrapped by the Enterprise Library
		/// policy injection.
		/// </summary>
		/// <typeparam name="T">Represents the type of entity service to create.</typeparam>
		/// <returns>IServiceHost</returns>
		public static IServiceHost CreateInstance<T>()
		{
			return PolicyInjection.Wrap<IServiceHost>(new ServiceHost<T>());
		}
	}
	/// <summary>
	/// Provides a simple WCF entity service.
	/// </summary>
	public class ServiceHost<T>
		: IServiceHost, IDisposable
	{
		private System.ServiceModel.ServiceHost _service;
		private bool _disposed;
		/// <summary>
		/// Create a new WCF entity service.
		/// </summary>
		public ServiceHost()
		{
			_service = new System.ServiceModel.ServiceHost(typeof(T));
		}
		/// <summary>
		/// Open a connection to the WCF entity service.
		/// </summary>
		public void Open()
		{
			_service.Open();
		}
		/// <summary>
		/// Close an open connection to the WCF entity service.
		/// </summary>
		/// <exception cref="System.Exception">Throws if the service is not running.</exception>
		public void Close()
		{
			if (_service == null)
			{
				throw new Exception("Service is not running");
			}
			_service.Close();
			_service = null;
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
					if (_service != null)
						_service.Close();
				}

				// Indicate that the instance has been disposed.
				_service = null;
				_disposed = true;
			}
		}
	}
}
