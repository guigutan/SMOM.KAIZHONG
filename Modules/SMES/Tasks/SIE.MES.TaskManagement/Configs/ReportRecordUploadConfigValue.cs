using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.TaskManagement.Configs
{
    /// <summary>
    /// 报工记录上传配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("报工记录上传配置值")]
    public class ReportRecordUploadConfigValue : ConfigValue 
    {
        #region 启用上传SAP EnableUpload
        /// <summary>
        /// 启用上传SAP
        /// </summary>
        [Label("启用上传SAP")]
        public static readonly Property<bool> EnableUploadProperty = P<ReportRecordUploadConfigValue>.Register(e => e.EnableUpload);

        /// <summary>
        /// 启用上传SAP
        /// </summary>
        public bool EnableUpload
        {
            get { return this.GetProperty(EnableUploadProperty); }
            set { this.SetProperty(EnableUploadProperty, value); }
        }
        #endregion

        #region 上传的工序控制码 UploadSteus
        /// <summary>
        /// 上传的工序控制码
        /// </summary>
        [Label("上传的工序控制码")]
        public static readonly Property<string> UploadSteusProperty = P<ReportRecordUploadConfigValue>.Register(e => e.UploadSteus);

        /// <summary>
        /// 上传的工序控制码
        /// </summary>
        public string UploadSteus
        {
            get { return this.GetProperty(UploadSteusProperty); }
            set { this.SetProperty(UploadSteusProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>编码规则名称|打印模板名称</returns>
        public override string Display()
        {
            return "启用上传SAP: {0} | 上传的工序控制码:{1} ".L10nFormat(EnableUpload, UploadSteus);
        }
    }
}
