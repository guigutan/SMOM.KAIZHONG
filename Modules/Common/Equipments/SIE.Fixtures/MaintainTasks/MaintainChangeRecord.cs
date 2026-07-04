using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Fixtures.MaintainTasks
{
    /// <summary>
	/// 更换记录
	/// </summary>
	[ChildEntity, Serializable]
    [Label("更换记录")]
    public partial class MaintainChangeRecord : DataEntity
    {
        #region 备件编码 Code
        /// <summary>
        /// 备件编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("备件编码")]
        public static readonly Property<string> CodeProperty = P<MaintainChangeRecord>.Register(e => e.Code);

        /// <summary>
        /// 备件编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 备件名称 Name
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> NameProperty = P<MaintainChangeRecord>.Register(e => e.Name);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        [Required]
        [MinValue(0)]
        public static readonly Property<int> QtyProperty = P<MaintainChangeRecord>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 更换说明 Description
        /// <summary>
        /// 更换说明
        /// </summary>
        [Label("更换说明")]
        public static readonly Property<string> DescriptionProperty = P<MaintainChangeRecord>.Register(e => e.Description);

        /// <summary>
        /// 更换说明
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 保养任务 MaintainTask
        /// <summary>
        /// 保养任务Id
        /// </summary>
        public static readonly IRefIdProperty MaintainTaskIdProperty = P<MaintainChangeRecord>.RegisterRefId(e => e.MaintainTaskId, ReferenceType.Parent);

        /// <summary>
        /// 保养任务Id
        /// </summary>
        public double MaintainTaskId
        {
            get { return (double)GetRefId(MaintainTaskIdProperty); }
            set { SetRefId(MaintainTaskIdProperty, value); }
        }

        /// <summary>
        /// 保养任务
        /// </summary>
        public static readonly RefEntityProperty<MaintainTask> MaintainTaskProperty = P<MaintainChangeRecord>.RegisterRef(e => e.MaintainTask, MaintainTaskIdProperty);

        /// <summary>
        /// 保养任务
        /// </summary>
        public MaintainTask MaintainTask
        {
            get { return GetRefEntity(MaintainTaskProperty); }
            set { SetRefEntity(MaintainTaskProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 更换记录 实体配置
    /// </summary>
    internal class MaintainChangeRecordConfig : EntityConfig<MaintainChangeRecord>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ELEC_MAINTAIN_CH_RECORD").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
