using SIE.Common.Configs;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.LES.RetreatItemManage.MaterialReturns
{
    [RootEntity, Serializable]
    [Label("生产退料选择视图")]
    [ConditionQueryType(typeof(MaterialReturnForSelectCriteria))]
    /// <summary>
    /// 选择用的退料对象
    /// </summary>
    public class MaterialReturnForSelect: MaterialReturn
    {
        //todo
    }
    internal class MaterialReturnForSelectConfig : EntityConfig<MaterialReturnForSelect>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MATERIA_RETURN").MapAllPropertiesExcept(MaterialReturn.SnProperty);
            Meta.Property(MaterialReturn.ReturnReasonDescProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}
