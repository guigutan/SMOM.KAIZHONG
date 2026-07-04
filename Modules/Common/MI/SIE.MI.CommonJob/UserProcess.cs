using System;
using System.Runtime.InteropServices;
using System.Security;

namespace SIE.MI.CommonJob
{
    class UserProcess
    {
        public const int GENERIC_ALL_ACCESS = 0x10000000;
        public const int TOKEN_DUPLICATE = 0x0002;
        public const uint MAXIMUM_ALLOWED = 0x2000000;
        public const int CREATE_NEW_CONSOLE = 0x00000010;

        public const int IDLE_PRIORITY_CLASS = 0x40;
        public const int NORMAL_PRIORITY_CLASS = 0x20;
        public const int HIGH_PRIORITY_CLASS = 0x80;
        public const int REALTIME_PRIORITY_CLASS = 0x100;
        public const int STARTF_USESHOWWINDOW = 0x00000001;

        public const int CREATE_UNICODE_ENVIRONMENT = 0x00000400;

        public static uint SESSIONID { get; set; } = AppRuntime.Config.Get<uint>("kettle.sessionId", 999);

        /// <summary>
        /// SECURITY_ATTRIBUTES结构包含一个对象的安全描述符，并指定检索到指定这个结构的句柄是否是可继承的
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]   ////顺序布局,按属性声明顺序存储
        public struct SecurityAttributes
        {
            public int Length;              // 结构体的大小，可用SIZEOF取得
            public IntPtr lpSecurityDescriptor; // 安全描述符
            public bool bInheritHandle; // 安全描述的对象能否被新创建的进程继承
        }

        /// <summary>
        /// STARTUPINFO用于指定新进程的主窗口特性的一个结构。
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct StartupInfo
        {
            public int cb;      //包含STARTUPINFO结构中的字节数.如果Microsoft将来扩展该结构,它可用作版本控制手段，应用程序必须将cb初始化为sizeof(STARTUPINFO)。
            public String lpReserved; //保留。必须初始化为NULL。
            public String lpDesktop;//用于标识启动应用程序所在的桌面的名字。如果该桌面存在，新进程便与指定的桌面相关联。如果桌面不存在，便创建一个带有默认属性的桌面，并使用为新进程指定的名字。如果lpDesktop是NULL（这是最常见的情况),那么该进程将与当前桌面相关联。
            public String lpTitle;//用于设定控制台窗口的名称
            public uint dwX;//用于设定应用程序窗口在屏幕上应该放置的位置的x和y坐标（以像素为单位）
            public uint dwY;
            public uint dwXSize; //用于设定应用程序窗口的宽度和长度
            public uint dwYSize;
            public uint dwXCountChars;//用于设定子应用程序的控制台窗口的宽度和高度
            public uint dwYCountChars;
            public uint dwFillAttribute;//用于设定子应用程序的控制台窗口使用的文本和背景颜色
            public uint dwFlags;
            public short wShowWindow; // 显示方式
            public short cbReserved2; //保留。必须被初始化为0
            public IntPtr lpReserved2; //保留。必须被初始化为NULL
            public IntPtr hStdInput; //用于设定供控制台输入和输出用的缓存的句柄
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        /// <summary>
        /// 创建进程时相关的数据结构之一，该结构返回有关新进程及其主线程的信息。
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ProcessInformation
        {
            public IntPtr hProcess;  //返回新进程的句柄
            public IntPtr hThread;  //返回主线程的句柄
            public uint dwProcessId; //返回一个全局进程标识符
            public uint dwThreadId;  //返回一个全局线程标识符
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WtsSessionInfo
        {
            public uint SessionId;
            public IntPtr pWinStationName;
            public WTS_CONNECTSTATE_CLASS State;
        }

        public enum WTS_CONNECTSTATE_CLASS
        {
            WTSActive,
            WTSConnected,
            WTSConnectQuery,
            WTSShadow,
            WTSDisconnected,
            WTSIdle,
            WTSListen,
            WTSReset,
            WTSDown,
            WTSInit
        }

        /// <summary>
        /// 令牌_类型枚举包含区分主令牌和模拟令牌的值。
        /// </summary>
        enum TOKEN_TYPE
        {
            TokenPrimary = 1,       //主令牌
            TokenImpersonation = 2  //模拟令牌
        }

        /// <summary>
        /// 安全模拟级别枚举包含指定安全模拟级别的值。安全模拟级别控制服务器进程可以代表客户端进程的程度。
        /// </summary>
        enum SECURITY_IMPERSONATION_LEVEL
        {
            SecurityAnonymous = 0,   // 服务器进程无法获取有关客户端的标识信息，也无法模拟客户端
            SecurityIdentification = 1, //服务器进程可以获取有关客户端的信息，例如安全标识符和特权，但它不能模拟客户端
            SecurityImpersonation = 2,  //服务器进程可以在其本地系统上模拟客户端的安全上下文。服务器无法模拟远程系统上的客户端。
            SecurityDelegation = 3, //服务器进程可以模拟远程系统上客户端的安全上下文
        }

        enum WTSInfoClass
        {
            InitialProgram,
            ApplicationName,
            WorkingDirectory,
            OEMId,
            SessionId,
            UserName,
            WinStationName,
            DomainName,
            ConnectState,
            ClientBuildNumber,
            ClientName,
            ClientDirectory,
            ClientProductId,
            ClientHardwareId,
            ClientAddress,
            ClientDisplay,
            ClientProtocolType
        }

        #region Win32 API Imports

        [DllImport("kernel32.dll")]
        static extern bool ProcessIdToSessionId(uint dwProcessId, ref uint pSessionId);

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

        [DllImport("advapi32", SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
        static extern bool OpenProcessToken(IntPtr ProcessHandle, int DesiredAccess, ref IntPtr TokenHandle);

        [DllImport("wtsapi32.dll", CharSet = CharSet.Unicode, SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
        static extern bool WTSQuerySessionInformation(System.IntPtr hServer, int sessionId, WTSInfoClass wtsInfoClass, out System.IntPtr ppBuffer, out uint pBytesReturned);

        /// <summary>
        /// 检索控制台会话的会话标识符。控制台会话是当前连接到物理控制台的会话
        /// </summary>
        /// <returns>返回连接到物理控制台的会话的会话标识符</returns>
        [DllImport("kernel32.dll")]
        static extern uint WTSGetActiveConsoleSessionId();

        /// <summary>
        /// 关闭句柄
        /// </summary>
        /// <param name="hSnapshot">句柄指针</param>
        /// <returns>返回是否执行成功</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hSnapshot);

        /// <summary>
        /// 创建新进程及其主线程。新进程在由指定令牌表示的用户的安全上下文中运行
        /// </summary>
        /// <param name="hToken">授权令牌</param>
        /// <param name="lpApplicationName">执行文件全路径</param>
        /// <param name="lpCommandLine">命令行</param>
        /// <param name="lpProcessAttributes">pointer to process SECURITY_ATTRIBUTES</param>
        /// <param name="lpThreadAttributes">pointer to thread SECURITY_ATTRIBUTES</param>
        /// <param name="bInheritHandle">如果此参数为真，则调用进程中的每个可继承句柄都由新进程继承。如果参数为假，则不继承句柄</param>
        /// <param name="dwCreationFlags">控制优先级类和进程创建的标志</param>
        /// <param name="lpEnvironment">指向新进程的环境块的指针</param>
        /// <param name="lpCurrentDirectory">当前目录路径</param>
        /// <param name="lpStartupInfo">pointer to STARTUPINFO structure</param>
        /// <param name="lpProcessInformation">返回关于新进程的信息</param>
        /// <returns></returns>
        [DllImport("advapi32.dll", EntryPoint = "CreateProcessAsUser", SetLastError = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static bool CreateProcessAsUser(IntPtr hToken, String lpApplicationName, String lpCommandLine, ref SecurityAttributes lpProcessAttributes,
            ref SecurityAttributes lpThreadAttributes, bool bInheritHandle, int dwCreationFlags, IntPtr lpEnvironment,
            String lpCurrentDirectory, ref StartupInfo lpStartupInfo, out ProcessInformation lpProcessInformation);

        /// <summary>
        /// 创建一个新的访问令牌，复制现有令牌
        /// </summary>
        /// <param name="ExistingTokenHandle">已经存在的令牌</param>
        /// <param name="dwDesiredAccess">指定新令牌的请求访问权限</param>
        /// <param name="lpThreadAttributes">线程属性</param>
        /// <param name="TokenType">令牌类型</param>
        /// <param name="ImpersonationLevel">新令牌的模拟级别</param>
        /// <param name="DuplicateTokenHandle">新令牌的指针</param>
        /// <returns>返回是否创建成功</returns>
        [DllImport("advapi32.dll", EntryPoint = "DuplicateTokenEx")]
        public extern static bool DuplicateTokenEx(IntPtr ExistingTokenHandle, uint dwDesiredAccess,
            ref SecurityAttributes lpThreadAttributes, int TokenType,
            int ImpersonationLevel, ref IntPtr DuplicateTokenHandle);

        /// <summary>
        /// 为指定的访问令牌设置各种类型的信息
        /// </summary>
        /// <param name="TokenHandle">要为其设置信息的访问令牌的句柄。</param>
        /// <param name="TokenInformationClass">标记信息类枚举类型中的一个值，用于标识函数集的信息类型</param>
        /// <param name="TokenInformation">指向包含访问令牌中设置的信息的缓冲区的指针</param>
        /// <param name="TokenInformationLength">指定标记信息指向的缓冲区的长度</param>
        /// <returns>返回是否设置成功</returns>
        [DllImport("advapi32.dll", EntryPoint = "SetTokenInformation")]
        public extern static bool SetTokenInformation(IntPtr TokenHandle, int TokenInformationClass,
            IntPtr TokenInformation, uint TokenInformationLength);

        /// <summary>
        /// 获取会话ID指定的登录用户的主访问令牌
        /// </summary>
        /// <param name="sessionId">远程桌面服务会话标识符</param>
        /// <param name="Token">如果函数成功，则为登录用户接收指向令牌句柄的指针</param>
        /// <returns>返回是否获取成功</returns>
        [DllImport("wtsapi32.dll", SetLastError = true)]
        public static extern bool WTSQueryUserToken(uint sessionId, out IntPtr Token);

        /// <summary>
        /// 检索指定用户的环境变量
        /// </summary>
        /// <param name="lpEnvironment">接收指向新环境块的指针</param>
        /// <param name="hToken">用户的令牌</param>
        /// <param name="bInherit">指定是否从当前进程的环境继承。如果该值为真，则进程继承当前进程的环境。如果该值为假，则进程不会继承当前进程的环境。</param>
        /// <returns>返回是否创建成功</returns>
        [DllImport("userenv.dll", SetLastError = true)]
        static extern bool CreateEnvironmentBlock(out IntPtr lpEnvironment, IntPtr hToken, bool bInherit);

        [DllImport("wtsapi32.dll", SetLastError = true)]
        public static extern bool WTSEnumerateSessions(
            IntPtr hServer,
            int Reserved,
            int Version,
            ref IntPtr ppSessionInfo,//WTS_SESSION_INFO PWTS_SESSION_INFO *ppSessionInfo,
            ref int pCount
            );

        [DllImport("wtsapi32.dll", SetLastError = true)]
        public static extern void WTSFreeMemory(IntPtr pMemory);
        #endregion

        public static uint GetSessionIdFromEnumerateSessions()
        {
            IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;
            uint dwSessionId = 0;
            uint dwConnectedId = 0;
            IntPtr pSessionInfo = IntPtr.Zero;
            int dwCount = 0;
            try
            {
                WTSEnumerateSessions(WTS_CURRENT_SERVER_HANDLE, 0, 1,
                                     ref pSessionInfo, ref dwCount);

                Int32 dataSize = Marshal.SizeOf(typeof(WtsSessionInfo));
                IntPtr current = pSessionInfo;
                for (int i = 0; i < dwCount; i++)
                {
                    WtsSessionInfo si = (WtsSessionInfo)Marshal.PtrToStructure(
                        current, typeof(WtsSessionInfo));
                    string msg = string.Format("GetSessionIdFromEnumerateSessions Session id:{0} state:{1} station name:{2} ", si.SessionId, si.State, Marshal.PtrToStringAnsi(si.pWinStationName));
                    log(msg);
                    if (WTS_CONNECTSTATE_CLASS.WTSActive == si.State)
                    {
                        dwSessionId = si.SessionId;
                        break;
                    }
                    else if (WTS_CONNECTSTATE_CLASS.WTSConnected == si.State)
                    {
                        dwConnectedId = si.SessionId;
                    }

                    current += dataSize;
                }

                if (dwSessionId == 0)
                {
                    dwSessionId = dwConnectedId;
                }
            }
            finally
            {
                if (pSessionInfo == null || pSessionInfo == IntPtr.Zero)
                {
                    WTSFreeMemory(pSessionInfo);
                }
            }

            return dwSessionId;
        }

        /// <summary>
        /// 创建一个可交互的复杂的UI进程
        /// </summary>
        /// <param name="applicationName">进程全路径</param>
        /// <param name="command">命令行信息</param>
        /// <param name="pi">主线程信息</param>
        /// <returns>返回是否创建成功</returns>
        public static bool CreateProcess(String applicationName, String command, out ProcessInformation pi)
        {
            bool result = false;
            IntPtr hToken;//WindowsIdentity.GetCurrent().Token;  //返回表示当前 Windows 用户的 WindowsIdentity 对象。
            IntPtr hDupedToken = IntPtr.Zero;
            pi = new ProcessInformation();

            try
            {
                SecurityAttributes sa = new SecurityAttributes();
                sa.Length = Marshal.SizeOf(sa);
                StartupInfo si = new StartupInfo();
                si.cb = Marshal.SizeOf(si);

                UInt32 dwSessionID = GetSessionIdFromEnumerateSessions(); // WTSGetActiveConsoleSessionId();  //检索控制台会话的会话标识符
                if (SESSIONID != 999 && dwSessionID != SESSIONID)
                {
                    string msg = string.Format("current SessionID:{0};change session:{1}", dwSessionID, SESSIONID);
                    log(msg);
                    dwSessionID = SESSIONID;
                }

                log(string.Format("current SessionID:{0};", dwSessionID));
                result = WTSQueryUserToken(dwSessionID, out hToken);    //获取会话ID指定的登录用户的主访问令牌
                if (!result)
                {
                    int error = Marshal.GetLastWin32Error();
                    string message = String.Format("WTSQueryUserToken Error: {0}", error);
                    log(message);
                }

                // 创建一个新的访问令牌，复制现有令牌
                result = DuplicateTokenEx(
                  hToken,
                  MAXIMUM_ALLOWED,
                  ref sa,
                  (int)SECURITY_IMPERSONATION_LEVEL.SecurityIdentification,
                  (int)TOKEN_TYPE.TokenPrimary,
                  ref hDupedToken
                 );

                if (!result)
                {
                    int error = Marshal.GetLastWin32Error();
                    string message = String.Format("DuplicateTokenEx Error: {0}", error);
                    log(message);
                }

                //检索指定用户的环境变量
                IntPtr lpEnvironment;
                result = CreateEnvironmentBlock(out lpEnvironment, hDupedToken, false);

                if (!result)
                {
                    int error = Marshal.GetLastWin32Error();
                    string message = String.Format("CreateEnvironmentBloc Error: {0}", error);
                    log(message);
                }

                si.dwFlags = STARTF_USESHOWWINDOW;
                si.wShowWindow = 0;   // 隐藏窗口            

                //创建新进程及其主线程。新进程在由指定令牌表示的用户的安全上下文中运行
                result = CreateProcessAsUser(
                      hDupedToken,
                      applicationName,
                      command,
                      ref sa, ref sa,
                      false, NORMAL_PRIORITY_CLASS | CREATE_NEW_CONSOLE | CREATE_UNICODE_ENVIRONMENT, lpEnvironment,
                      null, ref si, out pi);

                if (!result)
                {
                    int error = Marshal.GetLastWin32Error();
                    string message = String.Format("CreateProcessAsUser Error: {0}", error);
                    log(message);
                }
            }
            catch (Exception ex)
            {
                log(ex.Message);
            }
            finally
            {
                if (pi.hProcess != IntPtr.Zero)
                    CloseHandle(pi.hProcess);
                if (pi.hThread != IntPtr.Zero)
                    CloseHandle(pi.hThread);
                if (hDupedToken != IntPtr.Zero)
                    CloseHandle(hDupedToken);
            }

            return result;
        }

        private static void log(string content)
        {
            //日志
        }
    }
}