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
using System.Diagnostics.Contracts;
using System.Security.Principal;
using System.Xml.XPath;
using CiteSet.Model;

namespace CiteSet.Web
{
	/// <summary>
	/// HTML page indexing component.
	/// </summary>
	/// <typeparam name="TResult">Represents the entities created by the indexing process.</typeparam>
	public interface IBridge<out TResult>
	{
		/// <summary>
		/// Index an HTML page and create a representative entity.
		/// </summary>
		/// <param name="page">Address of the HTML page.</param>
		/// <param name="input">Portion of the HTML page to index.</param>
		/// <returns>An entity that represents the HTML page.</returns>
		TResult CreateEntity(Uri page, XPathNavigator input);
	}
	/// <summary>
	/// Index of information from the &lt;head&gt; section of an HTML page.
	/// </summary>
	public class ResourceBridge
		: IBridge<Resource>
	{
		private readonly IIdentity _user;
		/// <summary>
		/// Creates and initializes an instance.
		/// </summary>
		/// <param name="user">Identity of the active user.</param>
		public ResourceBridge(IIdentity user)
		{
			_user = user;
		}
		/// <summary>
		/// Build an entity from the specified HTML page.
		/// </summary>
		/// <param name="page">Address of the HTML page.</param>
		/// <param name="input">Portion of the HTML page to index.</param>
		/// <returns>An entity that represents the HTML page.</returns>
		public Resource CreateEntity(Uri page, XPathNavigator input)
		{
			var result = new Resource(_user);
			result.Host = page.Host;
			result.Path = page.LocalPath;
			result.Query = page.Query;
			XPathNavigator seek;
			if ((seek = input.SelectSingleNode("title")) != null)
				result.Title = seek.Value;

			var iterator = input.SelectChildren("meta", input.NamespaceURI);
			while(iterator.MoveNext())
			{
				if (iterator.Current == null)
					continue;

				var meta = new ResourceMeta(_user);
				meta.AuditNew(_user);
				if (iterator.Current.Select("@content").Count > 0)
					meta.Content = iterator.Current.GetAttribute("content", iterator.Current.NamespaceURI);
				if (iterator.Current.Select("@name").Count > 0)
					meta.Name = iterator.Current.GetAttribute("name", iterator.Current.NamespaceURI);
				if (result.Metadata == null)
					result.Metadata = new List<ResourceMeta>();
				result.Metadata.Add(meta);
			}
			iterator = input.SelectChildren("link", input.NamespaceURI);
			while (iterator.MoveNext())
			{
				if (iterator.Current == null)
					continue;

				var link = new ResourceLink(_user);
				link.AuditNew(_user);
				if (iterator.Current.Select("@href").Count > 0)
					link.Href = iterator.Current.GetAttribute("href", iterator.Current.NamespaceURI);
				if (iterator.Current.Select("@type").Count > 0)
					link.Type = iterator.Current.GetAttribute("type", iterator.Current.NamespaceURI);
				if (iterator.Current.Select("@rel").Count > 0)
					link.Rel = iterator.Current.GetAttribute("rel", iterator.Current.NamespaceURI);
				if (result.Links == null)
					result.Links = new List<ResourceLink>();
				result.Links.Add(link);
			}
			return result;
		}
	}
}
