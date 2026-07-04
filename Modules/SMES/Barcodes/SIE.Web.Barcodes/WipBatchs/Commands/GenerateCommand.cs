using SIE.Barcodes.WipBatchs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Web.Command;
using SIE.Web.Json;
using System;

namespace SIE.Web.Barcodes.WipBatchs.Commands
{
    /// <summary>
    /// 生成命令
    /// </summary>
    [JsCommand("SIE.Web.Barcodes.WipBatchs.GenerateCommand")]
    public class GenerateCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>json</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var model = args.Data.ToJsonObject<ViewModels.BatchGeneratingViewModel>();
            var brokenList = model.Validate();
            EntityJson json = new EntityJson();
            if (brokenList.Count > 0)
            {
                throw new ValidationException(brokenList.ToString(";\r\n", MetaModel.RuleLevel.Error));
            }
            if (!model.NumberRuleId.HasValue)
            {
                throw new ValidationException("批次编码规则不允许为空".L10N());
            }
            if (!model.BatchWoId.HasValue)
            {
                throw new ValidationException("批次工单不允许为空".L10N());
            }

          

            BatchBarcodeInfo info = new BatchBarcodeInfo
            {
                WorkOrderId = (double)model.BatchWoId,
                NumberRuleId = (double)model.NumberRuleId,
                BatchQty = model.BatchQty,
                GeneratingQty = model.GenerateingQty,
                GenerateChild = model.GenerateChildren,
                ChildNumberRuleId = model.ChildNumberRuleId ?? 0,
                ChildBatchQty = model.ChildBatchQty
            };

            Tuple<BatchWorkOrder, EntityList<WipBatch>> batchWoAndBarcodes = null;
            try
            {
                batchWoAndBarcodes = RT.Service.Resolve<WipBatchController>().BatchsGenerating(info);
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }

            if (batchWoAndBarcodes != null)
            {
                var batchWo = batchWoAndBarcodes.Item1;
                decimal generatedQty = RT.Service.Resolve<WipBatchController>().CountGenerateBarcode(batchWo.Id);
                model.BatchWo = batchWo;
                model.GeneratedQty = generatedQty;
                model.NotGenerateQty = batchWo.PlanQty - generatedQty;
                model.GenerateingQty = batchWo.PlanQty - generatedQty;
                json.SetProperty("errorMsg", string.Empty);
                json.SetProperty("GeneratedQty", generatedQty);
                json.SetProperty("NotGenerateQty", model.NotGenerateQty);
                json.SetProperty("GenerateingQty", model.GenerateingQty);
            }

            return json;
        }
    }
}
