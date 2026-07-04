using SIE.Common.ImportHelper;
using SIE.Web.Common.Import;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SIE.Web.EMS.EquipMaint.Common.Commands
{
    /// <summary>
    /// 导入公共方法
    /// </summary>
    public abstract class ImportDataCommonCommand : ImportCommandBase
    {
        /// <summary>
        /// 重载执行方法，添加自定义下载模板方法
        /// </summary>
        /// <param name="importViewArgs"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ImportViewArgs importViewArgs, string scope)
        {
            string behaviorName = importViewArgs.BehaviorName;
            if (behaviorName == "DownloadTemplate")
            {
                //自定义下载模板方法
                return DownloadTemplate(importViewArgs, scope);
            }
            return base.Excute(importViewArgs, scope);
        }

        /// <summary>
        /// 导入处理逻辑
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>导入结果信息</returns>
        protected override object ImportData(ImportViewArgs args, string sheetName = "", bool isFirstRowColumn = true)
        {
            DataTable dataTable = new SpreadsheetHelper().Base64ToDataTable(args.Data, sheetName, isFirstRowColumn);
            ImportDataHandleQms importDataHandleExt = new ImportDataHandleQms(args.SelectedParentId);
            string text = importDataHandleExt.ImportProcess(dataTable, this.GetImportHandleType(), this.GetImportCompleted(), "");
            return new
            {
                ImportSuccessNum = importDataHandleExt.DrSuccess == null ? 0 : importDataHandleExt.DrSuccess.Length,
                ImportMsg = (importDataHandleExt.DrSuccess == null && importDataHandleExt.DrFailed == null) ? text : "导入成功数据【{0}】条，失败数据【{1}】条。".FormatArgs(new object[]
                {
                    importDataHandleExt.DrSuccess.Length,
                    importDataHandleExt.DrFailed.Length
                }),
                FailedJson = this.GetFailedJsonStore(args.SelectedParentId, importDataHandleExt.DrFailed)
            };
        }

        /// <summary>
        /// 自定义下载模板方法
        /// </summary>
        /// <param name="importViewArgs"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected virtual object DownloadTemplate(ImportViewArgs importViewArgs, string scope)
        {
            var pathInfo = SetTemplateFileName(importViewArgs, scope);
            return new
            {
                FileName = pathInfo.FileName,
                FilePath = pathInfo.FullPath
            };
        }

        /// <summary>
        /// 设置模板文件信息
        /// </summary>
        /// <param name="importViewArgs"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected virtual TemplatePathInfo SetTemplateFileName(ImportViewArgs importViewArgs, string scope)
        {
            return new TemplatePathInfo
            {
                FullPath = "",
                FileName = ""
            };
        }

        /// <summary>
        /// 导入完成后处理
        /// </summary>
        /// <returns></returns>
        protected override ImportCompleted GetImportCompleted()
        {
            return (DataRow[] drSuccess, DataRow[] drFailed) =>
            {

            };
        }
    }

    /// <summary>
    /// 模板文件信息
    /// </summary>
    public struct TemplatePathInfo
    {
        /// <summary>
        /// 路径 
        /// </summary>
        public string FullPath { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
    }

    /// <summary>
    /// 销售订单导入处理
    /// </summary>
    public class ImportDataHandleQms : ImportDataHandleExt
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="parentId"></param>
        public ImportDataHandleQms(double parentId) : base(parentId)
        {
        }

        /// <summary>
        /// 重载文档列校验，添加列名比较
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnList"></param>
        /// <returns></returns>
        protected override bool ValidColumnNames(DataTable dt, List<string> columnList)
        {
            bool isSame = true;
            if (dt.Columns.Count != columnList.Count)
            {
                isSame = false;
                return isSame;
            }

            //比较每一列的名称
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (string.Compare(dt.Columns[i].ColumnName.Trim(), columnList[i].Trim(), true) != 0)
                {
                    isSame = false;
                    break;
                }
            }
            return isSame;
        }
    }
}
