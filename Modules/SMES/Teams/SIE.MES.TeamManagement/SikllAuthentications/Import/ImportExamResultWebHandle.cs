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
    /// 导入考试结果 处理逻辑类
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportExamResultWebHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportExamResultWebHandle : IDisposable, IBusinessImport
    {
        /// <summary>
        /// 导入列名列表
        /// </summary>
        private List<string> columnNameList = new List<string> { "工号*", "姓名", "性别", "员工状态", "考试得分(分)*", "考试时间*", "考试结果*" };

        /// <summary>
        /// 考试结果"工号"-"员工Id"
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
                { "考试得分(分)*", new ValidColumn(ImportDataType._String, true, ValidScore) },
                { "考试时间*", new ValidColumn(ImportDataType._String, true, ValidExamTime) },
                { "考试结果*", new ValidColumn(ImportDataType._String, true, ValidExamResult) }
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
            var skillAuthIdstr = drs[0][ImportDataHandle.ParentId].ToString().Trim();
            if (string.IsNullOrEmpty(skillAuthIdstr)) return;
            var skillAuthId = double.Parse(skillAuthIdstr);

            foreach (var mainDataItem in drs)
            {
                var employeeCode = mainDataItem.Field<string>(ColIndex("工号*"));
                var employeeName = mainDataItem.Field<string>(ColIndex("姓名"));
                var employeeSex = mainDataItem.Field<string>(ColIndex("性别"));
                var employeeStatus = mainDataItem.Field<string>(ColIndex("员工状态"));
                var examScore = mainDataItem.Field<string>(ColIndex("考试得分(分)*"));
                var examTime = mainDataItem.Field<string>(ColIndex("考试时间*"));
                var examResult = mainDataItem.Field<string>(ColIndex("考试结果*"));
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

                decimal score = 0;
                if (!decimal.TryParse(examScore, out score) || score < 0)
                {
                    mainDataItem[ImportDataHandle.MessageColumnName] = "考试得分必须是非负数".L10N();
                    continue;
                }

                using (var tran = DB.TransactionScope(TeamManagementDataProvider.ConnectionStringName))
                {
                    //如果不能新增记录错误信息
                    try
                    {
                        var examRes = new ExamResult();
                        examRes.EmployeeId = employeeId;
                        examRes.ExamTime = Convert.ToDateTime(examTime);
                        examRes.Score = score;
                        examRes.Result = GetExamResult(examResult);
                        examRes.SkillAuthId = skillAuthId;
                        RF.Save(examRes);
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
        /// 根据字符串值返回考试结果枚举
        /// </summary>
        /// <param name="val">字符串</param>
        /// <returns>考试结果枚举</returns>
        private ExamRequired GetExamResult(string val)
        {
            if (val == "优秀")
            {
                return ExamRequired.Excellent;
            }

            if (val == "及格")
            {
                return ExamRequired.Pass;
            }

            if (val == "不及格")
            {
                return ExamRequired.Fail;
            }

            return ExamRequired.NoMatter;
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
                isValid = false;
                return isValid;
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
        /// 验证考试得分
        /// </summary>
        /// <param name="obj">考试得分</param>
        /// <param name="messageTip">提示</param>
        /// <param name="row">数据行</param>
        /// <returns>验证结果</returns>
        private bool ValidScore(object obj, out string messageTip, DataRow row)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (obj == null)
            {
                messageTip = "为空值".L10N();
                isValid = false;
                return isValid;
            }

            decimal score = 0;
            if (!decimal.TryParse(obj.ToString(), out score) || score < 0)
            {
                messageTip = "必须是非负数".L10N();
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// 验证考试时间
        /// </summary>
        /// <param name="obj">考试时间</param>
        /// <param name="messageTip">提示</param>
        /// <param name="row">数据行</param>
        /// <returns>验证结果</returns>
        private bool ValidExamTime(object obj, out string messageTip, DataRow row)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (obj == null)
            {
                messageTip = "为空值".L10N();
                isValid = false;
                return isValid;
            }

            try
            {
                var examTime = Convert.ToDateTime(obj);
                if (examTime > DateTime.Now)
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
        /// 验证考试结果
        /// </summary>
        /// <param name="obj">考试结果</param>
        /// <param name="messageTip">提示</param>
        /// <param name="row">数据行</param>
        /// <returns>返回考试结果</returns>
        private bool ValidExamResult(object obj, out string messageTip, DataRow row)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (obj == null)
            {
                messageTip = "为空值".L10N();
                isValid = false;
                return isValid;
            }

            var examResult = obj.ToString();
            if (!"优秀".Equals(examResult) && !"及格".Equals(examResult) && !"不及格".Equals(examResult))
            {
                messageTip = "只能填入：优秀、及格、不及格".L10N();
                isValid = false;
            }

            return isValid;
        }
        #endregion
    }
}