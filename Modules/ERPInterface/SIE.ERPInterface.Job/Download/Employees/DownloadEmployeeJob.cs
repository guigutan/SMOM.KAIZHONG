using SIE.Common.Schdules;
using SIE.Domain;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Download.Employees;
using SIE.ERPInterface.Job.Common;
using SIE.ERPInterface.Smom.Download;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Job.Download.Employees
{
    /// <summary>
    /// 员工下载
    /// </summary>
    [Job("员工下载", typeof(IsCreateAccountParameter))]
    public class DownloadEmployeeJob : JobBase
    {
        /// <summary>
        /// 执行调度
        /// </summary>
        /// <param name="param">参数</param>
        protected override void ExecuteJob(object param)
        {
            var p = param as IsCreateAccountParameter;
            var isCreateAccount = p?.IsCreateAccount ?? false;
            if (p?.IsDownloadInf == true)
            {
                var resultInf = RT.Service.Resolve<SoapEmployeeController>().DownloadToInf();                     //执行中间表下载
                AddLog("中间表结束下载{0}".L10nFormat(resultInf == null ? "。" : "，" + resultInf.Msg));
            }

            var resultSmom = RT.Service.Resolve<DownloadEmployeeController>().DownloadEmployeeInfToBusiness(isCreateAccount);           //执行业务表下载
            AddLog("业务表结束下载{0}".L10nFormat(resultSmom == null ? "。" : "，" + resultSmom.Msg));
        }
    }

    /// <summary>
    /// 是否创建用户参数
    /// </summary>
    [RootEntity, Serializable]
    public class IsCreateAccountParameter : DLCommonParameter
    {
        #region 是否创建用户 IsCreateAccount
        /// <summary>
        /// 是否创建用户
        /// </summary>
        [Label("是否创建用户")]
        public static readonly Property<bool> IsCreateAccountProperty = P<IsCreateAccountParameter>.Register(e => e.IsCreateAccount);

        /// <summary>
        /// 是否创建用户
        /// </summary>
        public bool IsCreateAccount
        {
            get { return this.GetProperty(IsCreateAccountProperty); }
            set { this.SetProperty(IsCreateAccountProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 调度是否创建用户参数配置
    /// </summary>
    class IsCreateAccountParameterWebViewConfig : WebViewConfig<IsCreateAccountParameter>
    {
        protected override void ConfigView()
        {
            View.Property(p => p.IsCreateAccount).Show();
        }
    }

    /// <summary>
    /// 调度是否创建用户参数配置
    /// </summary>
    public class IsCreateAccountParameterViewConfig : WebViewConfig<IsCreateAccountParameter>
    {
        /// <summary>
        /// 默认配置
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(P => P.IsCreateAccount).Show();
        }
    }
}
