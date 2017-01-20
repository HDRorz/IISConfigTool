using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using log4net;

namespace IISConfigTool.Manager
{
	public static class Loger
	{
		private static ILog loger = LogManager.GetLogger("LOG");

		static Loger()
		{
			log4net.Config.XmlConfigurator.Configure(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config")));
		}

		public static void Info(string msg)
		{
			loger.Info(msg);
		}

		public static void Debug(string msg)
		{
			loger.Debug(msg);
		}

		public static void Error(string msg, Exception ex)
		{
			loger.Error(msg, ex);
		}
	}
}
