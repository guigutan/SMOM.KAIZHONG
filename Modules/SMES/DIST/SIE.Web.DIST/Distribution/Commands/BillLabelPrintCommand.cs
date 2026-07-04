using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.DIST;
using SIE.DIST.Distribution.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Web.Command;
using System;
using System.Linq;

namespace SIE.Web.DIST.Distribution.Commands
{
    /// <summary>
    /// 标签打印命令
    /// </summary>
    [JsCommand("SIE.Web.DIST.Distribution.Commands.BillLabelPrintCommand")]
    public class BillLabelPrintCommand : ListViewCommand
    {
        /// <summary>
        /// 添加命令返回默认模型
        /// </summary>
        /// <param name="args">a</param>
        /// <param name="scope">s</param>
        /// <returns>实体</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<DistributionBill>();
            var entity = CreateNewItem(data);
            return entity;
        }

        /// <summary>
        /// 创建编辑对象
        /// </summary>
        /// <param name="bill">配送单</param>
        /// <returns>配送单退料标签打印视图模型</returns>
        private object CreateNewItem(DistributionBill bill)
        {
            var config = ConfigService.GetConfig(new BillLabelConfig(), typeof(DistributionBill));
            if (config == null || config.NumberRule == null)
                throw new ValidationException("配送单退货标签生成规则配置项为空".L10N());
            if (config.PrintTemplate == null)
                throw new ValidationException("标签生成规则配置项的模板为空".L10N());
            ////生成单体条码批次
            var numberController = RT.Service.Resolve<NumberRuleController>();
            string labelNumber = string.Empty;
            string nGLabelNumber = string.Empty;
            if (bill.ReturnQty > 0)
            {
                var acceptBatchNo = numberController.GenerateSegment(config.NumberRule.Id, 1).FirstOrDefault();
                if (!acceptBatchNo.IsNotEmpty())
                    throw new ValidationException("配送单正常退货标签条码生成失败".L10N());
                labelNumber = acceptBatchNo;
            }

            if (bill.NgReturnQty > 0)
            {
                var negtiveBatchNo = numberController.GenerateSegment(config.NumberRule.Id, 1).FirstOrDefault();
                if (!negtiveBatchNo.IsNotEmpty())
                    throw new ValidationException("配送单不良退货标签条码生成失败".L10N());
                nGLabelNumber = negtiveBatchNo;
            }

            return new { labelNumber, nGLabelNumber, ItemCode = bill.Item.Code, ItemName = bill.Item.Name, bill.NgReturnQty, bill.ReturnQty };
        }
    }
}
