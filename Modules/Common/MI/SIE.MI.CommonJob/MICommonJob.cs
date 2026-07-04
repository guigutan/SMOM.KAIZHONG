using SIE.Common.Schdules;
using System;
using System.Diagnostics;
using System.IO;
using static SIE.MI.CommonJob.UserProcess;

namespace SIE.MI.CommonJob
{
    /// <summary>
    /// Kettle存储数据类型
    /// </summary>
    public enum KettleType
    {
        /// <summary>
        /// 文件
        /// </summary>
        File,

        /// <summary>
        /// 文件资源库
        /// </summary>
        FileRepository
    }

    /// <summary>
    /// 抽取、清洗QMS的数据，并且汇总到MI
    /// </summary>
    [Job("抽取、清洗数据的抽象类", typeof(JobParameter))]
    public  class MICommonJob : JobBase
    {
        /// <summary>
        /// KETTLE安装路径
        /// </summary>
        private readonly string installUrl;

        /// <summary>
        /// KETTLE文件（作业、转换）根目录
        /// </summary>
        private readonly string jobUrl;

        /// <summary>
        /// 日记文件夹路径
        /// </summary>
        private readonly string logDir;

        /// <summary>
        /// KETTLE文件日记路径
        /// </summary>
        private string logPath;

        /// <summary>
        /// 被调用的Job文件相对路径
        /// </summary>
        protected string jobPathName;

        /// <summary>
        /// 调用的Kettle存储数据类型(默认文件资源库)
        /// </summary>
        protected KettleType kettleType;

        /// <summary>
        /// 初始化
        /// </summary>
        protected MICommonJob()
        {
            kettleType = KettleType.FileRepository;
            installUrl = AppRuntime.Config.Get("kettle.installUrl");
            jobUrl = AppRuntime.Config.Get("kettle.jobUrl");
            logDir = AppRuntime.Config.Get("kettle.logPath");
        }

        /// <summary>
        /// 执行Job
        /// </summary>
        /// <param name="param">参数</param>
        protected override void ExecuteJob(object param)
        {
            ExecuteKettle();
        }

        /// <summary>
        /// 任务执行入口
        /// </summary>
        /// <returns>返回是否执行成功</returns>
        protected bool ExecuteKettle()
        {
            try
            {
                //校验参数是否获取成功
                if (!installUrl.IsNullOrEmpty())
                    LogToDb("配置参数：installUrl:{0}，jobUrl：{1}，logPath：{2}".L10nFormat(installUrl, jobUrl, logDir));
                else
                {
                    ErrorToDb("配置参数获取失败：installUrl:{0}，jobUrl：{1}，logPath：{2}".L10nFormat(installUrl, jobUrl, logDir));
                    return false;
                }
                if (string.IsNullOrEmpty(logPath))
                {
                    string[] logStrArr = jobPathName.Split('\\');
                    string[] logStrArr1 = logStrArr[logStrArr.Length - 1].Split('.');
                    string logFileName = logStrArr1[0] + ".txt";
                    logPath = Path.Combine(logDir, logFileName);
                }

                string batString = string.Empty;
                if (!ValidateData()) return false;
                if (!GenerateBatString(out batString)) return false;

                string batFilePath = GenerateBatName(batString);
                string cmdsting = string.Format("/c {0}", batFilePath);
                bool result = UserProcess.CreateProcess(@"C:\Windows\System32\Cmd.exe", cmdsting, out ProcessInformation processInfo);
                if (result)
                {
                    LogToDb("已启动KETTLE抽取、清洗数据{0}，进程Id:{1}，启动时间为：{2}，KETTLE日记路径为：{3}".L10nFormat(jobPathName, processInfo.dwProcessId, DateTime.Now, logPath));
                    try
                    {
                        Process process = Process.GetProcessById((int)processInfo.dwProcessId);
                        process.WaitForExit(10 * 60 * 1000);
                        LogToDb("命令窗口退出成功！".L10N());
                    }
                    catch (Exception ex)
                    {
                        ErrorToDb("执行失败，错误：{0}".L10nFormat(ex.Message));
                    }
                    LogToDb("抽取清洗数据完毕，进程Id:{0}，结束时间为：{1}，KETTLE日记路径为：{2}".L10nFormat(processInfo.dwProcessId, DateTime.Now, logPath));
                }
                else
                    LogToDb("创建失败！".L10N());

                return true;
            }
            catch (Exception ex)
            {
                ErrorToDb("执行失败，错误信息：{0}".L10nFormat(ex.Message));
                return false;
            }
        }

        /// <summary>
        /// 验证参数有效性
        /// </summary>
        /// <returns>返回是否验证成功</returns>
        private bool ValidateData()
        {
            if (string.IsNullOrEmpty(jobPathName))
            {
                ErrorToDb("执行失败，当前类为抽象类，不能创建调度".L10N());
                return false;
            }

            if (string.IsNullOrEmpty(installUrl) || string.IsNullOrEmpty(jobUrl))
            {
                ErrorToDb("请在App.config文件配置kettle.installUrl、kettle.jobUrl参数".L10N());
                return false;
            }
            return true;
        }

        /// <summary>
        /// 根据Kettle存储数据的方式生成Bat命令
        /// </summary>
        /// <param name="batString">生成的Bat命令</param>
        /// <returns>返回是否生成成功</returns>
        private bool GenerateBatString(out string batString)
        {
            batString = string.Empty;
            string dir = installUrl.Substring(0, installUrl.IndexOf(':') + 1);   // Kettle安排在哪一个磁盘
            string logConfig = string.Empty;
            if (kettleType == KettleType.File)
            {
                Uri jobUri = new Uri(jobUrl);
                Uri jobAbsoluteUri = new Uri(jobUri, jobPathName);
                string jobPath = jobAbsoluteUri.LocalPath;
                if (!File.Exists(jobPath))
                {
                    ErrorToDb("找不到对应的Job文件：{0}".L10nFormat(jobPath));
                    return false;
                }

                if (!string.IsNullOrEmpty(logPath))
                {
                    logConfig = string.Format(" >>{0}", logPath);
                }

                batString = @"
@echo off
{0}
cd {1}
kitchen.bat /norep  -file={2} {3}
exit".FormatArgs(dir, installUrl, jobPath, logConfig);
            }
            else if (kettleType == KettleType.FileRepository)
            {
                if (!string.IsNullOrEmpty(logPath))
                {
                    logConfig = string.Format(" -level=Debug>{0}", logPath);
                }

                batString = @"
@echo off
{0}
cd {1}
kitchen.bat -rep {2} -job {3} {4}
exit".FormatArgs(dir, installUrl, jobUrl, jobPathName, logConfig);
            }

            return true;
        }

        /// <summary>
        /// 生产BAT名称
        /// </summary>
        /// <param name="content">Bat内容</param>
        /// <returns>返回BAT名称</returns>
        private string GenerateBatName(string content)
        {
            string batDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BAT");
            if (!Directory.Exists(batDir))
            {
                Directory.CreateDirectory(batDir);
            }

            string[] jobPathNameArr = jobPathName.Split('\\');
            string[] NameArr = jobPathNameArr[jobPathNameArr.Length - 1].Split('.');
            string batName = NameArr[0];
            batName = string.Concat(batName, ".bat");
            string batFilePath = Path.Combine(batDir, batName);
            CreateBat(batFilePath, content);

            return batFilePath;
        }

        /// <summary>
        /// 创建BAT文件
        /// </summary>
        /// <param name="filePath">BAT路径</param>
        /// <param name="content">BAT内容</param>
        private void CreateBat(string filePath, string content)
        {
            FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
            //获得字节数组
            byte[] data = System.Text.Encoding.Default.GetBytes(content);
            //开始写入
            fs.Write(data, 0, data.Length);
            //清空缓冲区、关闭流
            fs.Flush();
            fs.Close();
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message"></param>
        public virtual void LogToDb(string message)
        {
            AddLog($"Kettle调度-数据清洗汇总执行结束，{message}".L10N());
        }

        /// <summary>
        /// 记录异常信息
        /// </summary>
        /// <param name="message"></param>
        public virtual void ErrorToDb(string message)
        {
            AddLog($"Kettle调度执行异常-数据清洗汇总执行结束，{message}".L10N());
        }
    }
}