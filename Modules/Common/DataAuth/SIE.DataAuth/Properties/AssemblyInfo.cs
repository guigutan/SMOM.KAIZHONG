using System.Reflection;
using System.Runtime.InteropServices;

// 有关程序集的一般信息由以下
// 控制。更改这些特性值可修改
// 与程序集关联的信息。
[assembly: AssemblyTitle("SIE.DataAuth")]
[assembly: AssemblyDescription(SIE.SMOM.AssemblyInformation.Description)]
[assembly: AssemblyCompany(SIE.SMOM.AssemblyInformation.AssemblyCompany)]
[assembly: AssemblyCopyright(SIE.SMOM.AssemblyInformation.AssemblyCopyright)]
[assembly: AssemblyTrademark(SIE.SMOM.AssemblyInformation.AssemblyTrademark)]
[assembly: AssemblyVersion(SIE.SMOM.AssemblyInformation.Version)]
[assembly: AssemblyFileVersion(SIE.SMOM.AssemblyInformation.FileVersion)]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyProduct("SIE.DataAuth")]
[assembly: AssemblyCulture("")]

//将 ComVisible 设置为 false 将使此程序集中的类型
//对 COM 组件不可见。  如果需要从 COM 访问此程序集中的类型，
//请将此类型的 ComVisible 特性设置为 true。
[assembly: ComVisible(false)]

// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
[assembly: Guid("456422E6-AD6D-46A8-B37F-AE7D8789A9C8")]

namespace SIE.SMOM
{
    /// <summary>
    /// 
    /// </summary>
    public static class AssemblyInformation
    {
        /// <summary>
        /// 生成模式
        /// </summary>
#if DEBUG
        public const string AssemblyConfiguration = "Debug - ";
#else
    public const string AssemblyConfiguration = "Release - ";
#endif

        /// <summary>
        /// 程序集版权信息
        /// </summary>
        public const string AssemblyCopyright = "Copyright © 2024 广州赛意信息科技股份有限公司";
        /// <summary>
        /// 自定义标识
        /// </summary>
        public const string AssemblyTrademark = "SIE.DataAuth.";
        /// <summary>
        /// 公司名称
        /// </summary>
        public const string AssemblyCompany = "广州赛意信息科技股份有限公司";
        /// <summary>
        /// 程序集版本
        /// </summary>
        public const string Version = "10.3.0";
        /// <summary>
        /// 文件版本
        /// </summary>
        public const string FileVersion = "10.3.0";
        /// <summary>
        /// 程序文本说明
        /// </summary>
        public const string Description = "警告：本计算机程序受著作权法和国际条约保护。如未经授权而擅自复制或传播本程序（或其中任何部分），将受到严厉的民事和刑事制裁，并将在法律许可的最大限制内受到起诉。";
        /// <summary>
        /// 产品介绍
        /// </summary>
        public const string ProductIntroduction = "产品介绍：\r\n赛意SMOM系统是以智能工厂为框架蓝图自主研发设计的一套工业软件产品群。" +
            "从供应链端开始，涵盖产品研发、物料流转、计划排程、制造执行、质量控制、仓储管理等全方向制造业的运转流程。" +
            "通过完整、实用的赛意SMOM系统，使工业互联网与生产制造之间的基础信息流整合，让多工厂、多车间的信息互通。" +
            "打破产能共享、质量、供应链需求、物流等生产环节之间的信息孤岛。实现研发到出货之间所有流程的资源信息共享，工业化和信息化两化融合。";
    }
}