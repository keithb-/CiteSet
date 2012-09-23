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
using System.Configuration;

namespace CiteSet.Web
{
	/// <summary>
	/// Convenience class for reading configuration settings from the app.config file.
	/// </summary>
	public class Configuration
		: ConfigurationSection
	{
		/// <summary>
		/// Connection string as defined in the app.config file.
		/// </summary>
		/// <remarks>
		/// The connection string is defined in the standard .NET &lt;connectionStrings&gt;
		/// element within the app.config file however the format is specific to CiteSet.
		/// <para>The connection string value is in the form of a <see cref="System.Uri"/> 
		/// with a custom scheme: <c>db4o://</c>. The <see cref="System.Uri"/> format 
		/// simply makes it easier to parse values from the string for use in configuring
		/// the CiteSet.Database.DatabaseServer class.
		/// </para>
		/// </remarks>
		[ConfigurationProperty("connectionString")]
		public ConnectionStringElement ConnectionString
		{
			get
			{
				return (ConnectionStringElement)this["connectionString"];
			}
			set
			{
				this["connectionString"] = value;
			}
		}
	}
	/// <summary>
	/// Convenience class for reading configuration settings from the app.config file.
	/// </summary>
	public class ConnectionStringElement
		: ConfigurationElement
	{
		/// <summary>
		/// Connection string as defined in the app.config file.
		/// </summary>
		/// <remarks>
		/// The <c>Name</c> attribute is a cross-reference value back to one of the 
		/// connection strings defined in the &lt;connectionStrings&gt; element in the
		/// app.config file. 
		/// <para>This mechanism makes it easy to configure several connection
		/// strings and relate them directly to the CiteSet.Database.DatabaseServer 
		/// instance.
		/// </para>
		/// </remarks>
		[ConfigurationProperty("name", DefaultValue = "Default")]
		public string Name
		{
			get
			{
				return (string)this["name"];
			}
			set
			{
				this["name"] = value;
			}
		}
	}
}
