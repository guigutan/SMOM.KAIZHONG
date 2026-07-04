using SIE.Common.Attachments;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Prints;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Core.PrintExt
{
    /// <summary>
    /// 打印模板下载 Ext
    /// </summary>
    public class PrintExtController : PrintsController
    {
        /// <summary>
        /// 下载模板
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <exception cref="ValidationException"></exception>
        [IgnoreProxy]
        public override string DownloadPrintTemplate(double templateId)
        {
            PrintTemplate template = RF.GetById<PrintTemplate>(templateId);
            if (template == null)
            {
                throw new EntityNotFoundException(typeof(PrintTemplate), templateId);
            }

            string fileName = template.FileName;
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ValidationException("文件名为空".L10N());
            }

            int templateVersion = GetTemplateVersion(templateId);
            string text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");
            if (!Directory.Exists(text))
            {
                Directory.CreateDirectory(text);
            }

            string text2 = fileName.Substring(0, fileName.LastIndexOf('.'));
            string value = fileName.Substring(fileName.LastIndexOf('.'), fileName.Length - text2.Length);
            string path = $"{text2}-V{templateVersion}{value}";
            string filePath = Path.Combine(text, path);
            if (!File.Exists(filePath))
            {
                byte[] array = template.Content;
                if (array == null || array.Length == 0)
                    array = AppRuntime.Service.Resolve<AttachmentController>().FileDownload(template.FilePath, fileName);

                using FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
                fileStream.Write(array, 0, array.Length);
            }

            return filePath;
        }
    }


    /// <summary>
    /// 打印服务接口扩展
    /// </summary>
    public class ExtPrintsSerivce : IPrintsSerivce
    {
        /// <summary>
        /// 获取打印模板
        /// </summary>
        /// <param name="tmplId"></param>
        /// <returns></returns>
        public virtual IPrintTemplate GetPrintTemplate(double tmplId)
        {
            return RT.Service.Resolve<PrintsController>().GetPrintTemplate(tmplId);
        }

        /// <summary>
        /// 保存模板
        /// </summary>
        /// <param name="tmplId"></param>
        /// <param name="content"></param>
        public virtual void SavePrintTemlateContent(double tmplId, byte[] content)
        {
            RT.Service.Resolve<PrintsController>().SavePrintTemlateContent(tmplId, content);
        }
    }
}
