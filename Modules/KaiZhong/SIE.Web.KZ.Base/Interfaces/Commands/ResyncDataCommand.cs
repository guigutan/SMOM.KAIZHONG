using SIE.KZ.Base.Interfaces;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.KZ.Base.Interfaces.Commands
{
    /// <summary>
    /// 重新同步接口
    /// </summary>
    public class ResyncDataCommand : ViewCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            var ids = args.Data.ToJsonObject<List<double>>();
            if (ids == null || ids.Count == 0)
            {
                return "同步失败,请先选择要同步的数据!".L10N();
            }
            var stringBuilder = RT.Service.Resolve<InfNcDataLogFactoryController>().ResyncExecuteInterface(ids);
            if (stringBuilder.Length > 0)
            {
                return "同步信息失败,原因是{0}".L10nFormat(stringBuilder.ToString());
            }
            else
            {
                return "同步信息成功".L10N();
            }
        }
    }
}
