using SIE.Domain;
using SIE.Domain.Validation;
using SIE.LES.LinesideWarehouses;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.LES.LinesideWarehouses.Commands
{
    /// <summary>
    /// 保存
    /// </summary>
    public class LinesideWarehouseSaveCommand : SaveCommand
    {
        /// <summary>
        /// 保存验证
        /// </summary>
        /// <param name="data"></param>
        protected override void OnSaving(EntityList data)
        {
            var list = data as EntityList<LinesideWarehouse>;
            if (list.Any(p => p.WorkShopId == null && p.WipResouceId == null))
            {
                throw new ValidationException("资源与车间不能同时为空".L10N());
            }
            base.OnSaving(data);
        }
    }
}
