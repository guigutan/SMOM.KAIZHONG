using SIE.AbnormalInfo.AbnormalMonitors.Service;
using SIE.Defects.InspectionItems;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.AbnormalInfo.AbnormalMonitors.AbnomalRule
{
    /// <summary>
    /// 异常来源非重复验证
    /// </summary>
    [System.ComponentModel.DisplayName("异常来源非重复验证")]
    [System.ComponentModel.Description("异常来源非重复验证")]
    public class ItemStandardDetailNameNotDupRule : EntityRule<AbnormalSource>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ItemStandardDetailNameNotDupRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var item = entity as AbnormalSource;
            var source = RT.Service.Resolve<AbnormalSourceService>().GetAbnormalSource(item.MonitorName,item.AbnormalEntityMetadataId);
            if (source != null)
            {
                e.BrokenDescription = "已经存在[监控名称]是{0}、[功能模块]是{1}的[异常来源]".L10nFormat(source.MonitorName, source.MetadataName);
            }
        }

    }
}
