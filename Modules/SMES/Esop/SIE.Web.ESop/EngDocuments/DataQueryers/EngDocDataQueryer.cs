using SIE.ESop.Documents;
using SIE.ESop.EngDocuments.Services;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.ESop.EngDocuments.DataQueryers
{
    /// <summary>
    /// 前端数据请求
    /// </summary>
    public class EngDocDataQueryer : DataQueryer
    {
        /// <summary>
        /// 根据使用类型获取配置项文件夹
        /// </summary>
        /// <param name="useType"></param>
        /// <returns></returns>
        public double? GetConfigUseTypeFolderId(string useType)
        {
            return RT.Service.Resolve<FileUseDetailService>().GetConfigUseTypeFolderId(useType);
        }

        /// <summary>
        /// 解析文件扩展名判断文件类型并生成加密信息
        /// </summary>
        /// <param name="exten"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public EngSaveExtData GetDocumentType(string exten, string fileName)
        {
            EngSaveExtData engSaveExtData = new EngSaveExtData();

            var docType = RT.Service.Resolve<DocumentCollectionController>().GetDocumentType(exten);

            var md5 = RT.Service.Resolve<EngDocumentDetailService>().CreateMD5(fileName);
            engSaveExtData.DocumentType = docType;
            engSaveExtData.MD5 = md5;
            return engSaveExtData;
        }
    }

    /// <summary>
    /// 文件管理额外扩展
    /// </summary>
    public class EngSaveExtData
    {
        /// <summary>
        /// 文件类型
        /// </summary>
        public DocumentType DocumentType { get; set; }

        /// <summary>
        /// MD5加密
        /// </summary>
        public string MD5 { get; set; }
    }
}
