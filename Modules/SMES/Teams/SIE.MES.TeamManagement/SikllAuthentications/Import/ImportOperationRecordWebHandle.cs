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
    /// 导入实操记录 处理逻辑类
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportOperationRecordWebHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportOperationRecordWebHandle : IDisposable, IBusinessImport
    {
        /// <summary>
        /// 导入列名列表
        /// </summary>
        private List<string> columnNameList = new List<string> { "工号*", "姓名", "性别", "员工状态", "考核人*", "考核意见", "考核时间*", "实操结果*" };

        /// <summary>
        /// 实操记录"工号"-"员工Id"
        /// </summary>
        private Dictionary<string, double> employeeCodeDic = new Dictionary<string, double>();

        /// <summary>
        /// 考核人"姓名"-"员工Id"
        /// </summary>
        private Dictionary<string, double> verifierNameDic = new Dictionary<string, double>();

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
                { "考核人*", new ValidColumn(ImportDataType._String, true, ValidVerifier) },
                { "考核意见", new ValidColumn(ImportDataType._String, false, 80) },
                { "考核时间*", new ValidColumn(ImportDataType._String, true, ValidAuditTime) },
                { "实操结果*", new ValidColumn(ImportDataType._String, true, ValidResult) }
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

            if (verifierNameDic != null)
            {
                verifierNameDic.Clear();
                verifierNameDic = null;
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
                var verifier = mainDataItem.Field<string>(ColIndex("考核人*"));
                var auditOpinion = mainDataItem.Field<string>(ColIndex("考核意见"));
                var auditTime = mainDataItem.Field<string>(ColIndex("考核时间*"));
                var result = mainDataItem.Field<string>(ColIndex("实操结果*"));
                double employeeId = GetEmployeeId(employeeCode);
                double verifierId = GetVerifierId(verifier);
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

                using (var tran = DB.TransactionScope(TeamManagementDataProvider.ConnectionStringName))
                {
                    //如果不能新增记录错误信息
                    try
                    {
                        var operRec = new OperationRecord();
                        operRec.EmployeeId = employeeId;
                        operRec.VerifierId = verifierId;
                        operRec.AuditOpinion = auditOpinion;
                        operRec.AuditTime = Convert.ToDateTime(auditTime);
                        operRec.Result = GetOperationResult(result);
                        operRec.SkillAuthId = skillAuthId;
                        RF.Save(operRec);
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
        /// 根据员工工号取员工ID
        /// </summary>
        /// <param name="key">工号</param>
        /// <returns>员工Id</returns>
        private double GetEmployeeId(string key)
        {
            if (employeeCodeDic.ContainsKey(key))
                return employeeCodeDic[key];
            return 0;
        }

        /// <summary>
        /// 根据审核人名称取审核人ID
        /// </summary>
        /// <param name="name">审核人名称</param>
        /// <returns>审核人ID</returns>
        private double GetVerifierId(string name)
        {
            if (verifierNameDic.ContainsKey(name))
                return verifierNameDic[name];
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
        /// 根据字符串值返回实操结果枚举
        /// </summary>
        /// <param name="val">字符串</param>
        /// <returns>实操结果枚举</returns>
        private OperationRequired GetOperationResult(string val)
        {
            if (val == "通过")
            {
                return OperationRequired.Pass;
            }

            if (val == "满意")
            {
                return OperationRequired.Satisfaction;
            }

            if (val == "不通过")
            {
                return OperationRequired.Fail;
            }

            return OperationRequired.NoMatter;
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
        /// 验证考核人
        /// </summary>
        /// <param name="obj">内容</param>
        /// <param name="messageTip">提示</param>
        /// <param name="row">数据行</param>
        /// <returns>验证结果</returns>
        private bool ValidVerifier(object obj, out string messageTip, DataRow row)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (obj == null)
            {
                messageTip = "为空值".L10N();
                isValid = false;
                return isValid;
            }

            if (verifierNameDic == null)
            {
                verifierNameDic = new Dictionary<string, double>();
            }

            if (!verifierNameDic.ContainsKey(obj.ToString()))
            {
                Employee employee = RT.Service.Resolve<EmployeeController>().GetEmployeeByName(obj.ToString());
                if (employee != null)
                {
                    verifierNameDic.Add(obj.ToString(), employee.Id);
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
        /// 验证考核时间
        /// </summary>
        /// <param name="obj">考核时间</param>
        /// <param name="messageTip">提示</param>
        /// <param name="row">数据行</param>
        /// <returns>验证结果</returns>
        private bool ValidAuditTime(object obj, out string messageTip, DataRow row)
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
                var auditTime = Convert.ToDateTime(obj);
                if (auditTime > DateTime.Now)
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
        /// 验证实操结果
        /// </summary>
        /// <param name="obj">实操结果</param>
        /// <param name="messageTip">提示</param>
        /// <param name="row">数据行</param>
        /// <returns>返回考试结果</returns>
        private bool ValidResult(object obj, out string messageTip, DataRow row)
        {
            bool isValid = true;
            messageTip = string.Empty;
            if (obj == null)
            {
                messageTip = "为空值".L10N();
                isValid = false;
                return isValid;
            }

            var result = obj.ToString();
            if (!"通过".Equals(result) && !"满意".Equals(result) && !"不通过".Equals(result))
            {
                messageTip = "只能填入：通过、满意、不通过".L10N();
                isValid = false;
            }

            return isValid;
        }
        #endregion
    }
}