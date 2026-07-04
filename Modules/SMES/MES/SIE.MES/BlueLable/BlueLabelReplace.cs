using SIE.Core.Items;
using SIE.Domain;
using SIE.KZ.Base.Interfaces;
using SIE.MES.ItemLine;
using SIE.MES.LineAndon;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.BlueLable
{
    /// <summary>
    /// 蓝标替换
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(BlueLabelReplaceCriteria))]
    [Label("蓝标替换")]
    public class BlueLabelReplace : DataEntity
    {
        #region 旧蓝标 OldBlueLabel
        /// <summary>
        /// 旧蓝标
        /// </summary>
        [Label("旧蓝标")]
        public static readonly Property<string> OldBlueLabelProperty = P<BlueLabelReplace>.Register(e => e.OldBlueLabel);

        /// <summary>
        /// 旧蓝标
        /// </summary>
        public string OldBlueLabel
        {
            get { return GetProperty(OldBlueLabelProperty); }
            set { SetProperty(OldBlueLabelProperty, value); }
        }
        #endregion

        #region 新蓝标 NewBlueLabel
        /// <summary>
        /// 新蓝标
        /// </summary>
        [Label("新蓝标")]
        public static readonly Property<string> NewBlueLabelProperty = P<BlueLabelReplace>.Register(e => e.NewBlueLabel);

        /// <summary>
        /// 新蓝标
        /// </summary>
        public string NewBlueLabel
        {
            get { return GetProperty(NewBlueLabelProperty); }
            set { SetProperty(NewBlueLabelProperty, value); }
        }
        #endregion
    }

    internal class BlueLabelReplaceConfig : EntityConfig<BlueLabelReplace>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("BLUE_LABEL_REPLACE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}