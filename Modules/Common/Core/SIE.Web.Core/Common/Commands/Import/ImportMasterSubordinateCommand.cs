using SIE.Core.Import;
using SIE.MetaModel;
using SIE.Security;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Data;

namespace SIE.Web.Core.Common.Commands.Import
{
    /// <summary>
    /// 主从表导入命令
    /// </summary>
    public abstract class ImportMasterSubordinateCommand : ImportCommandBase
    {
        /// <summary>
        /// 
        /// </summary>
        protected ImportMasterSubordinateCommand()
        {

        }

        /// <summary>
        /// 创建主从结构信息
        /// </summary>
        /// <returns></returns>
        protected virtual MasterSubordinateImportData CreateMasterSubordinateImportData() {
            return new MasterSubordinateImportData();
        }

        /// <summary>
        /// 重载执行方法，添加自定义下载模板方法
        /// </summary>
        /// <param name="importViewArgs"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ImportViewArgs importViewArgs, string scope)
        {
            EntityMeta entityMeta = ClientEntities.Find(importViewArgs.Type);
            if (scope != entityMeta.EntityType.GetQualifiedName())
            {
                throw new SecurityException("参数type[{0}]与令牌不一致".L10nFormat(importViewArgs.Type));
            }
            string behaviorName = importViewArgs.BehaviorName;
            if (behaviorName == "DownloadTemplate")
            {
                //自定义下载模板方法
                return DownloadTemplate(importViewArgs, scope);
            }
            return ImportData(importViewArgs);
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
            //1.创建主从结构信息
            MasterSubordinateImportData import = this.CreateMasterSubordinateImportData();

            import.SubordinateDataTables = new List<DataTable>();
            //2.获取主表的Sheet数据
            import.MasterDataTable= GetDataTable(args.Data, import.MasterSheetName);
            import.MasterDataTable.TableName = import.MasterSheetName;

            //3.获取从表的Sheet数据
            foreach (var subordinateSheetName in import.SubordinateSheetNameList)
            {
                DataTable table = GetDataTable(args.Data, subordinateSheetName);
                table.TableName = subordinateSheetName;
                import.SubordinateDataTables.Add(table);
            }

            //4.创建主从表导入的处理对象
            ImportMasterSubordinateDataHandle handle = new ImportMasterSubordinateDataHandle();
            string text = handle.ImportProcess(import, GetImportHandleType(), GetImportCompleted());
            return new
            {
                ImportSuccessNum = handle.DrSuccessArray?.Length,
                ImportMsg = ((handle.DrSuccessArray == null && handle.DrFailedListArray == null) ? text : "导入成功数据【{0}】条，失败数据【{1}】条。".L10nFormat(handle.DrSuccessArray.Length, handle.DrFailedListArray.Length)),
                FailedJson = GetFailedJsonStore(args.SelectedParentId, handle.DrFailedListArray)
            };
        }




        #region 下载模板 有关
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


        #endregion
    }
}
