using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Datas.EbsData;
using SIE.ERPInterface.Ebs.Connection;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Ebs.Download.OnHands
{
    /// <summary>
    /// 库存现有量下载控制器
    /// </summary>
    public class EbsOnHandController : DomainController
    {
        /// <summary>
        /// 库存现有量下载
        /// </summary>
        public virtual List<ErpOnHandData> Download(List<string> ErpWareCodes,string orgName)
        {
            var ebsPara = EbsHelper.GetEbsParameter();
            //var orgName = ErpWareHouse[0].ErpOrgName;
            //Copy必改内容
            ebsPara.InterfaceCode = "S_E2W_STOCK";//接口编码，接口卡有
            ebsPara.DownParameter.ParaStr = GetEbsParams(ErpWareCodes, orgName);
            var soapResult = EbsHelper.ExecuteEbs<ErpOnHandData>(ebsPara);
            var allData = soapResult.XV_RESULT;
            return allData;
        }

        /// <summary>
        /// 获取上传报文
        /// </summary>
        /// <returns></returns>
        private string GetEbsParams(List<string> ErpWareCodes,string orgName)
        {
            var str = string.Empty;
            str = @"{
                      ""PARA_DATA_TYPE"": ""C"",
                      ""PARA_NAME"": ""Organization_Name"",
                      ""PARA_OPERATOR"": ""="",
                      ""PARA_VALUE"": ""{0}""
                    },{
                      ""PARA_DATA_TYPE"": ""C"",
                      ""PARA_NAME"": ""Subinventory"",
                      ""PARA_OPERATOR"": ""IN"",
                      ""PARA_VALUE"": ""[{1}]""
                    }".FormatArgs(orgName, string.Join(",", ErpWareCodes));
            return str;
        }
    }
}
