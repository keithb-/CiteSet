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
using System.ServiceModel;
using System.ServiceModel.Web;
using CiteSet.Model;

namespace CiteSet.Web.Contract
{
	/// <summary>
	/// RESTful service for manipulating resource entities.
	/// </summary>
	/// <remarks>All service methods accept and return JSON entities.</remarks>
	[ServiceContract]
	public interface IResourceService
	{
		/// <summary>
		/// Root of RESTful service.
		/// </summary>
		/// <example>
		/// <code>GET http://localhost/</code>
		/// </example>
		/// <returns>A collection of resources.</returns>
		[OperationContract]
		[WebGet(UriTemplate = "/", ResponseFormat = WebMessageFormat.Json)]
		IList<Resource> GetAllResources();
		/// <summary>
		/// Request a specific resource.
		/// </summary>
		/// <example>
		/// <code>GET http://localhost/{id}</code>
		/// </example>
		/// <param name="id">Unique identifier for the resource.</param>
		/// <returns>A resource with the specified unique identifier.</returns>
		[OperationContract]
		[WebGet(UriTemplate = "/{id}", ResponseFormat = WebMessageFormat.Json)]
		Resource GetResource(string id);
		/// <summary>
		/// Request all links.
		/// </summary>
		/// <example>
		/// <code>GET http://localhost/{resourceId}/link</code>
		/// </example>
		/// <param name="resourceId">Unique identifier for the resource.</param>
		/// <returns>A collection of links for the specified resource.</returns>
		[OperationContract]
		[WebGet(UriTemplate = "/{resourceId}/link", ResponseFormat = WebMessageFormat.Json)]
		IList<ResourceLink> GetResourceLinks(string resourceId);
		/// <summary>
		/// Request all metadata.
		/// </summary>
		/// <example>
		/// <code>GET http://localhost/{resourceId}/meta</code>
		/// </example>
		/// <param name="resourceId">Unique identifier for the resource.</param>
		/// <returns>A collection of metadata for the specified resource.</returns>
		[OperationContract]
		[WebGet(UriTemplate = "/{resourceId}/meta", ResponseFormat = WebMessageFormat.Json)]
		IList<ResourceMeta> GetResourceMeta(string resourceId);
		/// <summary>
		/// Create a new resource.
		/// </summary>
		/// <example>
		/// <code>POST http://localhost/</code>
		/// </example>
		/// <param name="request">New index request with details about resource.</param>
		[OperationContract]
		[WebInvoke(Method = "POST", UriTemplate = "/", RequestFormat = WebMessageFormat.Json)]
		void Index(IndexRequest request);
		/// <summary>
		/// Save changes to a resource.
		/// </summary>
		/// <example>
		/// <code>PUT http://localhost/{id}</code>
		/// </example>
		/// <param name="id">Unique identifier for the resource.</param>
		/// <param name="resource">resource instance.</param>
		[OperationContract]
		[WebInvoke(Method = "PUT", UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json)]
		void Update(string id, Resource resource);
		/// <summary>
		/// Delete a resource.
		/// </summary>
		/// <example>
		/// <code>DELETE http://localhost/{id}</code>
		/// </example>
		/// <param name="id">Unique identifier for the resource.</param>
		/// <returns>Reference to the actual entity that was removed.</returns>
		[OperationContract]
		[WebInvoke(Method = "DELETE", UriTemplate = "/{id}", ResponseFormat = WebMessageFormat.Json)]
		Resource Delete(string id);
	}
	/// <summary>
	/// Represents the information needed to index a specific resource.
	/// </summary>
	/// <remarks>At this time, only the URL to the resource is necessary, but 
	/// future versions may add indexing instructions to this entity.</remarks>
	public class IndexRequest
	{
		/// <summary>
		/// Location of the resournce to be indexed.
		/// </summary>
		public string Path { get; set; }
	}
}
