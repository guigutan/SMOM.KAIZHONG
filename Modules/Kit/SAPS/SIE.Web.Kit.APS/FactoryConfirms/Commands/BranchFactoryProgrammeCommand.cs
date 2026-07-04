using SIE.Domain;
using SIE.Kit.APS.FactoryConfirms;
using SIE.Resources.Enterprises;
using SIE.Web.Command;
using System.Collections.Generic;
using System.Linq;
namespace SIE.Web.Kit.APS.FactoryConfirms.Commands
{
    /// <summary>
    /// 分配工厂命令
    /// </summary>
    public class BranchFactoryProgrammeCommand : ViewCommand
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var pendingDatas = args.Data.ToJsonObject<DataModel>();
            List<FactoryConfirmsViewModel> factoryConfirms = pendingDatas.FactoryConfirm;
            List<BranchFactoryProgrammeDetail> programmeDetails = pendingDatas.BranchProgramme;
            var ctrl2 = RT.Service.Resolve<EnterpriseController>();
            var allFo = ctrl2.GetEnterprises(EnterpriseType.Plant);
            if (allFo.Count > 0)
            {
                IntelligentDispatch intelligentDispatch = new IntelligentDispatch(allFo.ToList(), factoryConfirms, programmeDetails);
                intelligentDispatch.Dispathch();
                List<DispathchResult> lstResult = new List<DispathchResult>();
                var dic = allFo.ToDictionary(x => x.Id);
                foreach (var data in factoryConfirms)
                {
                    lstResult.Add(new DispathchResult() { DateID = data.Id, EnterpriseId = data.EnterpriseId, EnterpriseCode = dic[data.EnterpriseId].Code, EnterpriseName = dic[data.EnterpriseId].Name });
                }
                return lstResult;
            }
            else
                return string.Empty;

        }
    }

    /// <summary>
    /// 分配结果
    /// </summary>
    public class DispathchResult
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DispathchResult()
        { }

        /// <summary>
        /// 销售行 ID
        /// </summary>
        public double DateID { get; set; }

        /// <summary>
        /// 分配的工厂ID
        /// </summary>
        public double EnterpriseId { get; set; }

        /// <summary>
        /// 分配的编号
        /// </summary>
        public string EnterpriseCode { get; set; }

        /// <summary>
        /// 分配的工厂
        /// </summary>
        public string EnterpriseName { get; set; }
    }

    /// <summary>
    /// 保存数据类
    /// </summary>
    public class DataModel
    {
        /// <summary>
        /// 厂别确认列表
        /// </summary>
        public List<FactoryConfirmsViewModel> FactoryConfirm
        {
            get; set;
        }

        /// <summary>
        /// 分厂方案明细列表
        /// </summary>
        public List<BranchFactoryProgrammeDetail> BranchProgramme
        {
            get; set;
        }
    }
}

