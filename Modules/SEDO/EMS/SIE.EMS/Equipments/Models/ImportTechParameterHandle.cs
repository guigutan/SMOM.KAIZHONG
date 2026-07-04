using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.Equipments.EquipModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.EMS.Equipments.Models
{
    /// <summary>
    /// 导入类
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportTechParameterHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportTechParameterHandle : IDisposable, IBusinessImport
    {
        #region 私有属性
        /// <summary>
        /// 设备型号信息"型号编码"-"设备型号"
        /// </summary>
        private readonly Dictionary<string, EquipModel> equipModelCodeDic = new Dictionary<string, EquipModel>();

        #endregion

        /// <summary>
        /// 导入模板的列名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
            "设备型号编码","设备型号名称", "参数名称", "参数内容"
        };

        /// <summary>
        /// 列的标准验证 (列名 列对应验证 )
        /// </summary>
        public Dictionary<string, ValidColumn> ColumnValidList { get; set; }

        /// <summary>
        /// 创建导入技术参数标准对象
        /// </summary>
        /// <returns>返回技术参数对象</returns>
        public IBusinessImport CreaetColumnValid()
        {
            this.ColumnValidList = new Dictionary<string, ValidColumn>();
            this.ColumnValidList.Add("设备型号编码", new ValidColumn(ImportDataType._String, true, true));
            this.ColumnValidList.Add("设备型号名称", new ValidColumn(ImportDataType._String, false, true));
            this.ColumnValidList.Add("参数名称", new ValidColumn(ImportDataType._String, true, true));
            this.ColumnValidList.Add("参数内容", new ValidColumn(ImportDataType._String, true, true));
            return this;
        }

        /// <summary>
        /// 处理业务数据
        /// </summary>
        /// <param name="drs">需要处理的数据集</param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            if (drs.Length <= 0) return;

            using (var tran = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                foreach (var dr in drs)
                {
                    EquipModel equipModel = null;
                    var equipCode = dr["设备型号编码"].ToString();
                    if (equipCode.IsNullOrEmpty())
                    {
                        ImportExtension.BatchAppendText(new List<DataRow>() { dr }, ImportDataHandle.MessageColumnName, "设备型号编码不允许为空!".L10N());
                        continue;
                    }
                    if (equipModelCodeDic.Count > 0 && equipModelCodeDic.ContainsKey(equipCode))//取出已经存在的设备型号
                    {
                        equipModel = equipModelCodeDic[equipCode];
                    }
                    if (equipModel == null)
                    {
                        equipModel = RT.Service.Resolve<EquipModelController>().GetEquipModelByCode(equipCode);
                        if (equipModel == null)
                        {
                            ImportExtension.BatchAppendText(new List<DataRow>() { dr }, ImportDataHandle.MessageColumnName, "设备型号编码【{0}】,系统不存在!".L10nFormat(equipCode));
                            continue;
                        }
                        equipModelCodeDic.Add(equipCode, equipModel);
                    }
                    var parameter = SetParameter(dr, equipModel);
                    if (parameter == null)
                        continue;
                    try
                    {
                        RF.Save(parameter);
                    }
                    catch (Exception ex)
                    {
                        string strMsg = ex.GetBaseException()?.Message;
                        ImportExtension.BatchAppendText(new List<DataRow>() { dr }, ImportDataHandle.MessageColumnName, strMsg);
                        continue;
                    }
                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 设置参数内容 名称
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="equipModel"></param>
        private EquipModelTechParameter SetParameter(DataRow dr, EquipModel equipModel)
        {
            var paraName = dr["参数名称"].ToString();
            if (paraName.IsNullOrEmpty())
            {
                ImportExtension.BatchAppendText(new List<DataRow>() { dr }, ImportDataHandle.MessageColumnName, "设备型号编码【{0}】记录的参数名称不允许为空!".L10nFormat(equipModel.Code));
                return null;
            }
            var paraValue = dr["参数内容"].ToString();
            if (paraValue.IsNullOrEmpty())
            {
                ImportExtension.BatchAppendText(new List<DataRow>() { dr }, ImportDataHandle.MessageColumnName, "设备型号编码【{0}】记录的参数内容不允许为空!".L10nFormat(equipModel.Code));
                return null;
            }
            EquipModelTechParameter equipModelTechParameter = new EquipModelTechParameter();
            equipModelTechParameter.EquipModelId = equipModel.Id;
            equipModelTechParameter.ParameterName = paraName;
            equipModelTechParameter.ParameterValue = paraValue;
            return equipModelTechParameter;
        }


        /// <summary>
        /// 释放数据
        /// </summary>
        public void Dispose()
        {
            equipModelCodeDic.Clear();
            GC.SuppressFinalize(this);
        }
    }
}
