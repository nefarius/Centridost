using System;
using System.Diagnostics;
using System.IO;

namespace Centridost
{
	public static class Log
	{
		private static FileStream logFile;
		private static TextWriterTraceListener logFileListener;
		private static TextWriterTraceListener consoleListener;

		static Log()
		{
            logFile = new FileStream("Centridost.log", FileMode.OpenOrCreate);
			logFileListener = new TextWriterTraceListener (logFile);
			consoleListener = new TextWriterTraceListener (Console.Out);
			Trace.Listeners.Add (logFileListener);
			Trace.Listeners.Add (consoleListener);
		}

		public static void WriteLine(string line)
		{
			DateTime now = DateTime.Now;
			string date = now.ToShortDateString();
			string time = now.ToLongTimeString();
			
			Trace.WriteLine(string.Format ("{0}, {1} - {2}", date, time, line));
			Trace.Flush();
		}

		public static void WriteLine (string format, params object[] list)
		{
			WriteLine(string.Format (format, list));
		}

        public static void WriteLine(object obj)
        {
            WriteLine(obj.ToString());
        }
	}
}

