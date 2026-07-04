using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Resources.ProcessSegments;
using SIE.Tech.Routings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SIE.MES.Routings.RoutingBoms.ImportBoms
{
    /// <summary>
    /// 工序bom主表导入
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportRoutingBomHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportRoutingBomHandle : IDisposable, IBusinessImport
    {
        private const string NOT_EXISTS = "[{0}]不存在";
        private const string ROUTING_VERSION = "工艺路线版本";
        /// <summary>
        /// 导入列名列表
        /// </summary>
        private List<string> columnNameList = new List<string> { "产品编码", "工艺路线", ROUTING_VERSION, "工段" };

        #region 私有属性
        /// <summary>
        /// 导入成功列
        /// </summary>
        public static readonly string ImportSuccess = "_importSuccess";

        /// <summary>
        /// 产品编码
        /// </summary>
        private readonly Dictionary<string, double> _productDict = new Dictionary<string, double>();

        /// <summary>
        /// 工艺路线版本列表
        /// </summary>
        private readonly Dictionary<string, double> _versionDict = new Dictionary<string, double>();

        /// <summary>
        /// 工段字典列表
        /// </summary>
        private readonly Dictionary<string, double> _segmentDict = new Dictionary<string, double>();

        /// <summary>
        /// 工艺路线字典，工艺路线名称-工艺路线ID
        /// 值为空代表新工艺路线
        /// </summary>
        private readonly Dictionary<string, double> _routingDict = new Dictionary<string, double>();

        #endregion


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
                { "产品编码", new ValidColumn(ImportDataType._String, true, ValidateProduct) },
                { "工艺路线", new ValidColumn(ImportDataType._String, true, ValidateRouting) },
                { ROUTING_VERSION, new ValidColumn(ImportDataType._String, true, ValidateVersion) },
                { "工段", new ValidColumn(ImportDataType._String, false, ValidateSegment) }
            };
            return this;
        }

        /// <summary>
        /// 释放数据
        /// </summary>
        public void Dispose()
        {
            _routingDict.Clear();
            _segmentDict.Clear();
            _productDict.Clear();
            _versionDict.Clear();
        }

        /// <summary>
        /// 处理业务数据
        /// </summary>
        /// <param name="drs">需要处理的数据集</param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            if (drs == null || !drs.Any()) return;
            DataRow[] allRows = drs[0].Table.AsEnumerable().ToArray();
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                int index = 0;
                try
                {
                    for (index = 0; index < drs.Length; index++)
                    {
                        DataRow row = drs[index];
                        var productCode = row.Field<string>(ColIndex("产品编码")).Trim();
                        var routingName = row.Field<string>(ColIndex("工艺路线")).Trim();
                        var routingVersion = row.Field<string>(ColIndex(ROUTING_VERSION)).Trim();
                        var segmentName = row.Field<string>(ColIndex("工段"));
                        RoutingBom pdRoutingVersion = new RoutingBom();

                        double productId = _productDict[productCode];
                        double routingId = _routingDict[routingName];
                        double routingVersionId = _versionDict[routingVersion];
                        pdRoutingVersion.ProductId = productId;
                        pdRoutingVersion.RoutingId = routingId;
                        pdRoutingVersion.RoutingVersionId = routingVersionId;
                        if (!string.IsNullOrWhiteSpace(segmentName) && _segmentDict.ContainsKey(segmentName))
                        {
                            pdRoutingVersion.ProcessSegmentId = _segmentDict[segmentName.Trim()];
                        }
                        else
                        {
                            pdRoutingVersion.ProcessSegmentId = null;
                        }
                        var rb = RT.Service.Resolve<RoutingBomController>().GetRoutingBom(productId, routingId, routingVersionId, pdRoutingVersion.ProcessSegmentId);
                        if (rb != null)
                        {
                            throw new ValidationException("产品编码+工艺路线版本+工段信息已经存在,导入失败.".L10N());
                        }
                        RF.Save(pdRoutingVersion);
                    }
                    tran.Complete();
                }
                catch (Exception exc)
                {
                    SetRowError(allRows, index, exc);
                }
            }
        }

        /// <summary>
        /// 设置错误信息
        /// </summary>
        /// <param name="rows">行集合</param>
        /// <param name="rowNum">错误行号</param>
        /// <param name="exc">异常信息</param>
        private void SetRowError(DataRow[] rows, int rowNum, Exception exc)
        {
            var baseExc = exc.GetBaseException();
            if (baseExc is ValidationException)
                rows[rowNum][ImportDataHandle.MessageColumnName] += (baseExc as ValidationException).Message;
            else
                rows[rowNum][ImportDataHandle.MessageColumnName] += exc.Message;
        }

        #region 获取值

        /// <summary>
        /// 获取工艺路线ID
        /// </summary>
        /// <param name="routing">工艺路线名称</param>
        /// <returns>工艺路线ID</returns>
        double? GetRouting(string routing)
        {
            double result;
            if (_routingDict.TryGetValue(routing, out result))
                return result;
            else
                return null;
        }

        /// <summary>
        /// 获取指定列名的索引
        /// </summary>
        /// <param name="columnName">指定列名</param>
        /// <returns>返回对应索引</returns>
        private int ColIndex(string columnName)
        {
            return columnNameList.IndexOf(columnName);
        }
        #endregion

        #region 属性验证
        /// <summary>
        /// 验证产品编码
        /// </summary>
        /// <param name="obj">输入参数</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="row">验证数据的当前行对象</param>
        /// <returns>验证通过返回true，不通过返回false</returns>
        private bool ValidateProduct(object obj, out string messageTip, DataRow row)
        {
            string value = obj.ToString();
            messageTip = string.Empty;
            if (ValidateIsNull(ref messageTip, value, "产品编码"))
                return false;
            if (!_productDict.ContainsKey(value))
            {
                var item = RT.Service.Resolve<ItemController>().GetItem(value);

                if (item != null && item.Type == ItemType.Material)
                {
                    messageTip = "[{0}]是原材料，无法导入。".L10nFormat(value);
                    return false;
                }
                else if (item != null)
                {
                    _productDict.Add(value, item.Id);
                }
                else
                {
                    messageTip = NOT_EXISTS.L10nFormat(value);
                }
            }

            return _productDict.ContainsKey(value);
        }

        /// <summary>
        /// 验证工艺路线
        /// </summary>
        /// <param name="obj">结果值</param>
        /// <param name="messageTip">错误消息</param>
        /// <param name="row">当前行</param>
        /// <returns>验证通过返回true，不通过返回false</returns>
        private bool ValidateRouting(object obj, out string messageTip, DataRow row)
        {
            bool isValid = true;
            string value = obj.ToString();
            messageTip = string.Empty;
            if (ValidateIsNull(ref messageTip, value, "工艺路线"))
                return false;
            if (!_routingDict.ContainsKey(value))
            {
                var routing = RT.Service.Resolve<RoutingController>().GetRoutingByName(value);
                if (routing != null)
                    _routingDict.Add(value, routing.Id);
                else
                {
                    messageTip = NOT_EXISTS.L10nFormat(value);
                    isValid = false;
                }
            }

            return isValid;
        }

        /// <summary>
        /// 验证工艺路线版本
        /// </summary>
        /// <param name="obj">输入参数</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="row">验证数据的当前行对象</param>
        /// <returns>验证通过返回true，不通过返回false</returns>
        private bool ValidateVersion(object obj, out string messageTip, DataRow row)
        {
            bool isValid = true;
            string value = obj.ToString();
            messageTip = string.Empty;
            if (ValidateIsNull(ref messageTip, value, ROUTING_VERSION))
                return false;
            if (!_versionDict.ContainsKey(value))
            {
                string routing = row.Field<string>(ColIndex("工艺路线"));
                double? routingId = GetRouting(routing);
                if (routingId != null)
                {
                    var rv = RT.Service.Resolve<RoutingController>().GetRoutingVersion((double)routingId, value);
                    if (rv != null)
                    {
                        _versionDict.Add(value, rv.Id);
                    }
                    else
                    {
                        messageTip = NOT_EXISTS.L10nFormat(value);
                        isValid = false;
                    }
                }
                else
                {
                    messageTip = "工艺路线[{0}]不匹配".L10nFormat(routing);
                    isValid = false;
                }
            }

            return isValid;
        }

        /// <summary>
        /// 工段验证
        /// </summary>
        /// <param name="obj">结果值</param>
        /// <param name="messageTip">错误消息</param>
        /// <param name="row">当前行</param>
        /// <returns>验证通过返回true，不通过返回false</returns>
        private bool ValidateSegment(object obj, out string messageTip, DataRow row)
        {
            bool isValid = true;
            string value = obj.ToString();
            messageTip = string.Empty;
            if (ValidateIsNull(ref messageTip, value, "工段"))
                return true;
            if (!_segmentDict.ContainsKey(value))
            {
                ProcessSegment ps = RT.Service.Resolve<ProcessSegmentController>().GetProcessSegmentByName(value);
                if (ps != null)
                {
                    _segmentDict.Add(value, ps.Id);
                }
                else
                {
                    messageTip = NOT_EXISTS.L10nFormat(value);
                    isValid = false;
                }
            }

            return isValid;
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
    }
}


