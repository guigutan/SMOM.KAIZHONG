using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Workbench.StationChecks
{
    /// <summary>
    /// 点检项目
    /// </summary>
    [RootEntity, Serializable]
    public class CheckProject : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<CheckProject>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<CheckProject>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion
    }

    internal class CheckProjectConfig : EntityConfig<CheckProject>
    {
        /// <summary>
        /// 配置实体元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("CHK_STATION_PROJECT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
