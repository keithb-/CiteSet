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
	/// Represents information describing the characteristics of a resource.
	/// </summary>
	[Serializable]
	[DataContract]
	public class ResourceMeta
		: EntityBase
	{
		/// <summary>
		/// Creates a new instance without any initialization.
		/// </summary>
		/// <remarks><c>ResourceMeta</c> class represents a key-value pair.</remarks>
		public ResourceMeta()
			: base()
		{
		}
		/// <summary>
		/// Create and initialize a new instance.
		/// </summary>
		/// <remarks>When the new <c>EntityBase</c> is created, the Id, CreateUser and CreateDate
		/// values are initialized. Id is initialized to a new key value using the <see cref="CiteSet.Model.KeyGenerator.CreateKey"/> method.</remarks>
		/// <param name="user">Identity of the active user.</param>
		public ResourceMeta(IIdentity user)
			: base(user)
		{
		}
		/// <summary>
		/// Name of the characteristic.
		/// </summary>
		[DataMember]
		public string Name
		{
			get;
			set;
		}
		/// <summary>
		/// Information describing the characteristic.
		/// </summary>
		[DataMember]
		public string Content
		{
			get;
			set;
		}
	}
}
