using DocumentFormat.OpenXml.Office2010.Excel;
using NPOI.SS.UserModel;
using SIE.Common.Import;
using SIE.Common.ImportHelper;
using SIE.Core.Common.Controllers;
using SIE.Core.Import;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.DashBoard.KzReport.ProductionProcesss;
using SIE.MES.ItemChecker;
using SIE.MetaModel;
using SIE.MetaModel.View;
using SIE.MetaModel.XmlConfig;
using SIE.Tech.Processs;
using SIE.Web.Common.Export;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SIE.Web.MES.DashBoard.ProductionProcesss.Commands
{
    /// <summary>
    /// 导入
    /// </summary>
    public class ProductionProcessImportCommand : ImportExcelCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="batch"></param>
        protected override void OnSave(IList<RowData> batch)
        {
            List<string> invOrgCurr = new List<string>();
            List<string> processCodes = new List<string>();
            batch.ForEach(p =>
            {
                var entity = p.Entity as ProductionProcess;
                invOrgCurr.Add(entity.InventoryCode);
                processCodes.Add(entity.ProcessCode);
            });
            base.OnSave(batch);
        }

        protected override object ImportData(ImportViewArgs importViewArgs)
        {
            Tuple<FileType, byte[]> tuple = FileStreamHelper.Base64ToExcel(importViewArgs.Data);
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.Write(tuple.Item2);
            memoryStream.Seek(0L, SeekOrigin.Begin);
            new ProductionProcessImportImporter().Import(ref importResult, ImportType, ImportView, memoryStream, OnRowDataRead, OnSave, delegate (string errors)
            {
                OnComplete(errors);
            });
            return importResult;
        }
    }

    public class ProductionProcessImportImporter : CommonImporter
    {

        protected override Dictionary<string, object> GetRefData(CacheData cacheData, EntityPropertyViewMeta property)
        {
            return new Dictionary<string, object>() ;
        }
        protected override object GetRefId(Dictionary<string, object> refData, EntityPropertyViewMeta property, string value)
        {
            object value2 = null;
            return value2;

        }
    }

}
