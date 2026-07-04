using SIE.Equipments.EquipmentCards;
using SIE.MES.Outsourcing;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.MES.Outsourcing.Commands
{
    /// <summary>
    /// 删除
    /// </summary>
    public class DeleteRequestCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (args != null)
            {
                if (null == args.SelectedIds || args.SelectedIds.Length == 0)
                {
                    throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
                }
                List<double> requestIds = new List<double>();
                for (int i = 0; i < args.SelectedIds.Length; i++)
                {
                    requestIds.Add(args.SelectedIds[i]);
                }
                RT.Service.Resolve<OutsourcingRequestController>().DeleteRequest(requestIds);
            }
            return null;
        }
    }
}
