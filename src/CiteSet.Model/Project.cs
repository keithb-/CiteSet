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
	/// Represents a collection of journal entries, files,
	/// references, and metadata related to a planned activity.
	/// </summary>
	[Serializable]
	[DataContract]
	public class Project
		: EntityBase
	{
		//private bool _isInitized = false;
		/// <summary>
		/// Creates a new instance without any initialization.
		/// </summary>
		public Project()
		{
		}
		/// <summary>
		/// Create and initialize a new instance.
		/// </summary>
		/// <remarks>When the new <c>EntityBase</c> is created, the Id, CreateUser and CreateDate
		/// values are initialized. Id is initialized to a new key value using the <see cref="CiteSet.Model.KeyGenerator.CreateKey"/> method.</remarks>
		/// <param name="user">Identity of the active user.</param>
		public Project(IIdentity user)
			: base(user)
		{
		}
		/// <summary>
		/// A detailed overview of the project purpose, scope, status, etc.
		/// </summary>
		[DataMember]
		public string Summary
		{
			get;
			set;
		}
		/// <summary>
		/// A brief statement about the purpose of the project.
		/// </summary>
		[DataMember]
		public string Description
		{
			get;
			set;
		}
		/// <summary>
		/// Collection of activity related to the project.
		/// </summary>
		[DataMember]
		public IList<JournalEntry> JournalEntries
		{
			get;
			set;
		}
		/// <summary>
		/// Collection of unique identifiers for resources that are related to the project.
		/// </summary>
		[DataMember]
		public IList<Guid> Resources
		{
			get;
			set;
		}
	}
}
