using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.Resources.Employees;
using SIE.Resources.Skills;
using System;
using System.Collections.Generic;
using System.Data;

namespace SIE.MES.TeamManagement.SikllAuthentications.Import
{
    /// <summary>
    /// 导入培训记录 处理逻辑类
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportTrainingRecordWebHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportTrainingRecordWebHandle : IDisposable, IBusinessImport
    {
        /// <summary>
        /// 导入列名列表
        /// </summary>
        private List<string> columnNameList = new List<string> { "工号*", "姓名", "性别", "员工状态", "培训开始时间*", "培训结束时间*", "培训时长(h)", "培训结果*" };

        /// <summary>
        /// 培训记录"工号"-"员工Id"
        /// </summary>
        private Dictionary<string, double> employeeCodeDic = new Dictionary<string, double>();

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
        /// 列的标准验证 (列名 列对应验证 )
        /// </summary>
        public Dictionary<string, ValidColumn> ColumnValidList { get; set; }

        /// <summary>
        /// 创建列
        /// </summary>
        /// <returns>IBusinessImport</returns>
        public IBusinessImport CreaetColumnValid()
        {
            this.ColumnValidList = new Dictionary<string, ValidColumn>
            {
                { "工号*", new ValidColumn(ImportDataType._String, true, ValidEmployeeNo) },
                { "姓名", new ValidColumn(ImportDataType._String, false, 80) },
                { "性别", new ValidColumn(ImportDataType._String, false, 80) },
                { "员工状态", new ValidColumn(ImportDataType._String, false, 80) },
                { "培训开始时间*", new ValidColumn(ImportDataType._String, true, ValidBeginDate) },
                { "培训结束时间*", new ValidColumn(ImportDataType._String, true, ValidEndDate) },
                { "培训时长(h)", new ValidColumn(ImportDataType._String, false, 80) },
                { "培训结果*", new ValidColumn(ImportDataType._String, true, ValidTrainingResult) }
            };

            return this;
        }

        /// <summary>
        /// 释放数据
        /// </summary>
        public void Dispose()
        {
            if (employeeCodeDic != null)
            {
                employeeCodeDic.Clear();
                employeeCodeDic = null;
            }
        }

        /// <summary>
        /// 处理业务数据
        /// </summary>
        /// <param name="drs">需要处理的数据集</param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            if (drs.Length == 0) return;
            var ctl = RT.Service.Resolve<SkillAuthController>();
            var skillAuthIdstr = drs[0][ImportDataHandle.ParentId].ToString().Trim();
            if (string.IsNullOrEmpty(skillAuthIdstr)) return;
            var skillAuthId = double.Parse(skillAuthIdstr);

            foreach (var mainDataItem in drs)
            {
                var employeeCode = mainDataItem.Field<string>(ColIndex("工号*"));
                var employeeName = mainDataItem.Field<string>(ColIndex("姓名"));
                var employeeSex = mainDataItem.Field<string>(ColIndex("性别"));
                var employeeStatus = mainDataItem.Field<string>(ColIndex("员工状态"));
                var beginTime = mainDataItem.Field<string>(ColIndex("培训开始时间*"));
                var endTime = mainDataItem.Field<string>(ColIndex("培训结束时间*"));
                var duration = mainDataItem.Field<string>(ColIndex("培训时长(h)"));
                var trainingResult = mainDataItem.Field<string>(ColIndex("培训结果*"));
                double employeeId = GetEmployeeId(employeeCode);
                var employee = RT.Service.Resolve<EmployeeController>().GetEmployeeByCode(employeeCode);
                if (employeeName.IsNotEmpty())
                {
                    if (employee.Name != employeeName)
                    {
                        mainDataItem[ImportDataHandle.MessageColumnName] = "工号和姓名不一致".L10N();
                        continue;
                    }
                }

                if (employeeSex.IsNotEmpty())
                {
                    if (employee.Sex.ToLabel() != employeeSex)
                    {
                        mainDataItem[ImportDataHandle.MessageColumnName] = "工号和性别不一致".L10N();
                        continue;
                    }
                }

                if (employeeStatus.IsNotEmpty())
                {
                    if (employee.EmployeeStatus.ToLabel() != employeeStatus)
                    {
                        mainDataItem[ImportDataHandle.MessageColumnName] = "工号和员工状态不一致".L10N();
                        continue;
                    }
                }

                if (Convert.ToDateTime(endTime) < Convert.ToDateTime(beginTime))
                {
                    mainDataItem[ImportDataHandle.MessageColumnName] = "培训开始时间不能晚于培训结束时间".L10N();
                    continue;
                }

                if (duration.IsNotEmpty())
                {
                    decimal durationVal = 0;
                    if (!decimal.TryParse(duration, out durationVal) || durationVal < 0)
                    {
                        mainDataItem[ImportDataHandle.MessageColumnName] = "培训时长必须是非负数".L10N();
                        continue;
                    }

                    var sysDiff = ctl.GetHourDiff(Convert.ToDateTime(endTime), Convert.ToDateTime(beginTime));
                    if (durationVal != sysDiff)
                    {
                        mainDataItem[ImportDataHandle.MessageColumnName] = "培训时长应该是{0}".L10nFormat(sysDiff);
                        continue;
                    }
                }

                using (var tran = DB.TransactionScope(TeamManagementDataProvider.ConnectionStringName))
                {
                    //如果不能新增记录错误信息
                    try
                    {
                        var trainingRes = new TrainingRecord();
                        trainingRes.EmployeeId = employeeId;
                        trainingRes.BeginDate = Convert.ToDateTime(beginTime);
                        trainingRes.EndDate = Convert.ToDateTime(endTime);
                        trainingRes.Result = GetTrainingResult(trainingResult);
                        trainingRes.SkillAuthId = skillAuthId;
                        RF.Save(trainingRes);
                    }
                    catch (Exception exc)
                    {
                        string strMsg = AppRuntime.Location.ConnectDataDirectly ? exc.Message : exc.InnerException.Message;
                        mainDataItem[ImportDataHandle.MessageColumnName] = mainDataItem[ImportDataHandle.MessageColumnName] + strMsg;
                        continue;
                    }

                    tran.Complete();
                }
            }
        }

        #region 引用类型的属性从字典中取值
        /// <summary>
        /// 根据员工工号取值
        /// </summary>
        /// <param name="key">工号</param>
        /// <returns>员工Id</returns>
        private double GetEmployeeId(string key)
        {
            if (employeeCodeDic.ContainsKey(key))
                return employeeCodeDic[key];
            return 0;
        }
        #endregion

        /// <summary>
        /// 获取指定列名的索引
        /// </summary>
        /// <param name="columnName">指定列名</param>
        /// <returns>返回对应索引</returns>
        private int ColIndex(string columnName)
        {
            return columnNameList.IndexOf(columnName);
        }

        /// <summary>
        /// 根据字符串值返回培训结果枚举
        /// </summary>
        /// <param name="val">字符串</param>
        /// <returns>培训结果枚举</returns>
        private TrainingRequired GetTrainingResult(string val)
        {
            if (val == "完成")
            {
                return TrainingRequired.Finish;
            }

            if (val == "未完成")
            {
                return TrainingRequired.UnFinish;
            }

            return TrainingRequired.NoMatter;
        }

        #region 基础验证
        /// <summary>
        /// 验证工号
        /// </summary>
        /// <param name="obj">内容</param>
        /// <param name="messageTip">提示</param>
        /// <param name="row">数据行</param>
        /// <returns>验证结果</returns>
        private bool ValidEmployeeNo(object obj, out string messageTip, DataRow row)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (obj == null)
            {
                messageTip = "为空值".L10N();
                return false;
            }

            if (employeeCodeDic == null)
            {
                employeeCodeDic = new Dictionary<string, double>();
            }

            if (!employeeCodeDic.ContainsKey(obj.ToString()))
            {
                Employee employee = RT.Service.Resolve<EmployeeController>().GetEmployeeByCode(obj.ToString());
                if (employee != null)
                {
                    employeeCodeDic.Add(obj.ToString(), employee.Id);
                }
                else
                {
                    messageTip = "{0}不存在于系统".L10nFormat(obj.ToString());
                    isValid = false;
                }
            }

            return isValid;
        }

        /// <summary>
        /// 验证培训开始时间
        /// </summary>
        /// <param name="obj">培训开始时间</param>
        /// <param name="messageTip">提示</param>
        /// <param name="row">数据行</param>
        /// <returns>验证结果</returns>
        private bool ValidBeginDate(object obj, out string messageTip, DataRow row)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (obj == null)
            {
                messageTip = "为空值".L10N();
                return false;
            }

            try
            {
                var beginTime = Convert.ToDateTime(obj);
                if (beginTime > DateTime.Now)
                {
                    messageTip = "不能晚于当前时间".L10N();
                    isValid = false;
                }
            }
            catch (Exception)
            {
                messageTip = "{0}内容无效".L10nFormat(obj.ToString());
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// 验证培训结束时间
        /// </summary>
        /// <param name="obj">培训开始时间</param>
        /// <param name="messageTip">提示</param>
        /// <param name="row">数据行</param>
        /// <returns>验证结果</returns>
        private bool ValidEndDate(object obj, out string messageTip, DataRow row)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (obj == null)
            {
                messageTip = "为空值".L10N();
                return false;
            }

            try
            {
                var endTime = Convert.ToDateTime(obj);
                if (endTime > DateTime.Now)
                {
                    messageTip = "不能晚于当前时间".L10N();
                    isValid = false;
                }
            }
            catch (Exception)
            {
                messageTip = "{0}内容无效".L10nFormat(obj.ToString());
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// 验证培训结果
        /// </summary>
        /// <param name="obj">培训结果</param>
        /// <param name="messageTip">提示</param>
        /// <param name="row">数据行</param>
        /// <returns>返回培训结果</returns>
        private bool ValidTrainingResult(object obj, out string messageTip, DataRow row)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (obj == null)
            {
                messageTip = "为空值".L10N();
                return false;
            }

            var trainingResult = obj.ToString();
            if (!"完成".Equals(trainingResult) && !"未完成".Equals(trainingResult))
            {
                messageTip = "只能填入：完成、未完成".L10N();
                isValid = false;
            }

            return isValid;
        }
        #endregion
    }
}