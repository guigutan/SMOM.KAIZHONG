using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.Resources.WipResources;
using System;
using System.ComponentModel;

namespace SIE.Equipments.Abnormal
{
    /// <summary>
    /// 停线时间验证规则
    /// </summary>
    [DisplayName("停线时间验证")]
    [Description("停线时间验证")]
    public class AbnormalCauseDateRules : PropertyRule<AbnormalCause>
    {
        /// <summary>
        /// 托管属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return AbnormalCause.BeginDateProperty;
            }
        }

        /// <summary>
        /// 验证逻辑
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var vl = entity as AbnormalCause;
            if (vl.BeginDate > DateTime.Now)
            {
                e.BrokenDescription = "停线发生时间不能大于当前时间".L10N();
            }

            if (vl.EndDate.HasValue)
            {
                if (vl.EndDate > DateTime.Now)
                {
                    e.BrokenDescription = "停线结束时间不能大于当前时间".L10N();
                }

                if (vl.EndDate < vl.BeginDate)
                {
                    e.BrokenDescription = "停线结束时间不能小于发生时间".L10N();
                }
            }
        }
    }

    
    ///// <summary>
    ///// 异常停线非重验证规则（先注释掉，后面再按照具体情况做决定）
    ///// </summary>
    //[DisplayName("异常停线非重验证规则")]
    //[Description("同一产线停线时间范围不能重叠")]
    //public class AbnormalCauseNotDuplicateRule : EntityRule<AbnormalCause>
    //{
    //    /// <summary>
    //    /// 设置规则作用条件
    //    /// </summary>
    //    public AbnormalCauseNotDuplicateRule()
    //    {
    //        ConnectToDataSource = true;
    //        Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
    //    }

    //    /// <summary>
    //    /// 验证规则
    //    /// </summary>
    //    /// <param name="entity">异常停线</param>
    //    /// <param name="e">参数 </param>
    //    protected override void Validate(IEntity entity, RuleArgs e)
    //    {
    //        var abnormalCause = entity as AbnormalCause;
    //        DateTime? beginDate = abnormalCause.BeginDate;
    //        DateTime? endDate = abnormalCause.EndDate;
    //        var controller = AppRuntime.Service.Resolve<AbnormalCauseController>();
    //        var abnormalCauses = controller.GetAbnormalCauseDate(abnormalCause);
    //        bool isInside = false;

    //        //循环判断新增或修改的异常停线时间是否在与已有的异常停线时间范围重叠
    //        foreach (var cause in abnormalCauses)
    //        {
    //            if (cause.Id != abnormalCause.Id)
    //            {
    //                DateTime? outBeginDate = cause.BeginDate;
    //                DateTime? outEndDate = cause.EndDate;

    //                if (outEndDate == null && (endDate == null || outBeginDate <= endDate))
    //                {
    //                    isInside = true;
    //                    break; //新增异常停线开始时间在其中一个异常停线时间范围内
    //                }
    //                else if (endDate == null && outEndDate >= beginDate)
    //                {
    //                    isInside = true;
    //                    break; //新增异常停线开始时间在其中一个异常停线时间范围内
    //                }
    //                else if (outBeginDate <= beginDate && outEndDate >= beginDate)
    //                {
    //                    isInside = true;
    //                    break; //新增异常停线开始时间在其中一个异常停线时间范围内
    //                }
    //                else if (outBeginDate <= endDate && outEndDate >= endDate)
    //                {
    //                    isInside = true;
    //                    break; //新增异常停线开始时间在其中一个异常停线时间范围内
    //                }
    //                else if (outBeginDate >= beginDate && outEndDate <= endDate)
    //                {
    //                    isInside = true;
    //                    break; //新增异常停线开始时间在其中一个异常停线时间范围内
    //                }
    //            }
    //        }

    //        if (isInside)
    //        {
    //            e.BrokenDescription = "同一产线停线时间范围不能重叠".L10N();
    //        }
    //    }
    //}

    /// <summary>
    /// 停线管理必填项验证规则
    /// </summary>
    [DisplayName("停线管理必填项验证规则")]
    [Description("停线管理的产线和设备必须填一项")]
    public class AbnormalCauseRequiredRules : EntityRule<AbnormalCause>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AbnormalCauseRequiredRules()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 验证逻辑
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var vl = entity as AbnormalCause;
            if (vl.EquipAccountId == null && vl.ResourceId == null && vl.ProcessId == null)
            {
                e.BrokenDescription = "请选择需要停线的设备或者产线或者工步。".L10N();
            }
        }
    }

    /// <summary>
    /// 生产资源删除验证规则
    /// </summary>
    [DisplayName("生产资源删除验证规则")]
    [Description("异常停线引用的生产资源不能删除")]
    public class WipResourceDeleteRuleAbnormalCause : EntityRule<WipResource>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WipResourceDeleteRuleAbnormalCause()
        {
            Scope = EntityStatusScopes.Delete;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 根据生产资源是否被异常停线引用，判断是否能被删除
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var wipResource = entity as WipResource;
            var flag = AppRuntime.Service.Resolve<AbnormalCauseController>().AbnormalCauseHasUsedWipResource(wipResource.Id);
            if (flag)
            {
                e.BrokenDescription = "生产资源 [{0}] 被异常停线引用, 不能删除!".L10nFormat(wipResource.Code);
            }
        }
    }
}
