using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Recheck.RecheckInspBills;
using System;

namespace SIE.Kit.ReCheck.RecheckInspBills
{
    /// <summary>
    /// 超期复验扩展类
    /// </summary>
    [Serializable]
    [CompiledPropertyDeclarer]
    public static class RecheckInspBillEx
    {
        #region  ReelID清单
        /// <summary>
        /// ReelID清单
        /// </summary>
        [Label("ReelID清单")]
        public static readonly ListProperty<EntityList<BillReel>> ReelListProperty =
            P<RecheckInspBill>.RegisterExtensionList<EntityList<BillReel>>("ReelList", typeof(RecheckInspBillEx));

        /// <summary>
        /// 获取ReelID清单
        /// </summary>
        /// <param name="me">检验单</param>
        /// <returns>ReelID清单</returns>
        public static EntityList<BillReel> GetReelList(this RecheckInspBill me)
        {
            return me.GetProperty(ReelListProperty);
        }

        /// <summary>
        /// 设置ReelID清单
        /// </summary>
        /// <param name="me">检验单</param>
        /// <param name="value">值</param>
        public static void SetReelList(this RecheckInspBill me, EntityList<BillReel> value)
        {
            me.SetProperty(ReelListProperty, value);
        }
        #endregion

        #region  源ReelID清单
        /// <summary>
        /// ReelID清单
        /// </summary>
        [Label("源ReelID清单")]
        public static readonly ListProperty<EntityList<BillReel>> BillSourceReelListProperty =
            P<RecheckInspBill>.RegisterExtensionList<EntityList<BillReel>>("BillSourceReelList", typeof(RecheckInspBillEx));

        /// <summary>
        /// 获取ReelID清单
        /// </summary>
        /// <param name="me">检验单</param>
        /// <returns>ReelID清单</returns>
        public static EntityList<BillReel> GetBillSourceReelList(this RecheckInspBill me)
        {
            return me.GetProperty(BillSourceReelListProperty);
        }

        /// <summary>
        /// 设置ReelID清单
        /// </summary>
        /// <param name="me">检验单</param>
        /// <param name="value">值</param>
        public static void SetBillSourceReelList(this RecheckInspBill me, EntityList<BillReel> value)
        {
            me.SetProperty(BillSourceReelListProperty, value);
        }
        #endregion
    }

    internal class RecheckInspBillExtensionConfig : EntityConfig<RecheckInspBill>
    {
        protected override void ConfigMeta()
        {
            Meta.Property(RecheckInspBillEx.ReelListProperty).DontMapColumn();
        }
    }
}
