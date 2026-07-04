using Microsoft.Scripting.Utils;
using SIE.Common.ImportHelper;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Items;
using SIE.Items.ProductBoms;
using SIE.Items.ProductBoms.Models;
using SIE.MES.ProjectDesigns.ChildInfos;
using SIE.Tech.Routings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static IronPython.Modules._ast;

namespace SIE.MES.ProjectDesigns.ImportHandles
{
    /// <summary>
    /// 项目号需求设计-工序BOM明细导入
    /// </summary>
    [Services.Service(FallbackType = typeof(ProcessBomDtlImpHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ProcessBomDtlImpHandle : IDisposable, IBusinessImport
    {
        /// <summary>
        /// 导入列名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
            "项目号编码",
            "项目号产品编码",
            "层级",
            "层级产品编码",
            "物料编码",
            "单位用量",
            "工序名称",
            "顺序",
            "备注",
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

        /// <summary>
        /// 验证不能字段不能为空
        /// </summary>
        /// <param name="columnValue">列值</param>
        /// <param name="error">错误信息</param>
        /// <param name="result">结果</param>
        private void ValidateRequiredField(string columnValue, string error, ProcessBomDtlImpResult result)
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
        private decimal ValidateNumberField(string columnValue, string columnName, ProcessBomDtlImpResult result, bool moreZero, bool lessZero)
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
        private double ValidateExistsField(string columnName, ProcessBomDtlImpResult result, Dictionary<string, double> dic, string key)
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
        /// 导入校验
        /// </summary>
        /// <param name="row">数据行</param>
        /// <param name="routingDic">项目号层级产品对应的产品工艺路线设置</param>
        /// <param name="itemDic">物料Id字典</param>
        /// <param name="processDic">项目号工序清单字典</param>
        /// <param name="productBomLookUp">产品bom字典</param>
        /// <returns></returns>
        private ProcessBomDtlImpResult ImportValidate(ProcessBomDtlImpInfo row, Dictionary<string, double> routingDic, Dictionary<string, double> itemDic,
            Dictionary<string, double> processDic, ILookup<string, ProBomDtlInfo> productBomLookUp)
        {
            ProcessBomDtlImpResult result = new ProcessBomDtlImpResult();
            ValidateRequiredField(row.ProjectMaintainCode, "项目号编码不能为空;".L10N(), result);
            ValidateRequiredField(row.ProjectProductCode, "项目号产品编码不能为空;".L10N(), result);
            ValidateRequiredField(row.Level, "层级不能为空;".L10N(), result);
            ValidateRequiredField(row.ProductCode, "层级产品编码不能为空;".L10N(), result);
            ValidateRequiredField(row.MaterialCode, "物料编码不能为空;".L10N(), result);
            ValidateRequiredField(row.Amount, "单位耗用量不能为空;".L10N(), result);
            ValidateRequiredField(row.ProcessName, "工序名称不能为空;".L10N(), result);
            ValidateRequiredField(row.ProcessIndex, "工序顺序不能为空;".L10N(), result);
            if (productBomLookUp[row.ProductCode] != null && !productBomLookUp[row.ProductCode].Select(p => p.ItemCode).Contains(row.MaterialCode))
            {
                result.Pass = false;
                result.Error.Append("物料[{0}]不存在产品[{1}]的产品Bom设置中;".L10nFormat(row.MaterialCode, row.ProductCode));
            }

            result.DesignTreeRoutingId = ValidateExistsField("项目[{0}]项目产品[{1}]层级[{2}]产品[{3}]工艺路线设置".L10nFormat(row.ProjectMaintainCode, row.ProjectProductCode, row.Level, row.ProductCode), result, routingDic, "{0}@{1}@{2}@{3}".FormatArgs(row.ProjectMaintainCode, row.ProjectProductCode, row.Level, row.ProductCode));
            result.RoutingProcessId = ValidateExistsField("产品工艺路线设置工序清单工序[{0}]顺序[{1}]".L10nFormat(row.ProcessName, row.ProcessIndex), result, processDic, "{0}@{1}".FormatArgs(row.ProcessName, row.ProcessIndex));
            result.MaterialId = ValidateExistsField("物料编码[{0}]".L10nFormat(row.MaterialCode), result, itemDic, row.MaterialCode);
            result.Amount = ValidateNumberField(row.Amount, "单位耗用量", result, true, false);
            result.Remark = row.Remark;
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
                                 select new ProcessBomDtlImpInfo
                                 {
                                     ProjectMaintainCode = g.Field<string>(ColIndex("项目号编码")).Trim(),
                                     ProjectProductCode = g.Field<string>(ColIndex("项目号产品编码")).Trim(),
                                     Level = g.Field<string>(ColIndex("层级")),
                                     ProductCode = g.Field<string>(ColIndex("层级产品编码")).Trim(),
                                     MaterialCode = g.Field<string>(ColIndex("物料编码")).Trim(),
                                     Amount = g.Field<string>(ColIndex("单位用量")),
                                     ProcessName = g.Field<string>(ColIndex("工序名称")).Trim(),
                                     ProcessIndex = g.Field<string>(ColIndex("顺序")),
                                     Remark = g.Field<string>(ColIndex("备注")).Trim(),
                                     DetailInfo = g,
                                 };

            // 项目号编码
            HashSet<string> projectMaintainCodes = new HashSet<string>();
            // 项目号产品编码
            HashSet<string> projectProductCodes = new HashSet<string>();
            // 层级
            HashSet<int> levels = new HashSet<int>();
            // 层级产品编码
            HashSet<string> productCodes = new HashSet<string>();
            // 物料编码
            HashSet<string> materialCodes = new HashSet<string>();
            // 工序名称
            HashSet<string> processNames = new HashSet<string>();
            importDataList.ForEach(data =>
            {
                projectMaintainCodes.Add(data.ProjectMaintainCode);
                projectProductCodes.Add(data.ProjectProductCode);
                if (int.TryParse(data.Level, out int level))
                {
                    levels.Add(level);
                }
                productCodes.Add(data.ProductCode);
                materialCodes.Add(data.MaterialCode);
                processNames.Add(data.ProcessName);
            });

            // 依据项目号、项目号产品编码、层级、层级产品编码获取产品工艺路线设置
            var routingDic = RT.Service.Resolve<ProjectDesignController>().GetDesignTreeRoutings(projectMaintainCodes.ToList(), projectProductCodes.ToList(), levels.ToList(), productCodes.ToList());
            var routingIds = routingDic.Values;
            // 获取物料Id
            var itemDic = RT.Service.Resolve<ItemController>().GetItemIdByCodes(materialCodes.ToList());
            // 获取工序清单信息
            var processDic = RT.Service.Resolve<ProjectDesignController>().GetDesignTreeRoutingDetailProcessDic(routingIds);
            // 获取产品Bom信息
            var productBomLookUp = RT.Service.Resolve<ProductBomController>().GetProductBomDtls(productCodes.ToList());

            EntityList<DesignTreeRoutingProBom> saveList = new EntityList<DesignTreeRoutingProBom>();
            var importDataRows = importDataList.ToList();
            for (var i = 0; i < importDataRows.Count; i++)
            {
                // 导入校验
                ProcessBomDtlImpResult result = ImportValidate(importDataRows[i], routingDic, itemDic, processDic, productBomLookUp);

                if (result.Pass)
                {
                    DesignTreeRoutingProBom designTreeRoutingProBom = new DesignTreeRoutingProBom
                    {
                        DesignTreeRoutingId = result.DesignTreeRoutingId,
                        RoutingProcessId = result.RoutingProcessId,
                        MaterialId = result.MaterialId,
                        Amount = result.Amount,
                        Remark = result.Remark,
                    };
                    saveList.Add(designTreeRoutingProBom);
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
