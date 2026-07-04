using SIE.Common.ImportHelper;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.EMS.EquipLends.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SIE.EMS.EquipLends.ImportHandles
{
    /// <summary>
    /// 设备借还导入类
    /// </summary>
    [Services.Service(FallbackType = typeof(EquipLendImHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class EquipLendImHandle : IBusinessImport
    {
        /// <summary>
        /// 导入列头
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
            "设备编码",
            "借机对象",
            "借机部门",
            "借机人",
            "供应商编码",
            "借出原因",
            "备注",
        };

        /// <summary>
        /// 列标准验证
        /// </summary>
        public Dictionary<string, ValidColumn> ColumnValidList { get; set; }

        /// <summary>
        /// 创建导入数据列验证
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IBusinessImport CreaetColumnValid()
        {
            this.ColumnValidList = new Dictionary<string, ValidColumn>();
            this.ColumnValidList.Add("设备编码", new ValidColumn(ImportDataType._String, true, false));
            this.ColumnValidList.Add("借机对象", new ValidColumn(ImportDataType._String, true, false));
            this.ColumnValidList.Add("借机部门", new ValidColumn(ImportDataType._String, false, false));
            this.ColumnValidList.Add("借机人", new ValidColumn(ImportDataType._String, false, false));
            this.ColumnValidList.Add("供应商编码", new ValidColumn(ImportDataType._String, false, false));
            this.ColumnValidList.Add("借出原因", new ValidColumn(ImportDataType._String, true, false));
            this.ColumnValidList.Add("备注", new ValidColumn(ImportDataType._String, false, false));
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
        /// 转化枚举值
        /// </summary>
        /// <param name="obj">借机对象</param>
        /// <returns></returns>
        private LendObject? ChangeLendObjectEnum(string obj)
        {
            switch (obj)
            {
                case "内部":
                    return LendObject.Internal;
                case "外部":
                    return LendObject.External;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 导入处理
        /// </summary>
        /// <param name="drs"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            var importDataList = from g in drs
                                 select new
                                 {
                                     EquipAccountCode = g.Field<string>(ColIndex("设备编码")).Trim(),
                                     LendObject = g.Field<string>(ColIndex("借机对象")).Trim(),
                                     LendEnterpriseName = g.Field<string>(ColIndex("借机部门")).Trim(),
                                     LendEmployeeCode = g.Field<string>(ColIndex("借机人")).Trim(),
                                     SupplierCode = g.Field<string>(ColIndex("供应商编码")).Trim(),
                                     Reason = g.Field<string>(ColIndex("借出原因")).Trim(),
                                     Remark = g.Field<string>(ColIndex("备注")).Trim(),
                                     DetailInfo = g,
                                 };
            // 设备编码
            var equipAccountCodes = new List<string>();
            // 借机部门
            var lendEnterpriseNames = new List<string>();
            // 借机人
            var lendEmployeeCodes = new List<string>();
            // 供应商编码
            var supplierCodes = new List<string>();
            foreach (var data in importDataList)
            {
                equipAccountCodes.Add(data.EquipAccountCode);
                lendEnterpriseNames.Add(data.LendEnterpriseName);
                lendEmployeeCodes.Add(data.LendEmployeeCode);
                supplierCodes.Add(data.SupplierCode);
            }
            var ctl = RT.Service.Resolve<EquipLendController>();
            // 设备编码
            var equipAccounts = ctl.ImportGetEquipInfoByCodes(equipAccountCodes);
            // 借机部门
            var lendEnterprises = ctl.ImportGetEnterpriseByNames(lendEnterpriseNames);
            // 借机人
            var lendEmployees = ctl.ImportGetEmployeeByCodes(lendEmployeeCodes);
            // 供应商编码
            var suppliers = ctl.ImportGetSupplierByCodes(supplierCodes);
            // 已存在的借机单的设备Id
            var existsLendEquipIds = ctl.ImportGetStateEquipLend(equipAccounts.Select(p => p.Id).Distinct().ToList());

            EntityList<EquipLendManage> saveList = new EntityList<EquipLendManage>();
            var importDataRows = importDataList.ToList();
            for (int i = 0; i < importDataRows.Count; i++)
            {
                StringBuilder errMsg = new StringBuilder();

                var data = importDataRows[i];
                var equip = equipAccounts.FirstOrDefault(p => p.Code == data.EquipAccountCode);
                if (equip == null)
                {
                    errMsg.Append("设备{0}不存在;".L10nFormat(data.EquipAccountCode));
                }
                else if (existsLendEquipIds.FirstOrDefault(p => p.Id == equip.Id) != null
                    || saveList.FirstOrDefault(p => p.EquipAccountId == equip.Id) != null)
                {
                    errMsg.Append("设备{0}已存在状态为保存、借出待审核，已借出，归还待审核的单据;".L10nFormat(data.EquipAccountCode));
                }
                var lendObj = ChangeLendObjectEnum(data.LendObject);
                var lendEnterprise = lendEnterprises.FirstOrDefault(p => p.Name == data.LendEnterpriseName);
                var lendEmployee = lendEmployees.FirstOrDefault(p => p.Code == data.LendEmployeeCode);
                var supplier = suppliers.FirstOrDefault(p => p.Code == data.SupplierCode);
                if (lendObj == null)
                {
                    errMsg.Append("借机对象不为【内部】、【外部】;".L10N());
                }
                else
                {
                    if (lendObj == LendObject.Internal)
                    {
                        if (data.LendEnterpriseName == null || data.LendEnterpriseName.Length <= 0)
                        {
                            errMsg.Append("借机对象为【内部】时借机部门必填;".L10N());
                        }
                        else if (lendEnterprise == null)
                        {
                            errMsg.Append("借机部门{0}不存在;".L10nFormat(data.LendEnterpriseName));
                        }

                        if (data.LendEmployeeCode == null || data.LendEmployeeCode.Length <= 0)
                        {
                            errMsg.Append("借机对象为【内部】时借机人必填;".L10N());
                        }
                        else if (lendEmployee == null)
                        {
                            errMsg.Append("借机人{0}不存在;".L10nFormat(data.LendEmployeeCode));
                        }
                        
                    }
                    else
                    {
                        if (data.SupplierCode == null || data.SupplierCode.Length <= 0)
                        {
                            errMsg.Append("借机对象为【外部】时供应商必填;".L10N());
                        }
                        else if (supplier == null)
                        {
                            errMsg.Append("供应商{0}不存在;".L10nFormat(data.SupplierCode));
                        }
                    }
                }
                if (errMsg.ToString().Length <= 0)
                {
                    EquipLendManage equipLendManage = new EquipLendManage
                    {
                        No = ctl.GetLendNos().FirstOrDefault(),
                        EquipAccountId = equip.Id,
                        LendObject = lendObj.Value,
                        LendEnterpriseId = lendEnterprise != null ? lendEnterprise?.Id : null,
                        LendEmployeeId = lendEmployee != null ? lendEmployee?.Id : null,
                        SupplierId = supplier != null ? supplier?.Id : null,
                        Reason = data.Reason,
                        Remark = data.Remark,
                        LendState = LendState.Saved,
                    };
                    saveList.Add(equipLendManage);
                }
                else
                {
                    importDataRows[i].DetailInfo[ImportDataHandle.MessageColumnName] = errMsg.ToString();
                }
            }

            RT.Service.Resolve<CommonController>().BatchInsertSave(saveList);
        }
    }
}
