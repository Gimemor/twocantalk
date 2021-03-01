using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMSWeb.BusinessServices.Helpers
{
	public static class SqlCommandHelper
	{
		public static string ToSqlValue(this string raw) 
		{
			return raw == null
				? "''" : 
				$"'{raw.Replace("\'", "\\'")}'"; 
		}
		public static string ToSqlValue(this bool raw) { return (raw ? 1 : 0).ToString(); }
		public static string ToSqlValue(this uint? raw) { return raw == null ? "null" : raw.ToString(); }
		public static string ToSqlValue(this DateTime? raw) { return raw == null ? "null" : raw.ToString(); }
	}
}
