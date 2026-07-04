using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.PackRule
{
    public class PackRuleController : DomainController
    {
        /// <summary>
        /// 根据物料id获取规则
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public virtual ItemQRCodeRule GetItemQRCodeRule(double itemId)
        {
            var query = Query<ItemQRCodeRule>().Where(p => p.ItemId == itemId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            return query;
        }

        /// <summary>
        /// 根据ID获取二维码规则
        /// </summary>
        /// <param name="qRCodeRuleId"></param>
        /// <returns></returns>
        public virtual QRCodeRule GetQRCodeRule(double qRCodeRuleId)
        {
            var query=Query<QRCodeRule>().Where(p=>p.Id== qRCodeRuleId).ToList().FirstOrDefault();
            return query;
        }
    }
}
