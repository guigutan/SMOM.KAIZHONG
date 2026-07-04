using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.Resources.CalendarSchemes;
using System;
using System.ComponentModel;
using System.Linq;

namespace SIE.Resources.ShiftTypes
{
    /*
     * 1、当前班制中班次名称不能重复；
     * 2、当前班制中班次编码不能重复；
     * 3、缺省班制不能删除；
     * 4、缺省班制下必须存在一个班次；
     * 5、休息日班制下不能存在任何班次
     * 6、一个班制下的所有班次时间段组合最多24小时且班次开始时间必须小于结束时间；
     * 7、同一个班制下的班次时间段不重复验证；
     * 8、班制被日历方案的周方案引用，不允许删除
     * 9、当前班次中休息类型不能重复；
     * 10、班次休息时间段不重复验证；
     * 11、修改班次，班次休息时间必须包含在班次时间范围内；
     * 12、班次被日历方案的周方案引用，不允许删除；
     * 13、班次休息被日历方案的周方案引用，不允许删除；
     */

    #region 班制
    /// <summary>
    /// 班次名称不重复验证
    /// </summary>
    [DisplayName("班次验证规则")]
    [Description("当前班制中班次名称不能重复")]
    class ShiftNameNotDuplicateRule : NotDuplicateRule<Shift>
    {
        /// <summary>
        /// 不重复规则
        /// </summary>
        public ShiftNameNotDuplicateRule()
        {
            Properties.Add(Shift.ShiftTypeIdProperty);
            Properties.Add(Shift.NameProperty);
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            this.MessageBuilder = e =>
            {
                var r = e as Shift;
                return "当前班制中已经存在[名称]是{0}的班次".L10nFormat(r.Name);
            };
        }
    }

    /// <summary>
    /// 班次编码不重复验证
    /// </summary>
    [DisplayName("班次验证规则")]
    [Description("当前班制中班次编码不能重复")]
    class ShiftCodeNotDuplicateRule : NotDuplicateRule<Shift>
    {
        /// <summary>
        /// 不重复规则
        /// </summary>
        public ShiftCodeNotDuplicateRule()
        {
            Properties.Add(Shift.ShiftTypeIdProperty);
            Properties.Add(Shift.CodeProperty);
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            this.MessageBuilder = e =>
            {
                var r = e as Shift;
                return "当前班制中已经存在[编码]是{0}的班次".L10nFormat(r.Code);
            };
        }
    }

    /// <summary>
    /// 缺省班制不允许删除
    /// </summary>
    [DisplayName("班制验证规则")]
    [Description("缺省班制不允许删除")]
    public class ShiftTypeNotDelete : EntityRule<ShiftType>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ShiftTypeNotDelete()
        {
            Scope = EntityStatusScopes.Delete;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">班制</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var shiftType = entity as ShiftType;
            if (shiftType.IsDefault == YesNo.Yes)
                e.BrokenDescription = "缺省班制【{0}】不允许删除！".L10nFormat(shiftType.Name);
        }
    }

    /// <summary>
    /// 缺省班制下必须存在一个班次
    /// </summary>
    [DisplayName("缺省班制验证规则")]
    [Description("缺省班制下必须存在一个班次")]
    public class DefaultShiftTypeRule : EntityRule<ShiftType>
    {
        /// <summary>
        /// 班制删除验证规则
        /// </summary>
        public DefaultShiftTypeRule()
        {
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">班制实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var shiftType = entity as ShiftType;

            if (shiftType.IsDefault == YesNo.Yes && !shiftType.ShiftList.Any())
            {
                var model = RF.GetById<ShiftType>(shiftType.Id);
                if (model != null && model.ShiftList.Count <= shiftType.ShiftList.DeletedList.Count)
                {
                    e.BrokenDescription = "缺省班制【{0}】下至少有一个班次!".L10nFormat(shiftType.Name);
                }
            }
        }
    }

    /// <summary>
    /// 班制的班次时间段重复验证
    /// </summary>
    [DisplayName("班制验证规则")]
    [Description("班制的班次时间段重复验证")]
    public class ValidateShiftOfShiftTypeInsideTime : EntityRule<ShiftType>
    {
        /// <summary>
        /// 班制时间段重复验证
        /// </summary>
        public ValidateShiftOfShiftTypeInsideTime()
        {
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 班次时间段重复验证
        /// </summary>
        /// <param name="entity">班次</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var shiftType = entity as ShiftType;
            EntityList<Shift> shifts = null;
            //获取当前界面最新的班次列表----------web只传了修改过的数据
            var oldShiftType = RF.GetById<ShiftType>(shiftType.Id);
            if (oldShiftType != null)
            {
                shifts = oldShiftType.ShiftList;
                foreach (Shift deleteEntity in shiftType.ShiftList.DeletedList.OfType<Shift>())
                {
                    var a = shifts.FirstOrDefault(p => p.Id == deleteEntity.Id);
                    if (a != null)
                    {
                        shifts.Remove(a);
                    }
                }

                foreach (var shif in shiftType.ShiftList)
                {
                    var a = shifts.FirstOrDefault(p => p.Id == shif.Id);
                    if (a != null)
                    {
                        shifts.Remove(a);
                        shifts.Add(shif);
                    }
                    else
                    {
                        shifts.Add(shif);
                    }
                }
            }
            else
            {
                shifts = shiftType.ShiftList;
            }
            foreach (var shift in shifts)
            {
                int shiftBeginTime = int.Parse(shift.BeginTime.ToString("HHmm"));
                int shiftEndTime = int.Parse(shift.EndTime.ToString("HHmm"));

                int maxtime = 999999;
                if (shift.IsOverDay || shiftBeginTime > shiftEndTime)
                {
                    shiftEndTime += 2400;
                }

                var controller = RT.Service.Resolve<ShiftTypeController>();
                bool isInside = false;

                //循环判断新增休息时间段与当前班次下的其他休息时间段是否重叠。如有，则新增失败
                foreach (var item in shifts)
                {
                    if (item.Id != shift.Id)
                    {
                        int itemBegintime = int.Parse(item.BeginTime.ToString("HHmm"));

                        controller.GetTime(item.BeginTime, item.EndTime, shift.BeginTime,
                            out int outshiftrestbegintime, out int outshiftrestendtime);

                        if (shiftBeginTime >= outshiftrestbegintime && shiftBeginTime < outshiftrestendtime)
                        {
                            isInside = true;
                            break;
                        }
                        else if (shiftEndTime > outshiftrestbegintime && shiftEndTime < outshiftrestendtime)
                        {
                            isInside = true;
                            break;
                        }
                        else if (shiftBeginTime <= outshiftrestbegintime && shiftEndTime >= outshiftrestendtime)
                        {
                            isInside = true;
                            break;
                        }

                        if (itemBegintime < maxtime)
                        {
                            maxtime = itemBegintime;
                        }
                    }
                }

                if (shift.IsOverDay && shiftEndTime - 2400 > maxtime) ////跨日
                {
                    e.BrokenDescription = "一个班制最多24小时，请更正".L10N();
                }

                if (isInside)
                {
                    e.BrokenDescription = "时间段与已有时间段有重叠，请更正".L10N();
                }
            }
        }
    }

    /// <summary>
    /// 班制验证规则，班制被日历方案的周方案引用，不允许删除
    /// </summary>
    [DisplayName("班制验证规则")]
    [Description("班制被日历方案的周方案引用，不允许删除")]
    public class ShiftTypeReferenceByCalWeekRule : EntityRule<ShiftType>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ShiftTypeReferenceByCalWeekRule()
        {
            Scope = EntityStatusScopes.Delete;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">班制</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var shiftType = entity as ShiftType;
            if (RT.Service.Resolve<CalendarSchemeController>().GetCalendarSchemeWeeksByShiftTypeId(shiftType.Id).Count > 0)
                e.BrokenDescription = ("班制[{0}]被日历方案的周方案引用，不允许删除").L10nFormat(shiftType.Name);
        }
    }
    #endregion

    #region 班次 
    /// <summary>
    /// 班次休息类型不重复验证
    /// </summary>
    [DisplayName("班次休息验证规则")]
    [Description("当前班次中休息类型不能重复")]
    class ShiftRestNotDuplicateRule : NotDuplicateRule<ShiftRest>
    {
        /// <summary>
        /// 不重复规则
        /// </summary>
        public ShiftRestNotDuplicateRule()
        {
            Properties.Add(ShiftRest.ShiftIdProperty);
            Properties.Add(ShiftRest.TypeProperty);
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            this.MessageBuilder = e =>
            {
                var r = e as ShiftRest;
                return "当前班次中班次休息类型[{0}]不能重复".L10nFormat(r.Type);
            };
        }
    }


    /// <summary>
    /// 验证班次休息时间不在改动后时间段内
    /// </summary>
    [DisplayName("班次验证规则")]
    [Description("验证班次休息时间不在改动后时间段内")]
    public class ValidateShiftRestTime : EntityRule<Shift>
    {
        /// <summary>
        /// 班次修改验证规则
        /// 验证班次休息时间不在改动后时间段内
        /// </summary>
        public ValidateShiftRestTime()
        {
            ConnectToDataSource = true;
            Scope = EntityStatusScopes.Update;
        }

        /// <summary>
        /// 班次休息时间必须包含在班次时间范围内
        /// </summary>
        /// <param name="entity">班次</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var shift = entity as Shift;
            EntityList<ShiftRest> shiftRests;
            //获取当前界面最新的班次休息列表----------web只传了修改过的数据
            var oldShift = RF.GetById<Shift>(shift.Id);
            if (oldShift != null)
            {
                shiftRests = oldShift.ShiftRestList;
                foreach (ShiftRest deleteEntity in shift.ShiftRestList.DeletedList.OfType<ShiftRest>())
                {
                    var a = shiftRests.FirstOrDefault(p => p.Id == deleteEntity.Id);
                    if (a != null)
                        shiftRests.Remove(a);
                }
                foreach (var item in shift.ShiftRestList)
                {
                    var a = shiftRests.FirstOrDefault(p => p.Id == item.Id);
                    if (a != null)
                    {
                        shiftRests.Remove(a);
                        shiftRests.Add(item);
                    }
                    else
                    {
                        shiftRests.Add(item);
                    }
                }
            }
            else
            {
                shiftRests = shift.ShiftRestList;
            }
            var controller = RT.Service.Resolve<ShiftTypeController>();

            int maxTime = 0;
            int minTime = 999999;
            int shiftrestbegintime = int.Parse(shift.BeginTime.ToString("HHmm"));
            int shiftrestendtime = int.Parse(shift.EndTime.ToString("HHmm"));

            if (shift.IsOverDay || (shiftrestbegintime > shiftrestendtime)) //是否跨日
            {
                shiftrestendtime += 2400;
            }
            ////判断休息时间段与当前班次下的其他休息时间段是否有重叠，如有，则修改失败
            foreach (var shiftRest in shiftRests)
            {
                controller.GetTime(shiftRest.BeginTime, shiftRest.EndTime, shift.BeginTime, out int outshiftrestbegintime, out int outshiftrestendtime);
                if (outshiftrestendtime > maxTime)
                {
                    maxTime = outshiftrestendtime;
                }

                if (outshiftrestbegintime < minTime)
                {
                    minTime = outshiftrestbegintime;
                }
            }

            if (shiftrestbegintime > minTime || shiftrestendtime < maxTime)
            {
                e.BrokenDescription = "在作息时间栏发现有时间段不在修改后时间段内，请先维护作息时间".L10N();
            }
        }
    }

    /// <summary>
    /// 班次开始结束时间验证
    /// </summary>
    [DisplayName("班次验证规则")]
    [Description("班次开始结束时间验证")]
    public class CheckShiftTime : EntityRule<Shift>
    {
        /// <summary>
        /// 一个班制下的所有班次时间段组合最多24小时
        /// 班次开始时间必须小于结束时间
        /// </summary>
        /// <param name="entity">班次</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var shift = entity as Shift;
            var beginTime = int.Parse(shift.BeginTime.ToString("HHmm"));
            var endTime = int.Parse(shift.EndTime.ToString("HHmm"));
            ////一个班制最多24小时，同一个班制多个班次时间段组合最多24小时
            if (shift.IsOverDay && beginTime <= endTime) //跨日
            {
                e.BrokenDescription = "一个班制最多24小时，请更正".L10N();
            }
            else if (!shift.IsOverDay && beginTime >= endTime)
            {
                e.BrokenDescription = "开始时间必须小于结束时间".L10N();
            }
        }
    }

    /// <summary>
    /// 班次验证规则，班制被日历方案的周方案引用，不允许删除
    /// </summary>
    [DisplayName("班次验证规则")]
    [Description("班次被日历方案的周方案引用，不允许删除")]
    public class ShiftReferenceByCalWeekRule : EntityRule<Shift>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ShiftReferenceByCalWeekRule()
        {
            Scope = EntityStatusScopes.Delete;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">班次</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var shift = entity as Shift;
            if (RT.Service.Resolve<CalendarSchemeController>().GetCalendarSchemeWeeksByShiftTypeId(shift.ShiftTypeId).Count > 0)
                e.BrokenDescription = ("班次[{0}]被日历方案的周方案引用，不允许删除").L10nFormat(shift.Name);
        }
    }

    /// <summary>
    /// 班次的班次休息时间段不重复验证
    /// </summary>
    [DisplayName("班次验证规则")]
    [Description("班次的班次休息时间段不重复验证")]
    public class CheckShiftRestOfShiftInsideTime : EntityRule<Shift>
    {
        /// <summary>
        /// 班次休息验证规则
        /// </summary>
        public CheckShiftRestOfShiftInsideTime()
        {
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 验证班次休息
        /// </summary>
        /// <param name="entity">班次休息</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var shift = entity as Shift;
            EntityList<ShiftRest> shiftrests = shift.ShiftRestList;
            //获取当前界面最新的班次休息列表----------web只传了修改过的数据
            var oldShift = RF.GetById<Shift>(shift.Id);
            if (oldShift != null)
            {
                shiftrests = oldShift.ShiftRestList;
                foreach (ShiftRest deleteEntity in shift.ShiftRestList.DeletedList.OfType<ShiftRest>())
                {
                    var a = shiftrests.FirstOrDefault(p => p.Id == deleteEntity.Id);
                    if (a != null)
                        shiftrests.Remove(a);
                }
                foreach (var item in shift.ShiftRestList)
                {
                    var a = shiftrests.FirstOrDefault(p => p.Id == item.Id);
                    if (a != null)
                    {
                        shiftrests.Remove(a);
                    }
                    shiftrests.Add(item);
                }
            }

            foreach (var shiftRest in shiftrests)
            {
                int shiftrestbegintime = int.Parse(shiftRest.BeginTime.ToString("HHmm")); //休息开始时间
                int shiftrestendtime = int.Parse(shiftRest.EndTime.ToString("HHmm")); //休息结束时间
                int shiftbegintime = int.Parse(shift.BeginTime.ToString("HHmm")); //班次开始时间
                int shiftendtime = int.Parse(shift.EndTime.ToString("HHmm")); //班次结束时间
                var controller = RT.Service.Resolve<ShiftTypeController>();
                if (shift.IsOverDay) //跨日
                {
                    shiftendtime += 2400;
                }

                if (shiftrestbegintime > shiftrestendtime)
                {
                    shiftrestendtime += 2400;
                }

                if (shiftbegintime >= shiftrestendtime && shiftbegintime > shiftrestbegintime)
                {
                    shiftrestendtime += 2400;
                    shiftrestbegintime += 2400;
                }

                if (shiftrestbegintime == shiftrestendtime)
                {
                    e.BrokenDescription = "开始时间不能与结束时间相同".L10N();
                }
                if (!shift.IsOverDay && shiftrestbegintime >= shiftrestendtime)
                {
                    e.BrokenDescription = "开始时间必须小于结束时间".L10N();
                }
                else if (shiftrestbegintime < shiftbegintime || shiftrestendtime > shiftendtime)
                {
                    e.BrokenDescription = "新增休息时间段必须在当前班次时间段内。".L10N();
                }
                else if (shiftrestbegintime == shiftbegintime && shiftrestendtime == shiftendtime)
                {
                    e.BrokenDescription = "新增休息时间段不能与当前班次时间段完全一致。".L10N();
                }
                else
                {
                    bool isInside = CheckShiftRests(shift, shiftrests, shiftRest, shiftrestbegintime, shiftrestendtime, controller);

                    if (isInside)
                    {
                        e.BrokenDescription = "新增休息时间段与已有休息时间段有重叠，请更正".L10N();
                    }
                }
            }
        }

        /// <summary>
        /// 判断新增休息时间段
        /// </summary>
        /// <param name="shift"></param>
        /// <param name="shiftrests"></param>
        /// <param name="shiftRest"></param>
        /// <param name="shiftrestbegintime"></param>
        /// <param name="shiftrestendtime"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        private static bool CheckShiftRests(Shift shift, EntityList<ShiftRest> shiftrests, ShiftRest shiftRest, int shiftrestbegintime, int shiftrestendtime, ShiftTypeController controller)
        {
            bool isInside = false;
            ////循环判断新增休息时间段与当前班次下的其它休息时间段是否有重叠，如有，则新增失败
            foreach (var item in shiftrests)
            {
                if (item.Id == shiftRest.Id)
                {
                    continue;
                }
                controller.GetTime(item.BeginTime, item.EndTime, shift.BeginTime, out int outshiftrestbegintime, out int outshiftrestendtime);
                if (shiftrestbegintime >= outshiftrestbegintime && shiftrestbegintime < outshiftrestendtime)
                {
                    isInside = true;
                    break; ////新增休息的开始时间在其中一个时间范围内
                }
                if (shiftrestendtime > outshiftrestbegintime && shiftrestendtime < outshiftrestendtime)
                {
                    isInside = true;
                    break; ////新增休息的结束时间在其中一个时间范围内
                }
                if (shiftrestbegintime <= outshiftrestbegintime && shiftrestendtime >= outshiftrestendtime)
                {
                    isInside = true;
                    break; ////新增休息的时间段包含其中一个休息时间段
                }
            }

            return isInside;
        }
    }

    #endregion

    #region 班次休息   
    /// <summary>
    /// 班次休息验证规则，班次休息被日历方案的周方案引用，不允许删除
    /// </summary>
    [DisplayName("班次休息验证规则")]
    [Description("班次休息被日历方案的周方案引用，不允许删除")]
    public class ShiftRestReferenceByCalWeekRule : EntityRule<ShiftRest>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ShiftRestReferenceByCalWeekRule()
        {
            Scope = EntityStatusScopes.Delete;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">班次</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var shiftRest = entity as ShiftRest;
            if (RT.Service.Resolve<CalendarSchemeController>().GetCalendarSchemeWeeksByShiftTypeId(shiftRest.Shift.ShiftTypeId).Count > 0)
                e.BrokenDescription = "休息类型[{0}]被日历方案的周方案引用，不允许删除".L10nFormat(shiftRest.Type);
        }
    }
    #endregion
}
