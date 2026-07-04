using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces
{
    public class InfMappingController : DomainController
    {
        /// <summary>
        /// 获取基础数据接口信息
        /// </summary>
        /// <param name="infCode"></param>
        /// <returns></returns>
        public virtual InfMapping GetInfMapping(string infCode)
        {
            var infMapping = Query<InfMapping>().Where(p => p.InfCode == infCode).FirstOrDefault();
            return infMapping;
        }
    }
}
