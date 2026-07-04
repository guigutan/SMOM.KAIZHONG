using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Resources.CalendarSchemes;
using SIE.Resources.Employees;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.MES.TeamManagement.ShiftSchedules
{
    /// <summary>
    /// 排班导入
    /// </summary>
    [Services.Service(FallbackType = typeof(ScheduleImportHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ScheduleImportHandle : IDisposable, IBusinessImport
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ScheduleImportHandle()
        {
            for (int i = 1; i <= 31; i++)
            {
                columnNameList.Add(i + "号");
            }
        }

        /// <summary>
        /// 列名集合
        /// </summary>
        List<string> columnNameList = new List<string>() { "班组", "资源", "年_月" };

        /// <summary>
        /// 导入模板的列头名
        /// </summary>
        public List<string> ColumnNameList
        {
            get
            {
                return columnNameList;
            }

            set
            {
                columnNameList = value;
            }
        }

        /// <summary>
        /// 列验证字典
        /// </summary>
        public Dictionary<string, ValidColumn> ColumnValidList { get; set; }

        /// <summary>
        /// 创建验证列
        /// </summary>
        /// <returns>业务导入接口</returns>
        public IBusinessImport CreaetColumnValid()
        {
            this.ColumnValidList = new Dictionary<string, ValidColumn>
            {
                { "班组", new ValidColumn(ImportDataType._String, false, ValidateWorkGroup) },
                { "资源", new ValidColumn(ImportDataType._String, false, ValidateWipResource) },
            };

            return this;
        }

        /// <summary>
        /// 班组字典，key:班组名称 value:班组ID
        /// </summary>
        private readonly Dictionary<string, double> _workGroupDic = new Dictionary<string, double>();

        /// <summary>
        /// 资源字典 key:资源名称 value:资源
        /// </summary>
        private readonly Dictionary<string, WipResource> _wipResourceDic = new Dictionary<string, WipResource>();

        /// <summary>
        /// 车间字典 key:资源ID value:车间ID
        /// </summary>
        private readonly Dictionary<double, double> _workShopDic = new Dictionary<double, double>();

        /// <summary>
        /// 导入日期字典 key:列 value:日期
        /// </summary>
        private readonly Dictionary<int, DateTime> _importDate = new Dictionary<int, DateTime>();

        /// <summary>
        /// 验证资源
        /// </summary>
        /// <param name="obj">资源名称</param>
        /// <param name="messageTip">验证结果</param>
        /// <param name="row">数据行</param>
        /// <returns>验证通过返回true，失败返回false</returns>
        private bool ValidateWipResource(object obj, out string messageTip, DataRow row)
        {
            string value = obj.ToString();
            messageTip = string.Empty;
            if (ValidateIsNull(ref messageTip, value, "资源"))
            {
                return false;
            }
            if (!_wipResourceDic.ContainsKey(value))
            {
                var wipResource = RT.Service.Resolve<WipResourceController>().GetWipResourceByName(value);
                if (wipResource != null)
                {
                    _wipResourceDic.Add(value, wipResource);
                }
                else
                {
                    messageTip = "[{0}]不存在".L10nFormat(value);
                }
            }

            return _wipResourceDic.ContainsKey(value);
        }

        /// <summary>
        /// 验证班组
        /// </summary>
        /// <param name="obj">班组名称</param>
        /// <param name="messageTip">验证结果</param>
        /// <param name="row">数据行</param>
        /// <returns>验证通过返回true，失败返回false</returns>
        private bool ValidateWorkGroup(object obj, out string messageTip, DataRow row)
        {
            string value = obj.ToString();
            messageTip = string.Empty;
            if (ValidateIsNull(ref messageTip, value, "班组"))
                return false;
            if (!_workGroupDic.ContainsKey(value))
            {
                var workGroup = RT.Service.Resolve<EmployeeController>().GetWorkGroup(value);
                if (workGroup != null)
                {
                    _workGroupDic.Add(value, workGroup.Id);
                }
                else
                {
                    messageTip = "[{0}]不存在".L10nFormat(value);
                }
            }

            return _workGroupDic.ContainsKey(value);
        }

        /// <summary>
        /// 验证数据是否为空
        /// </summary>
        /// <param name="str">错误信息</param>
        /// <param name="value">值</param>
        /// <param name="colunmName">字段名称</param>
        /// <returns>是空返回true，否则返回false</returns>
        private bool ValidateIsNull(ref string str, string value, string colunmName)
        {
            if (!value.IsNullOrEmpty())
            {
                return false;
            }
            str = "{0}不能为空".L10nFormat(colunmName);
            return true;
        }

        /// <summary>
        /// 获取班组ID
        /// </summary>
        /// <param name="key">班组名称</param>
        /// <returns>班组ID</returns>
        private double GetWorkGroupId(string key)
        {
            if (_workGroupDic.ContainsKey(key))
                return _workGroupDic[key];
            return 0;
        }

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <param name="key">资源名称</param>
        /// <returns>资源</returns>
        private WipResource GetWipReourceId(string key)
        {
            if (_wipResourceDic.ContainsKey(key))
            {
                return _wipResourceDic[key];
            }
            return null;
        }

        /// <summary>
        /// 处理业务数据
        /// </summary>
        /// <param name="drs">需要处理的数据集</param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            if (drs == null || !drs.Any())
            {
                return;
            }
            var table = drs[0].Table;
            DataRow[] allRows = table.AsEnumerable().ToArray();
            for (int i = 0; i < allRows.Length; i++)
            {
                DateTime dbnow = RF.Find<ShiftType>().GetDbTime().Date;
                DataRow row = allRows[i];
                var workGroup = row.Field<string>(0);
                var wipResource = row.Field<string>(1);
                var yearMonth = row.Field<string>(2);
                var wipRes = GetWipReourceId(wipResource);
                ShiftSchedule shiftSchedule = new ShiftSchedule()
                {
                    WorkGroupId = GetWorkGroupId(workGroup),
                    WipResourceId = wipRes == null ? 0 : wipRes.Id
                };
                if (wipRes == null)
                {
                    SetRowError(row, "第{0}行资源【{1}】不存在".L10nFormat(i + 1, wipResource));
                    continue;
                }

                if (!wipRes.WorkShopId.HasValue)
                {
                    SetRowError(row, "第{0}行资源【{1}】没有所属车间".L10nFormat(i + 1, wipRes.Name));
                    continue;
                }

                shiftSchedule.WorkShopId = wipRes.WorkShopId.Value;
                string[] arr = yearMonth.Split('_');
                int year = 0;
                int month = 0;
                if (arr.Count() != 2 || !int.TryParse(arr[0], out year) || !int.TryParse(arr[1], out month))
                {
                    SetRowError(row, "第{0}行【年_月】格式错误，正确格式2019_1".L10nFormat(i + 1));
                    continue;
                }

                int columnCount = 0;
                DateTime curMonth = DateTime.Parse(year + "-" + month + "-01");
                var curMonthLastDay = curMonth.AddMonths(1).AddDays(-1);
                columnCount = curMonthLastDay.Day;
                if (curMonthLastDay <= dbnow)
                {
                    continue;
                }
                var shiftTypeList = RT.Service.Resolve<CalendarSchemeController>().GetShiftTypesByCalendarSchemeAndDataRange(wipRes.Scheme, curMonth, curMonthLastDay);

                for (int c = 3; c < (columnCount + 3); c++)
                {
                    try
                    {
                        shiftSchedule.ScheduleDate = curMonth.AddDays(c - 3);
                        ////只执行大于今天的数据
                        if (shiftSchedule.ScheduleDate <= dbnow)
                        {
                            continue;
                        }
                        shiftTypeList.TryGetValue(shiftSchedule.ScheduleDate, out ShiftType shiftType);
                        if (shiftType == null)
                        {
                            //SetRowError(row, "{0}产线未排班".L10nFormat(shiftSchedule.ScheduleDate.ToString("d")));
                            continue;
                        }

                        double shiftTypeId = 0;
                        double shiftId = 0;
                        var strShift = row.Field<string>(c);
                        if (!strShift.IsNullOrEmpty())
                        {
                            var shift = shiftType.ShiftList.FirstOrDefault(p => p.Name == strShift);
                            if (shift == null)
                            {
                                SetRowError(row, "{0}未找到班次{1}".L10nFormat(shiftSchedule.ScheduleDate.ToString("d"), strShift));
                                continue;
                            }

                            shiftTypeId = shift.ShiftTypeId;
                            shiftId = shift.Id;
                        }

                        shiftSchedule.ShiftTypeId = shiftTypeId;
                        shiftSchedule.ShiftId = shiftId;
                        RT.Service.Resolve<ShiftScheduleController>().SaveShiftSchedule(shiftSchedule);
                    }
                    catch (Exception exc)
                    {
                        SetRowError(row, exc);
                    }
                }
            }
        }

        /// <summary>
        /// 设置错误信息
        /// </summary>
        /// <param name="row">数据行</param>
        /// <param name="error">错误信息</param>
        private void SetRowError(DataRow row, string error)
        {
            row[ImportDataHandle.MessageColumnName] += error;
        }

        /// <summary>
        /// 设置错误信息
        /// </summary>
        /// <param name="row">数据行</param> 
        /// <param name="exc">异常信息</param>
        private void SetRowError(DataRow row, Exception exc)
        {
            var baseExc = exc.GetBaseException();
            if (baseExc is ValidationException)
            {
                row[ImportDataHandle.MessageColumnName] += (baseExc as ValidationException).Message;
            }
            else
            {
                row[ImportDataHandle.MessageColumnName] += exc.Message;
            }
        }

        /// <summary>
        /// 释放数据
        /// </summary>
        public void Dispose()
        {
            _workGroupDic.Clear();
            _wipResourceDic.Clear();
            _workShopDic.Clear();
            _importDate.Clear();
        }
    }
}