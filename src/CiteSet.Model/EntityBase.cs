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
using System.Runtime.Serialization;

namespace CiteSet.Model
{
	/// <summary>
	/// Collection of properties and methods that are necessary for all entities.
	/// </summary>
	[Serializable]
	[DataContract]
	public abstract class EntityBase
	{
		/// <summary>
		/// Creates a new instance without any initialization.
		/// </summary>
		protected EntityBase() 
		{
		}
		/// <summary>
		/// Create and initialize a new instance.
		/// </summary>
		/// <remarks>When the new <c>EntityBase</c> is created, the Id, CreateUser and CreateDate
		/// values are initialized. Id is initialized to a new key value using the <see cref="CiteSet.Model.KeyGenerator.CreateKey"/> method.</remarks>
		/// <param name="user">Identity of the active user.</param>
		protected EntityBase(IIdentity user)
		{
			Id = KeyGenerator.CreateKey();
			AuditNew(user);
		}
		/// <summary>
		/// Unique identifier for this entity.
		/// </summary>
		[DataMember]
		public string Id
		{
			get;
			set;
		}
		/// <summary>
		/// Name of user that created this entity.
		/// </summary>
		[DataMember]
		public string CreateUser
		{
			get;
			set;
		}
		/// <summary>
		/// Date that this entity was created.
		/// </summary>
		[DataMember]
		public DateTime? CreateDate
		{
			get;
			set;
		}
		/// <summary>
		/// Name of user that last modified this entity.
		/// </summary>
		[DataMember]
		public string UpdateUser
		{
			get;
			set;
		}
		/// <summary>
		/// Date that this entity was last modified.
		/// </summary>
		[DataMember]
		public DateTime? UpdateDate
		{
			get;
			set;
		}
		/// <summary>
		/// Convenience method to set the CreateUser and CreateDate properties in one call.
		/// </summary>
		/// <param name="user">Identity of the active user.</param>
		public void AuditNew(IIdentity user)
		{
			CreateDate = DateTime.Now;
			CreateUser = user.Name;
			UpdateDate = CreateDate;
			UpdateUser = CreateUser;
		}
		/// <summary>
		/// Convenience method to set the UpdateUser and UpdateDate properties in one call.
		/// </summary>
		/// <param name="user">Identity of the active user.</param>
		public void AuditUpdate(IIdentity user)
		{
			UpdateDate = DateTime.Now;
			UpdateUser = CreateUser;
		}
	}
}
