using SIE.Domain;
using SIE.Items;
using SIE.LES.MaterialReturnApplys.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.LES.MaterialReturnApplys
{
    /// <summary>
    /// 退料申请明细
    /// </summary>
    [RootEntity, Serializable]
    [Label("退料申请明细")]
    public class MaterialReturnApplyDetail : MaterialReturnApplyDetailBase
    {
        

    }

    /// <summary>
    /// 退料申请明细实体配置
    /// </summary>
    public class MaterialReturnApplyDetailConfig : EntityConfig<MaterialReturnApplyDetail>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("LES_MRAPPLY_DETAIL").MapAllProperties();
            Meta.Property(MaterialReturnApplyDetail.CtrlModeProperty).DontMapColumn();
            Meta.Property(MaterialReturnApplyDetail.AvailableQtyProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}
