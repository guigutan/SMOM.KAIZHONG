using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Web.Tech.Routings
{
    /// <summary>
    /// 工艺路线版本复制视图
    /// </summary>
    [RootEntity, Serializable]
    [Label("工艺路线版本复制视图")]
    public class RoutingVersionCopyViewModel : ViewModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RoutingVersionCopyViewModel()
        {
            IsCopyBom = true;
            IsCopyActivityProperty = true;
            IsCopyFixture = true;
        }

        #region 是否复制工序BOM IsCopyBom
        /// <summary>
        /// 是否复制工序BOM
        /// </summary>
        [Label("是否复制工序BOM")]
        public static readonly Property<bool> IsCopyBomProperty = P<RoutingVersionCopyViewModel>.Register(e => e.IsCopyBom);

        /// <summary>
        /// 是否复制工序Bom
        /// </summary>
        public bool IsCopyBom
        {
            get { return this.GetProperty(IsCopyBomProperty); }
            set { this.SetProperty(IsCopyBomProperty, value); }
        }
        #endregion

        #region 是否复制流程属性 IsCopyActivityProperty
        /// <summary>
        /// 是否复制流程属性
        /// </summary>
        [Label("是否复制流程属性")]
        public static readonly Property<bool> IsCopyActivityPropertyProperty = P<RoutingVersionCopyViewModel>.Register(e => e.IsCopyActivityProperty);

        /// <summary>
        /// 是否复制流程属性
        /// </summary>
        public bool IsCopyActivityProperty
        {
            get { return this.GetProperty(IsCopyActivityPropertyProperty); }
            set { this.SetProperty(IsCopyActivityPropertyProperty, value); }
        }
        #endregion

        #region 是否复制工治具需求 IsCopyActivityProperty
        /// <summary>
        /// 是否复制工治具需求
        /// </summary>
        [Label("是否复制工治具需求")]
        public static readonly Property<bool> IsCopyFixtureProperty = P<RoutingVersionCopyViewModel>.Register(e => e.IsCopyFixture);

        /// <summary>
        /// 是否复制工治具需求
        /// </summary>
        public bool IsCopyFixture
        {
            get { return this.GetProperty(IsCopyFixtureProperty); }
            set { this.SetProperty(IsCopyFixtureProperty, value); }
        }
        #endregion
    }
}
