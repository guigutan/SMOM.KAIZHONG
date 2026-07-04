using DocumentFormat.OpenXml.Bibliography;
using SIE.Common.ImportHelper;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Items;
using SIE.Items.ProductBoms;
using SIE.Items.ProductModels;
using SIE.MES.ProjectDesigns.ImportHandles;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.StandardWorkHours.ImportHandles
{
    /// <summary>
    /// 产品标准工时维护导入
    /// </summary>
    [Services.Service(FallbackType = typeof(StandardHourSetImportHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class StandardHourSetImportHandle : IDisposable, IBusinessImport
    {

        private const string _wipResourceCode = "生产资源编码";
        private const string _productModelName = "产品机型名称";
        private const string _productCode = "产品编码";
        private const string _processName = "工序名称";
        private const string _standardMin = "工序标准工时(分)";
        private const string _attachMin = "附加合计工时(分)";
        private const string _remark = "备注";

        /// <summary>
        /// 判断生产资源+产品机型+产品+工序唯一
        /// </summary>
        private HashSet<string> repeat { get; set; } = new HashSet<string>();

        /// <summary>
        /// 导入列名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
            _wipResourceCode,
            _productModelName,
            _productCode,
            _processName,
            _standardMin,
            _attachMin,
            _remark,
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

            repeat.Clear();
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
        /// 校验数据主键唯一
        /// </summary>
        /// <param name="mainKey">数据主键</param>
        /// <param name="dbDic">数据库数据</param>
        /// <param name="result">结果</param>
        private void ValidateRepeatField(string mainKey, Dictionary<string, double> dbDic, StandardHourSetResultInfo result)
        {
            if (!repeat.Contains(mainKey) && !dbDic.ContainsKey(mainKey))
            {
                repeat.Add(mainKey);
            }
            else
            {
                result.Pass = false;
                result.Error.Append("存在相同主键(生产资源+产品机型+产品+工序)");
            }
        }

        /// <summary>
        /// 验证列必填
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="columnValue">列值</param>
        /// <param name="result">结果</param>
        private void ValidateRequiredField(string columnName, string columnValue, StandardHourSetResultInfo result)
        {
            if (columnValue.IsNullOrEmpty())
            {
                result.Pass = false;
                result.Error.Append("{0}不能为空".FormatArgs(columnName));
            }
        }

        /// <summary>
        /// 验证列数字
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="columnValue">列值</param>
        /// <param name="result">结果</param>
        /// <param name="canNull">是否可空</param>
        /// <returns></returns>
        private decimal ValidateNumberField(string columnName, string columnValue, StandardHourSetResultInfo result)
        {
            decimal value = 0;
            if (columnValue.IsNullOrEmpty()) return 0;
            if (!Decimal.TryParse(columnValue, out value))
            {
                result.Pass = false;
                result.Error.Append("[{0}]必须为数字;".L10nFormat(columnName));
            }
            else
            {
                if (value <= 0)
                {
                    result.Pass = false;
                    result.Error.Append("[{0}]必须大于0;".L10nFormat(columnName));
                }
            }
            return value;
        }

        /// <summary>
        /// 验证列是否存在并返回Id
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="columnValue">列值</param>
        /// <param name="keyValuePairs">键值对</param>
        /// <param name="result">结果</param>
        /// <returns></returns>
        private double ValidateFiledExists(string columnName, string columnValue, Dictionary<string, double> keyValuePairs, StandardHourSetResultInfo result)
        {
            double id = 0;
            if (!keyValuePairs.TryGetValue(columnValue, out id))
            {
                result.Pass = false;
                result.Error.Append("[{0}]{1}不存在;".FormatArgs(columnName, columnValue));
            }
            return id;
        }

        /// <summary>
        /// 导入校验
        /// </summary>
        /// <param name="info">导入行</param>
        /// <param name="wipDic">生产资源名称-Id字典</param>
        /// <param name="modelDic">产品机型名称-Id字典</param>
        /// <param name="productDic">产品名称-Id字典</param>
        /// <param name="processDic">工序名称-Id字典</param>
        /// <param name="dbDic">数据库存在字典</param>
        /// <returns></returns>
        private StandardHourSetResultInfo ImportValidate(StandardHourSetImpInfo info, Dictionary<string, double> wipDic, Dictionary<string, double> modelDic, Dictionary<string, double> productDic, Dictionary<string, double> processDic, Dictionary<string, double> dbDic)
        {
            StandardHourSetResultInfo result = new StandardHourSetResultInfo();
            ValidateRequiredField(info.WipResourceCode, _wipResourceCode, result);
            ValidateRequiredField(info.ProductModelName, _productModelName, result);
            ValidateRequiredField(info.ProductCode, _productCode, result);
            ValidateRequiredField(info.ProcessName, _processName, result);
            ValidateRequiredField(info.StandardMin, _standardMin, result);
            result.WipResourceId = ValidateFiledExists(_wipResourceCode, info.WipResourceCode, wipDic, result);
            result.ProductModelId = ValidateFiledExists(_productModelName, info.ProductModelName, modelDic, result);
            result.ProductId = ValidateFiledExists(_productCode, info.ProductCode, productDic, result);
            result.ProcessId = ValidateFiledExists(_processName, info.ProcessName, processDic, result);
            result.StandardMin = ValidateNumberField(_standardMin, info.StandardMin, result);
            result.AttachMin = info.AttachMin.IsNotEmpty() ? ValidateNumberField(_attachMin, info.AttachMin, result) : null;
            result.Remark = info.Remark;
            ValidateRepeatField("{0}@{1}@{2}@{3}".FormatArgs(result.WipResourceId, result.ProductModelId, result.ProductId, result.ProcessId), dbDic, result);
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
                                 select new StandardHourSetImpInfo
                                 {
                                     WipResourceCode = g.Field<string>(ColIndex(_wipResourceCode)).Trim(),
                                     ProductModelName = g.Field<string>(ColIndex(_productModelName)).Trim(),
                                     ProductCode = g.Field<string>(ColIndex(_productCode)).Trim(),
                                     ProcessName = g.Field<string>(ColIndex(_processName)).Trim(),
                                     StandardMin = g.Field<string>(ColIndex(_standardMin)).Trim(),
                                     AttachMin = g.Field<string>(ColIndex(_attachMin)).Trim(),
                                     Remark = g.Field<string>(ColIndex(_remark)).Trim(),
                                     DetailInfo = g,
                                 };

            // 生产资源编码
            HashSet<string> wipResourceCodes = new HashSet<string>();
            // 产品机型名称
            HashSet<string> productModelNames = new HashSet<string>();
            // 产品编码
            HashSet<string> productCodes = new HashSet<string>();
            // 工序名称
            HashSet<string> processNames = new HashSet<string>();

            importDataList.ForEach(data =>
            {
                wipResourceCodes.Add(data.WipResourceCode);
                productModelNames.Add(data.ProductModelName);
                productCodes.Add(data.ProductCode);
                processNames.Add(data.ProcessName);
            });

            // 根据生产资源编码获取生产资源编码-Id
            var wipDic = RT.Service.Resolve<WipResourceController>().GetWipResourceCodeIdDic(wipResourceCodes.ToList());
            // 根据产品机型名称获取产品机型名称-Id
            var modelDic = RT.Service.Resolve<ProductModelController>().GetProductModelNameIdDic(productModelNames.ToList());
            // 根据产品编码获取产品编码-Id
            var productDic = RT.Service.Resolve<ItemController>().GetItemIdByCodes(productCodes.ToList());
            // 根据工序名称获取工序名称-Id
            var processDic = RT.Service.Resolve<ProcessController>().GetProcessNameIdDic(processNames.ToList());
            var processIds = processDic.Values;
            // 获取同工序的标准工时维护
            var dbDic = RT.Service.Resolve<StandardHourSetController>().GetStandardHourSets(processIds);
            EntityList<StandardHourSet> saveList = new EntityList<StandardHourSet>();
            var importDataRows = importDataList.ToList();
            for (var i = 0; i < importDataRows.Count; i++)
            {
                // 导入校验
                var result = ImportValidate(importDataRows[i], wipDic, modelDic, productDic, processDic, dbDic);
                if (result.Pass)
                {
                    StandardHourSet set = new StandardHourSet
                    {
                        WipResourceId = result.WipResourceId,
                        ProductModelId = result.ProductModelId,
                        ProductId = result.ProductId,
                        ProcessId = result.ProcessId,
                        StandardMin = result.StandardMin,
                        AttachMin = result.AttachMin,
                        Remark = result.Remark,
                    };
                    saveList.Add(set);
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
