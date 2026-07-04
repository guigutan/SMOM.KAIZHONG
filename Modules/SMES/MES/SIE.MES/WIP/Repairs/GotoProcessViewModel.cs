using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.Repairs
{
    /// <summary>
    /// 可选工序
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(PathName))]
    public class GotoProcessViewModel : ViewModel
    {
        #region 工序ID RoutingProcessId
        /// <summary>
        /// 工序ID
        /// </summary>
        [Label("工序ID")]
        public static readonly Property<double> RoutingProcessIdProperty = P<GotoProcessViewModel>.Register(e => e.RoutingProcessId);

        /// <summary>
        /// 工序ID
        /// </summary>
        public double RoutingProcessId
        {
            get { return this.GetProperty(RoutingProcessIdProperty); }
            set { this.SetProperty(RoutingProcessIdProperty, value); }
        }
        #endregion

        #region 工艺路线路径名称 PathName
        /// <summary>
        /// 工艺路线路径名称
        /// </summary>
        [Label("工艺路线路径名称")]
        public static readonly Property<string> PathNameProperty = P<GotoProcessViewModel>.Register(e => e.PathName);

        /// <summary>
        /// 工艺路线路径名称
        /// </summary>
        public string PathName
        {
            get { return this.GetProperty(PathNameProperty); }
            set { this.SetProperty(PathNameProperty, value); }
        }
        #endregion

        #region 工艺路线路径描述 PathDescription
        /// <summary>
        /// 工艺路线路径描述
        /// </summary>
        [Label("工艺路线路径描述")]
        public static readonly Property<string> PathDescriptionProperty = P<GotoProcessViewModel>.Register(e => e.PathDescription);

        /// <summary>
        /// 工艺路线路径描述
        /// </summary>
        public string PathDescription
        {
            get { return this.GetProperty(PathDescriptionProperty); }
            set { this.SetProperty(PathDescriptionProperty, value); }
        }
        #endregion

        #region 默认 IsDefault
        /// <summary>
        /// 默认
        /// </summary>
        [Label("默认")]
        public static readonly Property<bool> IsDefaultProperty = P<GotoProcessViewModel>.Register(e => e.IsDefault);

        /// <summary>
        /// 默认
        /// </summary>
        public bool IsDefault
        {
            get { return this.GetProperty(IsDefaultProperty); }
            set { this.SetProperty(IsDefaultProperty, value); }
        }
        #endregion

    }
}
