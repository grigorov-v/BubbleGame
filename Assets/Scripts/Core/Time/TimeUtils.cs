using UnityEngine;

using System;

namespace Core.Time {
	public static class TimeUtils {
		public static DateTime GetOriginTime() {
			return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);
		}

		public static int GetDateTimeDay(DateTime dateTime) {
			var dday = (dateTime - GetOriginTime()).TotalDays;
			return Mathf.FloorToInt((float)dday);
		}

		public static DateTime ConvertFromUnixTimestamp(long timestamp) {
			var origin = GetOriginTime();
			return origin.AddSeconds(timestamp);
		}
		
		public static long ConvertToUnixTimestamp(DateTime date) {
			var origin = GetOriginTime();
			return (long)Math.Floor((date - origin).TotalSeconds);
		}
		
		public static long ConvertToUnixMsTimestamp(DateTime date) {
			var origin = GetOriginTime();
			return (long)Math.Floor((date - origin).TotalMilliseconds);
		}
	}
}