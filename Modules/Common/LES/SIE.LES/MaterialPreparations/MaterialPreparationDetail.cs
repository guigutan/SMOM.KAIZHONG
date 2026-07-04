using SIE.Domain;
using SIE.Items;
using SIE.LES.MaterialPreparations.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.LES.MaterialPreparations
{
    /// <summary>
    /// 备料需求单明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("备料需求单明细")]
    public class MaterialPreparationDetail : MaterialPreparationDetailBase
    {
    }

    /// <summary>
    /// 数据库配置
    /// </summary>
    public class MaterialPreparationDetailConfig : EntityConfig<MaterialPreparationDetail>
    {
        /// <summary>
        /// 表配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("LES_MATERIAL_PREDTL").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.Property(MaterialPreparationDetail.ToReceiveQtyProperty).DontMapColumn();
            Meta.Property(MaterialPreparationDetail.CanPrepareQtyProperty).DontMapColumn();
        }
    }
}
