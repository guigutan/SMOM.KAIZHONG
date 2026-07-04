using SIE.Domain;
using SIE.MetaModel;
using System;

namespace SIE.MES.WIP.Runtime
{
    /// <summary>
    /// 采集运行时产品信息
    /// </summary>
    [RootEntity, Serializable]
    public class ProductEntity : StringEntity
    {
        #region 产品信息1 Product1
        /// <summary>
        /// 产品信息1
        /// </summary> 
        public static readonly Property<string> Product1Property = P<ProductEntity>.Register(e => e.Product1);

        /// <summary>
        /// 产品信息1
        /// </summary>
        public string Product1
        {
            get { return this.GetProperty(Product1Property); }
            set { this.SetProperty(Product1Property, value); }
        }
        #endregion

        #region 产品信息2 Product2
        /// <summary>
        /// 产品信息2
        /// </summary> 
        public static readonly Property<string> Product2Property = P<ProductEntity>.Register(e => e.Product2);

        /// <summary>
        /// 产品信息2
        /// </summary>
        public string Product2
        {
            get { return this.GetProperty(Product2Property); }
            set { this.SetProperty(Product2Property, value); }
        }
        #endregion

        #region 产品信息3 Product3
        /// <summary>
        /// 产品信息3
        /// </summary> 
        public static readonly Property<string> Product3Property = P<ProductEntity>.Register(e => e.Product3);

        /// <summary>
        /// 产品信息3
        /// </summary>
        public string Product3
        {
            get { return this.GetProperty(Product3Property); }
            set { this.SetProperty(Product3Property, value); }
        }
        #endregion

        #region 产品信息4 Product4
        /// <summary>
        /// 产品信息4
        /// </summary> 
        public static readonly Property<string> Product4Property = P<ProductEntity>.Register(e => e.Product4);

        /// <summary>
        /// 产品信息4
        /// </summary>
        public string Product4
        {
            get { return this.GetProperty(Product4Property); }
            set { this.SetProperty(Product4Property, value); }
        }
        #endregion

        #region 产品信息5 Product5
        /// <summary>
        /// 产品信息5
        /// </summary> 
        public static readonly Property<string> Product5Property = P<ProductEntity>.Register(e => e.Product5);

        /// <summary>
        /// 产品信息5
        /// </summary>
        public string Product5
        {
            get { return this.GetProperty(Product5Property); }
            set { this.SetProperty(Product5Property, value); }
        }
        #endregion

        #region 产品信息6 Product6
        /// <summary>
        /// 产品信息6
        /// </summary> 
        public static readonly Property<string> Product6Property = P<ProductEntity>.Register(e => e.Product6);

        /// <summary>
        /// 产品信息6
        /// </summary>
        public string Product6
        {
            get { return this.GetProperty(Product6Property); }
            set { this.SetProperty(Product6Property, value); }
        }
        #endregion

        #region 产品信息7 Product7
        /// <summary>
        /// 产品信息7
        /// </summary> 
        public static readonly Property<string> Product7Property = P<ProductEntity>.Register(e => e.Product7);

        /// <summary>
        /// 产品信息7
        /// </summary>
        public string Product7
        {
            get { return this.GetProperty(Product7Property); }
            set { this.SetProperty(Product7Property, value); }
        }
        #endregion

        #region 产品信息8 Product8
        /// <summary>
        /// 产品信息8
        /// </summary> 
        public static readonly Property<string> Product8Property = P<ProductEntity>.Register(e => e.Product8);

        /// <summary>
        /// 产品信息8
        /// </summary>
        public string Product8
        {
            get { return this.GetProperty(Product8Property); }
            set { this.SetProperty(Product8Property, value); }
        }
        #endregion

        #region 产品信息9 Product9
        /// <summary>
        /// 产品信息9
        /// </summary> 
        public static readonly Property<string> Product9Property = P<ProductEntity>.Register(e => e.Product9);

        /// <summary>
        /// 产品信息9
        /// </summary>
        public string Product9
        {
            get { return this.GetProperty(Product9Property); }
            set { this.SetProperty(Product9Property, value); }
        }
        #endregion

        #region 产品信息10 Product10
        /// <summary>
        /// 产品信息10
        /// </summary> 
        public static readonly Property<string> Product10Property = P<ProductEntity>.Register(e => e.Product10);

        /// <summary>
        /// 产品信息10
        /// </summary>
        public string Product10
        {
            get { return this.GetProperty(Product10Property); }
            set { this.SetProperty(Product10Property, value); }
        }
        #endregion 

        #region 产品信息11 Product11
        /// <summary>
        /// 产品信息11
        /// </summary> 
        public static readonly Property<string> Product11Property = P<ProductEntity>.Register(e => e.Product11);

        /// <summary>
        /// 产品信息11
        /// </summary>
        public string Product11
        {
            get { return this.GetProperty(Product11Property); }
            set { this.SetProperty(Product11Property, value); }
        }
        #endregion

        #region 产品信息12 Product12
        /// <summary>
        /// 产品信息12
        /// </summary> 
        public static readonly Property<string> Product12Property = P<ProductEntity>.Register(e => e.Product12);

        /// <summary>
        /// 产品信息12
        /// </summary>
        public string Product12
        {
            get { return this.GetProperty(Product12Property); }
            set { this.SetProperty(Product12Property, value); }
        }
        #endregion

        #region 产品信息13 Product13
        /// <summary>
        /// 产品信息13
        /// </summary> 
        public static readonly Property<string> Product13Property = P<ProductEntity>.Register(e => e.Product13);

        /// <summary>
        /// 产品信息13
        /// </summary>
        public string Product13
        {
            get { return this.GetProperty(Product13Property); }
            set { this.SetProperty(Product13Property, value); }
        }
        #endregion

        #region 产品信息14 Product14
        /// <summary>
        /// 产品信息14
        /// </summary> 
        public static readonly Property<string> Product14Property = P<ProductEntity>.Register(e => e.Product14);

        /// <summary>
        /// 产品信息14
        /// </summary>
        public string Product14
        {
            get { return this.GetProperty(Product14Property); }
            set { this.SetProperty(Product14Property, value); }
        }
        #endregion

        #region 产品信息15 Product15
        /// <summary>
        /// 产品信息15
        /// </summary> 
        public static readonly Property<string> Product15Property = P<ProductEntity>.Register(e => e.Product15);

        /// <summary>
        /// 产品信息15
        /// </summary>
        public string Product15
        {
            get { return this.GetProperty(Product15Property); }
            set { this.SetProperty(Product15Property, value); }
        }
        #endregion

        #region 产品信息16 Product16
        /// <summary>
        /// 产品信息16
        /// </summary> 
        public static readonly Property<string> Product16Property = P<ProductEntity>.Register(e => e.Product16);

        /// <summary>
        /// 产品信息16
        /// </summary>
        public string Product16
        {
            get { return this.GetProperty(Product16Property); }
            set { this.SetProperty(Product16Property, value); }
        }
        #endregion

        #region 产品信息17 Product17
        /// <summary>
        /// 产品信息17
        /// </summary> 
        public static readonly Property<string> Product17Property = P<ProductEntity>.Register(e => e.Product17);

        /// <summary>
        /// 产品信息17
        /// </summary>
        public string Product17
        {
            get { return this.GetProperty(Product17Property); }
            set { this.SetProperty(Product17Property, value); }
        }
        #endregion

        #region 产品信息18 Product18
        /// <summary>
        /// 产品信息18
        /// </summary> 
        public static readonly Property<string> Product18Property = P<ProductEntity>.Register(e => e.Product18);

        /// <summary>
        /// 产品信息18
        /// </summary>
        public string Product18
        {
            get { return this.GetProperty(Product18Property); }
            set { this.SetProperty(Product18Property, value); }
        }
        #endregion

        #region 产品信息19 Product19
        /// <summary>
        /// 产品信息19
        /// </summary> 
        public static readonly Property<string> Product19Property = P<ProductEntity>.Register(e => e.Product19);

        /// <summary>
        /// 产品信息19
        /// </summary>
        public string Product19
        {
            get { return this.GetProperty(Product19Property); }
            set { this.SetProperty(Product19Property, value); }
        }
        #endregion

        #region 产品信息20 Product20
        /// <summary>
        /// 产品信息20
        /// </summary> 
        public static readonly Property<string> Product20Property = P<ProductEntity>.Register(e => e.Product20);

        /// <summary>
        /// 产品信息20
        /// </summary>
        public string Product20
        {
            get { return this.GetProperty(Product20Property); }
            set { this.SetProperty(Product20Property, value); }
        }
        #endregion 

        #region 产品信息21 Product21
        /// <summary>
        /// 产品信息21
        /// </summary> 
        public static readonly Property<string> Product21Property = P<ProductEntity>.Register(e => e.Product21);

        /// <summary>
        /// 产品信息21
        /// </summary>
        public string Product21
        {
            get { return this.GetProperty(Product21Property); }
            set { this.SetProperty(Product21Property, value); }
        }
        #endregion

        #region 产品信息22 Product22
        /// <summary>
        /// 产品信息22
        /// </summary> 
        public static readonly Property<string> Product22Property = P<ProductEntity>.Register(e => e.Product22);

        /// <summary>
        /// 产品信息22
        /// </summary>
        public string Product22
        {
            get { return this.GetProperty(Product22Property); }
            set { this.SetProperty(Product22Property, value); }
        }
        #endregion

        #region 产品信息23 Product23
        /// <summary>
        /// 产品信息23
        /// </summary> 
        public static readonly Property<string> Product23Property = P<ProductEntity>.Register(e => e.Product23);

        /// <summary>
        /// 产品信息23
        /// </summary>
        public string Product23
        {
            get { return this.GetProperty(Product23Property); }
            set { this.SetProperty(Product23Property, value); }
        }
        #endregion

        #region 产品信息24 Product24
        /// <summary>
        /// 产品信息24
        /// </summary> 
        public static readonly Property<string> Product24Property = P<ProductEntity>.Register(e => e.Product24);

        /// <summary>
        /// 产品信息24
        /// </summary>
        public string Product24
        {
            get { return this.GetProperty(Product24Property); }
            set { this.SetProperty(Product24Property, value); }
        }
        #endregion

        #region 产品信息25 Product25
        /// <summary>
        /// 产品信息25
        /// </summary> 
        public static readonly Property<string> Product25Property = P<ProductEntity>.Register(e => e.Product25);

        /// <summary>
        /// 产品信息25
        /// </summary>
        public string Product25
        {
            get { return this.GetProperty(Product25Property); }
            set { this.SetProperty(Product25Property, value); }
        }
        #endregion

        #region 产品信息26 Product26
        /// <summary>
        /// 产品信息26
        /// </summary> 
        public static readonly Property<string> Product26Property = P<ProductEntity>.Register(e => e.Product26);

        /// <summary>
        /// 产品信息26
        /// </summary>
        public string Product26
        {
            get { return this.GetProperty(Product26Property); }
            set { this.SetProperty(Product26Property, value); }
        }
        #endregion

        #region 产品信息27 Product27
        /// <summary>
        /// 产品信息27
        /// </summary> 
        public static readonly Property<string> Product27Property = P<ProductEntity>.Register(e => e.Product27);

        /// <summary>
        /// 产品信息27
        /// </summary>
        public string Product27
        {
            get { return this.GetProperty(Product27Property); }
            set { this.SetProperty(Product27Property, value); }
        }
        #endregion

        #region 产品信息28 Product28
        /// <summary>
        /// 产品信息28
        /// </summary> 
        public static readonly Property<string> Product28Property = P<ProductEntity>.Register(e => e.Product28);

        /// <summary>
        /// 产品信息28
        /// </summary>
        public string Product28
        {
            get { return this.GetProperty(Product28Property); }
            set { this.SetProperty(Product28Property, value); }
        }
        #endregion

        #region 产品信息29 Product29
        /// <summary>
        /// 产品信息29
        /// </summary> 
        public static readonly Property<string> Product29Property = P<ProductEntity>.Register(e => e.Product29);

        /// <summary>
        /// 产品信息29
        /// </summary>
        public string Product29
        {
            get { return this.GetProperty(Product29Property); }
            set { this.SetProperty(Product29Property, value); }
        }
        #endregion

        #region 产品信息30 Product30
        /// <summary>
        /// 产品信息30
        /// </summary> 
        public static readonly Property<string> Product30Property = P<ProductEntity>.Register(e => e.Product30);

        /// <summary>
        /// 产品信息30
        /// </summary>
        public string Product30
        {
            get { return this.GetProperty(Product30Property); }
            set { this.SetProperty(Product30Property, value); }
        }
        #endregion

        #region 产品信息31 Product31
        /// <summary>
        /// 产品信息31
        /// </summary> 
        public static readonly Property<string> Product31Property = P<ProductEntity>.Register(e => e.Product31);

        /// <summary>
        /// 产品信息31
        /// </summary>
        public string Product31
        {
            get { return this.GetProperty(Product31Property); }
            set { this.SetProperty(Product31Property, value); }
        }
        #endregion

        #region 产品信息32 Product32
        /// <summary>
        /// 产品信息32
        /// </summary> 
        public static readonly Property<string> Product32Property = P<ProductEntity>.Register(e => e.Product32);

        /// <summary>
        /// 产品信息32
        /// </summary>
        public string Product32
        {
            get { return this.GetProperty(Product32Property); }
            set { this.SetProperty(Product32Property, value); }
        }
        #endregion

        #region 产品信息33 Product33
        /// <summary>
        /// 产品信息33
        /// </summary> 
        public static readonly Property<string> Product33Property = P<ProductEntity>.Register(e => e.Product33);

        /// <summary>
        /// 产品信息33
        /// </summary>
        public string Product33
        {
            get { return this.GetProperty(Product33Property); }
            set { this.SetProperty(Product33Property, value); }
        }
        #endregion

        #region 产品信息34 Product34
        /// <summary>
        /// 产品信息34
        /// </summary> 
        public static readonly Property<string> Product34Property = P<ProductEntity>.Register(e => e.Product34);

        /// <summary>
        /// 产品信息34
        /// </summary>
        public string Product34
        {
            get { return this.GetProperty(Product34Property); }
            set { this.SetProperty(Product34Property, value); }
        }
        #endregion

        #region 产品信息35 Product35
        /// <summary>
        /// 产品信息35
        /// </summary> 
        public static readonly Property<string> Product35Property = P<ProductEntity>.Register(e => e.Product35);

        /// <summary>
        /// 产品信息35
        /// </summary>
        public string Product35
        {
            get { return this.GetProperty(Product35Property); }
            set { this.SetProperty(Product35Property, value); }
        }
        #endregion

        #region 产品信息36 Product36
        /// <summary>
        /// 产品信息36
        /// </summary> 
        public static readonly Property<string> Product36Property = P<ProductEntity>.Register(e => e.Product36);

        /// <summary>
        /// 产品信息36
        /// </summary>
        public string Product36
        {
            get { return this.GetProperty(Product36Property); }
            set { this.SetProperty(Product36Property, value); }
        }
        #endregion

        #region 产品信息37 Product37
        /// <summary>
        /// 产品信息37
        /// </summary> 
        public static readonly Property<string> Product37Property = P<ProductEntity>.Register(e => e.Product37);

        /// <summary>
        /// 产品信息37
        /// </summary>
        public string Product37
        {
            get { return this.GetProperty(Product37Property); }
            set { this.SetProperty(Product37Property, value); }
        }
        #endregion

        #region 产品信息38 Product38
        /// <summary>
        /// 产品信息38
        /// </summary> 
        public static readonly Property<string> Product38Property = P<ProductEntity>.Register(e => e.Product38);

        /// <summary>
        /// 产品信息38
        /// </summary>
        public string Product38
        {
            get { return this.GetProperty(Product38Property); }
            set { this.SetProperty(Product38Property, value); }
        }
        #endregion

        #region 产品信息39 Product39
        /// <summary>
        /// 产品信息39
        /// </summary> 
        public static readonly Property<string> Product39Property = P<ProductEntity>.Register(e => e.Product39);

        /// <summary>
        /// 产品信息39
        /// </summary>
        public string Product39
        {
            get { return this.GetProperty(Product39Property); }
            set { this.SetProperty(Product39Property, value); }
        }
        #endregion

        #region 产品信息40 Product40
        /// <summary>
        /// 产品信息40
        /// </summary> 
        public static readonly Property<string> Product40Property = P<ProductEntity>.Register(e => e.Product40);

        /// <summary>
        /// 产品信息40
        /// </summary>
        public string Product40
        {
            get { return this.GetProperty(Product40Property); }
            set { this.SetProperty(Product40Property, value); }
        }
        #endregion

        #region 产品信息41 Product41
        /// <summary>
        /// 产品信息41
        /// </summary> 
        public static readonly Property<string> Product41Property = P<ProductEntity>.Register(e => e.Product41);

        /// <summary>
        /// 产品信息41
        /// </summary>
        public string Product41
        {
            get { return this.GetProperty(Product41Property); }
            set { this.SetProperty(Product41Property, value); }
        }
        #endregion

        #region 产品信息42 Product42
        /// <summary>
        /// 产品信息42
        /// </summary> 
        public static readonly Property<string> Product42Property = P<ProductEntity>.Register(e => e.Product42);

        /// <summary>
        /// 产品信息42
        /// </summary>
        public string Product42
        {
            get { return this.GetProperty(Product42Property); }
            set { this.SetProperty(Product42Property, value); }
        }
        #endregion

        #region 产品信息43 Product43
        /// <summary>
        /// 产品信息43
        /// </summary> 
        public static readonly Property<string> Product43Property = P<ProductEntity>.Register(e => e.Product43);

        /// <summary>
        /// 产品信息43
        /// </summary>
        public string Product43
        {
            get { return this.GetProperty(Product43Property); }
            set { this.SetProperty(Product43Property, value); }
        }
        #endregion

        #region 产品信息44 Product44
        /// <summary>
        /// 产品信息44
        /// </summary> 
        public static readonly Property<string> Product44Property = P<ProductEntity>.Register(e => e.Product44);

        /// <summary>
        /// 产品信息44
        /// </summary>
        public string Product44
        {
            get { return this.GetProperty(Product44Property); }
            set { this.SetProperty(Product44Property, value); }
        }
        #endregion

        #region 产品信息45 Product45
        /// <summary>
        /// 产品信息45
        /// </summary> 
        public static readonly Property<string> Product45Property = P<ProductEntity>.Register(e => e.Product45);

        /// <summary>
        /// 产品信息45
        /// </summary>
        public string Product45
        {
            get { return this.GetProperty(Product45Property); }
            set { this.SetProperty(Product45Property, value); }
        }
        #endregion

        #region 产品信息46 Product46
        /// <summary>
        /// 产品信息46
        /// </summary> 
        public static readonly Property<string> Product46Property = P<ProductEntity>.Register(e => e.Product46);

        /// <summary>
        /// 产品信息46
        /// </summary>
        public string Product46
        {
            get { return this.GetProperty(Product46Property); }
            set { this.SetProperty(Product46Property, value); }
        }
        #endregion

        #region 产品信息47 Product47
        /// <summary>
        /// 产品信息47
        /// </summary> 
        public static readonly Property<string> Product47Property = P<ProductEntity>.Register(e => e.Product47);

        /// <summary>
        /// 产品信息47
        /// </summary>
        public string Product47
        {
            get { return this.GetProperty(Product47Property); }
            set { this.SetProperty(Product47Property, value); }
        }
        #endregion

        #region 产品信息48 Product48
        /// <summary>
        /// 产品信息48
        /// </summary> 
        public static readonly Property<string> Product48Property = P<ProductEntity>.Register(e => e.Product48);

        /// <summary>
        /// 产品信息48
        /// </summary>
        public string Product48
        {
            get { return this.GetProperty(Product48Property); }
            set { this.SetProperty(Product48Property, value); }
        }
        #endregion

        #region 产品信息49 Product49
        /// <summary>
        /// 产品信息49
        /// </summary> 
        public static readonly Property<string> Product49Property = P<ProductEntity>.Register(e => e.Product49);

        /// <summary>
        /// 产品信息49
        /// </summary>
        public string Product49
        {
            get { return this.GetProperty(Product49Property); }
            set { this.SetProperty(Product49Property, value); }
        }
        #endregion

        #region 产品信息50 Product50
        /// <summary>
        /// 产品信息50
        /// </summary> 
        public static readonly Property<string> Product50Property = P<ProductEntity>.Register(e => e.Product50);

        /// <summary>
        /// 产品信息50
        /// </summary>
        public string Product50
        {
            get { return this.GetProperty(Product50Property); }
            set { this.SetProperty(Product50Property, value); }
        }
        #endregion
    }

    /// <summary>
    ///  实体配置
    /// </summary>
    internal class ProductInfoConfig : EntityConfig<ProductEntity>
    {
        /// <summary>
        /// 配置数据库映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_RT_PRODUCT").MapAllProperties();
            Meta.Property(ProductEntity.Product1Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product2Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product3Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product4Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product5Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product6Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product7Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product8Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product9Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product10Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product11Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product12Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product13Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product14Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product15Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product16Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product17Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product18Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product19Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product20Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product21Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product22Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product23Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product24Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product25Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product26Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product27Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product28Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product29Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product30Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product31Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product32Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product33Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product34Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product35Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product36Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product37Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product38Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product39Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product40Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product41Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product42Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product43Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product44Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product45Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product46Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product47Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product48Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product49Property).MapColumn().HasLength("4000");
            Meta.Property(ProductEntity.Product50Property).MapColumn().HasLength("4000");
            Meta.EnableTimeStamp(); //// 避免同时过站
        }
    }
}
