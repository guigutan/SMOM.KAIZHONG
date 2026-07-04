using SIE.Common.ImportHelper;
using SIE.Defects;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.Equipments.EquipFaults
{
    /// <summary>
    /// 导入设备故障与系统缺陷对应关系处理类 
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportEquipFaultAndDefectHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportEquipFaultAndDefectHandle : IDisposable, IBusinessImport
    {
        /// <summary>
        /// 列名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>()
        {
            "设备型号*", "型号名称", "设备不良代码*", "设备缺陷名称*", "缺陷代码*", "缺陷描述", "缺陷分类代码", "缺陷分类描述"
        };

        /// <summary>
        /// 列的标准验证 (列名 列对应验证 )
        /// </summary>
        public Dictionary<string, ValidColumn> ColumnValidList { get; set; }

        /// <summary>
        /// 创建列验证
        /// </summary>
        /// <returns>IBusinessImport</returns>
        public IBusinessImport CreaetColumnValid()
        {
            this.ColumnValidList = new Dictionary<string, ValidColumn>
            {
                { "设备型号*", new ValidColumn(ImportDataType._String, true, ValidateEquipModel) },
                { "型号名称", new ValidColumn(ImportDataType._String, false, true) },
                { "设备不良代码*", new ValidColumn(ImportDataType._String, true, 240) },
                { "设备缺陷名称*", new ValidColumn(ImportDataType._String, true, 240) },
                { "缺陷代码*", new ValidColumn(ImportDataType._String, true, ValidateDefect) },
                { "缺陷描述", new ValidColumn(ImportDataType._String, false, true) },
                { "缺陷分类代码", new ValidColumn(ImportDataType._String, false, true) },
                { "缺陷分类描述", new ValidColumn(ImportDataType._String, false, true) }
            };

            return this;
        }

        /// <summary>
        /// 处理业务数据
        /// </summary>
        /// <param name="drs">需要处理的数据集</param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            if (drs == null || !drs.Any())
                return;
            for (int i = 0; i < drs.Length; i++)
            {
                DataRow row = drs[i];
                try
                {
                    var equipModel = row.Field<string>(0);
                    var equipBadCode = row.Field<string>(2);
                    var equipDefectName = row.Field<string>(3);
                    var defectCode = row.Field<string>(4);
                    var faultDefect = new EquipFaultAndDefect()
                    {
                        EquipModelId = _modelDic.GetValue<double>(equipModel),
                        DefectId = _defectDic.GetValue<double>(defectCode),
                        EquipDefectName = equipDefectName,
                        EquipBadCode = equipBadCode
                    };
                    RT.Service.Resolve<EquipFaultAndDefectController>().SaveEquipFaultAndDefect(faultDefect);
                }
                catch (Exception exc)
                {
                    SetRowError(row, exc);
                }
            }
        }

        /// <summary>
        /// 设置错误信息
        /// </summary>
        /// <param name="row ">行 </param> 
        /// <param name="exc">异常信息</param>
        private void SetRowError(DataRow row, Exception exc)
        {
            var baseExc = exc.GetBaseException();
            if (baseExc is ValidationException)
                row[ImportDataHandle.MessageColumnName] += (baseExc as ValidationException).Message;
            else
                row[ImportDataHandle.MessageColumnName] += exc.Message;
        }

        #region 验证数据合法性 
        /// <summary>
        /// 设备型号字典  "设备型号编码"-"Id"
        /// </summary>
        private Dictionary<string, double> _modelDic;// = new Dictionary<string, double>()

        /// <summary>
        /// 缺陷代码字典  "缺陷代码编码"-"Id"
        /// </summary>
        private Dictionary<string, double> _defectDic;//= new Dictionary<string, double>()

        /// <summary>
        /// 验证缺陷代码
        /// </summary>
        /// <param name="obj">缺陷代码编码</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据的当前行对象</param>
        /// <returns>验证通过返回true，不通过返回false</returns>
        private bool ValidateDefect(object obj, out string messageTip, DataRow dr)
        {
            string value = obj.ToString();
            messageTip = string.Empty;
            if (ValidateIsNull(ref messageTip, value, "缺陷代码"))
                return false;
            if (_defectDic == null)
                _defectDic = RT.Service.Resolve<DefectController>().GetAllDefect().ToDictionary(p => p.Code, p => p.Id);
            if (_defectDic.ContainsKey(value))
                return true;
            messageTip = "[{0}]不存在".L10nFormat(value);
            return false;
        }

        /// <summary>
        /// 验证设备型号
        /// </summary>
        /// <param name="obj">设备型号编码</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据的当前行对象</param>
        /// <returns>验证通过返回true，不通过返回false</returns>
        private bool ValidateEquipModel(object obj, out string messageTip, DataRow dr)
        {
            string value = obj.ToString();
            messageTip = string.Empty;
            if (ValidateIsNull(ref messageTip, value, "设备型号"))
                return false;
            if (_modelDic == null)
                _modelDic = RT.Service.Resolve<CoreEquipController>().GetEquipModels().ToDictionary(p => p.Code, p => p.Id);
            if (_modelDic.ContainsKey(value))
                return true;
            messageTip = "[{0}]不存在".L10nFormat(value);
            return false;
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
                return false;
            str = "{0}不能为空".L10nFormat(colunmName);
            return true;
        }
        #endregion

        /// <summary>
        /// 释放数据
        /// </summary>
        public void Dispose()
        {
            _modelDic = null;
            _defectDic = null;
        }
    }
}
