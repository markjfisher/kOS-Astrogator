using System;
using UnityEngine;

namespace kOS.AddOns.kOSAstrogator
{
	/// Tools to help with debugging. Copied from Astrogator.DebugTools, but with our own name.
	public static class DebugTools {

		private static readonly object debugMutex = new object();

		/// <summary>
		/// Add a formattable string to the debug output.
		/// Automatically prepends the mod name and a timestamp.
		/// </summary>
		/// <param name="format">String.Format format string</param>
		/// <param name="args">Parameters for the format string, if any</param>
		[System.Diagnostics.Conditional("DEBUG")]
		public static void DbgFmt(string format, params object[] args)
		{
			string formattedMessage = string.Format(format, args);
			lock (debugMutex) {
				MonoBehaviour.print($"[{KOSAstrogatorAddon.Name} {Time.realtimeSinceStartup:000.000}] {formattedMessage}");
			}
		}

		/// <summary>
		/// Log a debug message about an Exception
		/// </summary>
		/// <param name="description">Explanation of the context in which the exception was raised</param>
		/// <param name="ex">The exception to log</param>
		[System.Diagnostics.Conditional("DEBUG")]
		public static void DbgExc(string description, Exception ex) {
			DbgFmt(
				"{0}: {1}\n{2}",
				description,
				ex.Message,
				ex.StackTrace
			);
		}

	}
}
