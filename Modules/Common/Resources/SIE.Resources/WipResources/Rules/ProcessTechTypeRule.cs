using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.Resources.ProcessTechTypes;
using System;
using System.ComponentModel;

namespace SIE.Resources.WipResources.Rules
{
    /// <summary>
    /// 制程工艺类型被生产资源引用
    /// </summary>
    [DisplayName("制程工艺类型被生产资源引用")]
    [Description("制程工艺类型被生产资源引用不允许删除")]
    class ProcessTechTypeRule : NoReferencedRule<ProcessTechType>
    {
        /// <summary>
        /// 日历方案关联计划资源
        /// </summary>
        public ProcessTechTypeRule()
        {
            Properties.Add(WipResource.ProcessTechTypeIdProperty);
            Scope = EntityStatusScopes.Delete;
            MessageBuilder = (o, e) =>
            {
                var entity = o as ProcessTechType;
                return "制程工艺类型[{0}]被生产资源引用,不能删除.".L10nFormat(entity.Name);
            };
        }
    }

    /// <summary>
    /// 制程工艺类型被生产资源引用不允许修改算法类型
    /// </summary>
    [System.ComponentModel.DisplayName("制程工艺类型被生产资源引用不允许修改算法类型")]
    [System.ComponentModel.Description("制程工艺类型被生产资源引用不允许修改算法类型")]
    class UpdateProcessTechTypeRule : EntityRule<ProcessTechType>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UpdateProcessTechTypeRule()
        {
            Scope = EntityStatusScopes.Update;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var proTechType = entity as ProcessTechType;
            if (RT.Service.Resolve<WipResourceController>().IsExistsProcessTechType(proTechType.Id))
            {
                var oldProTechType = RF.GetById<ProcessTechType>(proTechType.Id);
                if (oldProTechType.AlgorithmMarking != proTechType.AlgorithmMarking)
                {
                    e.BrokenDescription = "制程工艺类型[{0}]已经被生产资源引用，不允许修改算法类型".L10nFormat(proTechType.Name);
                }
            }
        }
    }
}
