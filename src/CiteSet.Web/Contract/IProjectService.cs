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
	/// RESTful service for manipulating project entities.
	/// </summary>
	/// <remarks>All service methods accept and return JSON entities.</remarks>
	[ServiceContract]
	public interface IProjectService
	{
		/// <summary>
		/// Root of RESTful service.
		/// </summary>
		/// <example>
		/// <code>GET http://localhost/</code>
		/// </example>
		/// <returns>A collection of projects.</returns>
		[OperationContract]
		[WebGet(UriTemplate = "/", ResponseFormat = WebMessageFormat.Json)]
		IList<Project> GetAllProjects();
		/// <summary>
		/// Request a specific project.
		/// </summary>
		/// <example>
		/// <code>GET http://localhost/{id}</code>
		/// </example>
		/// <param name="id">Unique identifier for the project.</param>
		/// <returns>A project with the specified unique identifier.</returns>
		[OperationContract]
		[WebGet(UriTemplate = "/{id}", ResponseFormat = WebMessageFormat.Json)]
		Project GetProject(string id);
		/// <summary>
		/// Request all journals.
		/// </summary>
		/// <example>
		/// <code>GET http://localhost/{projectId}/journal</code>
		/// </example>
		/// <param name="projectId">Unique identifier for the project.</param>
		/// <returns>A collection of journal entries for the specified project.</returns>
		[OperationContract]
		[WebGet(UriTemplate = "/{projectId}/journal", ResponseFormat = WebMessageFormat.Json)]
		IList<JournalEntry> GetJournalEntries(string projectId);
		/// <summary>
		/// Request all resources.
		/// </summary>
		/// <example>
		/// <code>GET http://localhost/{projectId}/resource</code>
		/// </example>
		/// <param name="projectId">Unique identifier for the project.</param>
		/// <returns>A collection of resources for the specified project.</returns>
		[OperationContract]
		[WebGet(UriTemplate = "/{projectId}/resource", ResponseFormat = WebMessageFormat.Json)]
		IList<Guid> GetProjectResources(string projectId);
		/// <summary>
		/// Create a new project.
		/// </summary>
		/// <example>
		/// <code>POST http://localhost/</code>
		/// </example>
		/// <param name="newProject">New project instance.</param>
		[OperationContract]
		[WebInvoke(Method = "POST", UriTemplate = "/", RequestFormat = WebMessageFormat.Json)]
		void Add(Project newProject);
		/// <summary>
		/// Save changes to a project.
		/// </summary>
		/// <example>
		/// <code>PUT http://localhost/{id}</code>
		/// </example>
		/// <param name="id">Unique identifier for the project.</param>
		/// <param name="project">Project instance.</param>
		[OperationContract]
		[WebInvoke(Method = "PUT", UriTemplate = "/{id}", RequestFormat = WebMessageFormat.Json)]
		void Update(string id, Project project);
		/// <summary>
		/// Delete a project.
		/// </summary>
		/// <example>
		/// <code>DELETE http://localhost/{id}</code>
		/// </example>
		/// <param name="id">Unique identifier for the project.</param>
		/// <returns>Reference to the actual entity that was removed.</returns>
		[OperationContract]
		[WebInvoke(Method = "DELETE", UriTemplate = "/{id}", ResponseFormat = WebMessageFormat.Json)]
		Project Delete(string id);
	}
}
