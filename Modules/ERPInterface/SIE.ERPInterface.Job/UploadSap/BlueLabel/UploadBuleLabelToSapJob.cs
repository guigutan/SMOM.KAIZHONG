using SIE.Common.Schdules;
using SIE.Domain;
using SIE.ERPInterface.Sap.Upload.BlueLabel;
using SIE.MES.PackingQC;
using SIE.ObjectModel;
using System;
using System.Linq;

namespace SIE.ERPInterface.Job.UploadSap.BlueLabel
{

    /// <summary>
    /// 蓝标外箱HU装箱标记推送SAP调度
    /// </summary>
    [Job("蓝标外箱HU装箱标记推送SAP调度", typeof(UploadBuleLabelToSapJobParameter))]
    public class UploadBuleLabelToSapJob : JobBase
    {
        /// <summary>
        /// 调度执行主键
        /// </summary>
        public static string _jobKey = "蓝标外箱HU装箱标记推送SAP-" + typeof(UploadBuleLabelToSapJob).FullName + "!" + RT.InvOrg;

        /// <summary>
        /// 执行调度
        /// </summary>
        /// <param name="param">参数</param>
        protected override void ExecuteJob(object param)
        {
            var p = param as UploadBuleLabelToSapJobParameter;


            //上传[蓝标装箱]标签数据至SAP
            var datas = RT.Service.Resolve<PackingQcController>().GetUnUploadSapDatas(p?.Days ?? 1);

            if (datas.Count == 0) {
                AddLog("没有要推送的数据");
                return;
            }

            var (totalCount, successCount, result) = RT.Service.Resolve<HttpSapBlueLabelController>().UploadBlueLabelToSap(datas);

            AddLog("推送, 总共[{0}]成功[{1}]; ".L10nFormat(totalCount, successCount));


        }

    }

    /// <summary>
    /// 推送蓝标外箱HU装箱标记到SAP调度参数
    /// </summary>
    [RootEntity, Serializable]
    public class UploadBuleLabelToSapJobParameter : JobParameter
    {
        /// <summary>
        ///         
        /// </summary>
        public UploadBuleLabelToSapJobParameter()
        {
            Days = 1;
        }

        #region 上传几天前数据 Days
        /// <summary>
        /// 上传前几天数据
        /// </summary>
        [Label("推送/装箱几天前数据")]
        public static readonly Property<int> DaysProperty = P<UploadBuleLabelToSapJobParameter>.Register(e => e.Days);

        /// <summary>
        /// 上传前几天数据
        /// </summary>
        public int Days
        {
            get { return this.GetProperty(DaysProperty); }
            set { this.SetProperty(DaysProperty, value); }
        }
        #endregion


    }

    class UploadBuleLabelToSapJobParameterWebViewConfig : WebViewConfig<UploadBuleLabelToSapJobParameter>
    {
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Days).Show();
            }
        }
    }
}
