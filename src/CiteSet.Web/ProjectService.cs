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
using System.Security.Principal;
using System.Net;
using CiteSet.Model;
using CiteSet.Web.Contract;

namespace CiteSet.Web
{
	/// <summary>
	/// RESTful service implementation for manipulating project entities.
	/// </summary>
	[ServiceBehavior(UseSynchronizationContext = false)]
	public class ProjectService
		: IProjectService
	{
		private readonly IEntityContext _ctx;
		private readonly IIdentity _user;
		/// <summary>
		/// Create a new instance.
		/// </summary>
		public ProjectService() {}
		/// <summary>
		/// Create and initialize a new instance.
		/// </summary>
		/// <param name="ctx">Entity context for the service.</param>
		/// <param name="user">Identity of the active user.</param>
		public ProjectService(IEntityContext ctx, IIdentity user)
		{
			_ctx = ctx;
			_user = user;
		}
		/// <summary>
		/// Get all projects.
		/// </summary>
		/// <returns>A collection of projects.</returns>
		public IList<Project> GetAllProjects()
		{
			return _ctx.FindAll<Project>();
		}
		/// <summary>
		/// Get a specific project.
		/// </summary>
		/// <param name="id">Unique identifier for the project.</param>
		/// <returns>A project with the specified unique identifier.
		/// <para>Return null if no such project exists.
		/// </para></returns>
		public Model.Project GetProject(string id)
		{
			var result = _ctx.FindAll<Project>(new Project { Id = id });
			if (result.Any())
				return result[0];

			return null;
		}
		/// <summary>
		/// Get all journal entries for a project.
		/// </summary>
		/// <param name="projectId">Unique identifier for the project.</param>
		/// <returns>A collection of journal entries.
		/// Returns null if no such project exists.</returns>
		public IList<JournalEntry> GetJournalEntries(string projectId)
		{
			var result = GetProject(projectId);
			if (result != null)
				return result.JournalEntries;

			return null;
		}
		/// <summary>
		/// Get all unique identifiers for resources that are related to the project.
		/// </summary>
		/// <param name="projectId">Unique identifier for the project.</param>
		/// <returns>A collection of unique identifiers.
		/// Returns null if no such project exists.</returns>
		public IList<Guid> GetProjectResources(string projectId)
		{
			var result = GetProject(projectId);
			if (result != null)
				return result.Resources;

			return null;
		}
		/// <summary>
		/// Create a new project.
		/// </summary>
		/// <remarks>
		/// Request is redirected to the location of the newly added project.
		/// </remarks>
		/// <param name="newProject">New project.</param>
		/// <returns>201 Created</returns>
		public void Add(Project newProject)
		{
			// Clone the object sent from the client.
			var project = new Project(_user);
			project.Summary = newProject.Summary;
			project.Description = newProject.Description;
			project.JournalEntries = newProject.JournalEntries;
			project.Resources = newProject.Resources;
			project = _ctx.Save<Project>(project);
			if (WebOperationContext.Current != null)
			{
				WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Created;
				WebOperationContext.Current.OutgoingResponse.Location = "/project/" + project.Id;
			}
		}
		/// <summary>
		/// Save changes to a project.
		/// </summary>
		/// <param name="id">Unique identifier for the project.</param>
		/// <param name="project">Updated project.</param>
		/// <exception cref="System.Exception">Returns 304 NotModified if an exception occurs
		/// while saving the project.</exception>
		public void Update(string id, Project project)
		{
			var previous = GetProject(id);
			if (previous != null) Delete(id);
			try
			{
				_ctx.Save<Project>(project);
			}
			catch (Exception ex)
			{
				if (previous != null) _ctx.Save<Project>(previous);
				if (WebOperationContext.Current != null)
				{
					WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NotModified;
				}
				Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(ex);
			}
		}
		/// <summary>
		/// Delete a project.
		/// </summary>
		/// <param name="id">Unique identifier for the project.</param>
		/// <returns>Reference to the actual entity that was removed.
		/// <para>404 NotFound if the specified project does not exist.
		/// </para></returns>
		public Project Delete(string id)
		{
			var project = GetProject(id);
			if (project == null)
			{
				if (WebOperationContext.Current != null)
				{
					WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
				}
				return null;
			}
			_ctx.Delete<Project>(id);
			return project;
		}
	}
}
