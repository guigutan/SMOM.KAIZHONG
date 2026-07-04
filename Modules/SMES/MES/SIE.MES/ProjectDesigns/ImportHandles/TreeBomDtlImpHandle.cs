using Microsoft.Scripting.Utils;
using SIE.Common.ImportHelper;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Items;
using SIE.MES.ProjectDesigns.ChildInfos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProjectDesigns.ImportHandles
{
    /// <summary>
    /// 项目号需求设计-产品BOM明细导入
    /// </summary>
    [Services.Service(FallbackType = typeof(TreeBomDtlImpHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class TreeBomDtlImpHandle : IDisposable, IBusinessImport
    {
        /// <summary>
        /// 导入列名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
            "项目号编码",
            "项目号产品编码",
            "产品Bom编码",
            "物料编码",
            "单位耗用量",
            "损耗率",
            "是否反冲物料",
        };

        /// <summary>
        /// 验证列
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
            return this;
        }

        /// <summary>
        /// 释放数据
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (ColumnValidList != null)
            {
                ColumnValidList.Clear();
                ColumnValidList = null;
            }
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
        /// 释放数据
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Dispose()
        {
            Dispose(true);
        }

        private bool SwitchIsRecoilItem(string isRecoilItem)
        {
            return isRecoilItem == "是";
        }

        /// <summary>
        /// 验证不能字段不能为空
        /// </summary>
        /// <param name="columnValue">列值</param>
        /// <param name="error">错误信息</param>
        /// <param name="result">结果</param>
        private void ValidateRequiredField(string columnValue, string error, TreeBomDtlImpResult result)
        {
            if (columnValue.IsNullOrEmpty())
            {
                result.Pass = false;
                result.Error.Append(error);
            }
        }

        /// <summary>
        /// 验证数值是否合法
        /// </summary>
        /// <param name="columnValue">验证列的值</param>
        /// <param name="columnName">验证列的名</param>
        /// <param name="result">结果</param>
        /// <param name="moreZero">必须大于0</param>
        /// <param name="lessZero">不能小于0</param>
        private decimal ValidateNumberField(string columnValue, string columnName, TreeBomDtlImpResult result, bool moreZero, bool lessZero)
        {
            decimal value = 0;
            if (columnValue.IsNullOrEmpty()) return value;
            if (!Decimal.TryParse(columnValue, out value))
            {
                result.Pass = false;
                result.Error.Append("[{0}]必须为数字;".L10nFormat(columnName));
            }
            else 
            {
                if (moreZero && value <= 0)
                {
                    result.Pass = false;
                    result.Error.Append("[{0}]必须大于0;".L10nFormat(columnName));
                }
                else if (lessZero && value < 0)
                {
                    result.Pass = false;
                    result.Error.Append("[{0}]不能小于0;".L10nFormat(columnName));
                }
            }
            return value;
        }

        /// <summary>
        /// 验证列是否存在
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="result">结果</param>
        /// <param name="dic">编码Id字典</param>
        /// <param name="key">列值</param>
        /// <returns></returns>
        private double ValidateExistsField(string columnName, TreeBomDtlImpResult result, Dictionary<string, double> dic, string key)
        {
            double id = 0;
            if (!dic.TryGetValue(key, out id))
            {
                result.Pass = false;
                result.Error.Append("{0}不存在;".L10nFormat(columnName, key));
            }
            return id;
        }

        /// <summary>
        /// 数据导入校验
        /// </summary>
        /// <param name="row">数据行</param>
        /// <param name="itemDic">物料信息 key:物料编码 value:物料Id</param>
        /// <param name="treeBomDic">项目号产品bom信息 key:产品Bom编码 value:产品BomId</param>
        private TreeBomDtlImpResult ImportValidate(TreeBomDtlImpInfo row, Dictionary<string, double> itemDic, Dictionary<string, double> treeBomDic)
        {
            TreeBomDtlImpResult result = new TreeBomDtlImpResult();
            ValidateRequiredField(row.ProjectMaintainCode, "项目号编码不能为空;".L10N(), result);
            ValidateRequiredField(row.ProjectProductCode, "项目号产品编码不能为空;".L10N(), result);
            ValidateRequiredField(row.BomCode, "产品Bom编码不能为空;".L10N(), result);
            ValidateRequiredField(row.MaterialCode, "物料编码不能为空;".L10N(), result);
            ValidateRequiredField(row.UnitQty, "单位耗用量不能为空;".L10N(), result);
            result.BomId = ValidateExistsField("项目号编码[{0}]项目号产品[{1}]产品Bom[{2}]".L10nFormat(row.ProjectMaintainCode, row.ProjectProductCode, row.BomCode), result, treeBomDic, "{0}@{1}@{2}".FormatArgs(row.ProjectMaintainCode, row.ProjectProductCode, row.BomCode));
            result.ItemId = ValidateExistsField("物料编码[{0}]".L10nFormat(row.MaterialCode), result, itemDic, row.MaterialCode);
            result.UnitQty = ValidateNumberField(row.UnitQty, "单位耗用量", result, true, false);
            result.LossRate = ValidateNumberField(row.LossRate, "损耗率", result, false, true);
            return result;
        }

        /// <summary>
        /// 处理业务数据
        /// </summary>
        /// <param name="drs"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            var importDataList = from g in drs
                                 select new TreeBomDtlImpInfo
                                 {
                                     ProjectMaintainCode = g.Field<string>(ColIndex("项目号编码")).Trim(),
                                     ProjectProductCode = g.Field<string>(ColIndex("项目号产品编码")).Trim(),
                                     BomCode = g.Field<string>(ColIndex("产品Bom编码")).Trim(),
                                     MaterialCode = g.Field<string>(ColIndex("物料编码")).Trim(),
                                     UnitQty = g.Field<string>(ColIndex("单位耗用量")),
                                     LossRate = g.Field<string>(ColIndex("损耗率")),
                                     IsRecoilItem = g.Field<string>(ColIndex("是否反冲物料")).Trim(),
                                     DetailInfo = g,
                                 };
            // 项目号编码
            HashSet<string> projectMaintainCodes = new HashSet<string>();
            // 项目号产品编码
            HashSet<string> projectProductCodes = new HashSet<string>();
            // 产品Bom编码
            HashSet<string> bomCodes = new HashSet<string>();
            // 物料编码
            HashSet<string> materialCodes = new HashSet<string>();
            importDataList.ForEach(data =>
            {
                projectMaintainCodes.Add(data.ProjectMaintainCode);
                projectProductCodes.Add(data.ProjectProductCode);
                bomCodes.Add(data.BomCode);
                materialCodes.Add(data.MaterialCode);
            });

            // 依据项目号、项目号产品编码、Bom编码定位数据
            var treeBomDic = RT.Service.Resolve<ProjectDesignController>().GetDesignTreeBoms(projectMaintainCodes.ToList(), projectProductCodes.ToList(), bomCodes.ToList());
            // 获取物料Id
            var itemDic = RT.Service.Resolve<ItemController>().GetItemIdByCodes(materialCodes.ToList());

            EntityList<DesignTreeBomDetail> saveList = new EntityList<DesignTreeBomDetail>();
            var importDataRows = importDataList.ToList();
            for (var i = 0; i < importDataRows.Count; i++)
            {
                // 导入校验
                TreeBomDtlImpResult result = ImportValidate(importDataRows[i], itemDic, treeBomDic);
                if (result.Pass)
                {
                    DesignTreeBomDetail designTreeBomDetail = new DesignTreeBomDetail
                    {
                        DesignTreeBomId = result.BomId,
                        ItemId = result.ItemId,
                        UnitQty = result.UnitQty,
                        LossRate = result.LossRate,
                        IsRecoilItem = SwitchIsRecoilItem(importDataRows[i].IsRecoilItem),
                    };
                    saveList.Add(designTreeBomDetail);
                }
                else
                {
                    importDataRows[i].DetailInfo[ImportDataHandle.MessageColumnName] = result.Error.ToString();
                }
            }

            if (saveList.Any())
            {
                using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
                {
                    RT.Service.Resolve<CommonController>().BatchInsertSave(saveList);
                    tran.Complete();
                }
            }
        }
    }
}
