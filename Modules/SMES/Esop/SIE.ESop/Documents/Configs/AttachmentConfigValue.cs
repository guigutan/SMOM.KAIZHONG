using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.ESop.Configs
{
    /// <summary>
    /// 文件上传参数配置 
    /// </summary>
    [RootEntity, Serializable]
    [Label("文档服务器配置")]
    [DisplayMember(nameof(AttachmentConfigValue.Id))]
    public class AttachmentConfigValue : ConfigValue
    {
        /// <summary>
        /// @包装1#01
        /// </summary>
        public const string REGULAR = @"(?<Process>(?<=^@)[^#]+)|(?<Seq>(?<=\#)\d+)";

        /// <summary>
        /// 工作表:适用物料 
        /// 位置：第一行第一列 至 第一列 第 N 行
        /// </summary>
        public const string APPLY_ITEM_SHEET = "适用物料[1,1,N,1]";

        /// <summary>
        /// 工作表:适用工单
        /// 位置：第一行第一列 至 第一列 第 N 行
        /// </summary>
        public const string APPLY_WORKORDER_SHEET = "适用工单[1,1,N,1]";

        #region 文件大小限制（Mb）MaxSize
        /// <summary>
        /// 文件大小限制（Mb）
        /// </summary>
        [MinValue(1)]
        [Label("文件大小限制(Mb)")]
        public static readonly Property<double> MaxSizeProperty = P<AttachmentConfigValue>.Register(e => e.MaxSize);

        /// <summary>
        /// 文件大小限制（Mb）
        /// </summary>
        public double MaxSize
        {
            get { return this.GetProperty(MaxSizeProperty); }
            set { this.SetProperty(MaxSizeProperty, value); }
        }
        #endregion

        #region 工序匹配正则表达式 MappingSheetRegular
        /// <summary>
        /// 工序匹配正则表达式
        /// </summary>
        [Label("工序匹配正则表达式")]
        public static readonly Property<string> MappingSheetRegularProperty = P<AttachmentConfigValue>.Register(e => e.MappingSheetRegular);

        /// <summary>
        /// 工序匹配正则表达式
        /// </summary>
        public string MappingSheetRegular
        {
            get { return this.GetProperty(MappingSheetRegularProperty); }
            set { this.SetProperty(MappingSheetRegularProperty, value); }
        }
        #endregion

        #region 适用产品页 ItemSheet
        /// <summary>
        /// 适用产品页
        /// </summary>
        [Label("适用产品页")]
        public static readonly Property<string> ItemSheetProperty = P<AttachmentConfigValue>.Register(e => e.ItemSheet);

        /// <summary>
        /// 适用产品页
        /// </summary>
        public string ItemSheet
        {
            get { return this.GetProperty(ItemSheetProperty); }
            set { this.SetProperty(ItemSheetProperty, value); }
        }
        #endregion

        #region 适用工单页 WorkOrderSheet
        /// <summary>
        /// 适用工单页
        /// </summary>
        [Label("适用工单页")]
        public static readonly Property<string> WorkOrderSheetProperty = P<AttachmentConfigValue>.Register(e => e.WorkOrderSheet);

        /// <summary>
        /// 适用工单页
        /// </summary>
        public string WorkOrderSheet
        {
            get { return this.GetProperty(WorkOrderSheetProperty); }
            set { this.SetProperty(WorkOrderSheetProperty, value); }
        }
        #endregion

        #region 使用Com组件 UseCom
        /// <summary>
        /// 使用Com组件
        /// </summary>
        [Label("使用Com组件")]
        public static readonly Property<bool> UseComProperty = P<AttachmentConfigValue>.Register(e => e.UseCom);

        /// <summary>
        /// 使用Com组件
        /// </summary>
        public bool UseCom
        {
            get { return this.GetProperty(UseComProperty); }
            set { this.SetProperty(UseComProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>返回显示内容</returns>
        public override string Display()
        {
            return "大小限制:{0} Mb,工序映射工作表正则表达式:{1},产品页{2},工单页{3}".L10nFormat(MaxSize, MappingSheetRegular, ItemSheet, WorkOrderSheet);
        }
    }
}