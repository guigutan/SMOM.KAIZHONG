using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Outsourcing
{
    /// <summary>
    /// 查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("查询实体")]
    public class ProcessingOutboundSelectCriteria : Criteria
    {
        #region 产品条码 Sn
        /// <summary>
        /// 产品条码
        /// </summary>
        [Label("产品条码")]
        public static readonly Property<string> SnProperty = P<ProcessingOutboundSelectCriteria>.Register(e => e.Sn);

        /// <summary>
        /// 产品条码
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<OutsourcingController>().CriteriaProcessingOutboundSelect(this);
        }
    }
}
