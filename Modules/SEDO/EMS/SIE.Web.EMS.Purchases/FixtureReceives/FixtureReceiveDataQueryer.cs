using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Equipments.Accounts.ViewModels;
using SIE.EMS.Purchases.FixtureAcceptances;
using SIE.EMS.Purchases.FixtureReceives;
using SIE.EMS.Purchases.FixtureReceives.Model;
using SIE.Web.Common.Prints;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.Purchases.FixtureReceives
{
    /// <summary>
    /// 工治具接收数据查询器
    /// </summary>
    public class FixtureReceiveDataQueryer : DataQueryer
    {
        /// <summary>
        /// 创建一个新的工治具接收
        /// </summary>
        /// <returns>新的设备接收</returns>
        public FixtureReceive GetNewFixtureReceive()
        {
            return RT.Service.Resolve<FixtureReceiveController>().GetNewFixtureReceive();
        }

        /// <summary>
        /// 获取工治具信息
        /// </summary>
        /// <returns></returns>
        public FixtureEncodeInfo GetFixtureInfo(double Id)
        {
            return RT.Service.Resolve<FixtureReceiveController>().GetFixtureEncodeInfo(Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiveId"></param>
        /// <returns></returns>
        public ReceiveScanViewModel GetReceiveScanInfo(double receiveId)
        {
            return RT.Service.Resolve<FixtureReceiveController>().GetReceiveScanInfo(receiveId);
        }

        /// <summary>
        /// 获取扫描的子列表数据
        /// </summary>
        /// <returns></returns>
        public Tuple<EntityList<FixtureReceiveDetail>, EntityList<FixtureReceiveSn>> LoadScanChildData(double fixtureReceiveId)
        {
            var fixtureReceiveDetailList = RT.Service.Resolve<FixtureReceiveController>().GetDetailsByReceiveId(fixtureReceiveId, null);
            var fixtureReceiveSn = RT.Service.Resolve<FixtureReceiveSnController>().GetFixtureReceiveSnByReceiveId(fixtureReceiveId);
            return new Tuple<EntityList<FixtureReceiveDetail>, EntityList<FixtureReceiveSn>>(fixtureReceiveDetailList, fixtureReceiveSn);
        }

        /// <summary>
        /// 保存接收扫描
        /// </summary>
        /// <param name="model"></param>
        /// <param name="details"></param>
        /// <param name="snList"></param>
        public void SaveReceiveScan(ReceiveScanViewModel model, IList<FixtureReceiveDetail> details, IList<FixtureReceiveSn> snList)
        {
            RT.Service.Resolve<FixtureReceiveScanController>().SaveReceiveScan(model, details, snList);
        }

        /// <summary>
        /// 接收执行
        /// </summary>
        /// <param name="code"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ReceiveExecuteInfo ReceiveExecute(string code, ReceiveScanViewModel model)
        {
            return RT.Service.Resolve<FixtureReceiveScanController>().ReceiveExecute(code, model);
        }

        /// <summary>
        /// 序列号打印
        /// </summary>
        /// <param name="barcodeIds">条码id列表</param>
        /// <param name="printInfo">打印信息</param>
        /// <returns>打印结果</returns>
        public QRCodePrintResult SnPrint(List<double> barcodeIds, SIE.EMS.Purchases.EquipmentReceives.ReceiveSnPrintViewModel printInfo)
        {
            if (printInfo == null)
            {
                throw new ValidationException("数据异常".L10N());
            }
            var prtResult = new QRCodePrintResult();
            try
            {
                PrintTemplate template = RF.GetById<PrintTemplate>(printInfo.TemplateId);
                if (template.State == State.Disable)
                {
                    throw new ValidationException("打印模板禁用状态,不可使用，请确认".L10N());
                }
                var report = ReportFactory.Current.GetReportByExtension(template.Type);
                var printable = new FixtureReceiveSnPrintable();
                prtResult.ErrMsg = string.Empty;
                if (template.EntityType != typeof(FixtureReceiveSnPrintable).GetQualifiedName())
                {
                    throw new ValidationException("打印模板错误，请选择【工治具接收序列号】类型的模板！".L10N());
                }
                prtResult.Type = template.Type;
                var snList = RT.Service.Resolve<FixtureReceiveController>().GetFixtureReceiveSnIds(barcodeIds);
                if (!snList.Any())
                {
                    throw new ValidationException("未选择打印数据，请选择".L10N());
                }
                prtResult.Url = report.PrintProcess(printable, template.Id, template.Content, () =>
                {
                    return snList;
                });
            }
            catch (Exception exc)
            {
                prtResult.ErrMsg = exc.Message;
            }
            return prtResult;
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="model"></param>
        /// <param name="maxLineNo"></param>
        /// <returns></returns>

        public virtual IList<FixtureReceiveSn> SnDetermine(ReceiveScanViewModel model,int maxLineNo)
        {
            return RT.Service.Resolve<FixtureReceiveScanController>().SnDetermine(model, maxLineNo);
        }


        /// <summary>
        /// 序列号打印
        /// </summary>
        /// <param name="snList">序列号列表</param>
        /// <param name="printInfo">打印信息</param>
        /// <returns>打印结果</returns>
        public QRCodePrintResult DeterminePrint(List<FixtureReceiveSn> snList, ReceiveSnPrintViewModel printInfo)
        {
            if (printInfo == null)
            {
                throw new ValidationException("数据异常".L10N());
            }
            var prtResult = new QRCodePrintResult();
            try
            {
                PrintTemplate template = RF.GetById<PrintTemplate>(printInfo.TemplateId);
                if (template.State == State.Disable)
                {
                    throw new ValidationException("打印模板禁用状态,不可使用，请确认".L10N());
                }

                var report = ReportFactory.Current.GetReportByExtension(template.Type);
                var printable = new FixtureReceiveSnPrintable();
                prtResult.ErrMsg = string.Empty;
                if (template.EntityType != typeof(FixtureReceiveSnPrintable).GetQualifiedName())
                {
                    throw new ValidationException("打印模板错误，请选择【工治具接收序列号】类型的模板！".L10N());
                }
                prtResult.Type = template.Type;
                prtResult.Url = report.PrintProcess(printable, template.Id, template.Content, () =>
                {
                    return snList;
                });
            }
            catch (Exception exc)
            {
                prtResult.ErrMsg = exc.Message;
            }
            return prtResult;
        }
    }

}
