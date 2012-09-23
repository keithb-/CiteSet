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
using System.Text;

namespace CiteSet.Database
{
	/// <summary>
	/// A simple database service interface. 
	/// </summary>
	/// <remarks>The <c>IDatabaseServer</c> represents the bare minimum for interacting with a 
	/// stand-alone database service. 
	/// </remarks>
	public interface IDatabaseServer
	{
		/// <summary>
		/// Open a connection with the database server.
		/// </summary>
		void Open();
		/// <summary>
		/// Close an open connection to the database server.
		/// </summary>
		void Close();
	}
}
