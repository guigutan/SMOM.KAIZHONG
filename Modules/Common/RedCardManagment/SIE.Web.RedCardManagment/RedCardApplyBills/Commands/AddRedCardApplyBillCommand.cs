using SIE.Core.Inspections;
using SIE.Core.RedCardManagments;
using SIE.RedCardManagment.RedCardApplyBills;
using SIE.RedCardManagment.RedCardApplyBills.Service;
using SIE.Web.Command;
using System;

namespace SIE.Web.RedCardManagment.RedCardApplyBills.Commands
{
    /// <summary>
    /// 申请单添加命令
    /// </summary>
    public class AddRedCardApplyBillCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">SIE.MetaModel.View.Block.EntityType</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var bill = args.Data.ToJsonObject<RedCardApplyBill>();
            if(null == bill)
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(bill)));
            bill.PersistenceStatus = Domain.PersistenceStatus.New;
            bill.No = RT.Service.Resolve<RedCardApplyBillService>().GenerateNo();
            bill.ApplyType = ApplyType.Manual;
            bill.ApplySource = ApplySource.Manual;
            bill.BillStatus = BillStatus.ToDo;
            return bill;
        }
    }
}
