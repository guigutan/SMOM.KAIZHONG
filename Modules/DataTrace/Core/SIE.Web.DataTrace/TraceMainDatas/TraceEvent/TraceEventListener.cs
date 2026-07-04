using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using SIE.Common.Attachments;
using SIE.Common.Prints;
using SIE.DataTrace.TraceMainDatas;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MQueue;
using SIE.Web.Common.Attachments.Commands;
using SIE.Web.Common.Prints;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;

namespace SIE.Web.DataTrace.TraceMainDatas.TraceEvent
{
    /// <summary>
    /// 追溯数据存档EventBus监听类
    /// </summary>
    public class TraceEventListener : IDisposable
    {
        /// <summary>
        /// 实例
        /// </summary>
        public static TraceEventListener Instance { get; set; } = new TraceEventListener();

        private readonly CancellationTokenSource cancelToken = new CancellationTokenSource();
        /// <summary>
        /// 订阅追溯数据存档接收信息
        /// </summary>
        public void Start()
        {
            if (!RT.IsOnScheduleServer())
            {
                AppRuntime.MQueueEventBus.SubscribeAsync<TraceFileViewmodel>(p => SaveTraceFile(p), cancelToken, new Event.MQueue.SubscribeOptions() { AutoAck = false });
            }
        }

        /// <summary>
        /// 追溯数据存档
        /// </summary>
        /// <param name="args"></param>
        private void SaveTraceFile(SubscribeEventArgs<TraceFileViewmodel> args)
        {
            var data=args.Body;
            try
            {
                var ctl = RT.Service.Resolve<TraceMainDataController>();
                // 1.获取打印模板
                var printTemplates = ctl.GetBillPrintTemplates(data.TraceMainId, data.InvOrg);
                var printPaths = new List<string>();
                if (RT.InvOrg == null) RT.InvOrg = data.InvOrg;
                printTemplates.ForEach(print =>
                {
                    printPaths.Add(GeneralTemplate(print));
                });
                UploadAttachment(printPaths, data);
            }
            catch (Exception ex)
            {
                RT.Logger.Warn(ex.Message);
            }
            args.Ack = true;
        }

        /// <summary>
        /// 生成打印模板
        /// </summary>
        /// <param name="printBind"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        private string GeneralTemplate(TraceDataPrintDataViewmodel printBind)
        {
            var template = printBind.Template;
            var report = ReportFactory.Current.GetReportByExtension(template.Type);

            //3.获取打印实体对像
            var printableType = Type.GetType(template.EntityType);
            if (printableType == null)
                throw new ValidationException("不存在实体类型[{0}]".L10nFormat(template.EntityType));
            //4.创建实体打印对像 如果清楚实体打印对像自己NEW 一个出来也行
            var printable = Activator.CreateInstance(printableType) as IPrintable;
            if (printable == null)
                throw new ValidationException("创建实体类型[{0}]失败！".L10nFormat(template.EntityType));
            //template.Type = ".sied"返回的是路径,template.Type = ".siedev"返回是流
            var reportPrintProcess = report.PrintProcess(printable, template.Id, template.Content, () =>
            {
                return printBind.ListData as IEnumerable<object>;
            });
            return reportPrintProcess;

        }

        /// <summary>
        /// 模板pdf文件上传
        /// </summary>
        /// <param name="paths"></param>
        /// <param name="traceMain"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        private void UploadAttachment( List<string> paths, TraceFileViewmodel traceMain)
        {
            try
            {
                var entityType = typeof(TraceMainDataAttachment);
                var file = new UploadAttachmentViewArgs();
                file.Attachment = new UpLoadAttachment();
                List<byte[]> docs = new List<byte[]>();
                paths.ForEach(item => {
                    var report = this.GetReport(item);
                    var document = StiNetCoreReportResponse.ResponseAsPdf(report);
                    file.Attachment.FileExtesion = ".pdf";
                    docs.Add(document.Data);
                });
                var merdeDoc = new PdfDocument();
                foreach (var docbyte in docs)
                {
                    var docment = PdfReader.Open(new MemoryStream(docbyte), PdfDocumentOpenMode.Import);
                    //合拼文档
                    foreach (var page in docment.Pages)
                    {
                        merdeDoc.AddPage(page);
                    }
                }
                var sm = new MemoryStream();
                merdeDoc.Save(sm, false);
                byte[] mergeBytes = sm.ToArray();
                //文件保存
                file.Attachment.FileName = $"电子批[{traceMain.TraceMainNo}]{DateTime.Now.ToString("yyyyMMddhhmmss")}.pdf";
                file.Attachment.OwnerType = entityType;
                file.Attachment.Content = mergeBytes;
                file.Attachment.OwnerId = traceMain.TraceMainId.ToString();
                file.Attachment.FileSize = mergeBytes.Length.ToString();
                this.OnSavingAttachement(file);

                // 保存
                this.SaveAttachement(file, entityType);

                // 保存附件后的事件
                this.OnSavedAttachement(file);
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
        }

        private StiReport GetReport(string path)
        {
            string spath = path.Substring(0, path.LastIndexOf(Path.DirectorySeparatorChar));
            var guid = spath.Substring(spath.LastIndexOf(Path.DirectorySeparatorChar) + 1);
            var report = new StiReport();
            var data = new DataSet("data");
            data.ReadXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates",
                DateTime.Today.ToString("yyyy-MM-dd"), guid, "data.xml"));
            report.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates",
                DateTime.Today.ToString("yyyy-MM-dd"), guid, "stiReport.sied"));
            if (report.Dictionary.Databases.Count > 0)
                report.Dictionary.Databases.Clear();
            report.RegData(data);
            return report;
        }

        /// <summary>
        /// 附件保存前事件，子类可以根据需要扩展
        /// </summary>
        public event Func<UploadAttachmentViewArgs, string> SavingAttachement;


        /// <summary>
        /// 附件保存后事件，子类可以根据需要扩展，比如保存实体相关的内容
        /// </summary>
        public event Func<UploadAttachmentViewArgs, string> SavedAttachement;
        /// <summary>
        /// 保存前事件
        /// </summary>
        /// <param name="viewArgs"></param>
        /// <returns></returns>
        private string OnSavingAttachement(UploadAttachmentViewArgs viewArgs)
        {
            if (SavingAttachement == null)
            {
                return string.Empty;
            }

            return SavingAttachement.Invoke(viewArgs);
        }

        private string OnSavedAttachement(UploadAttachmentViewArgs viewArgs)
        {
            if (SavedAttachement == null)
            {
                return string.Empty;
            }

            return SavedAttachement.Invoke(viewArgs);
        }

        /// <summary>
        /// 保存附件
        /// </summary>
        /// <param name="viewArgs"></param>
        /// <param name="type"></param>
        private void SaveAttachement(UploadAttachmentViewArgs viewArgs, Type type)
        {
            var obj = Activator.CreateInstance(type) as Attachment;
            var attachment = viewArgs.Attachment;
            obj.FileExtesion = attachment.FileExtesion;
            obj.FileName = attachment.FileName;
            obj.FileSize = attachment.FileSize;
            obj.Content = attachment.Content;
            obj.OwnerId = Convert.ToDouble(attachment.OwnerId);

            obj.PersistenceStatus = PersistenceStatus.New;
            var repo = RF.Find(type);
            repo.Save(obj);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放数据
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                cancelToken.Cancel();
            }
        }
    }
}
