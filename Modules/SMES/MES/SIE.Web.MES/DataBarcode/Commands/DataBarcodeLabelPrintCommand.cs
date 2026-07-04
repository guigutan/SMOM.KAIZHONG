
using SIE.Barcodes.Barcodes.ViewModels;
using SIE.Barcodes.Printables;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.MES.DataBarcode;
using SIE.Packages.ItemLabels;
using SIE.Web.Command;
using SIE.Web.Common.Prints;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIE.Web.MES.DataBarcode.Commands
{
        /// <summary>
        /// 批次信息补打
        /// </summary>
        public class DataBarcodeLabelPrintCommand : ListViewCommand
        {
            /// <summary>
            /// 补打命令执行方法
            /// </summary>
            /// <param name="args">args</param>
            /// <param name="scope">scope</param>
            /// <returns>补打结果</returns>
            protected override object Excute(ViewArgs args, string scope)
            {
                var rstPrint = new RstBarcodePrint();
                rstPrint.ErrMsg = string.Empty;

                try
                {
                    var entity = args.Data.ToJsonObject<BatchReprintDataModel>();
                    var template = RF.GetById<PrintTemplate>(entity.TemplateId);
                    Check.NotNull(template, nameof(PrintTemplate));

                //if (template.EntityType != typeof(WipBatchPrintable).GetQualifiedName())
                //{
                //    throw new ValidationException("打印模板的类型必须是生产批次".L10N());
                //}

                rstPrint.Type = template.Type;

                var wipBatchs = RT.Service.Resolve<DataBarcodeController>().GetDataBarcodePrintDatas(entity.DataBarcodeIds);
                if (wipBatchs[0].BarcodeType != template.FileName.Replace(".siedev", "")) {
                    throw new ValidationException("打印模板的类型【"+ wipBatchs[0].BarcodeType+ "】与选择的类型【"+ template.FileName.Replace(".siedev", "") + "】不一致".L10N());
                }
                var report = ReportFactory.Current.GetReportByExtension(template.Type);
                
                    var printable = new DataBarcodePrintable();

                    rstPrint.Url = report.PrintProcess(printable, template.Id, template.Content, () =>
                    {
                        return wipBatchs;
                    });
                }
                catch (Exception exc)
                {
                    rstPrint.ErrMsg = exc.Message;
                }

                return rstPrint;
            }


            /// <summary>
            /// 批次补打数据类
            /// </summary>
            public class BatchReprintDataModel
            {
                /// <summary>
                /// 选中条码Id列表
                /// </summary>
                public List<double> DataBarcodeIds { get; set; }

                /// <summary>
                /// 补打模板Id
                /// </summary>
                public double TemplateId { get; set; }
            }
        }
    }
