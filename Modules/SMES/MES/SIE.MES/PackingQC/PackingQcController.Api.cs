using SIE.Api;
using SIE.Domain;
using SIE.MES.PackingQC.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IronPython.Modules._ast;

namespace SIE.MES.PackingQC
{
    /// <summary>
    /// QC确认接口
    /// </summary>
    public partial class PackingQcController : DomainController
    {

        /// <summary>
        /// 获取待确认的QC清单列表
        /// </summary>
        /// <returns></returns>
        [ApiService("获取待确认的QC清单列表")]
        public virtual List<PackingQcData> GetPackingQcDataList([ApiParameter("过滤条件")] string keyWord)
        {
            var list = RT.Service.Resolve<PackingQcController>().GetPackingQcDatas(keyWord);

            return list;
        }

        /// <summary>
        /// 获取待确认的QC清单单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ApiService("获取待确认的QC清单明细")]
        public virtual PackingQcData GetPackingQcData([ApiParameter("过滤条件")] double id)
        {
            var list = RT.Service.Resolve<PackingQcController>().GetPackingQcDatas(null);

            return list.Where(p => p.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// 提交待确认的QC清单明细
        /// </summary>
        /// <param name="query"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [ApiService("提交待确认的QC清单明细")]
        public virtual bool SubmitPackingQcData([ApiParameter("请求参数")] List<SubmitPackingQcData> query, double id)
        {
            var masterData = RF.GetById<PackingQc>(id);
            PackingReportRecord record = new PackingReportRecord();
            record.BeginDate = DateTime.Now;
            record.BlueLabel = masterData.BlueLabel;
            record.Report = ReportType.QC;
            var message= RT.Service.Resolve<PackingQcController>().SubmitData(query, id);
            record.EndDate= DateTime.Now;
            RF.Save(record);
            return message;
        }


    }
}
