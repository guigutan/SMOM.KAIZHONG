using SIE.Common.ImportHelper;
using SIE.MES.Routings.RoutingBoms;
using SIE.MES.Routings.RoutingBoms.ImportBoms;
using SIE.Web.Command;
using SIE.Web.Common.Import;
using SIE.Web.Common.Import.Commands;
using System;
using System.Data;

namespace SIE.Web.MES.Routings.RoutingBoms.Commands
{
    /// <summary>
    /// 工序bom明细导入处理
    /// </summary>
    [JsCommand("SIE.Web.MES.Routings.RoutingBoms.Commands.RoutingBomDetailImportCommand")]
    public class ImportRoutingBomDetail : ImportCommandBase
    {
        /// <summary>
        /// 导入完成回调方法
        /// </summary>
        /// <returns>导入结果</returns>
        protected override ImportCompleted GetImportCompleted()
        {
            return (DataRow[] drSuccess, DataRow[] drFailed) =>
            {
            };
        }

        /// <summary>
        /// 获取导入处理类型
        /// </summary>
        /// <returns>导入处理类</returns>
        protected override Type GetImportHandleType()
        {
            return typeof(ImportRoutingBomDetailHandle);
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
            var bomImportInfo = args.ExtendData.ToJsonObject<BomImportInfo>();
            var attachmentId = RT.Service.Resolve<RoutingBomController>().SaveBomAttachment(args.Data, args.SelectedParentId, bomImportInfo.FileName, bomImportInfo.FileSize);

            //1.0 获取datatable数据
            var dataTable = GetDataTable(args.Data, sheetName);
            //处理导入逻辑
            ImportDataHandleExt importDataHandle = new ImportDataHandleExt(attachmentId);
            //导入数据
            var resultMsg = importDataHandle.ImportProcess(dataTable, GetImportHandleType(), GetImportCompleted());
            return new
            {
                ImportSuccessNum = importDataHandle.DrSuccess?.Length,
                ImportMsg = importDataHandle.DrSuccess == null && importDataHandle.DrFailed == null ? resultMsg :
                        ("导入成功数据【{0}】条，失败数据【{1}】条。".FormatArgs(importDataHandle.DrSuccess.Length, importDataHandle.DrFailed.Length)),
                FailedJson = GetFailedJsonStore(args.SelectedParentId, importDataHandle.DrFailed)
            };


        }

        /// <summary>
        /// 直接从服务器上下载模板
        /// </summary>
        /// <returns></returns>
        public virtual object DownloadTemplate()
        {
            const string templateFileName = "工序Bom导入模板.xlsx";
            const string fileRelativePath = "Templates/" + templateFileName;

            return new
            {
                FilePath = fileRelativePath,
                FileName = templateFileName
            };
        }

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="importViewArgs">导入视图参数</param>
        /// <param name="scope">使用范围</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ImportViewArgs importViewArgs, string scope)
        {
            if (importViewArgs.BehaviorName == "DownloadCustom")
            {
                return DownloadTemplate();
            }
            else
            {
                return ImportData(importViewArgs);
            }
        }

        /// <summary>
        /// 导入信息
        /// </summary>
        class BomImportInfo { 
            /// <summary>
            /// 文件名
            /// </summary>
            public string FileName { get; set; }
            /// <summary>
            /// 文件长度
            /// </summary>
            public double FileSize { get; set; }
        }
    }
}
