using DevExpress.Spreadsheet;
using SIE.Common.Utils;
using SIE.ESop.Documents;
using SIE.Wpf.ESOP.ESOPFactory;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace SIE.Wpf.ESop.Editors
{
    /// <summary>
    /// ExcelPlayer.xaml 的交互逻辑
    /// </summary>
    public partial class ExcelPlayer : UserControl, IPlayer
    {
        /// <summary>
        /// 记录当前文件路径
        /// </summary>
        public Uri CurrentUri { get; private set; }

        /// <summary>
        /// 记录当前文件MD5加密内容
        /// </summary>
        private string MD5 { get; } = "";

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow { get; set; }

        private int? defaultZoom = null;

        /// <summary>
        /// 当前激活页签的用户范围的宽度
        /// </summary>
        private double useRangeWidth;

        /// <summary>
        /// 当前激活页签的用户范围的高度
        /// </summary>
        private double useRangeHeight;

        /// <summary>
        /// 当前激活页签的用户范围
        /// </summary>
        private CellRange useRange;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ExcelPlayer()
        {
            InitializeComponent();
            ExcelPlayer owner = this;
            spreadsheetControl.DataContext = owner;
            spreadsheetControl.SetBinding(FrameworkElement.WidthProperty, "ActualWidth");
            spreadsheetControl.SetBinding(FrameworkElement.HeightProperty, "ActualHeight");
            spreadsheetControl.Visibility = Visibility.Visible;
            spreadsheetControl.Loaded += SpreadsheetControl_Loaded;
            spreadsheetControl.OverridesDefaultStyle = true;
        }

        /// <summary>
        /// 带参构造
        /// </summary>
        /// <param name="showControl"></param>
        public ExcelPlayer(FactoryShowControl showControl)
        {
            InitializeComponent();
            ExcelPlayer owner = this;
            spreadsheetControl.DataContext = owner;
            spreadsheetControl.SetBinding(FrameworkElement.WidthProperty, "ActualWidth");
            spreadsheetControl.SetBinding(FrameworkElement.HeightProperty, "ActualHeight");
            spreadsheetControl.Visibility = Visibility.Visible;
            spreadsheetControl.Loaded += SpreadsheetControl_Loaded;
            spreadsheetControl.OverridesDefaultStyle = true;
            this.DataContext = showControl;
            this.HideUI();
            foreach (var ctr in showControl.Children)
            {
                var child = ctr as ExcelPlayer;
                if (child != null)
                {
                    return;
                }
            }
            showControl.Children.Add(owner);

        }

        /// <summary>
        /// 控件初始化加载数据
        /// </summary>
        /// <param name="sender">当前初始化控件</param>
        /// <param name="e">路由事件参数</param>
        private void SpreadsheetControl_Loaded(object sender, RoutedEventArgs e)
        {
            MatchUseRangeSize();
            ZoomUseRange();
        }

        /// <summary>
        /// 根据路径加载文件
        /// </summary>
        /// <param name="uri">路径</param>
        public void LoadData(Uri uri)
        {
            var md5 = FileHelper.ComputeHash(new FileInfo(uri.OriginalString));
            if (uri.OriginalString == CurrentUri?.OriginalString && md5 == MD5) return;
            spreadsheetControl.LoadDocument(uri.OriginalString);
            foreach (var item in spreadsheetControl.Document.Worksheets)
            {
                item.ActiveView.ShowHeadings = false;
            }

            CurrentUri = uri;
        }

        /// <summary>
        /// 显示指定页签
        /// </summary>
        /// <param name="sheetName">页签名称</param>
        public void Show(string sheetName)
        {
            ActiveWorksheet(sheetName);
            MatchUseRangeSize();
            ZoomUseRange();
        }

        /// <summary>
        /// 激动指定的页签
        /// </summary>
        /// <param name="sheetName">页签名称</param>
        private void ActiveWorksheet(string sheetName)
        {
            useRangeWidth = 0;
            useRangeHeight = 0;
            spreadsheetControl.Document.Worksheets.ActiveWorksheet = spreadsheetControl.Document.Worksheets[sheetName];
            this.spreadsheetControl.Document.Worksheets.ActiveWorksheet.SelectedCell = this.spreadsheetControl.Document.Worksheets.ActiveWorksheet[999, 999];
        }

        /// <summary>
        /// 计算用户范围大小
        /// </summary>
        private void MatchUseRangeSize()
        {
            this.spreadsheetControl.Document.Unit = DevExpress.Office.DocumentUnit.Point;
            useRange = this.spreadsheetControl.Document.Worksheets.ActiveWorksheet.GetDataRange();
            if (this.spreadsheetControl.Document.Worksheets.ActiveWorksheet.Shapes.Any())
            {
                var topColumn = this.spreadsheetControl.Document.Worksheets.ActiveWorksheet.Shapes.Min(f => f.TopLeftCell.ColumnIndex);
                var topRow = this.spreadsheetControl.Document.Worksheets.ActiveWorksheet.Shapes.Min(f => f.TopLeftCell.RowIndex);

                var bottomColumn = this.spreadsheetControl.Document.Worksheets.ActiveWorksheet.Shapes.Max(f => f.BottomRightCell.ColumnIndex);
                var bottomRow = this.spreadsheetControl.Document.Worksheets.ActiveWorksheet.Shapes.Max(f => f.BottomRightCell.RowIndex);

                useRange = this.spreadsheetControl.Document.Worksheets.ActiveWorksheet.Range.FromLTRB(
                    Math.Min(useRange.LeftColumnIndex, topColumn), Math.Min(useRange.TopRowIndex, topRow),
                    Math.Max(useRange.RightColumnIndex, bottomColumn), Math.Max(useRange.BottomRowIndex, bottomRow));
            }

            useRangeWidth = useRangeHeight = 0;
            for (int i = 0; i < useRange.ColumnCount; i++)
                useRangeWidth += useRange[i].ColumnWidth;
            for (int i = 0; i < useRange.RowCount; i++)
                useRangeHeight += useRange[i].RowHeight;
        }

        /// <summary>
        /// 焦点根据用户范围和控件大小进行调整
        /// </summary>
        private void ZoomUseRange()
        {
            try
            {
                this.spreadsheetControl.Document.BeginUpdate();
                if (useRange == null) return;
                ////计算激活页用户内容占控件的长宽比
                ////var useRangeHeightMultiple = this.spreadsheetControl.ActualHeight / Math.Ceiling(useRangeHeight);
                var useRangeWidthMultiple = this.spreadsheetControl.ActualWidth / Math.Ceiling(useRangeWidth);
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    int zoom = (int)(useRangeWidthMultiple * 100 - 100);
                    spreadsheetControl.ActiveWorksheet.ActiveView.Zoom = zoom > 100 ? zoom : 100;
                }));
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message);
            }
            finally
            {
                this.spreadsheetControl.Document.EndUpdate();
            }
        }

        /// <summary>
        /// 计算子元素空间
        /// </summary>
        /// <param name="constraint">父容器可用空间</param>
        /// <returns>父容器空间</returns>
        protected override Size MeasureOverride(Size constraint)
        {
            spreadsheetControl.Measure(constraint);
            return base.MeasureOverride(constraint);
        }

        /// <summary>
        /// 放大
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BtnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            if (!defaultZoom.HasValue)
            {
                defaultZoom = spreadsheetControl.ActiveWorksheet.ActiveView.Zoom;
            }
            spreadsheetControl.ActiveWorksheet.ActiveView.Zoom += 5;
        }

        /// <summary>
        /// 缩小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BtnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (!defaultZoom.HasValue)
            {
                defaultZoom = spreadsheetControl.ActiveWorksheet.ActiveView.Zoom;
            }
            spreadsheetControl.ActiveWorksheet.ActiveView.Zoom -= 5;
        }

        /// <summary>
        /// 放大
        /// </summary>
        public void MagnifyAdd()
        {
            BtnZoomIn_Click(null, null);
        }

        /// <summary>
        /// 缩小
        /// </summary>
        public void MagnifyMinus()
        {
            BtnZoomOut_Click(null, null);
        }

        /// <summary>
        /// 还原
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        internal void SetActualSize()
        {
            if (defaultZoom.HasValue)
            {
                spreadsheetControl.ActiveWorksheet.ActiveView.Zoom = defaultZoom.Value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        public void UpdatePlayer(Document document)
        {
            this.ShowUI();
            this.LoadData(new Uri(document.FilePath));
            this.Show(document.FileName);
        }

        public void Play()
        {
            // 播放图片
            // ...
        }

        public void Stop()
        {
            // 停止播放图片
            // ...
        }

        /// <summary>
        /// 
        /// </summary>
        public void ActualSize()
        {
            this.SetActualSize();
        }
        /// <summary>
        ///  添加显示界面的方法
        /// </summary>
        public void ShowUI()
        {
            this.Visibility = Visibility.Visible;
            IsShow = true;

        }

        /// <summary>
        /// 添加隐藏界面的方法
        /// </summary>
        public void HideUI()
        {
            this.Visibility = Visibility.Collapsed;
            IsShow = false;
        }
    }
}