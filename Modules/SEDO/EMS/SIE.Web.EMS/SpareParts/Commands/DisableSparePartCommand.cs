using SIE.EMS.SpareParts;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.SpareParts.Commands
{
    /// <summary>
    /// 禁用备件命令
    /// </summary>
    public class DisableSparePartCommand : ListViewCommand
    {

        /// <summary>
        ///  禁用备件
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (null == args.SelectedIds || args.SelectedIds.Length == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
            }

            List<double> idList = args.SelectedIds.ToList();

            RT.Service.Resolve<SparePartController>().DisableSparePart(idList);

            return true;
        }
    }
}
