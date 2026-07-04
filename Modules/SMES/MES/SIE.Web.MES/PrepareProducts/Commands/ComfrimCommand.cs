using DevExpress.Office.Utils;
using Newtonsoft.Json;
using SIE.Common;
using SIE.Domain;
using SIE.MES.Outsourcing;
using SIE.MES.PrepareProducts;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SIE.Web.MES.PrepareProducts.Commands
{
    /// <summary>
    /// 委外入保存命令
    /// </summary>
    [JsCommand("SIE.Web.MES.PrepareProducts.Commands.ComfrimCommand")]
    public class ComfrimCommand : FormSaveCommand
    {
        //protected override void DoSave(Entity entity)
        //{
        //    var item = entity as PrepareRecord;
        //    RT.Service.Resolve<PrepareProductsController>().Comfrim(item.PrepareRecordDetail);
        //}
        protected override object Excute(ViewArgs args, string scope)
        {
            var prepareRecordDetail =JsonConvert.DeserializeObject <List < PrepareRecordDetail>> ( args.Data);

            RT.Service.Resolve<PrepareProductsController>().Comfrim(prepareRecordDetail.AsEntityList());
            return true;
        }
    }
}