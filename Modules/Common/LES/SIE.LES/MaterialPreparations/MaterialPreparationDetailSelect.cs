using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.MaterialPreparations
{
    /// <summary>
    /// 备料需求单明细选择数据
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(MaterialPreparationDtlSelCriteria))]
    [Label("备料需求单明细选择数据")]
    public class MaterialPreparationDetailSelect : MaterialPreparationDetailBase
    {
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    public class MaterialPreparationDetailSelectConfig : EntityConfig<MaterialPreparationDetailSelect>
    {
        /// <summary>
        /// 表配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("LES_MATERIAL_PREDTL").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.Property(MaterialPreparationDetailSelect.ToReceiveQtyProperty).DontMapColumn();
            Meta.Property(MaterialPreparationDetailSelect.CanPrepareQtyProperty).DontMapColumn();
        }
    }
}
