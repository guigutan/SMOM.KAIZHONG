using DevExpress.CodeParser;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Scripting.Utils;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SIE.Common.Import;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MES.Capacitys;
using SIE.MetaModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Threading;
using SIE.Web.Common.Export;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SIE.Web.MES.Capacitys.Commands
{
    /// <summary>
    /// 导入
    /// </summary>
    public class StandardCapacityImportCommand : ImportExcelCommand
    {
        private bool isSuccess = true;

        /// <summary>
        /// 先将全部数据存起来
        /// </summary>
        private IList<RowData> batchs { get; set; } = new List<RowData>();

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="batch"></param>
        protected override void OnSave(IList<RowData> batch)
        {
            var list = batch.Select(p => p.Entity as StandardCapacity).ToList();
            var inCodes = list.Select(p => p.ItemCode).Distinct().ToList();
            var pnCodes=list.Select(p=>p.ProcessCode).Distinct().ToList();
            var rnCodes=list.Select(p=>p.ResourceCode).Distinct().ToList();
            var itemList = RT.Service.Resolve<ItemController>().GetItemDrList(inCodes);
            var processList = RT.Service.Resolve<ProcessController>().GetProcessesList(pnCodes);
            var resourceList = RT.Service.Resolve <WipResourceController>().GetWipResourceByCodes(rnCodes);
            batch.ForEach(p =>
            {
                try
                {
                    var entity = p.Entity as StandardCapacity;
                    var item = itemList.FirstOrDefault(x => x.Code == entity.ItemCode);
                    if (item == null)
                        throw new ValidationException("物料编码【{0}】不存在".L10nFormat(entity.ItemCode));
                    entity.Item = item;
                    var process = processList.FirstOrDefault(x => x.Code == entity.ProcessCode);
                    if (process == null)
                        throw new ValidationException("工序编码【{0}】不存在".L10nFormat(entity.ProcessCode));
                    entity.Process = process;
                    var resource = resourceList.FirstOrDefault(x => x.Code == entity.ResourceCode);
                    if (resource == null)
                        throw new ValidationException("资源编码【{0}】不存在".L10nFormat(entity.ResourceCode));
                    entity.Resource = resource;
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                    importResult.MessageList.Add(new ImportMessageResult
                    {
                        RowNum = p.RowIndex + 1,
                        MsgType = ImportMessageType.SaveFail,
                        Message = ex.GetBaseException().Message
                    });
                }
            });
            batchs.AddRange(batch);
            //if (isSuccess)
            //    base.OnSave(batch);
        }

        protected override void OnComplete(string errors)
        {
            if (isSuccess && batchs.Count > 0)
                importResult.MessageList.AddRange(AppRuntime.Service.Resolve<ImportController>().Save2(batchs));

            base.OnComplete(errors);
        }
    }

}
