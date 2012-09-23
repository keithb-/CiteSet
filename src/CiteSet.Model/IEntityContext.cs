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
using System.Collections.Generic;

namespace CiteSet.Model
{
	/// <summary>
	/// A simple context interface for manipulating entities.
	/// </summary>
	/// <remarks>Represents the minimum set of capabilities that any entity
	/// service will provide.</remarks>
	public interface IEntityContext
	{
		/// <summary>
		/// Identity of the active user.
		/// </summary>
		IIdentity Identity { get; set; }
		/// <summary>
		/// Open a connection to the entity service.
		/// </summary>
		void Open();
		/// <summary>
		/// Close an open connection to the entity service.
		/// </summary>
		void Close();
		/// <summary>
		/// Store a entity using the entity service.
		/// </summary>
		/// <typeparam name="T">Represents an entity that inherits from the <c>EntityBase</c> class.</typeparam>
		/// <param name="input">Entity to be stored by the entity service.</param>
		/// <returns>Reference to the actual entity stored by the entity service and which may include
		/// properties that have been supplied by the entity service (e.g. unique identifier).
		/// Therefore the reference may differ from the <c>input</c> parameter.</returns>
		T Save<T>(T input) where T : EntityBase;
		/// <summary>
		/// Delete an entity using a unique identifier.
		/// </summary>
		/// <typeparam name="T">Represents an entity that inherits from the <c>EntityBase</c> class.</typeparam>
		/// <param name="id">Unique identifier for the entity.</param>
		/// <returns>Reference to the actual entity that was removed from storage.</returns>
		T Delete<T>(string id) where T : EntityBase;
		/// <summary>
		/// Delete one or more entities using an entity prototype.
		/// </summary>
		/// <typeparam name="T">Represents an entity that inherits from the <c>EntityBase</c> class.</typeparam>
		/// <param name="proto">Entity prototype. Property values will be used as
		/// criteria for locating entities.</param>
		/// <returns>A collection of references to the actual entities that were removed
		/// from storage, if any.</returns>
		IList<T> DeleteAll<T>(T proto) where T : EntityBase;
		/// <summary>
		/// Find an entity using a unique identifier.
		/// </summary>
		/// <typeparam name="T">Represents an entity that inherits from the <c>EntityBase</c> class.</typeparam>
		/// <param name="id">Unique identifier for the entity.</param>
		/// <returns>Reference to the actual entity with the specified unique identifier.</returns>
		T Find<T>(string id) where T : EntityBase;
		/// <summary>
		/// Find all entities for a specific class type.
		/// </summary>
		/// <typeparam name="T">Represents an entity that inherits from the <c>EntityBase</c> class.</typeparam>
		/// <returns>A collection of references to the actual entities of the specified class type.</returns>
		IList<T> FindAll<T>() where T : EntityBase;
		/// <summary>
		/// Find all entities using an entity prototype.
		/// </summary>
		/// <typeparam name="T">Represents an entity that inherits from the <c>EntityBase</c> class.</typeparam>
		/// <param name="proto">Entity prototype. Property values will be used as
		/// criteria for locating entities.</param>
		/// <returns>A collection of references to the actual entities that match 
		/// the specified prototype, if any.</returns>
		IList<T> FindAll<T>(T proto) where T : EntityBase;
	}
}
