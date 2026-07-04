using SIE.Common.ImportHelper;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.WorkBenchCommon.Workbench.Base
{
    /// <summary>
    /// 布局管理导入
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportLayoutInfoHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportLayoutInfoHandle : IDisposable, IBusinessImport
    {
        /// <summary>
        /// 列名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
            "编码", "描述",  "布局内容"
        };

        #region 私有属性
        /// <summary>
        /// 编码-布局信息
        /// </summary>
        private Dictionary<string, string> dicLayoutInfo;
        #endregion

        /// <summary>
        /// 列的标准验证 (列名 列对应验证 )
        /// </summary>
        public Dictionary<string, ValidColumn> ColumnValidList { get; set; }

        /// <summary>
        /// 创建列的验证
        /// </summary>
        /// <returns>返回当前对象</returns>
        public IBusinessImport CreaetColumnValid()
        {
            this.ColumnValidList = new Dictionary<string, ValidColumn>();
            this.ColumnValidList.Add(ColumnNameList[0], new ValidColumn(ImportDataType._Custom, true, VaildLayoutCode));    // 编码
            this.ColumnValidList.Add(ColumnNameList[1], new ValidColumn(ImportDataType._String, false, true));           // 描述           
            this.ColumnValidList.Add(ColumnNameList[2], new ValidColumn(ImportDataType._String, false, true));            // 布局内容

            return this;
        }

        /// <summary>
        /// 释放数据
        /// </summary>
        public void Dispose()
        {
            if (dicLayoutInfo != null)
            {
                dicLayoutInfo.Clear();
                dicLayoutInfo = null;
            }
        }

        #region 验证数据
        /// <summary>
        /// 验证行号
        /// </summary>
        /// <param name="obj">行号</param>
        /// <param name="messageTip">信息提示</param>
        /// <param name="dr">当前行数据</param>
        /// <returns>是否验证通过</returns>
        private bool VaildLayoutCode(object obj, out string messageTip, DataRow dr)
        {
            bool isValid = true;
            messageTip = string.Empty;
            string layoutCode = obj.ToString();
            if (dicLayoutInfo == null)
            {
                dicLayoutInfo = new Dictionary<string, string>();
            }

            if (!dicLayoutInfo.ContainsKey(layoutCode))
            {
                var layout = RT.Service.Resolve<WorkbenchController>().GetLayoutByCode(layoutCode);
                if (layout != null)
                {
                    messageTip = "编码已存在,不能重复添加;".L10N();
                    AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                    isValid = false;
                    return isValid;
                }

                dicLayoutInfo.Add(layoutCode, layoutCode);
            }
            else
            {
                messageTip = "编码已存在,不能重复添加;".L10N();
                AppendErrorMsg(dr, ImportDataHandle.MessageColumnName, messageTip);
                isValid = false;
            }

            return isValid;
        }
        #endregion

        /// <summary>
        /// 业务数据处理
        /// </summary>
        /// <param name="drs">数据集合</param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            if (drs.Length == 0)
            {
                return;
            }

            var mainDataList = from g in drs
                               select new
                               {
                                   Code = g.Field<string>(ColIndex("编码")),
                                   Desc = g.Field<string>(ColIndex("描述")),
                                   Content = g.Field<string>(ColIndex("布局内容")),
                                   DetailInfo = g
                               };

            EntityList<LayoutInfo> layoutInfos = new EntityList<LayoutInfo>();
            foreach (var data in mainDataList)
            {
                layoutInfos.Add(new LayoutInfo
                {
                    Code = data.Code,
                    Description = data.Desc,
                    Content = data.Content,
                });
            }

            RT.Service.Resolve<SIE.Core.Common.Controllers.CommonController>().BatchInsertSave(layoutInfos);
        }

        /// <summary>
        /// 获取指定列名的索引
        /// </summary>
        /// <param name="columnName">指定列名</param>
        /// <returns>返回对应索引</returns>
        private int ColIndex(string columnName)
        {
            return ColumnNameList.IndexOf(columnName);
        }

        /// <summary>
        /// 给保存错误的数据行记录错误数据信息
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="columnName">列名</param>
        /// <param name="errorMsg">错误信息</param>
        public void AppendErrorMsg(DataRow row, string columnName, string errorMsg)
        {
            row[columnName] += errorMsg;
        }
    }
}
