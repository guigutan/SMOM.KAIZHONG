using DocumentFormat.OpenXml.Bibliography;
using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.EMS.MainenanceProjects;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EMS.Equipments.Models.ImportEquipModel
{
    /// <summary>
    /// 设备型号点检保养项目维护导入
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportEquipModelProjectHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportEquipModelProjectHandle : IBusinessImport
    {
        /// <summary>
        /// 导入模板列头
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
            "设备型号编码*",
            "项目类型*",
            "项目名称*",
            "周期类型*",
            "责任部门",
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
        public Dictionary<string, ValidColumn> ColumnValidList { get; set; }

        /// <summary>
        /// 验证
        /// </summary>
        /// <returns></returns>
        public IBusinessImport CreaetColumnValid()
        {
            this.ColumnValidList = new Dictionary<string, ValidColumn>();
            this.ColumnValidList.Add("设备型号编码*", new ValidColumn(ImportDataType._String, true, false));
            this.ColumnValidList.Add("项目名称*", new ValidColumn(ImportDataType._String, true, false));
            this.ColumnValidList.Add("项目类型*", new ValidColumn(ImportDataType._String, true, false));
            this.ColumnValidList.Add("周期类型*", new ValidColumn(ImportDataType._String, true, false));
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
            switch (type)
            {
                case "点检":
                    return ProjectType.Check;
                case "保养":
                    return ProjectType.Maintain;
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
            switch (type)
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
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="drs"></param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            var importDataList = from g in drs
                                 select new
                                 {
                                     EquipModelCode = g.Field<string>(ColIndex("设备型号编码*")).Trim(),
                                     Name = g.Field<string>(ColIndex("项目名称*")).Trim(),
                                     ProjectType = g.Field<string>(ColIndex("项目类型*")).Trim(),
                                     CycleTypeInfo = g.Field<string>(ColIndex("周期类型*")).Trim(),
                                     Dept = g.Field<string>(ColIndex("责任部门")).Trim(),
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

            // 设备型号编码
            var equipModelCodes = new List<string>();
            // 项目名称
            var projectNames = new List<string>();
            // 部门名称
            var deptNames = new List<string>();
            importDataList.ForEach(p =>
            {
                equipModelCodes.Add(p.EquipModelCode);
                projectNames.Add(p.Name);
                deptNames.Add(p.Dept);
            });

            // 设备型号
            var equipModelBaseInfos = RT.Service.Resolve<EquipController>().GetEquipModelBaseInfos(equipModelCodes);
            var equipModelIds = equipModelBaseInfos.Select(p => p.Id).ToList();

            // 项目
            var projectDetails = RT.Service.Resolve<ProjectDetailController>().GetProjectDetails(projectNames);

            // 部门
            var deptBaseInfos = RT.Service.Resolve<EnterpriseController>().GetEnterprisesByNames(deptNames, EnterpriseType.Department);

            // 设备型号下的点检项目
            var equipCheckProjects = RT.Service.Resolve<EquipController>().GetEquipModelCheckProjects(equipModelIds);
            // 设备型号下的保养项目
            var equipMaintainProjects = RT.Service.Resolve<EquipController>().GetEquipModelMaintainProjects(equipModelIds);

            // 导入保存点检项目
            EntityList<EquipModelCheckProject> checks = new EntityList<EquipModelCheckProject>();
            // 导入保存保养项目
            EntityList<EquipModelMaintainProject> maintains = new EntityList<EquipModelMaintainProject>();


            var importDataRows = importDataList.ToList();

            for (int i = 0; i < importDataRows.Count; i++)
            {
                StringBuilder errMsg = new StringBuilder();

                var data = importDataRows[i];
                var equipModel = equipModelBaseInfos.FirstOrDefault(p => p.Code == data.EquipModelCode);
                var project = projectDetails.FirstOrDefault(p => p.Name == data.Name);
                if (equipModel == null)
                {
                    errMsg.Append("设备型号{0}不存在;".L10nFormat(data.EquipModelCode));
                }
                if (project == null)
                {
                    errMsg.Append("项目{0}不存在;".L10nFormat(data.Name));
                }
                if (SwitchType(data.ProjectType) == null)
                {
                    errMsg.Append("项目类型不属于点检、保养;".L10N());
                }
                if (data.CycleTypeInfo.IsNotEmpty() && SwitchCycleType(data.CycleTypeInfo) == null)
                {
                    errMsg.Append("周期类型不属于班、日、周、双周、月、双月、季、半年、年;".L10N());

                }
                // 设备型号+项目名称+项目类型唯一
                if (importDataRows.Count(p => p.EquipModelCode == data.EquipModelCode && p.Name == data.Name && p.ProjectType == data.ProjectType) > 1 
                    || equipCheckProjects.Count(p => p.EquipModelId == equipModel.Id && p.ProjectDetailName == data.Name && p.ProjectType == SwitchType(data.ProjectType)) > 0
                    || equipMaintainProjects.Count(p => p.EquipModelId == equipModel.Id && p.ProjectName == data.Name && p.ProjectType == SwitchType(data.ProjectType)) > 0)
                {
                    errMsg.Append("设备型号{0} + 项目名称{1} + 项目类型{2}唯一;".L10nFormat(data.EquipModelCode, data.Name, data.ProjectType));
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

                var dept = deptBaseInfos.FirstOrDefault(p => p.Name == data.Dept);
                if (data.Dept.IsNotEmpty() && dept == null)
                {
                    errMsg.Append("当前库存组织不存在[{0}]部门;".L10nFormat(data.Dept));

                }

                if (errMsg.ToString().Length <= 0)
                {
                    if (SwitchType(data.ProjectType).Value == ProjectType.Check)
                    {
                        EquipModelCheckProject check = new EquipModelCheckProject
                        {
                            EquipModelId = equipModel.Id,
                            ProjectDetailId = project.Id,
                            DepartmentId = data.Dept.IsNotEmpty() ? dept.Id : null,
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
                        checks.Add(check);
                    }
                    else
                    {
                        EquipModelMaintainProject maintain = new EquipModelMaintainProject
                        {
                            EquipModelId = equipModel.Id,
                            ProjectDetailId = project.Id,
                            DepartmentId = data.Dept.IsNotEmpty() ? dept.Id : null,
                            Part = data.Part,
                            Consumable = data.Consumable,
                            Method = data.Method,
                            Standard = data.Standard,
                            MinValue = decimal.Parse(data.MinValue),
                            MaxValue = decimal.Parse(data.MaxValue),
                            Unit = data.Unit,
                            ProjectType = SwitchType(data.ProjectType).Value,
                            CycleType = SwitchCycleType(data.CycleTypeInfo).Value,
                            UseTime = data.UseTime.IsNotEmpty() ? usetime : null,
                        };
                        maintains.Add(maintain);
                    }
                }
                else
                {
                    importDataRows[i].DetailInfo[ImportDataHandle.MessageColumnName] = errMsg.ToString();
                }
            }
            using(var tran = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.BatchInsert(checks);
                RF.BatchInsert(maintains);
                tran.Complete();
            }
        }
    }
}
