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
	/// Represents a relation between two resources.
	/// </summary>
	[Serializable]
	[DataContract]
	public class ResourceLink
		: EntityBase
	{
		/// <summary>
		/// Creates a new instance without any initialization.
		/// </summary>
		public ResourceLink()
			: base()
		{
		}
		/// <summary>
		/// Create and initialize a new instance.
		/// </summary>
		/// <remarks>When the new <c>EntityBase</c> is created, the Id, CreateUser and CreateDate
		/// values are initialized. Id is initialized to a new key value using the <see cref="CiteSet.Model.KeyGenerator.CreateKey"/> method.</remarks>
		/// <param name="user">Identity of the active user.</param>
		public ResourceLink(IIdentity user)
			: base(user)
		{
		}
		/// <summary>
		/// Specifies the URL of the resource.
		/// </summary>
		[DataMember]
		public string Href
		{
			get;
			set;
		}
		/// <summary>
		/// Specifies the MIME type of the linked resource.
		/// </summary>
		[DataMember]
		public string Type
		{
			get;
			set;
		}
		/// <summary>
		/// Specifies the relationship between the current resource and the linked resource.
		/// </summary>
		[DataMember]
		public string Rel
		{
			get;
			set;
		}
		/// <summary>
		/// Title of the link.
		/// </summary>
		[DataMember]
		public string Title
		{
			get;
			set;
		}
	}
}
