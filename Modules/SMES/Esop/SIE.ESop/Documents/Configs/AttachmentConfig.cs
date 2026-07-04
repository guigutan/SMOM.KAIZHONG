using SIE.Common.Configs;
using System;

namespace SIE.ESop.Configs
{
    /// <summary>
    /// 文档服务器配置
    /// </summary>
    [System.ComponentModel.DisplayName("文件上传配置")]
    [System.ComponentModel.Description("文件上传配置")]
    public partial class AttachmentConfig : ModuleConfig<AttachmentConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly AttachmentConfigValue defaultValue = new AttachmentConfigValue()
        {
            MaxSize = 50,
            MappingSheetRegular = AttachmentConfigValue.REGULAR,
            ItemSheet = AttachmentConfigValue.APPLY_ITEM_SHEET,
            WorkOrderSheet = AttachmentConfigValue.APPLY_WORKORDER_SHEET,
            UseCom = false
        };

        /// <summary>
        /// 默认值
        /// </summary>
        public override AttachmentConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 获取界面的值，假设工序匹配正则表达式或适用产品页为空则设置默认值
        /// </summary>
        /// <returns>返回界面设置的值</returns>
        protected override AttachmentConfigValue GetValue()
        {
            var configValue = base.GetValue();
            if (configValue != null && configValue.MappingSheetRegular.IsNullOrWhiteSpace())
            {
                configValue.MappingSheetRegular = AttachmentConfigValue.REGULAR;
            }
            if (configValue != null && configValue.ItemSheet.IsNullOrWhiteSpace())
            {
                configValue.ItemSheet = AttachmentConfigValue.APPLY_ITEM_SHEET;
            }
            if (configValue != null && configValue.WorkOrderSheet.IsNullOrWhiteSpace())
            {
                configValue.WorkOrderSheet = AttachmentConfigValue.APPLY_WORKORDER_SHEET;
            }
            return configValue;
        }
    }
}