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
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;
using System.Net;
using CiteSet.Web.Contract;
using CiteSet.Model;

namespace CiteSet.Web
{
	/// <summary>
	/// RESTful service implementation for manipulating project entities.
	/// </summary>
	[ServiceBehavior(UseSynchronizationContext = false)]
	public class ResourceService
		: IResourceService
	{
		private readonly IEntityContext _ctx;
		private readonly PageAdapter<Resource> _adapter;
		/// <summary>
		/// Create a new instance.
		/// </summary>
		public ResourceService() { }
		/// <summary>
		/// Create and initialize a new instance.
		/// </summary>
		/// <param name="ctx">Entity context for the service.</param>
		/// <param name="adapter">Adapter for parsing HTML pages.</param>
		public ResourceService(IEntityContext ctx, PageAdapter<Resource> adapter)
		{
			_ctx = ctx;
			_adapter = adapter;
		}
		/// <summary>
		/// Perform an index on the specified resource.
		/// </summary>
		/// <param name="request">Information needed to index a specific resource.</param>
		public void Index(IndexRequest request)
		{
			Uri address;
			if (!Uri.TryCreate(request.Path, UriKind.Absolute, out address))
			{
				//TODO: Return an exception.
			}
			Add(address);
		}
		/// <summary>
		/// Perform an index on the specified resource.
		/// </summary>
		/// <remarks>
		/// Request is redirected to the location of the newly added project.
		/// </remarks>
		/// <param name="page">Location of the resournce to be indexed.</param>
		/// <returns>201 Created</returns>
		public void Add(Uri page)
		{
			var resource = _adapter.Get(page);
			resource = _ctx.Save<Resource>(resource);
			if (WebOperationContext.Current != null)
			{
				WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Created;
				WebOperationContext.Current.OutgoingResponse.Location = "/resource/" + resource.Id;
			}
		}
		/// <summary>
		/// Save changes to resource.
		/// </summary>
		/// <param name="id">Unique identifier for the resource.</param>
		/// <param name="resource">Updated resource.</param>
		/// <exception cref="System.Exception">Returns 304 NotModified if an exception occurs
		/// while saving the resource.</exception>
		public void Update(string id, Resource resource)
		{
			var previous = GetResource(id);
			if (previous != null) Delete(id);
			try
			{
				_ctx.Save<Resource>(resource);
			}
			catch (Exception ex)
			{
				if (previous != null) _ctx.Save<Resource>(previous);
				if (WebOperationContext.Current != null)
				{
					WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NotModified;
				}
				Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(ex);
			}
		}
		/// <summary>
		/// Delete a resource.
		/// </summary>
		/// <param name="id">Unique identifier for the resource.</param>
		/// <returns>Reference to the actual entity that was removed.
		/// <para>404 NotFound if the specified resource does not exist.
		/// </para></returns>
		public Resource Delete(string id)
		{
			var resource = GetResource(id);
			if (resource == null)
			{
				if (WebOperationContext.Current != null)
				{
					WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
				}
				return null;
			}
			_ctx.Delete<Resource>(id);
			return resource;
		}
		/// <summary>
		/// Get all resources.
		/// </summary>
		/// <returns>A collection of resources.</returns>
		public IList<Resource> GetAllResources()
		{
			return _ctx.FindAll<Resource>();
		}
		/// <summary>
		/// Get a specific resource.
		/// </summary>
		/// <param name="id">Unique identifier for the resource.</param>
		/// <returns>A resource with the specified unique identifier.
		/// <para>Return null if no such resource exists.
		/// </para></returns>
		public Resource GetResource(string id)
		{
			var result = _ctx.FindAll<Resource>(new Resource { Id = id });
			if (result.Any())
				return result[0];

			return null;
		}
		/// <summary>
		/// Get all links for a specific resource.
		/// </summary>
		/// <param name="resourceId">Unique identifier for the resource.</param>
		/// <returns>A collection of links for the specified resource.
		/// <para>Return null if no such resource exists.
		/// </para></returns>
		public IList<ResourceLink> GetResourceLinks(string resourceId)
		{
			var result = GetResource(resourceId);
			if (result != null)
				return result.Links;

			return null;
		}
		/// <summary>
		/// Get all metadata for a specific resource.
		/// </summary>
		/// <param name="resourceId">Unique identifier for the resource.</param>
		/// <returns>A collection of metadata for the specified resource.
		/// <para>Return null if no such resource exists.
		/// </para></returns>
		public IList<ResourceMeta> GetResourceMeta(string resourceId)
		{
			var result = GetResource(resourceId);
			if (result != null)
				return result.Metadata;

			return null;
		}
	}
}
