using SIE.Domain;
using SIE.EMS.SpareParts;
using SIE.ObjectModel;
using System;

namespace SIE.Web.EMS.SpareParts
{
    /// <summary>
	/// 备件更换记录 视图配置
	/// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class SparePartChangedRecViewConfig : WebViewConfig<SparePartChangedRecord>
    {
        /// <summary>
        /// 备件寿命监控视图
        /// </summary>
        public const string SparePartAgeViewGroup = "SparePartAgeViewGroup";

        #region 下次更换日期 NextChangeDate
        /// <summary>
        /// 下次更换日期
        /// </summary>
        [Label("下次更换日期")]
        public static readonly Property<DateTime?> NextChangeDateProperty = P<SparePartChangedRecord>.RegisterExtensionReadOnly("NextChangeDate", typeof(SparePartChangedRecViewConfig),
            GetNextChangeDate, SparePartChangedRecord.UpdateDateProperty);

        /// <summary>
        /// 下次更换日期
        /// </summary>
        public static DateTime? GetNextChangeDate(SparePartChangedRecord me)
        {
            if (me == null) return null;
            var lifeTime = me.SparePart.LifeTime ?? 0;

            return me.UpdateDate.AddDays(lifeTime);
        }
        #endregion

        #region 剩余生命周期 RemainingAge
        /// <summary>
        /// 剩余生命周期
        /// </summary>
        [Label("剩余生命周期(天)")]
        public static readonly Property<double> RemainingAgeProperty = P<SparePartChangedRecord>.RegisterExtensionReadOnly("RemainingAge", typeof(SparePartChangedRecViewConfig),
            GetRemainingAge, SparePartChangedRecord.UpdateDateProperty);

        /// <summary>
        /// 剩余生命周期
        /// </summary>
        public static double GetRemainingAge(SparePartChangedRecord me)
        {
            var nextChangeDate = GetNextChangeDate(me);
            if (!nextChangeDate.HasValue) return 0;

            var remainingAge = (nextChangeDate.Value - DateTime.Now).TotalDays;
            return Math.Floor(remainingAge);
        }
        #endregion


        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(SparePartAgeViewGroup);
            if (ViewGroup == SparePartAgeViewGroup)
                ConfigSparePartAgeViewGroup();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.SparePartCodeView).Readonly();
                View.Property(p => p.SparePartNameView).Readonly();
                View.Property(p => p.SpecificationView).Readonly();
                View.Property(p => p.BatchNumber).Readonly();
                View.Property(p => p.SerialNumber).Readonly();
                View.Property(p => p.OldSerialNumber).Readonly();
                View.Property(p => p.SparePartTypeView).Readonly();
                View.Property(p => p.Source).Readonly();
                View.Property(p => p.SourceNo).Readonly();
                View.Property(p => p.Qty).Readonly();
                View.Property(p => p.SparePartUnitView).Readonly();
                View.Property(p => p.CreateByName).HasLabel("更换人").Readonly();
                View.Property(p => p.CreateDate).HasLabel("更换时间").Readonly();

                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 备件寿命监控视图
        /// </summary>
        void ConfigSparePartAgeViewGroup()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.SparePartCodeView).Show();
                View.Property(p => p.SparePartNameView).Show();
                View.Property(p => p.SpecificationView).Show();
                View.Property(p => p.SparePartTypeView).Show();
                View.Property(NextChangeDateProperty).Show().ShowInList(width:155);
                View.Property(RemainingAgeProperty).Show().ShowInList(width: 125);

                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
