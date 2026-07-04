using SIE.Domain;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProjectDesigns.ChildCommands
{
    /// <summary>
    /// 工艺资料删除命令
    /// </summary>
    public class ProductTreeDeleteCommand : DeleteCommand
    {
        /// <summary>
        /// 删除前校验产品bom和产品工艺路线是否删除
        /// </summary>
        /// <param name="data"></param>
        protected override void OnSaving(EntityList data)
        {
            // todo
            base.OnSaving(data);
        }
    }
}
