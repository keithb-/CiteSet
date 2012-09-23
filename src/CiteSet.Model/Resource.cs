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
using System.Security.Principal;
using System.Runtime.Serialization;

namespace CiteSet.Model
{
	/// <summary>
	/// Represents a file or references that is associated with a specific project.
	/// </summary>
	[Serializable]
	[DataContract]
	public class Resource
		: EntityBase
	{
		/// <summary>
		/// Creates a new instance without any initialization.
		/// </summary>
		public Resource()
			: base()
		{
		}
		/// <summary>
		/// Create and initialize a new instance.
		/// </summary>
		/// <remarks>When the new <c>EntityBase</c> is created, the Id, CreateUser and CreateDate
		/// values are initialized. Id is initialized to a new key value using the <see cref="CiteSet.Model.KeyGenerator.CreateKey"/> method.</remarks>
		/// <param name="user">Identity of the active user.</param>
		public Resource(IIdentity user)
			: base(user)
		{
		}
		/// <summary>
		/// Gets the host component of this instance.
		/// </summary>
		[DataMember]
		public string Host
		{
			get;
			set;
		}
		/// <summary>
		/// Gets the path component of this instance.
		/// </summary>
		[DataMember]
		public string Path
		{
			get;
			set;
		}
		/// <summary>
		/// Gets the query component of this instance.
		/// </summary>
		[DataMember]
		public string Query
		{
			get;
			set;
		}
		/// <summary>
		/// Title of resource.
		/// </summary>
		[DataMember]
		public string Title
		{
			get;
			set;
		}
		/// <summary>
		/// A collection of relations to other resources.
		/// </summary>
		public IList<ResourceLink> Links
		{
			get;
			set;
		}
		/// <summary>
		/// A collection of charateristics that describe the resource.
		/// </summary>
		public IList<ResourceMeta> Metadata
		{
			get;
			set;
		}		
	}
}
