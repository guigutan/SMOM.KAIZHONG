using SIE.Core.Algorithms.KZ;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.LineAndon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Reports
{
    /// <summary>
    /// 客户特征码控制器(接口实现)
    /// </summary>
    public partial class CustomCodeController : DomainController, IBydCode
    {
        /// <summary>
        /// 获取比亚迪特征码
        /// 特征码: AB111
        /// A:固定不变
        /// B代表白班Y代表夜班
        /// 第一个1：代表包塑线（1线）
        /// 第二个1：代表折弯线（1线）
        /// 第三个1：代表成品线（1线）
        /// </summary>
        /// <param name="batchNo">报工批次号</param>
        /// <param name="lineCode">当前产线</param>
        /// <returns></returns>
        public string GetBydCode(string batchNo, string lineCode)
        {
            var reportWipBatchs = Query<ReportWipBatch>().Where(p => p.BatchNo == batchNo).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var lineCode1 = reportWipBatchs.FirstOrDefault(p => p.ProcessCode.Contains("包塑"))?.ResourceCode;
            var lineCode2 = reportWipBatchs.FirstOrDefault(p => p.ProcessCode.Contains("折弯"))?.ResourceCode;
            var lineCodes = new List<string>() { lineCode1, lineCode2, lineCode };
            var lines = Query<AndonLine>().Where(p => lineCodes.Contains(p.MachineCode)).ToList();
            var seq1 = lines.FirstOrDefault(p => p.MachineCode == lineCode1)?.Seq ?? new Random().Next(1, 9).ToString();
            var seq2 = lines.FirstOrDefault(p => p.MachineCode == lineCode2)?.Seq ?? "1";
            var seq3 = lines.FirstOrDefault(p => p.MachineCode == lineCode)?.Seq ?? "1";
            //if (seq1.IsNullOrEmpty())
            //    throw new ValidationException("资源[{0}]未维护产线序号,请检查".L10nFormat(lineCode1));
            //if (seq2.IsNullOrEmpty())
            //    throw new ValidationException("资源[{0}]未维护产线序号,请检查".L10nFormat(lineCode2));
            //if (seq3.IsNullOrEmpty())
            //    throw new ValidationException("资源[{0}]未维护产线序号,请检查".L10nFormat(lineCode));
            var now = DateTime.Now;
            var shift = (now >= now.Date.AddHours(8) && now < now.Date.AddHours(20)) ? "B" : "Y"; //8:00-20:00 为白班
            return "A{0}{1}{2}{3}".FormatArgs(shift, seq1, seq2, seq3);
        }
    }
}
