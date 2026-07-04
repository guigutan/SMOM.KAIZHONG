using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.AssetDisposals;
using SIE.EMS.Equipments.Accounts.ViewModels;
using SIE.EMS.SpareParts;
using SIE.Equipments.Configs;
using SIE.Web.Common.Prints;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.AssetDisposals.DataQueryer
{
    /// <summary>
    /// 资产处置查询器
    /// </summary>
    public class AssetDisposalDataQueryer : Data.DataQueryer
    {
        /// <summary>
        /// 获取处置单号
        /// </summary>
        /// <returns>处置单号</returns>
        public string GetAssetDisposalNo()
        {
            var code = RT.Service.Resolve<AssetDisposalController>().GetNo();
            return code;
        }

        /// <summary>
        /// 获取审批流配置信息
        /// </summary>
        /// <returns>审批流配置信息</returns>
        public ApprovalConfigValue GetApprovalConfigValue()
        {
            var configValue = RT.Service.Resolve<AssetDisposalController>().GetApprovalFlowConfigValue();
            return configValue;
        }

        /// <summary>
        /// 获取批次号
        /// </summary>
        /// <returns>批次号</returns>
        public object GetLotNo()
        {
            var code = RT.Service.Resolve<SparePartController>().GetBatchCode();
            return code;
        }

        /// <summary>
        /// 获取新增的序列号备件回收清单
        /// </summary>
        /// <param name="formEntity">备件回收清单实体</param>
        /// <returns>新增的序列号备件回收清单集合</returns>
        public EntityList<AssetDisposalSparePart> GetAddAssetDisposalSpareParts(AssetDisposalSparePart formEntity)
        {
            var list = new EntityList<AssetDisposalSparePart>();
            var snList = RT.Service.Resolve<SparePartController>().GetSnCode(formEntity.Qty);

            foreach (var sn in snList)
            {
                var item = new AssetDisposalSparePart();
                item.SparePartId = formEntity.SparePartId;
                item.SparePartCode = formEntity.SparePart.SparePartCode;
                item.SparePartName = formEntity.SparePart.SparePartName;
                item.Specification = formEntity.SparePart.Specification;
                item.SpartType = formEntity.SparePart.SpartType;
                item.ControlMethod = formEntity.SparePart.ControlMethod;
                item.UnitName = formEntity.SparePart.Unit.Name;
                item.Sn = sn;
                item.Qty = 1;
                item.QualityStatus = formEntity.QualityStatus;
                item.WarehouseId = formEntity.WarehouseId;
                list.Add(item);
            }
            return list;
        }

        /// <summary>
        /// 新增的备件序列号打印
        /// </summary>
        /// <param name="assetSparePartList">新增的备件回收列表</param>
        /// <param name="templateId">打印模板Id</param>
        /// <returns>打印结果</returns>
        public QRCodePrintResult AddSnPrint(List<AssetDisposalSparePart> assetSparePartList, double templateId)
        {
            var prtResult = new QRCodePrintResult();

            try
            {
                PrintTemplate template = RF.GetById<PrintTemplate>(templateId);
                var report = ReportFactory.Current.GetReportByExtension(template.Type);
                var printable = new AssetDisposalSparePartPrintable();
                prtResult.ErrMsg = string.Empty;
                if (template.EntityType != typeof(AssetDisposalSparePartPrintable).GetQualifiedName())
                {
                    throw new ValidationException("打印模板错误，请选择【备件回收条码】类型的模板！".L10N());
                }
                prtResult.Type = template.Type;

                prtResult.Url = report.PrintProcess(printable, template.Id, template.Content, () =>
                {
                    return assetSparePartList;
                });
            }
            catch (Exception exc)
            {
                prtResult.ErrMsg = exc.Message;
            }
            return prtResult;
        }

        /// <summary>
        /// 序列号打印
        /// </summary>
        /// <param name="barcodeIds">条码所属实体Id列表</param>
        /// <param name="templateId">打印模板Id</param>
        /// <returns>打印结果</returns>
        public QRCodePrintResult SnPrint(List<double> barcodeIds, double templateId)
        {
            var prtResult = new QRCodePrintResult();

            try
            {
                PrintTemplate template = RF.GetById<PrintTemplate>(templateId);
                var report = ReportFactory.Current.GetReportByExtension(template.Type);
                var printable = new AssetDisposalSparePartPrintable();
                prtResult.ErrMsg = string.Empty;
                if (template.EntityType != typeof(AssetDisposalSparePartPrintable).GetQualifiedName())
                {
                    throw new ValidationException("打印模板错误，请选择【备件回收条码】类型的模板！".L10N());
                }
                prtResult.Type = template.Type;
                var snList = RT.Service.Resolve<AssetDisposalController>().GetAssetDisposalSparePartListByIds(barcodeIds);
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
