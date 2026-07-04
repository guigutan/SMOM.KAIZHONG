using SIE.Common.ImportHelper;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SIE.EMS.MainenanceProjects
{
    /// <summary>
    /// 点检保养项目维护导入
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportProjectDetailHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportProjectDetailHandle : IBusinessImport
    {
        /// <summary>
        /// 导入模板的列头名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
            "项目名称*",
            "项目类型*",
            "周期类型",
            "部位",
            "项目耗材",
            "操作方法",
            "标准",
            "最小值",
            "最大值",
            "单位",
            "用时(分钟)",
        };

        /// <summary>
        /// 验证列
        /// </summary>
        public Dictionary<string, ValidColumn> ColumnValidList { get; set ; }

        /// <summary>
        /// 创建导入对象
        /// </summary>
        public IBusinessImport CreaetColumnValid()
        {
            this.ColumnValidList = new Dictionary<string, ValidColumn>();
            this.ColumnValidList.Add("项目名称*", new ValidColumn(ImportDataType._String, true, false));
            this.ColumnValidList.Add("项目类型*", new ValidColumn(ImportDataType._String, true, false));
            this.ColumnValidList.Add("最小值", new ValidColumn(ImportDataType._Double, false, false));
            this.ColumnValidList.Add("最大值", new ValidColumn(ImportDataType._Double, false, false));
            this.ColumnValidList.Add("用时(分钟)", new ValidColumn(ImportDataType._Double, false, false));
            return this;
        }

        /// <summary>
        /// 获取指定列名的索引
        /// </summary>
        /// <param name="columnName">指定列名</param>
        /// <returns>返回对应索引</returns>
        protected virtual int ColIndex(string columnName)
        {
            return ColumnNameList.IndexOf(columnName);
        }

        /// <summary>
        /// 项目类型转化
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private ProjectType? SwitchType(string type)
        {
            switch(type)
            {
                case "点检":
                    return ProjectType.Check;
                case "保养":
                    return ProjectType.Maintain;
                case "润滑":
                    return ProjectType.Lubrication;
                case "设备定检":
                    return ProjectType.PeriodicalInsp;
                case "计量校验":
                    return ProjectType.Verify;
                case "计划维修":
                    return ProjectType.PlanRepair;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 周期类型转化
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private CycleType? SwitchCycleType(string type)
        {
            switch(type)
            {
                case "班":
                    return CycleType.Class;
                case "日":
                    return CycleType.Day;
                case "周":
                    return CycleType.Week;
                case "双周":
                    return CycleType.DoubleWeek;
                case "月":
                    return CycleType.Month;
                case "双月":
                    return CycleType.DoubleMonth;
                case "季":
                    return CycleType.Season;
                case "半年":
                    return CycleType.HalfYear;
                case "年":
                    return CycleType.Year;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 验证项目类型与周期类型
        /// </summary>
        /// <param name="pType"></param>
        /// <param name="cType"></param>
        /// <returns></returns>
        private bool ValidateCycleType(ProjectType? pType, CycleType? cType)
        {
            if (pType == ProjectType.Check)
            {
                return (int)cType <= (int)CycleType.Day;
            }
            else if (pType == ProjectType.Maintain)
            {
                return (int)cType >= (int)CycleType.Week;
            }
            else if (pType == ProjectType.Lubrication)
            {
                return cType == CycleType.Day;
            }
            else if (pType == ProjectType.PeriodicalInsp)
            {
                return cType == null;
            }
            else if (pType == ProjectType.Verify)
            {
                return cType == CycleType.Week;
            }
            else if (pType == ProjectType.PlanRepair)
            {
                return cType == null;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 数据导入处理
        /// </summary>
        /// <param name="drs"></param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            var importDataList = from g in drs
                                 select new
                                 {
                                     Name = g.Field<string>(ColIndex("项目名称*")).Trim(),
                                     ProjectType = g.Field<string>(ColIndex("项目类型*")).Trim(),
                                     CycleTypeInfo = g.Field<string>(ColIndex("周期类型")).Trim(),
                                     Part = g.Field<string>(ColIndex("部位")).Trim(),
                                     Consumable = g.Field<string>(ColIndex("项目耗材")).Trim(),
                                     Method = g.Field<string>(ColIndex("操作方法")).Trim(),
                                     Standard = g.Field<string>(ColIndex("标准")).Trim(),
                                     MinValue = g.Field<string>(ColIndex("最小值")).Trim(),
                                     MaxValue = g.Field<string>(ColIndex("最大值")).Trim(),
                                     Unit = g.Field<string>(ColIndex("单位")).Trim(),
                                     UseTime = g.Field<string>(ColIndex("用时(分钟)")).Trim(),
                                     DetailInfo = g,
                                 };

            // 项目名称
            var names = new List<string>();
            importDataList.ForEach(data =>
            {
                names.Add(data.Name);
            });

            // 数据库名称
            var dbNameList = RT.Service.Resolve<ProjectDetailController>().GetProjectDetails(names);

            var importDataRows = importDataList.ToList();

            // 保存列表
            EntityList<ProjectDetail> projectDetails = new EntityList<ProjectDetail>();
            for (int i = 0; i < importDataRows.Count; i++)
            {
                var data = importDataRows[i];
                StringBuilder errMsg = new StringBuilder();
                if (SwitchType(data.ProjectType) == null)
                {
                    errMsg.Append("项目类型不属于点检、保养、润滑、设备定检、计量校验、计划维修;".L10N());
                }
                if (data.CycleTypeInfo.IsNotEmpty() && SwitchCycleType(data.CycleTypeInfo) == null)
                {
                    errMsg.Append("周期类型不属于班、日、周、双周、月、双月、季、半年、年;".L10N());

                }
                // 项目名称+项目类型唯一
                if (importDataRows.Count(p => p.Name == data.Name && p.ProjectType == data.ProjectType) > 1 || dbNameList.Count(p => p.Name == data.Name && p.ProjectType == SwitchType(data.ProjectType)) > 0)
                {
                    errMsg.Append("项目名称{0} + 项目类型{1}唯一;".L10nFormat(data.Name, data.ProjectType));
                }

                var useTime = int.TryParse(data.UseTime, out var usetime);
                if (data.UseTime.IsNotEmpty())
                {
                    if (useTime && usetime <= 0)
                    {
                        errMsg.Append("用时(分钟)必须为正数;".L10N());
                    }
                }

                if (!ValidateCycleType(SwitchType(data.ProjectType).Value, SwitchCycleType(data.CycleTypeInfo).Value))
                {
                    errMsg.Append("项目类型与周期类型不对应;".L10N());
                }

                var maxValue = decimal.TryParse(data.MaxValue, out decimal max);
                var minValue = decimal.TryParse(data.MinValue, out decimal min);
                if (data.MaxValue.IsNotEmpty())
                {
                    if (maxValue && max < 0)
                    {
                        errMsg.Append("最大值不能小于0;".L10N());
                    }
                }
                if (data.MinValue.IsNotEmpty())
                {
                    if (minValue && min < 0)
                    {
                        errMsg.Append("最小值不能小于0;".L10N());
                    }

                }
                if (data.MaxValue.IsNotEmpty() && data.MinValue.IsNotEmpty() && maxValue && minValue && max < min)
                {
                    errMsg.Append("最小值不能大于最大值;".L10N());
                }

                if (errMsg.ToString().Length <= 0)
                {
                    ProjectDetail projectDetail = new ProjectDetail
                    {
                        Name = data.Name,
                        Part = data.Part,
                        Consumable = data.Consumable,
                        Method = data.Method,
                        Standard = data.Standard,
                        MinValue = data.MinValue.IsNotEmpty() ? min : null,
                        MaxValue = data.MaxValue.IsNotEmpty() ? max : null,
                        Unit = data.Unit,
                        ProjectType = SwitchType(data.ProjectType).Value,
                        CycleType = SwitchCycleType(data.CycleTypeInfo).Value,
                        UseTime = data.UseTime.IsNotEmpty() ? usetime : null,
                    };
                    projectDetails.Add(projectDetail);
                }
                else
                {
                    importDataRows[i].DetailInfo[ImportDataHandle.MessageColumnName] = errMsg.ToString();
                }
            }

            RF.BatchInsert(projectDetails);
        }
    }
}
