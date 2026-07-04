using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.Resources.ProcessSegments;
using SIE.Resources.ProcessTechTypes;
using System;
using System.ComponentModel;

namespace SIE.Resources.ProcessTechs
{
    /// <summary>
    /// 制程工艺验证规则
    /// </summary>
    [DisplayName("制程工艺验证规则")]
    [Description("是否排产为否时，必须维护偏移时间")]
    public class ProcessTechIsSchedulingRule : EntityRule<ProcessTech>
    {
        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">制程工艺</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var processTech = entity as ProcessTech;
            if (!processTech.IsScheduling && processTech.OffsetTime == null)
            {
                e.BrokenDescription = "请维护偏移时间！".L10N();
            }
        }
    }

    /// <summary>
    /// 制程工艺类型被生产资源引用
    /// </summary>
    [DisplayName("制程工艺类型被生产资源引用")]
    [Description("制程工艺类型被生产资源引用不允许删除")]
    class NotProcessTechTypeRule : NoReferencedRule<ProcessTechType>
    {
        /// <summary>
        /// 日历方案关联计划资源
        /// </summary>
        public NotProcessTechTypeRule()
        {
            Properties.Add(ProcessTech.ProcessTechTypeIdProperty);
            Scope = EntityStatusScopes.Delete;
            MessageBuilder = (o, e) =>
            {
                var entity = o as ProcessTechType;
                return "制程工艺类型[{0}]被制程工艺引用,不能删除\n".L10nFormat(entity.Name);
            };
        }
    }

    /// <summary>
    /// 被制程工艺引用的工段，不允许删除
    /// </summary>
    [DisplayName("被制程工艺引用的工段，不允许删除")]
    [Description("被制程工艺引用的工段，不允许删除")]
    public class IsReferencedByProcessTechRule : NoReferencedRule<ProcessSegment>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public IsReferencedByProcessTechRule()
        {
            Properties.Add(ProcessTech.ProcessSegmentIdProperty);
            MessageBuilder = (o, e) =>
            {
                var ps = o as ProcessSegment;
                return "工段[{0}]被制程工艺引用，不能删除".L10nFormat(ps.Code);
            };
        }
    }

    /// <summary>
    /// 制程工艺类型被生产资源引用不允许修改算法类型
    /// </summary>
    [System.ComponentModel.DisplayName("制程工艺类型被生产资源引用不允许修改算法类型")]
    [System.ComponentModel.Description("制程工艺类型被生产资源引用不允许修改算法类型")]
    class ModifyProcessTechTypeRule : EntityRule<ProcessTechType>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ModifyProcessTechTypeRule()
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
            if (RT.Service.Resolve<ProcessTechBaseController>().IsExistsProcessTechType(proTechType.Id))
            {
                var oldProTechType = RF.GetById<ProcessTechType>(proTechType.Id);
                if (oldProTechType.AlgorithmMarking != proTechType.AlgorithmMarking)
                {
                    e.BrokenDescription = "制程工艺类型[{0}]已经被制程工艺引用，不允许修改算法类型\n".L10nFormat(proTechType.Name);
                }
            }
        }
    }


    #region 同一个制程类型只能对应一个工段
    /// <summary>
    /// 同一个制程类型只能对应一个工段
    /// </summary>
    [DisplayName("同一个制程类型只能对应一个工段")]
    [Description("同一个制程类型只能对应一个工段")]
    class ProcessTechTypeProcessSegmentRule : EntityRule<ProcessTech>
    {
        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">制程工艺</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            //var processTech = entity as ProcessTech;
            //var oldprocessTech = RT.Service.Resolve<ProcessTechBaseController>().GetProcessTechTypeId(processTech.ProcessTechTypeId);
            //if (oldprocessTech != null && oldprocessTech.ProcessSegmentId!=null && oldprocessTech.ProcessSegmentId != processTech.ProcessSegmentId)
            //{
            //   e.BrokenDescription = "同一个[{0}]制程类型只能对应一个工段".L10nFormat(processTech.ProcessTechType.Name);
            //}
            
        }
    }
    #endregion
}
