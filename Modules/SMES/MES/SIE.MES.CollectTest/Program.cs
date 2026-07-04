using SIE.Configuration;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.CollectTest
{
    static class Program
    {
        static void Main(string[] args)
        {
            Startup();

            var modes = new List<IRunMode>();
            modes.Add(new IntervalRunMode());
            modes.Add(new TaskPoolRunMode());
            ShowHelp(modes);

            string read;
            while ((read = Console.ReadLine()) != "exit")
            {
                if (read == "help" || read == "?" || read == "？")
                {
                    ShowHelp(modes);
                    continue;
                }
                var @params = read.Split(',', '，');
                if (int.TryParse(@params[0], out int index) && index >= 0 && index < modes.Count)
                {
                    modes[index].Run(@params.Skip(1).ToArray());
                    ShowHelp(modes);
                }
            }
        }

        static void Startup()
        {

            ConfigManager.Create().UserJsonConfig("appsettings.json");
            //读取Debugging环境
            RT.Provider.IsDebuggingEnabled = RT.Config.Get(ConfigKeys.IsDebuggingEnabled, false);
            //注册程序未处理的异常处理方法
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            //
            new DomainApp().Startup();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {

        }

        static void ShowHelp(List<IRunMode> modes)
        {
            Console.WriteLine("输入编号执行，如：0,参数1，参数2");
            for (var i = 0; i < modes.Count; i++)
                Console.WriteLine(string.Format("{0}:执行[{1}]->{2}", i, modes[i].Name, modes[i].Help));
            Console.WriteLine("help/?:显示帮助");
            Console.WriteLine("exit:退出");
            Console.WriteLine();
        }
    }
}
