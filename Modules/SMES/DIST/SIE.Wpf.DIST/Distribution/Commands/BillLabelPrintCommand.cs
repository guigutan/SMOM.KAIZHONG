using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Common.Properties;
using SIE.DIST;
using SIE.DIST.Distribution.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.WorkOrders;
using SIE.Packages.ItemLabels;
using SIE.Wpf.Command;
using SIE.Wpf.Common.Prints;
using System;
using System.Linq;

namespace SIE.Wpf.DIST.Distribution
{
    /// <summary>
    /// 标签打印命令
    /// </summary>
    [Command(ImageName = "Printer", Label = "标签打印", GroupType = CommandGroupType.Edit)]
    public class BillLabelPrintCommand : ListViewCommand
    {
        /// <summary>
        /// 判断标签打印命令能否执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>能执行返回true，不能执行返回false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var bill = view.Current as DistributionBill;
            return bill != null && (bill.PrintQty < bill.ReturnQty + bill.NgReturnQty);
        }

        /// <summary>
        /// 标签打印命令执行方法
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var entity = CreateNewItem();
            var template = new DetailsUITemplate(typeof(BillLabelPrintViewModel));
            var ui = template.CreateUI();
            ui.MainView.Data = entity;
            var result = CRT.Workbench.ShowDialog(ui, w =>
             {
                 w.Title = "标签打印".L10N();
                 w.Width = 600;
                 w.Height = 300;
             });
            if (result == 0)
            {
                Save(entity);
            }
        }

        /// <summary>
        /// 创建编辑对象
        /// </summary>
        /// <returns>配送单退料标签打印视图模型</returns>
        private BillLabelPrintViewModel CreateNewItem()
        {
            var bill = View.Current as DistributionBill;
            var model = new BillLabelPrintViewModel()
            {
                Bill = bill
            };
            var config = ConfigService.GetConfig(new BillLabelConfig(), typeof(DistributionBill));
            if (config == null || config.NumberRule == null)
                throw new ValidationException("配送单退货标签生成规则配置项为空".L10N());
            if (config.PrintTemplate == null)
                throw new ValidationException("标签生成规则配置项的模板为空".L10N());
            ////生成单体条码批次
            var numberController = RT.Service.Resolve<NumberRuleController>();
            if (bill.ReturnQty > 0)
            {
                var acceptBatchNo = numberController.GenerateSegment(config.NumberRule.Id, 1).FirstOrDefault();
                if (!acceptBatchNo.IsNotEmpty())
                    throw new ValidationException("配送单正常退货标签条码生成失败".L10N());
                model.LabelNumber = acceptBatchNo;
            }

            if (bill.NgReturnQty > 0)
            {
                var negtiveBatchNo = numberController.GenerateSegment(config.NumberRule.Id, 1).FirstOrDefault();
                if (!negtiveBatchNo.IsNotEmpty())
                    throw new ValidationException("配送单不良退货标签条码生成失败".L10N());
                model.NGLabelNumber = negtiveBatchNo;
            }

            return model;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="model">待保存实体</param>
        void Save(BillLabelPrintViewModel model)
        {
            try
            {
                var config = ConfigService.GetConfig(new BillLabelConfig(), typeof(DistributionBill));
                string filePath = RT.Service.Resolve<PrintsController>().DownloadPrintTemplate(config.PrintTemplateId.Value);
                var controller = RT.Service.Resolve<ItemLabelController>();
                var printable = new ItemLabelPrintable();
                var report = ReportFactory.Current.GetReportByExtension(config.PrintTemplate.Type);
                EntityList<ItemLabel> prints = new EntityList<ItemLabel>();
                var factoryId = RT.Service.Resolve<IWorkOrderQuery>().GetWorkOrderFactoryId(model.Bill.WorkOrderId);
                if (model.Bill.ReturnQty > 0)
                {
                    var itemLabel = controller.CreateItemLabel(model.Bill.Item, model.Bill.ReturnQty, model.LabelNumber,
                        LabelSource.Distribution, model.Bill.WorkOrderId, factoryId, itemExtProp: string.Empty, itemExtPropName: string.Empty);
                    prints.Add(itemLabel);
                }

                if (model.Bill.NgReturnQty > 0)
                {
                    var itemLabel = controller.CreateItemLabel(model.Bill.Item, model.Bill.NgReturnQty, model.NGLabelNumber,
                        LabelSource.Distribution, model.Bill.WorkOrderId, factoryId, itemExtProp: string.Empty, itemExtPropName: string.Empty);
                    prints.Add(itemLabel);
                }

                model.Bill.PrintQty += (model.Bill.ReturnQty + model.Bill.NgReturnQty);
                Print(filePath, printable, report, prints);
            }
            catch (Exception ex)
            {
                CRT.MessageService.ShowException(ex);
                return;
            }

            RF.Save(model.Bill);
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="filePath">模板路径</param>
        /// <param name="printable">标签打印</param>
        /// <param name="report">报表接口</param>
        /// <param name="prints">物料标签集合</param>
        private void Print(string filePath, ItemLabelPrintable printable, IReport report, EntityList<ItemLabel> prints)
        {
            report.Print(printable, filePath, Settings.Default.PrinterName, () =>
            {
                return prints;
            }, () =>
            {
                CRT.MessageService.ShowMessage("打印成功".L10N(), "操作提示".L10N());
            });
        }
    }
}
