using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.SpareParts.ViewModels
{
    /// <summary>
    /// 备件导入ViewModel
    /// </summary>
    [RootEntity]
    public class ImportSparePartCheckViewModel : ViewModel
    {
        #region ImportFilePath 导入模板文件路径

        /// <summary>
        /// 导入模板文件路径
        /// </summary>
        [Label("导入文件")]
        public static readonly Property<string> ImportFilePathProperty = P<ImportSparePartCheckViewModel>.Register(e => e.ImportFilePath);

        /// <summary>
        /// 导入模板文件路径
        /// </summary>
        public string ImportFilePath
        {
            get { return this.GetProperty(ImportFilePathProperty); }
            set { this.SetProperty(ImportFilePathProperty, value); }
        }

        #endregion

        #region ImportSuccessAmount 导入成功数量

        /// <summary>
        /// 导入成功数量
        /// </summary>
        [Label("导入成功数量")]
        public static readonly Property<int> ImportSuccessAmountProperty = P<ImportSparePartCheckViewModel>.Register(e => e.ImportSuccessAmount);

        /// <summary>
        /// 导入成功数量
        /// </summary>
        public int ImportSuccessAmount
        {
            get { return this.GetProperty(ImportSuccessAmountProperty); }
            set { this.SetProperty(ImportSuccessAmountProperty, value); }
        }

        #endregion

        #region ImportFailAmount 导入失败数量

        /// <summary>
        /// 导入失败数量
        /// </summary>
        [Label("导入失败数量")]
        public static readonly Property<int> ImportFailAmountProperty = P<ImportSparePartCheckViewModel>.Register(e => e.ImportFailAmount);

        /// <summary>
        /// 导入失败数量
        /// </summary>
        public int ImportFailAmount
        {
            get { return this.GetProperty(ImportFailAmountProperty); }
            set { this.SetProperty(ImportFailAmountProperty, value); }
        }

        #endregion

        #region ImportProcessMsg 导入处理消息

        /// <summary>
        /// 导入处理消息
        /// </summary>
        [Label("导入处理消息")]
        public static readonly Property<string> ImportProcessMsgProperty = P<ImportSparePartCheckViewModel>.Register(e => e.ImportProcessMsg);

        /// <summary>
        /// 导入处理消息
        /// </summary>
        public string ImportProcessMsg
        {
            get { return this.GetProperty(ImportProcessMsgProperty); }
            set { this.SetProperty(ImportProcessMsgProperty, value); }
        }

        #endregion


    }

    /// <summary>
    /// 备件导入 视图配置
    /// </summary>
    class ImportSparePartCheckViewModelConfig : WebViewConfig<ImportSparePartCheckViewModel>
    {
        /// <summary>
        /// 配置默认试图
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("数据导入");
        }

        /// <summary>
        /// 配置明细试图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.HasDetailColumnsCount(6);
            View.ClearCommands();
            View.Property(p => p.ImportFilePath).ShowInDetail(columnSpan: 4);
            View.Property(p => p.ImportSuccessAmount).ShowInDetail(columnSpan: 1).Readonly();
            View.Property(p => p.ImportFailAmount).ShowInDetail(columnSpan: 1).Readonly();
            View.Property(p => p.ImportProcessMsg).ShowInDetail(columnSpan: 6).Readonly();
        }
    }
}
