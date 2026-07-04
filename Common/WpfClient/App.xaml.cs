using SIE;
using SIE.Configuration;
using SIE.Wpf;
using SIE.Wpf.Modules;
using SIE.Wpf.Windows;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace WpfClient
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application, IClientAppRuntime
    {
        public App()
        {
            try
            {
                var _ = SIE.Wpf.Properties.Settings.Default.IsAutoLogin;
            }
            catch (ConfigurationErrorsException exc)
            {
                DeleteConfigFile(exc);
                MessageBox.Show("用户配置文件损坏，已自动删除损坏文件，需要重新启动", "异常警告");
                Shutdown();
            }

            int workerThreads, completionPortThreads;
            ThreadPool.GetMinThreads(out workerThreads, out completionPortThreads);


            // 设置新的最大线程数
            ThreadPool.SetMinThreads(workerThreads: 50, completionPortThreads: 50);


            Hook("Lib");
            ConfigManager.Create().UserJsonConfig("appsettings.json");
            //读取Debugging环境
            RT.Provider.IsDebuggingEnabled = RT.Config.Get(ConfigKeys.IsDebuggingEnabled, false);
            Startup += App_Startup;
            //注册程序未处理的异常处理方法
            DispatcherUnhandledException += OnDispatcherUnhandledException;
            //
            //ClientApp.LoginHandler = () => new SIE.Wpf.Windows.Login.LoginWin().ShowDialog() == true;
            ClientApp.LoginHandler = new LoginHandler(LoginHandler);
            App app = this;
            ClientApp.Register(app);
            ClientApp.Current.LoginSuccessed += delegate { MoveUpdatorFiles(); };
#if DEBUG
            SIE.Data.DbAccesserFactory.DbCommandPrepared += (s, e) =>
            {
                string sqlDebug = e.DbCommand.ToTraceString();
                System.Diagnostics.Trace.WriteLine(sqlDebug);
            };
#endif
        }

        bool LoginHandler()
        {
            var login = new SIE.Wpf.Windows.Login.LoginWin();

            var dllName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SIE.MOM.Ext.dll");
            if (File.Exists(dllName))
            {
                object result = Assembly.LoadFrom(dllName)?.GetType("SIE.MOM.ServerManager")?.InvokeMember("RegisterMultiServer", BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod, null, null, new object[] { login });
            }

            return login.ShowDialog() == true;
        }

        void DeleteConfigFile(ConfigurationErrorsException exc)
        {
            if (exc == null)
                return;
            if (exc.Filename.IsNotEmpty() && File.Exists(exc.Filename))
                File.Delete(exc.Filename);
            DeleteConfigFile(exc.InnerException as ConfigurationErrorsException);
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            CRT.Service.Register<IModuleTypeLoader, ModuleTypeLoader>();
        }

        void MoveUpdatorFiles()
        {
            Task.Run(() =>
            {
                var file = RT.Config.Get(ConfigKeys.AutoUpdator, "S-MOM.exe");
                MoveUpdatorFile(file);
                MoveUpdatorFile(file + ".config");
                MoveUpdatorFile("resources/splash.mp4");
            });
        }

        void MoveUpdatorFile(string file)
        {
            try
            {
                var source = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AutoUpdator", file);
                if (File.Exists(source))
                {
                    var target = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);
                    if (File.Exists(target))
                        File.Delete(target);
                    File.Move(source, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, target));
                }
            }
            catch (Exception exc) { RT.Logger.Error("AutoUpdator", exc); }
        }

        void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                if (RT.Service.IsRegistered<SIE.Services.IMessageService>())
                    e.Exception.Alert();
                else
                    SIE.Wpf.Services.WpfMessageService.DoShowErrorDetail(e.Exception, "发生错误:".L10N() + e.Exception.Message);
                e.Handled = true;
            }
            catch (Exception exc)
            {
                try
                {
                    //记录异常信息
                    CRT.Logger.Error("显示异常失败:".L10N() + e.Exception.GetExceptionDetail(), exc);
                }
                catch
                {
                    //不处理
                }
            }
        }

        /// <summary>
        /// 添加Dll失败事件，从指定目录加载
        /// </summary>
        /// <param name="folders"></param>
        void Hook(params string[] folders)
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                Console.WriteLine(args.Name);
                string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                foreach (var dir in folders)
                {
                    string assemblyPath = Path.Combine(assemblyFolder, dir, new AssemblyName(args.Name).Name + ".dll");

                    if (File.Exists(assemblyPath))
                        return Assembly.LoadFrom(assemblyPath);
                }


                return null;
            };
        }
    }
}
