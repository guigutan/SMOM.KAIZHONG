using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Equipments.Accounts.ViewModels;
using SIE.EMS.InventoryTasks;
using SIE.EMS.InventoryTasks.ViewModels;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.EMS.SpareParts.Printables;
using SIE.Equipments.EquipAccounts;
using SIE.Web.Common.Prints;
using SIE.Web.Data;
using System;

namespace SIE.Web.EMS.InventoryTasks
{
    /// <summary>
    /// 盘点任务查询器
    /// </summary>
    public class InventoryTaskDataQueryer : DataQueryer
    {
        /// <summary>
        /// 新增盘盈
        /// </summary>
        /// <param name="model">新增盘盈信息</param>
        public void AddProfit(AddProfitViewModel model)
        {
            RT.Service.Resolve<InventoryTaskController>().AddProfit(model);
        }

        /// <summary>
        /// 新增盘盈
        /// </summary>
        /// <param name="model"></param>
        public void AddFixtureProfit(AddFixtureProfitViewModel model)
        {
            RT.Service.Resolve<InventoryTaskController>().AddFixtureProfit(model);
        }
        /// <summary>
        /// 新增盘盈查询
        /// </summary>
        /// <param name="code">设备编码</param>
        /// <returns>设备</returns>
        public EquipAccount AddProfitQuery(string code)
        {
            return RT.Service.Resolve<EquipAccountController>().GetEquipAccountsByCode(code);
        }

        /// <summary>
        /// 新增盘盈
        /// </summary>
        /// <param name="model"></param>
        public double AddSparePartProfit(AddSparePartProfitViewModel model)
        {
            return RT.Service.Resolve<InventoryTaskSpartPartController>().AddProfit(model);
        }

        /// <summary>
        /// 序列号打印
        /// </summary>
        /// <param name="spareDetailId">备件盘点明细ID</param>
        /// <param name="printInfo">打印信息</param>
        /// <returns>打印结果</returns>
        public QRCodePrintResult PrintAddSparePartProfitViewModel(double spareDetailId, AddSparePartProfitPrintViewModel printInfo)
        {
            if (printInfo == null)
            {
                throw new ValidationException("数据异常".L10N());
            }
            var prtResult = new QRCodePrintResult();
            try
            {
                PrintTemplate template = RF.GetById<PrintTemplate>(printInfo.TemplateId);
                var report = ReportFactory.Current.GetReportByExtension(template.Type);

                var sparePartDetail = RT.Service.Resolve<InventoryTaskSpartPartController>().GetInventoryTaskSparePartDetailById(spareDetailId);

                if (sparePartDetail.ControlMethod == ControlMethod.Batch)
                {
                    var printable = new StoreSummaryLotPrintable();
                    prtResult.ErrMsg = string.Empty;
                    if (template.EntityType != typeof(StoreSummaryLotPrintable).GetQualifiedName())
                    {
                        throw new ValidationException("打印模板错误，请选择【备件批次标签打印】类型的模板！".L10N());
                    }

                    prtResult.Type = template.Type;

                    EntityList< StoreSummaryLot> storeSummaryLots = new EntityList<StoreSummaryLot>();
                    storeSummaryLots.Add(new StoreSummaryLot()
                    {
                        SparePartCode = sparePartDetail.SparePartCode,
                        BatchNumber= sparePartDetail.LotNo,
                    });

                    prtResult.Url = report.PrintProcess(printable, template.Id, template.Content, () =>
                    {
                        return storeSummaryLots;
                    });
                }
                else if (sparePartDetail.ControlMethod == ControlMethod.Sn)
                {
                    var printable = new StoreSummaryDetailPrintable();
                    prtResult.ErrMsg = string.Empty;

                    if (template.EntityType != typeof(StoreSummaryDetailPrintable).GetQualifiedName())
                    {
                        throw new ValidationException("打印模板错误，请选择【备件序列号标签打印】类型的模板！".L10N());
                    }

                    prtResult.Type = template.Type;

                    EntityList<StoreSummaryDetail> storeSummaryDetails = new EntityList<StoreSummaryDetail>();
                    storeSummaryDetails.Add(new StoreSummaryDetail()
                    {
                        SparePartCode = sparePartDetail.SparePartCode,
                        OrderNumberCode = sparePartDetail.LotNo,
                    });

                    prtResult.Url = report.PrintProcess(printable, template.Id, template.Content, () =>
                    {
                        return storeSummaryDetails;
                    });
                }
            }
            catch (Exception exc)
            {
                prtResult.ErrMsg = exc.Message;
            }
            return prtResult;
        }
    }
}
