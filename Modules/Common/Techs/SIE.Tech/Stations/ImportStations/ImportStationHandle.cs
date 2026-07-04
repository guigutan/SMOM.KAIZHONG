using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Resources.ProcessSegments;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Routings.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SIE.Tech.Stations.ImportStations
{

    /// <summary>
    /// 工位导入
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportStationHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportStationHandle : IDisposable, IBusinessImport
    {
        /// <summary>
        /// 导入列名列表
        /// </summary>
        private List<string> columnNameList = new List<string> { "编码", "名称", "资源编码"};

        #region 私有属性
        /// <summary>
        /// 导入成功列
        /// </summary>
        public static readonly string ImportSuccess = "_importSuccess";

        /// <summary>
        /// 名称
        /// </summary>
        private readonly Dictionary<string, string> _nameDict = new Dictionary<string, string>();

        /// <summary>
        /// 编码
        /// </summary>
        private readonly Dictionary<string, string> _codeDict = new Dictionary<string, string>();

        /// <summary>
        /// 资源
        /// </summary>
        private readonly Dictionary<string, double> _resourceDict = new Dictionary<string, double>();

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
                { "编码", new ValidColumn(ImportDataType._String, true, ValidateCode) },
                { "名称", new ValidColumn(ImportDataType._String, true, ValidateName) },
                { "资源编码", new ValidColumn(ImportDataType._String, true, ValidateResource) },
            };
            return this;
        }

        /// <summary>
        /// 释放数据
        /// </summary>
        public void Dispose()
        {
            _resourceDict.Clear();
        }

        /// <summary>
        /// 处理业务数据
        /// </summary>
        /// <param name="drs">需要处理的数据集</param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            if (drs == null || !drs.Any()) return;
            DataRow[] allRows = drs[0].Table.AsEnumerable().ToArray();
            using (var tran = DB.TransactionScope(TechEntityDataProvider.ConnectionStringName))
            {
                int index = 0;
                try
                {
                    for (index = 0; index < allRows.Length; index++)
                    {
                        DataRow row = allRows[index];
                        var code = row.Field<string>(ColIndex("编码")).Trim();
                        var name = row.Field<string>(ColIndex("名称")).Trim();
                        var resource = row.Field<string>(ColIndex("资源编码")).Trim();
                        Station station = new Station();
                        station.Code = code;
                        station.Name = name;
                        station.ResourceId = _resourceDict[resource];
                        station.IsImportData = true;
                        RF.Save(station);
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

        /// <summary>
        /// 获取指定列名的索引
        /// </summary>
        /// <param name="columnName">指定列名</param>
        /// <returns>返回对应索引</returns>
        private int ColIndex(string columnName)
        {
            return columnNameList.IndexOf(columnName);
        }

        #region 属性验证

        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="obj">结果值</param>
        /// <param name="messageTip">错误消息</param>
        /// <param name="row">当前行</param>
        /// <returns>验证通过返回true，不通过返回false</returns>
        private bool ValidateCode(object obj, out string messageTip, DataRow row)
        {
            const bool isValid = true;
            string value = obj.ToString().Trim();
            messageTip = string.Empty;
            if (ValidateIsNull(ref messageTip, value, "编码"))
                return true;
            if (_codeDict.ContainsKey(value))
            {
                messageTip = "[{0}]在导入列表中重复存在".L10nFormat(value);
                return false;
            }
            Station ps = RT.Service.Resolve<StationController>().GetStation(value);
            if (ps != null)
            {
                messageTip = "[{0}]已经存在".L10nFormat(value);
                return false;
            }
            _codeDict.Add(value, value);

            return isValid;
        }

        /// <summary>
        /// 名称
        /// </summary>
        /// <param name="obj">结果值</param>
        /// <param name="messageTip">错误消息</param>
        /// <param name="row">当前行</param>
        /// <returns>验证通过返回true，不通过返回false</returns>
        private bool ValidateName(object obj, out string messageTip, DataRow row)
        {
            const bool isValid = true;
            string value = obj.ToString().Trim();
            messageTip = string.Empty;
            if (ValidateIsNull(ref messageTip, value, "名称"))
                return true;
            if (_nameDict.ContainsKey(value))
            {
                messageTip = "[{0}]在导入列表中重复存在".L10nFormat(value);
                return false;
            }
            Station ps = RT.Service.Resolve<StationController>().GetStationByName(value);
            if (ps != null)
            {
                messageTip = "[{0}]已经存在".L10nFormat(value);
                return false;
            }
            _nameDict.Add(value, value);

            return isValid;
        }

        /// <summary>
        /// 生产资源验证
        /// </summary>
        /// <param name="obj">结果值</param>
        /// <param name="messageTip">错误消息</param>
        /// <param name="row">当前行</param>
        /// <returns>验证通过返回true，不通过返回false</returns>
        private bool ValidateResource(object obj, out string messageTip, DataRow row)
        {
            bool isValid = true;
            string value = obj.ToString();
            messageTip = string.Empty;
            if (ValidateIsNull(ref messageTip, value, "资源编码"))
                return true;
            if (!_resourceDict.ContainsKey(value))
            {
                WipResource ps = RT.Service.Resolve<WipResourceController>().GetWipResourceByCode(value);
                if (ps != null)
                {
                    _resourceDict.Add(value, ps.Id);
                }
                else
                {
                    messageTip = "[{0}]不存在".L10nFormat(value);
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

