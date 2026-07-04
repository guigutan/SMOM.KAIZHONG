using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Topshelf.Logging;
using Excel = Microsoft.Office.Interop.Excel;
using Range = Microsoft.Office.Interop.Excel.Range;
using Shape = Microsoft.Office.Interop.Excel.Shape;
using Worksheet = Microsoft.Office.Interop.Excel.Worksheet;
using XlCopyPictureFormat = Microsoft.Office.Interop.Excel.XlCopyPictureFormat;
using XlPictureAppearance = Microsoft.Office.Interop.Excel.XlPictureAppearance;

namespace SIE.Wpf.ESop.DocumentTransform.Converters
{
    /// <summary>
    /// excel页签转换为Image
    /// </summary>
    public class ExcelSheetConvertToImg : IDisposable
    {
        /// <summary>
        /// 引用非托管dll的方法
        /// </summary>
        /// <param name="hwnd">句柄指针</param>
        /// <param name="lpdwProcessId">返回进程ID</param>
        /// <returns>返回线程号</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int GetWindowThreadProcessId(IntPtr hwnd, out int lpdwProcessId);

        #region 属性和字段
        /// <summary>
        /// 日记对象
        /// </summary>
        private static readonly LogWriter Logwriter = HostLogger.Get("DocumentTransformService");

        /// <summary>
        /// Excel应用程序对象
        /// </summary>
        private Excel.Application app;

        /// <summary>
        /// Excel工作簿集合对象
        /// </summary>
        public Excel.Workbooks Workbooks { get; private set; }

        /// <summary>
        /// Excel工作簿
        /// </summary>
        public Excel.Workbook Book { get; private set; }

        /// <summary>
        /// Excel页签集合
        /// </summary>
        public Excel.Sheets Sheets { get; private set; }

        /// <summary>
        /// 是否打开了文件
        /// </summary>
        private bool IsOpen { get; set; }
        #endregion

        /// <summary>
        /// 初始化Excel应用程序对象
        /// </summary>
        public ExcelSheetConvertToImg()
        {
            app = new Excel.Application();
            app.Visible = false;
            app.ScreenUpdating = true;
            app.DisplayAlerts = false;
            app.Interactive = false;
            app.CopyObjectsWithCells = true;
            app.CutCopyMode = Microsoft.Office.Interop.Excel.XlCutCopyMode.xlCopy;
            app.DisplayClipboardWindow = false;
        }

        /// <summary>
        /// 获取页签中边框的左上角、右下角的单元格对象
        /// </summary>
        /// <param name="sheet">页签对象</param>
        /// <param name="startCell">左上角对象</param>
        /// <param name="endCell">右下角对象</param>
        private void GetDataRange(Worksheet sheet, ref Range startCell, ref Range endCell)
        {
            int startRowNum = sheet.UsedRange.Row;
            int endRowNum = (sheet.UsedRange.Row + sheet.UsedRange.Rows.Count - 1);
            int startColNum = sheet.UsedRange.Column;
            int endColNum = (sheet.UsedRange.Column + sheet.UsedRange.Columns.Count - 1);
            foreach (Shape sh in sheet.Shapes)
            {
                startRowNum = Math.Min(sh.TopLeftCell.Row, startRowNum);
                endRowNum = Math.Max(sh.BottomRightCell.Row, endRowNum);

                startColNum = Math.Min(sh.TopLeftCell.Column, startColNum);
                endColNum = Math.Max(sh.BottomRightCell.Column, endColNum);
            }

            startCell = (Range)sheet.Cells[startRowNum, startColNum];
            endCell = (Range)sheet.Cells[endRowNum, endColNum];
        }

        #region 根据路径解析excel
        /// <summary>
        /// 转换指定excel文件所有页签为图片（根据边框来确定范围）
        /// </summary>
        /// <param name="strFilePath">excel文件路径</param>
        /// <param name="dicImageByte">输出excel页签名称-图片</param>
        public void ConvertSheetsToMs(string strFilePath, out Dictionary<string, byte[]> dicImageByte)
        {
            dicImageByte = null;
            try
            {
                Workbooks = app.Workbooks;
                Book = Workbooks.Open(strFilePath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                      Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                      Missing.Value, Missing.Value);
                IsOpen = true;

                Sheets = Book.Worksheets;
                dicImageByte = new Dictionary<string, byte[]>();
                foreach (Microsoft.Office.Interop.Excel.Worksheet sheet in Book.Worksheets)
                {
                    ProcessSheet(dicImageByte, sheet);
                }
            }
            catch (Exception ex)
            {
                Logwriter.Error(ex);
            }
            finally
            {
                Cleanup(null, Workbooks, Book, Sheets);
            }
        }

        /// <summary>
        /// 转换指定页签为图片
        /// </summary>
        /// <param name="dicImageByte">excel页签名称-图片</param>
        /// <param name="sheet">页签对象</param>
        private void ProcessSheet(Dictionary<string, byte[]> dicImageByte, Worksheet sheet)
        {
            Range startCell = null;
            Range endCell = null;
            this.GetDataRange(sheet, ref startCell, ref endCell);
            Range chartRange = sheet.get_Range(startCell, endCell);

            Clipboard.Clear();
            chartRange.CopyPicture(XlPictureAppearance.xlScreen, XlCopyPictureFormat.xlBitmap);
            var image = Clipboard.GetImage();

            if (!Clipboard.ContainsImage()) throw new IOException("剪贴板丢失图片数据".L10N());
            byte[] byteImage = ImageToByte(image);
            if (byteImage == null || byteImage.Length == 0) throw new IOException("无法获取图片字节".L10N());
            dicImageByte[sheet.Name] = byteImage;
        }

        /// <summary>
        /// 根据图片对象转为byte[]
        /// </summary>
        /// <param name="image">图片对象</param>
        /// <returns>返回图片byte[]数组</returns>
        public byte[] ImageToByte(Image image)
        {
            byte[] picture;
            using (MemoryStream ms = new MemoryStream())
            {
                if (image == null)
                    return new byte[ms.Length];
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                picture = ms.GetBuffer();
            }
            return picture;
        }
        #endregion

        /// <summary>
        /// 转换excel所有页签为图片（根据单元格有效范围）
        /// </summary>
        /// <param name="fileName">excel文件路径</param>
        /// <param name="saveBmpBasePath">保存图片路径</param>
        /// <returns>返回图片保存的路径</returns>
        public IEnumerable<string> Converter(string fileName, string saveBmpBasePath)
        {
            List<string> paths = new List<string>();
            try
            {
                Workbooks = app.Workbooks;
                Book = Workbooks.Open(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                      Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                      Type.Missing, Type.Missing);
                Sheets = Book.Worksheets;
                Directory.CreateDirectory(saveBmpBasePath);
                for (int i = 0; i < Sheets.Count; i++)
                {
                    Clipboard.Clear();
                    var sheet = ((Excel.Worksheet)Sheets.get_Item(i + 1));

                    sheet.UsedRange.CopyPicture(Microsoft.Office.Interop.Excel.XlPictureAppearance.xlScreen, Excel.XlCopyPictureFormat.xlBitmap);
                    var bmpName = string.Format("{0}.bmp", sheet.Name);
                    paths.Add(bmpName);
                    var filePath = saveBmpBasePath + "/" + bmpName;
                    using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
                    {
                        if (Clipboard.ContainsData(System.Windows.DataFormats.EnhancedMetafile))
                        {
                            Metafile metafile = Clipboard.GetData(System.Windows.DataFormats.EnhancedMetafile) as Metafile;
                            metafile.Save(filePath);
                        }
                        else if (Clipboard.ContainsData(System.Windows.DataFormats.Bitmap))
                        {
                            BitmapSource bitmapSource = Clipboard.GetData(System.Windows.DataFormats.Bitmap) as BitmapSource;

                            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                            encoder.QualityLevel = 100;
                            encoder.Save(fileStream);
                        }
                        else
                        {
                            //
                        }
                    }
                }

                return paths;
            }
            finally
            {
                Cleanup(app, Workbooks, Book, Sheets);
                KillSpecialExcel(app);
            }
        }

        /// <summary>
        /// 清空Excel对象
        /// </summary>
        /// <param name="app">Excel应用程序对象</param>
        /// <param name="workbooks">工作薄集合</param>
        /// <param name="book">工作薄对象</param>
        /// <param name="sheets">页签集合</param>
        private void Cleanup(Excel.Application app, Excel.Workbooks workbooks, Excel.Workbook book, Excel.Sheets sheets)
        {
            if (IsOpen)
            {
                Clipboard.Clear();
                book?.Close(false, Type.Missing, Type.Missing);
                workbooks?.Close();
                if (app != null)
                {
                    app.Quit();
                }

                if (sheets != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(sheets);
                }

                if (book != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
                }

                if (workbooks != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbooks);
                }

                if (app != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
                IsOpen = false;
            }
        }

        #region Kill Special Excel Process

        /// <summary>
        /// 杀掉指定excel对象的进程
        /// </summary>
        /// <param name="excel">excel应用对象</param>
        public void KillSpecialExcel(Microsoft.Office.Interop.Excel.Application excel)
        {
            if (excel != null)
            {
                int lpdwProcessId;
                GetWindowThreadProcessId(new IntPtr(excel.Hwnd), out lpdwProcessId);
                System.Diagnostics.Process.GetProcessById(lpdwProcessId).Kill();
            }
        }
        #endregion

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            try
            {
                Cleanup(app, Workbooks, Book, Sheets);
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message);
            }

            if (app != null)
            {
                Logwriter.Info("Kill Excel Completed");
                KillSpecialExcel(app);
                app = null;
            }
        }
    }
}