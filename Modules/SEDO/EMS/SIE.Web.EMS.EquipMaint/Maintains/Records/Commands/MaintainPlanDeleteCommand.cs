using SIE.EMS.Maintains.Controller;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.EMS.EquipMaint.Maintains.Records.Commands
{
    /// <summary>
    /// 保养记录删除命令
    /// </summary>
    public class MaintainPlanDeleteCommand : ViewCommand
    {
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (args != null)
            {
                if (null == args.SelectedIds || args.SelectedIds.Length == 0)
                {
                    throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
                }

                List<double> Ids = new List<double>();
                for (int i = 0; i < args.SelectedIds.Length; i++)
                {
                    Ids.Add(args.SelectedIds[i]);
                }
                RT.Service.Resolve<MaintainController>().DeleteMaintainByIds(Ids);
            }
            return null;
        }
    }
}
