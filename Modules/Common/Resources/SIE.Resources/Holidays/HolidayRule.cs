using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.Resources.Holidays
{
    /// <summary>
    /// 法定假期验证规则
    /// </summary>
    [DisplayName("法定假期验证规则")]
    [Description("结束日期必须大于开始日期")]
    public class HolidayRule : EntityRule<Holiday>
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public HolidayRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }
        /// <summary>
        /// 法定假期验证规则
        /// </summary>
        /// <param name="entity">法定假期</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var holiday = entity as Holiday;
            if (holiday != null && holiday.EndDate < holiday.BeginDate)
            {
                e.BrokenDescription = "结束日期必须大于开始日期".L10N();
            }
        }
    }

    /// <summary>
    /// 法定假期验证规则
    /// </summary>
    [DisplayName("法定假期验证规则")]
    [Description("开始日期和结束日期都必须大于今天")]
    public class HolidayMoreThanTodayRule : EntityRule<Holiday>
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public HolidayMoreThanTodayRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }
        /// <summary>
        /// 法定假期验证规则
        /// </summary>
        /// <param name="entity">法定假期</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var holiday = entity as Holiday;
            var oldholiday = RF.GetById<Holiday>(holiday.Id);
            if (oldholiday != null && holiday.Remark != oldholiday.Remark)
            {
                return;
            }
            if (holiday.BeginDate < DateTime.Today)
            {
                e.BrokenDescription = "开始日期不能小于今天".L10N();
            }
            if (holiday.EndDate < DateTime.Today)
            {
                e.BrokenDescription = "结束日期不能小于今天".L10N();
            }
            if (holiday.BeginDate < DateTime.Today && holiday.EndDate < DateTime.Today)
            {
                e.BrokenDescription = "开始日期和结束日期都不能小于今天".L10N();
            }
        }
    }

    /// <summary>
    /// 法定假期验证规则
    /// </summary>
    [DisplayName("法定假期验证规则")]
    [Description("法定假期时间段不能重合")]
    public class HolidayNotDuplicateRule : EntityRule<Holiday>
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public HolidayNotDuplicateRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 法定假期时间段不能重合
        /// </summary>
        /// <param name="entity">法定假期</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var holiday = entity as Holiday;
            var holidayList = RT.Service.Resolve<HolidayController>().GetHoliday();
            if (holidayList.Count < 1 || e == null)
            {
                return;
            }

            bool isInside = false;
            foreach (var item in holidayList)
            {
                if (item.Id != holiday.Id)
                {
                    if (holiday.BeginDate >= item.BeginDate && holiday.BeginDate < item.EndDate)
                    {
                        isInside = true;
                        break;
                    }
                    if (holiday.EndDate >= item.BeginDate && holiday.EndDate < item.EndDate)
                    {
                        isInside = true;
                        break;
                    }
                    if (holiday.BeginDate <= item.BeginDate && holiday.EndDate >= item.EndDate)
                    {
                        isInside = true;
                        break;
                    }
                }
            }

            if (isInside)
            {
                e.BrokenDescription = "法定假期时间段不能重合，请更正".L10N();
            }
        }
    }

    /// <summary>
    /// 法定假期验证规则
    /// </summary>
    [DisplayName("法定假期验证规则")]
    [Description("今天及以前的日期不能删除")]
    public class BeforeHolidayDeleteRule : EntityRule<Holiday>
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public BeforeHolidayDeleteRule()
        {
            Scope = EntityStatusScopes.Delete;
        }
        /// <summary>
        /// 法定假期验证规则
        /// </summary>
        /// <param name="entity">法定假期</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var holiday = entity as Holiday;
            if (holiday != null && (holiday.BeginDate <= DateTime.Today || holiday.EndDate <= DateTime.Today))
            {
                e.BrokenDescription = "今天及以前的日期不能删除".L10N();
            }
        }
    }
}
