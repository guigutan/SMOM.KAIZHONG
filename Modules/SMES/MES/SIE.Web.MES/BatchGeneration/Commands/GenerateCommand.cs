using SIE.Barcodes.WipBatchs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.BatchGeneration;
using SIE.MES.BatchGeneration.Services;
using SIE.MES.WIP;
using SIE.Web.Command;
using SIE.Web.Json;
using System;

namespace SIE.Web.MES.BatchGeneration.Commands
{
    /// <summary>
    /// 生成命令
    /// </summary>
    [JsCommand("SIE.Web.MES.BatchGeneration.Commands.GenerateCommand")]
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
            var model = args.Data.ToJsonObject<BatchGeneratingViewModel>();
            var brokenList = model.Validate();
            EntityJson json = new EntityJson();

            if (brokenList.Count > 0)
            {
                throw new ValidationException(brokenList.ToString(";\r\n", MetaModel.RuleLevel.Error));
            }
            if (model.NumberRuleId == null)
            {
                throw new ValidationException("编码规则不能为空，请检查".L10N());
            }
            if (model.BatchWoId == null)
            {
                throw new ValidationException("批次工单不能为空，请检查".L10N());
            }

            BatchBarcodeInfo info = new BatchBarcodeInfo
            {
                WorkOrderId = (double)model.BatchWoId,
                NumberRuleId = (double)model.NumberRuleId,
                BatchQty = model.BatchQty,
                GeneratingQty = 1,
            };
            if (model.ProcessId <= 0)
            {
                throw new ValidationException("工作单元-工序不能为空".L10N());
            }

            if (model.StationId <= 0)
            {
                throw new ValidationException("工作单元-工位不能为空".L10N());
            }
            if (model.ResourceId <= 0)
            {
                throw new ValidationException("工作单元-资源不能为空".L10N());
            }
            var workeCell = new Workcell()
            {

                EmployeeId = RT.IdentityId,
                ProcessId = model.ProcessId,
                StationId = model.StationId,
                ResourceId = model.ResourceId,
            };
            var batchWoAndBarcodes = RT.Service.Resolve<WOBatchGenerationService>().BatchsGeneratingAndMove(info, workeCell);

            if (batchWoAndBarcodes != null)
            {
                var batchWo = batchWoAndBarcodes.Item1;
                decimal generatedQty = RT.Service.Resolve<WipBatchController>().CountGenerateBarcode(batchWo.Id);
                //model.BatchWo = batchWo;
                model.GeneratedQty = generatedQty;
                model.NotGenerateQty = batchWo.PlanQty - generatedQty + batchWo.ScrapQty;
                model.GenerateingQty = batchWo.PlanQty - generatedQty + batchWo.ScrapQty;
                json.SetProperty("errorMsg", string.Empty);
                json.SetProperty("GeneratedQty", generatedQty);
                json.SetProperty("NotGenerateQty", model.NotGenerateQty);
                json.SetProperty("GenerateingQty", model.GenerateingQty);
            }

            return json;
        }
    }
}
