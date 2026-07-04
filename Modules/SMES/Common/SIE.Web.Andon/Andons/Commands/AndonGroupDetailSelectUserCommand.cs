using SIE.Andon.Andons;
using SIE.Domain;
using SIE.Resources.UserGroups;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Andon.Andons.Commands
{
    public class AndonGroupDetailSelectUserCommand : ViewCommand
    {
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var meta = ClientEntities.Find(args.Type);
            var savedData = RF.Find(meta.EntityType).NewList();
            var list = args.Data.ToJsonObject<List<AndonGroupDetail>>();
            Check.NotNullOrEmpty(list, nameof(list));

            if (null == list || list.Count == 0)
                throw new ArgumentNullException(nameof(list));

            RT.Service.Resolve<AndonController>().SaveAndonGroupDetail(list);

            return true;
        }
    }
}
