using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces.ViewModels
{
    /// <summary>
    /// 同步其他工厂
    /// </summary>
    [RootEntity,Serializable]
    [Label("同步其他工厂")]
    public class LogGroupSyncOtherFactoryViewModel : ViewModel
    {
        #region 工厂 Factory
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> FactoryProperty = P<LogGroupSyncOtherFactoryViewModel>.Register(e => e.Factory);

        /// <summary>
        /// 工厂
        /// </summary>
        public string Factory
        {
            get { return this.GetProperty(FactoryProperty); }
            set { this.SetProperty(FactoryProperty, value); }
        }
        #endregion

        #region 数据Ids(多数据用,隔开) Ids
        /// <summary>
        /// 数据Ids(多数据用,隔开)
        /// </summary>
        [Label("数据Ids(多数据用,隔开")]
        public static readonly Property<string> IdsProperty = P<LogGroupSyncOtherFactoryViewModel>.Register(e => e.Ids);

        /// <summary>
        /// 数据Ids(多数据用,隔开)
        /// </summary>
        public string Ids
        {
            get { return this.GetProperty(IdsProperty); }
            set { this.SetProperty(IdsProperty, value); }
        }
        #endregion

    }
}
