using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammatus.Helpers
{
	public sealed class NameKeyGenerator
	{
		private NameKeyGenerator ()
	    {

	    }

		public static string BuildFullKey<T>(object userKey = null)
		{
			if (userKey == null)
				return typeof(T).FullName;
			return typeof (T).FullName + userKey;
		}
	}
}
