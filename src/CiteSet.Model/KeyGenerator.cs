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

namespace CiteSet.Model
{
	/// <summary>
	/// KeyGenerator is used to create a new GUID for entities that require a unique identifier.
	/// </summary>
	public class KeyGenerator
	{
		/// <summary>
		/// Creates a GUID that can be used to uniquely identify an entity.
		/// </summary>
		/// <returns></returns>
		public static string CreateKey()
		{
			return Guid.NewGuid().ToString();
		}
	}
}
