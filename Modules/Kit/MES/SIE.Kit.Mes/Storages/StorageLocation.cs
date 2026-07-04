using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Kit.MES.Storages
{
    /// <summary>
    /// 产线货区货位
    /// </summary>
    [RootEntity, Serializable]
    [Label("货位")]
    [DisplayMember(nameof(Code))]
    public partial class StorageLocation : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(40)]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<StorageLocation>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(40)]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<StorageLocation>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 位置 Location
        /// <summary>
        /// 位置
        /// </summary>
        [Label("位置")]
        public static readonly Property<string> LocationProperty = P<StorageLocation>.Register(e => e.Location);

        /// <summary>
        /// 位置
        /// </summary>
        public string Location
        {
            get { return GetProperty(LocationProperty); }
            set { SetProperty(LocationProperty, value); }
        }
        #endregion

        #region 货区 StorageArea
        /// <summary>
        /// 货区
        /// </summary>
        [Label("货区")]
        public static readonly IRefIdProperty StorageAreaIdProperty = P<StorageLocation>.RegisterRefId(e => e.StorageAreaId, ReferenceType.Parent);

        /// <summary>
        /// 货区
        /// </summary>
        public double StorageAreaId
        {
            get { return (double)GetRefId(StorageAreaIdProperty); }
            set { SetRefId(StorageAreaIdProperty, value); }
        }

        /// <summary>
        /// 货区
        /// </summary>
        public static readonly RefEntityProperty<StorageArea> StorageAreaProperty = P<StorageLocation>.RegisterRef(e => e.StorageArea, StorageAreaIdProperty);

        /// <summary>
        /// 货区
        /// </summary>
        public StorageArea StorageArea
        {
            get { return GetRefEntity(StorageAreaProperty); }
            set { SetRefEntity(StorageAreaProperty, value); }
        }
        #endregion

        #region 是否通用 IsCommon
        /// <summary>
        /// 是否通用
        /// </summary>
        [Label("通用")]
        public static readonly Property<bool> IsCommonProperty = P<StorageLocation>.Register(e => e.IsCommon);

        /// <summary>
        /// 是否通用
        /// </summary>
        public bool IsCommon
        {
            get { return this.GetProperty(IsCommonProperty); }
            set { this.SetProperty(IsCommonProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 产线货区货位 实体配置
    /// </summary>
    internal class StorageLocationConfig : EntityConfig<StorageLocation>
    {
        /// <summary>
        /// 配置数据库的映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_WH_LOCATION").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.Property(StorageLocation.StorageAreaIdProperty).ColumnMeta.HasIndex();
            Meta.Property(StorageLocation.CodeProperty).ColumnMeta.HasIndex();
            Meta.Property(StorageLocation.NameProperty).ColumnMeta.HasIndex();
        }
    }
}