using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Core.Prints
{
    /// <summary>
    /// KZ模板打印设置,不分组织
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("KZ模板打印设置")]
    [DisplayMember(nameof(TplName))]
    public class PrinterSettingTpl : DataEntity
    {
        #region 员工 Employee
        /// <summary>
        /// 员工Id
        /// </summary>
        [Label("员工")]
        public static readonly IRefIdProperty EmployeeIdProperty =
            P<PrinterSettingTpl>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 员工Id
        /// </summary>
        public double? EmployeeId
        {
            get { return (double?)this.GetRefNullableId(EmployeeIdProperty); }
            set { this.SetRefNullableId(EmployeeIdProperty, value); }
        }

        /// <summary>
        /// 员工
        /// </summary>
        public static readonly RefEntityProperty<Employee> EmployeeProperty =
            P<PrinterSettingTpl>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 员工
        /// </summary>
        public Employee Employee
        {
            get { return this.GetRefEntity(EmployeeProperty); }
            set { this.SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<PrinterSettingTpl>.Register(e => e.ProductCode);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
            set { this.SetProperty(ProductCodeProperty, value); }
        }
        #endregion

        #region 打印设置 TplName
        /// <summary>
        /// 打印设置
        /// </summary>
        [Label("打印设置")]
        //[Required]
        [MaxLength(255)]
        public static readonly Property<string> TplNameProperty = P<PrinterSettingTpl>.Register(e => e.TplName);

        /// <summary>
        /// 打印设置
        /// </summary>
        public string TplName
        {
            get { return this.GetProperty(TplNameProperty); }
            set { this.SetProperty(TplNameProperty, value); }
        }
        #endregion

        #region 打印机名称 PrinterName
        /// <summary>
        /// 打印机名称
        /// </summary>
        [Label("打印机名称")]
        public static readonly Property<string> PrinterNameProperty = P<PrinterSettingTpl>.Register(e => e.PrinterName);

        /// <summary>
        /// 打印机名称
        /// </summary>
        public string PrinterName
        {
            get { return this.GetProperty(PrinterNameProperty); }
            set { this.SetProperty(PrinterNameProperty, value); }
        }
        #endregion

        #region 页面宽度 PageWidth
        /// <summary>
        /// 页面宽度
        /// </summary>
        [Label("页面宽度")]
        public static readonly Property<int> PageWidthProperty = P<PrinterSettingTpl>.Register(e => e.PageWidth);

        /// <summary>
        /// 页面宽度
        /// </summary>
        public int PageWidth
        {
            get { return this.GetProperty(PageWidthProperty); }
            set { this.SetProperty(PageWidthProperty, value); }
        }
        #endregion

        #region 页面高度 PageHeight
        /// <summary>
        /// 页面高度
        /// </summary>
        [Label("页面高度")]
        public static readonly Property<int> PageHeightProperty = P<PrinterSettingTpl>.Register(e => e.PageHeight);

        /// <summary>
        /// 页面高度
        /// </summary>
        public int PageHeight
        {
            get { return this.GetProperty(PageHeightProperty); }
            set { this.SetProperty(PageHeightProperty, value); }
        }
        #endregion


        #region 上边距 MarginsTop
        /// <summary>
        /// 上边距
        /// </summary>
        [Label("上边距")]
        public static readonly Property<int> MarginsTopProperty = P<PrinterSettingTpl>.Register(e => e.MarginsTop);

        /// <summary>
        /// 上边距
        /// </summary>
        public int MarginsTop
        {
            get { return this.GetProperty(MarginsTopProperty); }
            set { this.SetProperty(MarginsTopProperty, value); }
        }
        #endregion

        #region 下边距 MarginsBottom
        /// <summary>
        /// 下边距
        /// </summary>
        [Label("下边距")]
        public static readonly Property<int> MarginsBottomProperty = P<PrinterSettingTpl>.Register(e => e.MarginsBottom);

        /// <summary>
        /// 下边距
        /// </summary>
        public int MarginsBottom
        {
            get { return this.GetProperty(MarginsBottomProperty); }
            set { this.SetProperty(MarginsBottomProperty, value); }
        }
        #endregion

        #region 左边距 MarginsLeft
        /// <summary>
        /// 左边距
        /// </summary>
        [Label("左边距")]
        public static readonly Property<int> MarginsLeftProperty = P<PrinterSettingTpl>.Register(e => e.MarginsLeft);

        /// <summary>
        /// 左边距
        /// </summary>
        public int MarginsLeft
        {
            get { return this.GetProperty(MarginsLeftProperty); }
            set { this.SetProperty(MarginsLeftProperty, value); }
        }
        #endregion

        #region 右边距 MarginsRight
        /// <summary>
        /// 右边距
        /// </summary>
        [Label("右边距")]
        public static readonly Property<int> MarginsRightProperty = P<PrinterSettingTpl>.Register(e => e.MarginsRight);

        /// <summary>
        /// 右边距
        /// </summary>
        public int MarginsRight
        {
            get { return this.GetProperty(MarginsRightProperty); }
            set { this.SetProperty(MarginsRightProperty, value); }
        }
        #endregion

        #region 分辨率 Resolution
        /// <summary>
        /// 分辨率
        /// </summary>
        [Label("分辨率")]
        public static readonly Property<int> ResolutionProperty = P<PrinterSettingTpl>.Register(e => e.Resolution);

        /// <summary>
        /// 分辨率
        /// </summary>
        public int Resolution
        {
            get { return this.GetProperty(ResolutionProperty); }
            set { this.SetProperty(ResolutionProperty, value); }
        }
        #endregion

        #region 列数 ColumnCount
        /// <summary>
        /// 列数
        /// </summary>
        [Label("列数")]
        public static readonly Property<int> ColumnCountProperty = P<PrinterSettingTpl>.Register(e => e.ColumnCount);

        /// <summary>
        /// 列数
        /// </summary>
        public int ColumnCount
        {
            get { return this.GetProperty(ColumnCountProperty); }
            set { this.SetProperty(ColumnCountProperty, value); }
        }
        #endregion

        #region 二维码X坐标 QrcodeX
        /// <summary>
        /// 二维码X坐标
        /// </summary>
        [Label("二维码X坐标")]
        public static readonly Property<int> QrcodeXProperty = P<PrinterSettingTpl>.Register(e => e.QrcodeX);

        /// <summary>
        /// 二维码X坐标
        /// </summary>
        public int QrcodeX
        {
            get { return this.GetProperty(QrcodeXProperty); }
            set { this.SetProperty(QrcodeXProperty, value); }
        }
        #endregion

        #region 二维码Y坐标 QrcodeY
        /// <summary>
        /// 二维码Y坐标
        /// </summary>
        [Label("二维码Y坐标")]
        public static readonly Property<int> QrcodeYProperty = P<PrinterSettingTpl>.Register(e => e.QrcodeY);

        /// <summary>
        /// 二维码Y坐标
        /// </summary>
        public int QrcodeY
        {
            get { return this.GetProperty(QrcodeYProperty); }
            set { this.SetProperty(QrcodeYProperty, value); }
        }
        #endregion

        #region 二维码宽度 QrcodeWidth
        /// <summary>
        /// 二维码宽度
        /// </summary>
        [Label("二维码宽度")]
        public static readonly Property<int> QrcodeWidthProperty = P<PrinterSettingTpl>.Register(e => e.QrcodeWidth);

        /// <summary>
        /// 二维码宽度
        /// </summary>
        public int QrcodeWidth
        {
            get { return this.GetProperty(QrcodeWidthProperty); }
            set { this.SetProperty(QrcodeWidthProperty, value); }
        }
        #endregion

        #region 二维码高度 QrcodeHeight
        /// <summary>
        /// 二维码高度
        /// </summary>
        [Label("二维码高度")]
        public static readonly Property<int> QrcodeHeightProperty = P<PrinterSettingTpl>.Register(e => e.QrcodeHeight);

        /// <summary>
        /// 二维码高度
        /// </summary>
        public int QrcodeHeight
        {
            get { return this.GetProperty(QrcodeHeightProperty); }
            set { this.SetProperty(QrcodeHeightProperty, value); }
        }
        #endregion


        #region 项目名称X坐标 ProjectNameX
        /// <summary>
        /// 项目X坐标
        /// </summary>
        [Label("项目名称X坐标")]
        public static readonly Property<int> ProjectNameXProperty = P<PrinterSettingTpl>.Register(e => e.ProjectNameX);

        /// <summary>
        /// 项目名称X坐标
        /// </summary>
        public int ProjectNameX
        {
            get { return this.GetProperty(ProjectNameXProperty); }
            set { this.SetProperty(ProjectNameXProperty, value); }
        }
        #endregion

        #region 项目名称Y坐标 ProjectNameY
        /// <summary>
        /// 项目名称Y坐标
        /// </summary>
        [Label("项目Y坐标")]
        public static readonly Property<int> ProjectNameYProperty = P<PrinterSettingTpl>.Register(e => e.ProjectNameY);

        /// <summary>
        /// 项目名称Y坐标
        /// </summary>
        public int ProjectNameY
        {
            get { return this.GetProperty(ProjectNameYProperty); }
            set { this.SetProperty(ProjectNameYProperty, value); }
        }
        #endregion

        #region 项目名称字体大小 ProjectFontSize
        /// <summary>
        /// 项目名称字体大小
        /// </summary>
        [Label("项目字体大小")]
        public static readonly Property<decimal> ProjectFontSizeProperty = P<PrinterSettingTpl>.Register(e => e.ProjectFontSize);

        /// <summary>
        /// 项目名称字体大小
        /// </summary>
        public decimal ProjectFontSize
        {
            get { return this.GetProperty(ProjectFontSizeProperty); }
            set { this.SetProperty(ProjectFontSizeProperty, value); }
        }
        #endregion

        #region 项目名称是否加粗 ProjectFontBold
        /// <summary>
        /// 项目名称是否加粗
        /// </summary>
        [Label("项目是否加粗")]
        public static readonly Property<bool> ProjectFontBoldProperty = P<PrinterSettingTpl>.Register(e => e.ProjectFontBold);

        /// <summary>
        /// 项目名称是否加粗
        /// </summary>
        public bool ProjectFontBold
        {
            get { return this.GetProperty(ProjectFontBoldProperty); }
            set { this.SetProperty(ProjectFontBoldProperty, value); }
        }
        #endregion

        #region 项目名称字体名称 ProjectFontName
        /// <summary>
        /// 项目名称字体名称
        /// </summary>
        [Label("项目字体名称")]
        [MaxLength(500)]
        public static readonly Property<string> ProjectFontNameProperty = P<PrinterSettingTpl>.Register(e => e.ProjectFontName);

        /// <summary>
        /// 项目名称字体名称
        /// </summary>
        public string ProjectFontName
        {
            get { return this.GetProperty(ProjectFontNameProperty); }
            set { this.SetProperty(ProjectFontNameProperty, value); }
        }
        #endregion


        #region SN编码X坐标 CodeStrX
        /// <summary>
        /// SN编码X坐标
        /// </summary>
        [Label("SN编码X坐标")]
        public static readonly Property<int> CodeStrXProperty = P<PrinterSettingTpl>.Register(e => e.CodeStrX);

        /// <summary>
        /// SN编码X坐标
        /// </summary>
        public int CodeStrX
        {
            get { return this.GetProperty(CodeStrXProperty); }
            set { this.SetProperty(CodeStrXProperty, value); }
        }
        #endregion

        #region SN编码Y坐标 CodeStrY
        /// <summary>
        /// SN编码Y坐标
        /// </summary>
        [Label("SN编码Y坐标")]
        public static readonly Property<int> CodeStrYProperty = P<PrinterSettingTpl>.Register(e => e.CodeStrY);

        /// <summary>
        /// SN编码Y坐标
        /// </summary>
        public int CodeStrY
        {
            get { return this.GetProperty(CodeStrYProperty); }
            set { this.SetProperty(CodeStrYProperty, value); }
        }
        #endregion

        #region SN编码字体大小 CodeStrFontSize
        /// <summary>
        /// SN编码字体大小
        /// </summary>
        [Label("SN编码字体大小")]
        public static readonly Property<decimal> CodeStrFontSizeProperty = P<PrinterSettingTpl>.Register(e => e.CodeStrFontSize);

        /// <summary>
        /// SN编码字体大小
        /// </summary>
        public decimal CodeStrFontSize
        {
            get { return this.GetProperty(CodeStrFontSizeProperty); }
            set { this.SetProperty(CodeStrFontSizeProperty, value); }
        }
        #endregion

        #region SN编码字体名称 CodeStrFontName
        /// <summary>
        /// SN编码字体名称
        /// </summary>
        [Label("SN编码字体名称")]
        [MaxLength(500)]
        public static readonly Property<string> CodeStrFontNameProperty = P<PrinterSettingTpl>.Register(e => e.CodeStrFontName);

        /// <summary>
        /// SN编码字体名称
        /// </summary>
        public string CodeStrFontName
        {
            get { return this.GetProperty(CodeStrFontNameProperty); }
            set { this.SetProperty(CodeStrFontNameProperty, value); }
        }
        #endregion

        #region SN编码是否加粗 CodeStrFontBold
        /// <summary>
        /// SN编码是否加粗
        /// </summary>
        [Label("SN编码是否加粗")]
        public static readonly Property<bool> CodeStrFontBoldProperty = P<PrinterSettingTpl>.Register(e => e.CodeStrFontBold);

        /// <summary>
        /// SN编码是否加粗
        /// </summary>
        public bool CodeStrFontBold
        {
            get { return this.GetProperty(CodeStrFontBoldProperty); }
            set { this.SetProperty(CodeStrFontBoldProperty, value); }
        }
        #endregion

        #region SN编码每行长度 CodeStrLineSize
        /// <summary>
        /// SN编码每行长度
        /// </summary>
        [Label("SN编码每行长度")]
        public static readonly Property<int> CodeStrLineSizeProperty = P<PrinterSettingTpl>.Register(e => e.CodeStrLineSize);

        /// <summary>
        /// SN编码每行长度
        /// </summary>
        public int CodeStrLineSize
        {
            get { return this.GetProperty(CodeStrLineSizeProperty); }
            set { this.SetProperty(CodeStrLineSizeProperty, value); }
        }
        #endregion

        #region SN编码行高 CodeStrLineHeight
        /// <summary>
        /// SN编码行高
        /// </summary>
        [Label("SN编码行高")]
        public static readonly Property<int> CodeStrLineHeightProperty = P<PrinterSettingTpl>.Register(e => e.CodeStrLineHeight);

        /// <summary>
        /// SN编码行高
        /// </summary>
        public int CodeStrLineHeight
        {
            get { return this.GetProperty(CodeStrLineHeightProperty); }
            set { this.SetProperty(CodeStrLineHeightProperty, value); }
        }
        #endregion

    }


    /// <summary>
    /// 打印日志 实体配置
    /// </summary>
    internal class PrinterSettingTplEntityConfig : EntityConfig<PrinterSettingTpl>
    {
        /// <summary>
        /// Meta属性配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("PRINT_SETTING_TPL").MapAllProperties();
            Meta.DisableInvOrg();
            //Meta.EnablePhantoms();
        }
    }
}
