using Microsoft.AspNetCore.StaticFiles;
using SIE.Common.Attachments;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Equipments;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.Equipments.Accounts.ViewModels;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.EMS.SpareParts.Printables;
using SIE.EMS.SpareParts.ViewModels;
using SIE.Web.Common.Prints;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SIE.Web.EMS.SpareParts.DataQuery
{
    /// <summary>
    /// 备件查询器
    /// </summary>
    public class SparePartDataQueryer : DataQueryer
    {
        /// <summary>
        /// 查看第一张图片附件
        /// </summary>
        /// <param name="Id">备件记录Id</param>
        /// <returns></returns>
        public object ViewFirstPictureAttachment(double Id)
        {
            var sparePart = RF.GetById<SparePart>(Id);
            string retFileName = "";
            string retFileContent = "";
            if (sparePart.PictureAttachmentList.Any())
            {
                string filePath = sparePart.PictureAttachmentList[0].FilePath;
                string fileName = sparePart.PictureAttachmentList[0].FileName;
                var fileBytes = RT.Service.Resolve<AttachmentController>().FileDownload(filePath, fileName);
                retFileContent = fileByteToBase64(fileBytes, fileName);
                retFileName = fileName;
            }
            return new { FileName = retFileName, FileContent = retFileContent };
        }

        /// <summary>
        /// 下载图片附件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public object DownLoadPictureAttachment(string filePath, string fileName)
        {
            var pathFileName = System.IO.Path.GetFileName(filePath);
            var fileBytes = RT.Service.Resolve<AttachmentController>().FileDownload(filePath, pathFileName);
            return new { FileName = fileName, FileContent = fileByteToBase64(fileBytes, pathFileName) };
        }

        private string fileByteToBase64(byte[] buffer, string fileName)
        {
            Check.NotNullOrWhiteSpace(fileName, nameof(fileName));
            Check.NotNull(buffer, nameof(buffer));

            var fileExt = Path.GetExtension(fileName);
            if (fileExt.IsNullOrWhiteSpace())
            {
                throw new ValidationException("{0}文件没有扩展名，无法解析对应的文件类型".L10nFormat(fileName));
            }
            var provider = new FileExtensionContentTypeProvider();
            var contentType = provider.Mappings[fileExt];
            if (contentType.IsNullOrWhiteSpace())
            {
                throw new ValidationException("{0}扩展名无法解析对应的类型，请在应用服务的{1}节点配置中进行维护".L10nFormat(fileExt, "MimeMap"));
            }
            return "data:{0};base64,{1}".FormatArgs(contentType, Convert.ToBase64String(buffer));
        }

        /// <summary>
        /// GetStoreDetailById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object GetStoreDetailById(double id)
        {
            return RT.Service.Resolve<SparePartController>().GetStoreDetailById(id);
        }

        /// <summary>
        /// 判断备件是否启用了WMS管控
        /// </summary>
        /// <returns></returns>
        public object VerifyIsWmsControl()
        {
            return RT.Service.Resolve<SparePartController>().IsWmsControl();
        }

        /// <summary>
        /// 备件条码入库查询
        /// </summary>
        /// <param name="barcode">备件条码</param>
        /// <param name="form">入库单头信息</param>
        /// <returns>入库条码扫描返回信息</returns>
        public SparePartStoreQueryInfo SparePartStoreBarcodeQuery(string barcode, SparePartStore form)
        {
            return RT.Service.Resolve<SparePartController>().SparePartStoreBarcodeQuery(barcode, form);
        }

        /// <summary>
        /// 备件条码入库查询
        /// </summary>
        /// <param name="barcode">备件条码</param>
        /// <param name="form">入库单头信息</param>
        /// <returns>入库条码扫描返回信息</returns>
        public SparePartStoreQueryInfo StoreDetailsBarcodeQuery(string barcode, SparePartStore form)
        {
            return RT.Service.Resolve<SparePartController>().StoreDetailsBarcodeQuery(barcode, form);
        }

        /// <summary>
        /// 获取入库单号
        /// </summary>
        /// <returns>入库单号</returns>
        public object GetSparePartStoreNo() 
        {
            var code = RT.Service.Resolve<SparePartController>().GetStoreCode();
            return code;
        }

        /// <summary>
        /// 获取批次号
        /// </summary>
        /// <returns>批次号</returns>
        public object GetBatchCode()
        {
            var code = RT.Service.Resolve<SparePartController>().GetBatchCode();
            return code;
        }

        /// <summary>
        /// 获取序列号
        /// </summary>
        /// <returns>序列号</returns>
        public object GetSnCode()
        {
            var code = RT.Service.Resolve<SparePartController>().GetSnCode(1).FirstOrDefault();
            return code;
        }

        /// <summary>
        /// 入库明细标签打印
        /// </summary>
        /// <param name="barcodeIds">条码所属实体Id列表</param>
        /// <param name="controlMethod">管控方式</param>
        /// <returns>打印结果</returns>
        public QRCodePrintResult LabelPrint(List<double> barcodeIds,int controlMethod)
        {
            var prtResult = new QRCodePrintResult();
            var clt = RT.Service.Resolve<SparePartController>();
            PrintTemplate template = null;

            var printType = Enum.Parse<ControlMethod>(controlMethod.ToString());

            if (printType == ControlMethod.Batch)
            {
                template = clt.GetStoreDetailLotPrintConfig();
            }
            else 
            {
                template = clt.GetStoreDetailSnPrintConfig();
            }

            try
            {
                prtResult.ErrMsg = string.Empty;
                prtResult.Type = template.Type;
                var report = ReportFactory.Current.GetReportByExtension(template.Type);
                var storeDetailList = clt.GetSparePartStoreDetailListByIds(barcodeIds);

                if (printType == ControlMethod.Batch)
                {
                    prtResult.Url = report.PrintProcess(new StoreDetailLotPrintable(), template.Id, template.Content, () =>
                    {
                        return storeDetailList;
                    });
                }
                else 
                {
                    prtResult.Url = report.PrintProcess(new StoreDetailSnPrintable(), template.Id, template.Content, () =>
                    {
                        return storeDetailList;
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
