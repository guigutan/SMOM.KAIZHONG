using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Dispatchs.Datas
{
    [Serializable]
    public class BlankingAssemblyGetWipResources
    {
        /// <summary>
        /// 是否资源(不是资源就是供料区)
        /// </summary>
        public bool IsResource { get; set; }
    
        /// <summary>
        /// 机台Id
        /// </summary>
        public double ResourceId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}
