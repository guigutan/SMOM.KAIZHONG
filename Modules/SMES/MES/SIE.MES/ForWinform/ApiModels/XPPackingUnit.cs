using SIE.Packages.Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ForWinform.ApiModels
{
    /// <summary>
    /// XP端包装单位
    /// </summary>
    [Serializable]
    public class XPPackingUnit
    {
        /// <summary>
        /// 
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否主单位
        /// </summary>
        public bool IsMasterUnit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="packingUnit"></param>
        public XPPackingUnit(PackingUnit packingUnit)
        {
            if (packingUnit == null)
                return;

            this.Id = packingUnit.Id;
            this.Code = packingUnit.Code;
            this.Name = packingUnit.Name;
            this.Description = packingUnit.Description;
            this.IsMasterUnit = packingUnit.IsMasterUnit;
        }
    }
}
