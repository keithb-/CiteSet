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
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using HtmlAgilityPack;

namespace CiteSet.Web
{
	/// <summary>
	/// Simple abstraction for separating the HTML page indexing component
	/// from other portions of the calling class.
	/// </summary>
	/// <remarks>The goal of the adapter+bridge abstraction is to allow indexing to 
	/// evolve over time. The current version only indexes information from the 
	/// &lt;head&gt; section, but other sections (standard or custom) might be developed
	/// in the future. When that happens, the adapter will only require a new bridge
	/// component to perform the indexing process.
	/// <para>Additionally, the adapter contains references to the <c>HtmlAgilityPack</c>.
	/// In the future, the adapter could be abstracted into an interface that worked with
	/// other types of resources e.g. XML files, source code files, etc.</para>
	/// </remarks>
	/// <typeparam name="TResult">Represents the entities created by the indexing process.</typeparam>
	public class PageAdapter<TResult>
	{
		private readonly IBridge<TResult> _bridge;
		/// <summary>
		/// Creates and initializes an instance.
		/// </summary>
		/// <param name="bridge">HTML page indexing component.</param>
		public PageAdapter(IBridge<TResult> bridge)
		{
			_bridge = bridge;
		}
		/// <summary>
		/// Invokes the bridge method to return the expected result.
		/// </summary>
		/// <param name="page">Address of the HTML page.</param>
		/// <returns>An entity that represents the HTML page.</returns>
		public TResult Get(Uri page)
		{
			TResult result;
			var request = HttpWebRequest.Create(page);
			using (var response = request.GetResponse().GetResponseStream())
			{
				var document = new HtmlDocument();
				document.Load(response);
				result = _bridge.CreateEntity(page, document.CreateNavigator().SelectSingleNode("//head"));
				response.Close();
			}
			return result;
		}
	}
}
