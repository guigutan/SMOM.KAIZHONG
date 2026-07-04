using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.ExperienceDepots
{
    /// <summary>
    /// 维修经验库规则
    /// </summary>
    [System.ComponentModel.DisplayName("维修经验库验证规则")]
    [System.ComponentModel.Description("维修经验库验证规则")]
    public class ExperienceDepotRule : EntityRule<ExperienceDepot>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ExperienceDepotRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var experienceDepot = entity as ExperienceDepot;
            if (experienceDepot.RepairType == Enums.ExpRepairType.Account && experienceDepot.EquipAccountId == null)
                e.BrokenDescription += "维修类型为设备维修时，设备编码必须填写!".L10N() + "</br>";

            if (experienceDepot.RepairType == Enums.ExpRepairType.SparePart && experienceDepot.SparePartId == null)
                e.BrokenDescription += "维修类型为备件维修时，备件类型必须填写!".L10N() + "</br>";

            //if (!experienceDepot.FaultPhenomenonId.HasValue && experienceDepot.FaultPhenomenonRemark.IsNullOrEmpty())
            //    e.BrokenDescription += "故障现象或故障现象备注必填其中一项!</br>".L10N();

            if (!experienceDepot.FaultDescribeId.HasValue && experienceDepot.FaultDescribeRemark.IsNullOrEmpty())
                e.BrokenDescription += "故障描述或故障描述备注必填其中一项!".L10N()+"</br>";
        }
    }
}