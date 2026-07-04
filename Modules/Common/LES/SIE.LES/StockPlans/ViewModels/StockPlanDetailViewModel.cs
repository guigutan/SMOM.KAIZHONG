using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.LES.StockPlans.ViewModels
{
    /// <summary>
    /// 齐套分析弹出
    /// </summary>
    [RootEntity, Serializable]
    [Label("齐套分析弹出")]
    public class StockPlanDetailViewModel:ViewModel
    {
        #region 考虑在途库存 BuyOnLoad
        /// <summary>
        /// 考虑在途库存
        /// </summary>
        [Label("考虑在途库存")]
        public static readonly Property<bool> BuyOnLoadProperty = P<StockPlanDetailViewModel>.Register(e => e.BuyOnLoad);

        /// <summary>
        /// 考虑在途库存
        /// </summary>
        public bool BuyOnLoad
        {
            get { return this.GetProperty(BuyOnLoadProperty); }
            set { this.SetProperty(BuyOnLoadProperty, value); }
        }
        #endregion

        #region 考虑生产在制 MakeOnLoad
        /// <summary>
        /// 考虑生产在制
        /// </summary>
        [Label("考虑生产在制")]
        public static readonly Property<bool> MakeOnLoadProperty = P<StockPlanDetailViewModel>.Register(e => e.MakeOnLoad);

        /// <summary>
        /// 考虑生产在制
        /// </summary>
        public bool MakeOnLoad
        {
            get { return this.GetProperty(MakeOnLoadProperty); }
            set { this.SetProperty(MakeOnLoadProperty, value); }
        }
        #endregion
    }
}
