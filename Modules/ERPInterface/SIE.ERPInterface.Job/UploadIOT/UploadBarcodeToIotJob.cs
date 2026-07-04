using SIE.Common.Schdules;
using SIE.Domain;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.UploadTransactionRules;
using SIE.ERPInterface.Job.Common;
using SIE.ERPInterface.Sap.Upload.SaleReturn;
using SIE.ERPInterface.Sap.Upload.TaskReport;
using SIE.Inventory.Transactions;
using SIE.MES.TaskManagement.HeatTreatments;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Job.Upload.TaskReport
{

    /// <summary>
    /// 工序标签数据推送IOT调度
    /// </summary>
    [Job("工序标签数据推送IOT调度", typeof(BarcodeToIotJobParameter))]
    public class UploadBarcodeToIotJob : JobBase
    {
        /// <summary>
        /// 调度执行主键
        /// </summary>
        public static string _jobKey = "工序标签数据推送IOT-" + typeof(UploadBarcodeToIotJob).FullName + "!" + RT.InvOrg;

        /// <summary>
        /// 执行调度
        /// </summary>
        /// <param name="param">参数</param>
        protected override void ExecuteJob(object param)
        {
            var p = param as BarcodeToIotJobParameter;

            if (p?.AutoUpload == true)
            {
                //上传[热处理]前置工序标签数据至IOT
                var (totalCount, successCount, result) = RT.Service.Resolve<HeatTreatmentController>().UploadBarcodeToScada("热处理", p?.Days ?? 1);

                AddLog("推送, 总共[{0}]成功[{1}] 返回结果[{2}]; ".L10nFormat(totalCount, successCount, result));
            }

            if (p?.AutoReport == true)
            {
                var (totalCount, successCount, result) = RT.Service.Resolve<HeatTreatmentController>().HeatTreatmentReport(p?.Days ?? 1);

                AddLog("报工, 总共[{0}]成功[{1}] 返回结果[{2}]; ".L10nFormat(totalCount, successCount, result));
            }
        }

    }

    /// <summary>
    /// 工序标签数据推送IOT调度参数
    /// </summary>
    [RootEntity, Serializable]
    public class BarcodeToIotJobParameter : JobParameter
    {
        /// <summary>
        ///         
        /// </summary>
        public BarcodeToIotJobParameter()
        {
            Days = 1;
            AutoUpload = true;
            AutoUpload = true;
        }

        #region 上传前几天数据 Days
        /// <summary>
        /// 上传前几天数据
        /// </summary>
        [Label("推送/报工前几天数据")]
        public static readonly Property<int> DaysProperty = P<BarcodeToIotJobParameter>.Register(e => e.Days);

        /// <summary>
        /// 上传前几天数据
        /// </summary>
        public int Days
        {
            get { return this.GetProperty(DaysProperty); }
            set { this.SetProperty(DaysProperty, value); }
        }
        #endregion

        #region 标签自动推送 AutoUpload
        /// <summary>
        /// 自动推送标签
        /// </summary>
        [Label("标签自动推送")]
        public static readonly Property<bool?> AutoUploadProperty = P<BarcodeToIotJobParameter>.Register(e => e.AutoUpload);

        /// <summary>
        /// 标签自动推送
        /// </summary>
        public bool? AutoUpload
        {
            get { return this.GetProperty(AutoUploadProperty); }
            set { this.SetProperty(AutoUploadProperty, value); }
        }
        #endregion

        #region 出炉自动报工 AutoReport
        /// <summary>
        /// 出炉自动报工
        /// </summary>
        [Label("出炉自动报工")]
        public static readonly Property<bool?> AutoReportProperty = P<BarcodeToIotJobParameter>.Register(e => e.AutoReport);

        /// <summary>
        /// 出炉自动报工
        /// </summary>
        public bool? AutoReport
        {
            get { return this.GetProperty(AutoReportProperty); }
            set { this.SetProperty(AutoReportProperty, value); }
        }
        #endregion

    }

    class BarcodeToIotJobParameterWebViewConfig : WebViewConfig<BarcodeToIotJobParameter>
    {
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Days).Show();
                View.Property(p => p.AutoUpload).Show();
                View.Property(p => p.AutoReport).Show();
            }
        }
    }
}
