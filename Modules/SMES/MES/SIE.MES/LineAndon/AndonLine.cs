using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.EquipAccounts;
using SIE.MES.Andon;
using SIE.MES.Checker;
using SIE.MES.EmpWork;
using SIE.MES.ItemEquipAccount;
using SIE.MES.ItemFixture;
using SIE.MES.ProcessProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WorkCenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.LineAndon
{
    /// <summary>
    /// 产线与安灯区域
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(AndonLineCriterial))]
    [Label("产线与安灯区域")]
    [DisplayMember(nameof(MachineCode))]
    public partial class AndonLine : DataEntity
    {
        #region 序号 Seq
        /// <summary>
        /// 序号
        /// </summary>
        [Label("序号")]
        public static readonly Property<string> SeqProperty = P<AndonLine>.Register(e => e.Seq);

        /// <summary>
        /// 序号
        /// </summary>
        public string Seq
        {
            get { return this.GetProperty(SeqProperty); }
            set { this.SetProperty(SeqProperty, value); }
        }
        #endregion

        #region 产线/机台编码 MachineCode
        /// <summary>
        /// 产线/机台编码
        /// </summary>
        [Required]
        [Label("产线/机台编码")]
        public static readonly Property<string> MachineCodeProperty = P<AndonLine>.Register(e => e.MachineCode);

        /// <summary>
        /// 产线/机台编码
        /// </summary>
        public string MachineCode
        {
            get { return this.GetProperty(MachineCodeProperty); }
            set { this.SetProperty(MachineCodeProperty, value); }
        }
        #endregion

        #region 产线/机台名称 MachineName
        /// <summary>
        /// 产线/机台名称
        /// </summary>
        [Required]
        [Label("产线/机台名称")]
        public static readonly Property<string> MachineNameProperty = P<AndonLine>.Register(e => e.MachineName);

        /// <summary>
        /// 产线/机台名称
        /// </summary>
        public string MachineName
        {
            get { return this.GetProperty(MachineNameProperty); }
            set { this.SetProperty(MachineNameProperty, value); }
        }
        #endregion

        //EquipAccount
        //#region 主设备号 EquipmentNo
        ///// <summary>
        ///// 主设备号
        ///// </summary>
        //[Required]
        //[Label("主设备号")]
        //public static readonly Property<string> EquipmentNoProperty = P<AndonLine>.Register(e => e.EquipmentNo);

        ///// <summary>
        ///// 主设备号
        ///// </summary>
        //public string EquipmentNo
        //{
        //    get { return this.GetProperty(EquipmentNoProperty); }
        //    set { this.SetProperty(EquipmentNoProperty, value); }
        //}
        //#endregion


        #region 主设备号 Equipment
        /// <summary>
        /// 主设备号Id
        /// </summary>
        [Label("主设备号")]
        public static readonly IRefIdProperty EquipmentIdProperty = P<AndonLine>.RegisterRefId(e => e.EquipmentId, ReferenceType.Normal);

        /// <summary>
        /// 主设备号Id
        /// </summary>
        public double? EquipmentId
        {
            get { return (double?)GetRefNullableId(EquipmentIdProperty); }
            set { SetRefNullableId(EquipmentIdProperty, value); }
        }

        /// <summary>
        /// 主设备号
        /// </summary>
        public static readonly RefEntityProperty<EquipAccount> EquipmentProperty = P<AndonLine>.RegisterRef(e => e.Equipment, EquipmentIdProperty);

        /// <summary>
        /// 主设备号
        /// </summary>
        public EquipAccount Equipment
        {
            get { return GetRefEntity(EquipmentProperty); }
            set { SetRefEntity(EquipmentProperty, value); }
        }
        #endregion


        #region 设备名称 EquipmentName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipmentNameProperty = P<AndonLine>.RegisterView(e => e.EquipmentName, p => p.Equipment.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName
        {
            get { return GetProperty(EquipmentNameProperty); }
        }
        #endregion

        #region 购置日期 EquipmentDate
        /// <summary>
        /// 购置日期
        /// </summary>
        [Label("购置日期")]
        public static readonly Property<string> EquipmentDateProperty = P<AndonLine>.RegisterView(e => e.EquipmentDate, p => p.Equipment.PurchaseDate);

        /// <summary>
        /// 购置日期
        /// </summary>
        public string EquipmentDate
        {
            get { return GetProperty(EquipmentDateProperty); }
        }
        #endregion

        #region 工作中心编码 WorkCenter
        /// <summary>
        /// 工作中心编码Id
        /// </summary>
        [Label("工作中心编码")]
        public static readonly IRefIdProperty WorkCenterIdProperty = P<AndonLine>.RegisterRefId(e => e.WorkCenterId, ReferenceType.Normal);

        /// <summary>
        /// 工作中心编码Id
        /// </summary>
        public double WorkCenterId
        {
            get { return (double)GetRefNullableId(WorkCenterIdProperty); }
            set { SetRefNullableId(WorkCenterIdProperty, value); }
        }

        /// <summary>
        /// 工作中心编码
        /// </summary>
        public static readonly RefEntityProperty<WorkCenter> WorkCenterProperty = P<AndonLine>.RegisterRef(e => e.WorkCenter, WorkCenterIdProperty);

        /// <summary>
        /// 工作中心编码
        /// </summary>
        public WorkCenter WorkCenter
        {
            get { return GetRefEntity(WorkCenterProperty); }
            set { SetRefEntity(WorkCenterProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty =
            P<AndonLine>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double WorkShopId
        {
            get { return (double)this.GetRefNullableId(WorkShopIdProperty); }
            set { this.SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty =
            P<AndonLine>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        [Required]
        public static readonly IRefIdProperty FactoryIdProperty =
            P<AndonLine>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? FactoryId
        {
            get { return (double?)this.GetRefNullableId(FactoryIdProperty); }
            set { this.SetRefNullableId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty =
            P<AndonLine>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 区域描述 AndonUphold
        /// <summary>
        /// 区域描述Id
        /// </summary>
        [Label("区域描述")]
        public static readonly IRefIdProperty AndonUpholdIdProperty =
            P<AndonLine>.RegisterRefId(e => e.AndonUpholdId, ReferenceType.Normal);

        /// <summary>
        /// 区域描述Id
        /// </summary>
        public double? AndonUpholdId
        {
            get { return (double?)this.GetRefNullableId(AndonUpholdIdProperty); }
            set { this.SetRefNullableId(AndonUpholdIdProperty, value); }
        }

        /// <summary>
        /// 区域描述
        /// </summary>
        public static readonly RefEntityProperty<AndonUphold> AndonUpholdProperty =
            P<AndonLine>.RegisterRef(e => e.AndonUphold, AndonUpholdIdProperty);

        /// <summary>
        /// 区域描述
        /// </summary>
        public AndonUphold AndonUphold
        {
            get { return this.GetRefEntity(AndonUpholdProperty); }
            set { this.SetRefEntity(AndonUpholdProperty, value); }
        }
        #endregion

        #region 安灯编码 AndonCode
        /// <summary>
        /// 安灯编码
        /// </summary>
        [Label("安灯编码")]
        public static readonly Property<string> AndonCodeProperty = P<AndonLine>.Register(e => e.AndonCode);

        /// <summary>
        /// 安灯编码
        /// </summary>
        public string AndonCode
        {
            get { return this.GetProperty(AndonCodeProperty); }
            set { this.SetProperty(AndonCodeProperty, value); }
        }
        #endregion

        #region 是否本地打印 IsLocalPrint
        /// <summary>
        /// 是否本地打印
        /// </summary>
        [Label("是否本地打印")]
        public static readonly Property<bool> IsLocalPrintProperty = P<AndonLine>.Register(e => e.IsLocalPrint);

        /// <summary>
        /// 是否本地打印
        /// </summary>
        public bool IsLocalPrint
        {
            get { return this.GetProperty(IsLocalPrintProperty); }
            set { this.SetProperty(IsLocalPrintProperty, value); }
        }
        #endregion

        #region 打印机IP PrinterIp
        /// <summary>
        /// 打印机IP
        /// </summary>
        [Label("打印机IP")]
        public static readonly Property<string> PrinterIpProperty = P<AndonLine>.Register(e => e.PrinterIp);

        /// <summary>
        /// 打印机IP
        /// </summary>
        public string PrinterIp
        {
            get { return this.GetProperty(PrinterIpProperty); }
            set { this.SetProperty(PrinterIpProperty, value); }
        }
        #endregion

        #region IOT指令 AndonOrder
        /// <summary>
        /// IOT指令
        /// </summary>
        [Label("IOT指令")]
        public static readonly Property<string> AndonOrderProperty = P<AndonLine>.Register(e => e.AndonOrder);

        /// <summary>
        /// IOT指令
        /// </summary>
        public string AndonOrder
        {
            get { return this.GetProperty(AndonOrderProperty); }
            set { this.SetProperty(AndonOrderProperty, value); }
        }
        #endregion

        #region IOT实体 AndonEntity
        /// <summary>
        /// IOT实体
        /// </summary>
        [Label("IOT实体")]
        public static readonly Property<string> AndonEntityProperty = P<AndonLine>.Register(e => e.AndonEntity);

        /// <summary>
        /// IOT实体
        /// </summary>
        public string AndonEntity
        {
            get { return this.GetProperty(AndonEntityProperty); }
            set { this.SetProperty(AndonEntityProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 车间编码 WorkShopCode
        /// <summary>
        /// 车间编码
        /// </summary>
        [Label("车间编码")]
        public static readonly Property<string> WorkShopCodeProperty = P<AndonLine>.RegisterView(e => e.WorkShopCode, p => p.WorkShop.Code);

        /// <summary>
        /// 车间编码
        /// </summary>
        public string WorkShopCode
        {
            get { return this.GetProperty(WorkShopCodeProperty); }
        }

        #endregion

        #region 车间名称 WorkShopName
        /// <summary>
        /// 车间名称
        /// </summary>
        [Label("车间名称")]
        public static readonly Property<string> WorkShopNameProperty = P<AndonLine>.RegisterView(e => e.WorkShopName, p => p.WorkShop.Name);

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName
        {
            get { return this.GetProperty(WorkShopNameProperty); }
        }

        #endregion

        #region 工厂编码 FactoryCode
        /// <summary>
        /// 工厂编码
        /// </summary>
        [Label("工厂编码")]
        public static readonly Property<string> FactoryCodeProperty = P<AndonLine>.RegisterView(e => e.FactoryCode, p => p.Factory.Code);

        /// <summary>
        /// 工厂编码
        /// </summary>
        public string FactoryCode
        {
            get { return this.GetProperty(FactoryCodeProperty); }
        }

        #endregion

        #region 设备编码 EquipmentNo
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipmentNoProperty = P<AndonLine>.RegisterView(e => e.EquipmentNo, p => p.Equipment.Code);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentNo
        {
            get { return this.GetProperty(EquipmentNoProperty); }
        }

        #endregion

        #region 工厂名称 FactoryName
        /// <summary>
        /// 工厂名称
        /// </summary>
        [Label("工厂名称")]
        public static readonly Property<string> FactoryNameProperty = P<AndonLine>.RegisterView(e => e.FactoryName, p => p.Factory.Name);

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName
        {
            get { return this.GetProperty(FactoryNameProperty); }
        }

        #endregion

        #region 工作中心名称 WorkCenterName
        /// <summary>
        /// 工作中心名称
        /// </summary>
        [Label("工作中心名称")]
        public static readonly Property<string> WorkCenterNameProperty = P<AndonLine>.RegisterView(e => e.WorkCenterName, p => p.WorkCenter.Name);

        /// <summary>
        /// 工作中心名称
        /// </summary>
        public string WorkCenterName
        {
            get { return this.GetProperty(WorkCenterNameProperty); }
        }
        #endregion

        #region 工作中心编码 WorkCenterCode
        /// <summary>
        /// 工作中心编码
        /// </summary>
        [Label("工作中心编码")]
        public static readonly Property<string> WorkCenterCodeProperty = P<AndonLine>.RegisterView(e => e.WorkCenterCode, p => p.WorkCenter.Code);

        /// <summary>
        /// 工作中心编码
        /// </summary>
        public string WorkCenterCode
        {
            get { return this.GetProperty(WorkCenterCodeProperty); }
        }

        #endregion

        #region 安灯描述 AndonDesc
        /// <summary>
        /// 安灯描述
        /// </summary>
        [Label("安灯描述")]
        public static readonly Property<string> AndonDescProperty = P<AndonLine>.RegisterView(e => e.AndonDesc, p => p.AndonUphold.AndonDesc);

        /// <summary>
        /// 安灯描述
        /// </summary>
        public string AndonDesc
        {
            get { return this.GetProperty(AndonDescProperty); }
        }

        #endregion

        //#region 安灯编码 AndonCode
        ///// <summary>
        ///// 安灯编码
        ///// </summary>
        //[Label("安灯编码")]
        //public static readonly Property<string> AndonCodeProperty = P<AndonLine>.RegisterView(e => e.AndonCode, p => p.AndonUphold.AndonCode);

        ///// <summary>
        ///// 安灯编码
        ///// </summary>
        //public string AndonCode
        //{
        //    get { return this.GetProperty(AndonCodeProperty); }
        //}

        //#endregion

        #endregion
    }

    /// <summary>
    /// 产线与安灯区域 实体配置
    /// </summary>
    internal class AndonLineConfig : EntityConfig<AndonLine>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(AndonLine.MachineCodeProperty, new NotDuplicateRule());
            rules.AddRule(AndonLine.MachineNameProperty, new NotDuplicateRule());

            rules.AddRule(new NotDuplicateRule()
            {
                Properties =
                {
                    AndonLine.MachineCodeProperty,
                    AndonLine.MachineNameProperty,
                    AndonLine.WorkShopIdProperty,
                    AndonLine.FactoryIdProperty,
                    AndonLine.AndonUpholdIdProperty,
                    AndonLine.WorkCenterIdProperty,
                    AndonLine.AndonCodeProperty
                },
                MessageBuilder = (e) =>
                {
                    return "数据已存在!".L10N();
                }
            });
            base.AddValidations(rules);
        }
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ANDON_LINE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
    /// <summary>
    /// 工作中心被产线与安灯区域关系引用不允许删除
    /// </summary>
    //[System.ComponentModel.DisplayName("工作中心被产线与安灯区域关系引用不允许删除")]
    //[System.ComponentModel.Description("工作中心被产线与安灯区域关系引用不允许删除")]
    //public partial class UndeleteAndonLine : NoReferencedRule<WorkCenter>
    //{
    //    /// <summary>
    //    /// 构造函数
    //    /// </summary>
    //    public UndeleteAndonLine()
    //    {
    //        Properties.Add(AndonLine.WorkCenterIdProperty);
    //        MessageBuilder = (o, e) =>
    //        {
    //            var process = o as WorkCenter;
    //            return "工作中心[{0}]已经被[{1}]引用,不能删除".L10nFormat(process.Code, "工作中心被产线与安灯区域的关系".L10N());
    //        };
    //    }
    //}

    /// <summary>
    /// 车间被产线与安灯区域关系引用不允许删除
    /// </summary>
    //[System.ComponentModel.DisplayName("车间被产线与安灯区域关系引用不允许删除")]
    //[System.ComponentModel.Description("车间被产线与安灯区域关系引用不允许删除")]
    //public partial class UndeleteWorkShop : NoReferencedRule<Enterprise>
    //{
    //    /// <summary>
    //    /// 构造函数
    //    /// </summary>
    //    public UndeleteWorkShop()
    //    {
    //        Properties.Add(AndonLine.WorkShopIdProperty);
    //        MessageBuilder = (o, e) =>
    //        {
    //            var process = o as WorkCenter;
    //            return "工作中心[{0}]已经被[{1}]引用,不能删除".L10nFormat(process.Code, "车间被产线与安灯区域的关系".L10N());
    //        };
    //    }
    //}
    
    /// <summary>
    /// 安灯区域被产线与安灯区域关系引用不允许删除
    /// </summary>
    //[System.ComponentModel.DisplayName("安灯区域被产线与安灯区域关系引用不允许删除")]
    //[System.ComponentModel.Description("安灯区域被产线与安灯区域关系引用不允许删除")]
    //public partial class UndeleteAndonUphold : NoReferencedRule<AndonUphold>
    //{
    //    /// <summary>
    //    /// 构造函数
    //    /// </summary>
    //    public UndeleteAndonUphold()
    //    {
    //        Properties.Add(AndonLine.AndonUpholdIdProperty);
    //        MessageBuilder = (o, e) =>
    //        {
    //            var process = o as WorkCenter;
    //            return "工作中心[{0}]已经被[{1}]引用,不能删除".L10nFormat(process.Code, "安灯区域被产线与安灯区域的关系".L10N());
    //        };
    //    }
    //}

}
