using SIE.Domain.Validation;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using System;
using System.ComponentModel;

namespace SIE.MES.TeamManagement.ShiftSchedules
{
    /// <summary>
    /// 班组排班重复验证规则
    /// </summary>
    [DisplayName("班组排班重复验证规则")]
    [Description("同一资源、班组、日期不能重复排班")]
    public class ScheduleNotDuplicateRule : NotDuplicateRule<ShiftSchedule>
    {
        /// <summary>
        /// 验证规则
        /// </summary>
        public ScheduleNotDuplicateRule()
        {
            Properties.Add(ShiftSchedule.WorkGroupIdProperty);
            Properties.Add(ShiftSchedule.ScheduleDateProperty);
            MessageBuilder = e =>
            {
                var schedule = e as ShiftSchedule;
                return "不能重复排班，[{0}]在[{1}]已排班".L10nFormat(schedule.WorkGroup?.Name, schedule.ScheduleDate.ToString("d"));
            };
        }
    }

    /// <summary>
    /// 班组被排班引用验证规则
    /// </summary>
    [DisplayName("班组被排班引用验证规则")]
    [Description("班组被排班引用不能删除")]
    public class WorkGroupNotRefScheduleRule : NoReferencedRule<WorkGroup>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkGroupNotRefScheduleRule()
        {
            Properties.Add(ShiftSchedule.WorkGroupIdProperty);
            MessageBuilder = (e, c) =>
            {
                return "班组[{0}]被排班引用[{1}]次不能删除".L10nFormat((e as WorkGroup)?.Name, c);
            };
        }
    }

    /// <summary>
    /// 车间被排班引用验证规则
    /// </summary>
    [DisplayName("车间被排班引用验证规则")]
    [Description("车间被排班引用不能删除")]
    public class WorkShopNotRefScheduleRule : NoReferencedRule<Enterprise>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkShopNotRefScheduleRule()
        {
            Properties.Add(ShiftSchedule.WorkShopIdProperty);
            MessageBuilder = (e, c) =>
            {
                return "车间[{0}]被排班引用[{1}]次不能删除".L10nFormat((e as Enterprise)?.Name, c);
            };
        }
    }

    /// <summary>
    /// 资源被排班引用验证规则
    /// </summary>
    [DisplayName("资源被排班引用验证规则")]
    [Description("资源被排班引用不能删除")]
    public class WipResourceNotRefScheduleRule : NoReferencedRule<WipResource>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WipResourceNotRefScheduleRule()
        {
            Properties.Add(ShiftSchedule.WipResourceIdProperty);
            MessageBuilder = (e, c) =>
            {
                return "资源[{0}]被排班引用[{1}]次不能删除".L10nFormat((e as WipResource)?.Name, c);
            };
        }
    }

    /// <summary>
    /// 班制被排班引用验证规则
    /// </summary>
    [DisplayName("班制被排班引用验证规则")]
    [Description("班制被排班引用不能删除")]
    public class ShiftTypeNotRefScheduleRule : NoReferencedRule<ShiftType>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ShiftTypeNotRefScheduleRule()
        {
            Properties.Add(ShiftSchedule.ShiftTypeIdProperty);
            MessageBuilder = (e, c) =>
            {
                return "班制[{0}]被排班引用[{1}]次不能删除".L10nFormat((e as ShiftType)?.Name, c);
            };
        }
    }

    /// <summary>
    /// 班次被排班引用验证规则
    /// </summary>
    [DisplayName("班次被排班引用验证规则")]
    [Description("班次被排班引用不能删除")]
    public class ShiftNotRefScheduleRule : NoReferencedRule<Shift>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ShiftNotRefScheduleRule()
        {
            Properties.Add(ShiftSchedule.ShiftIdProperty);
            MessageBuilder = (e, c) =>
            {
                return "班次[{0}]被排班引用[{1}]次不能删除".L10nFormat((e as Shift)?.Name, c);
            };
        }
    }
}