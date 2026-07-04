using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Common.Exceptions;
using SIE.XPCJ.Common.ApiCall;
using SIE.XPCJ.Common.Settings;

namespace SIE.XPCJ.Common.Print
{
    /// <summary>
    /// 打印组件（打印模板表名：PRT_TEMPLATE 打印任务表名：PRT_BC）
    /// </summary>
    public class XpPrinter
    {
        private static XpPrinter _instance;
        public static XpPrinter Instance
        {
            get {
                if (_instance == null)
                    _instance = new XpPrinter();

                return _instance;
            }
        }

        /// <summary>
        /// 报表数据源
        /// </summary>
        DataSet dataSet = new DataSet("Data");

        /// <summary>
        /// 打印模板和数据文件所在目录
        /// </summary>
        private string TemplatesDir { get; set; }
        /// <summary>
        /// BarTender打印数据文件路径
        /// </summary>
        private string BarTenderDataFilePath { get; set; }
        /// <summary>
        /// SieDev打印数据文件路径
        /// </summary>
        private string SieDevDataFilePath { get; set; }


        private XpPrinter()
        {
            this.TemplatesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");
            if (!Directory.Exists(this.TemplatesDir))
                Directory.CreateDirectory(this.TemplatesDir);

            this.BarTenderDataFilePath = Path.Combine(this.TemplatesDir, "data.csv");
            this.SieDevDataFilePath = Path.Combine(this.TemplatesDir, "data.xml");
        }

        /// <summary>
        /// 打印（从服务器下载模板并打印）
        /// </summary>
        /// <param name="templateId">模板ID</param>
        /// <param name="invOrgId">库存组织ID</param>
        /// <param name="data">要打印的数据对象</param>
        /// <param name="isShowPreView"></param>
        /// <param name="copies"></param>
        public void Print(double templateId, double invOrgId, List<object> datas, string printerName = "", bool isShowPreView = false, int copies = 1)
        {
            string templateFilePath = DownloadTempalteWithFileName(templateId, invOrgId);
            Print(templateFilePath, datas, printerName, isShowPreView, copies);
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="templateFilePath">打印模板文件完整路径</param>
        /// <param name="data">要打印的数据对象</param>
        /// <param name="isShowPreView">是否显示打印预览(SieDev打印才起作用)</param>
        /// <param name="copies">打印份数(BarTender打印才起作用)</param>
        public void Print(string templateFilePath, List<object> datas, string printerName = "", bool isShowPreView = false, int copies = 1)
        {
            if (datas == null)
                throw new ArgumentNullException(nameof(datas));

            if(string.IsNullOrEmpty(templateFilePath))
                throw new ArgumentNullException(nameof(templateFilePath));

            if (templateFilePath.ToLower().EndsWith(".btw"))
            {
                ConvertDataToCSV(datas); //转换为 data.csv 文件
            }
            else if (templateFilePath.ToLower().EndsWith(".siedev"))
            {
                ConvertDateToXml(datas); //转换为 data.xml 文件
            }
            Print(templateFilePath, printerName, isShowPreView, copies);
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="printInfo">打印任务</param>
        /// <param name="invOrgId">库存组织ID</param>
        /// <param name="isShowPreView">是否显示打印预览(SieDev打印才起作用)</param>
        /// <param name="copies">打印份数(BarTender打印才起作用)</param>
        public void Print(PrintInfo printInfo, double invOrgId, string printerName = "", bool isShowPreView = false, int copies = 1)
        {
            if (printInfo == null)
                throw new ValidationException("打印任务不能为空".L10N());

            if (string.IsNullOrEmpty(printInfo.FileContent))
                throw new ValidationException("打印内容不能为空".L10N());

            if (string.IsNullOrEmpty(printInfo.FileName))
                throw new ValidationException("打印模板不能为空".L10N());

            if (printInfo.FileName.ToLower().EndsWith(".btw"))
            {
                //保存打印数据文件
                var dataContentBytes = Convert.FromBase64String(printInfo.FileContent);
                var dataContent = Encoding.UTF8.GetString(dataContentBytes);
                var csvFiles = Directory.GetFiles(this.TemplatesDir, "*.csv");
                foreach (var csvFile in csvFiles)
                {
                    File.Delete(csvFile);
                }
                File.WriteAllText(this.BarTenderDataFilePath, dataContent + "\r\n", Encoding.UTF8);

                //获取打印模板文件
                var fileName = printInfo.FileName;
                var version = printInfo.Version;
                var name = fileName.Substring(0, fileName.LastIndexOf('.'));
                var extension = fileName.Substring(fileName.LastIndexOf('.'), fileName.Length - name.Length);
                var templateFilePath = Path.Combine(this.TemplatesDir, $"{name}-V{version}{extension}");

                //文件不存在，从服务器下载打印模板
                if (!File.Exists(templateFilePath))
                {
                    var result = ApiHelper.Post<string>("PrintsController", "GetPrintFileContent", printInfo.TemplateId, invOrgId);
                    if (!result.Success)
                        throw new ValidationException("下载模板文件失败".L10N());

                    byte[] fileContent = Convert.FromBase64String(result.Result);
                    if (fileContent == null || fileContent.Length <= 0)
                        throw new ValidationException("保存模板文件失败，模板文件内容不能为空".L10N());

                    using (var stream = new FileStream(templateFilePath, FileMode.OpenOrCreate))
                    {
                        stream.Write(fileContent, 0, fileContent.Length);
                    }
                }

                //打印
                Print(templateFilePath, printerName, isShowPreView, copies);
            }
            else if (printInfo.FileName.ToLower().EndsWith(".siedev"))
            {
                //保存打印数据文件
                var xmlFiles = Directory.GetFiles(this.TemplatesDir, "*.xml");
                foreach (var csvFile in xmlFiles)
                {
                    File.Delete(csvFile);
                }
                File.WriteAllText(this.SieDevDataFilePath, printInfo.FileContent + "\r\n", Encoding.UTF8);

                //获取打印模板文件
                var fileName = printInfo.FileName;
                var name = fileName.Substring(0, fileName.LastIndexOf('.'));
                var extension = fileName.Substring(fileName.LastIndexOf('.'), fileName.Length - name.Length);
                var templateFilePath = Path.Combine(this.TemplatesDir, $"{name}{extension}");

                //文件不存在，从服务器下载打印模板
                if (!File.Exists(templateFilePath))
                {
                    var result = ApiHelper.Post<string>("PrintsController", "GetPrintFileContent", printInfo.TemplateId, invOrgId);
                    if (!result.Success)
                        throw new ValidationException("下载模板文件失败".L10N());

                    byte[] fileContent = Convert.FromBase64String(result.Result);
                    if (fileContent == null || fileContent.Length <= 0)
                        throw new ValidationException("保存模板文件失败，模板文件内容不能为空".L10N());

                    using (var stream = new FileStream(templateFilePath, FileMode.OpenOrCreate))
                    {
                        stream.Write(fileContent, 0, fileContent.Length);
                    }
                }

                //打印
                Print(templateFilePath, printerName, isShowPreView, copies);
            }
            else
                throw new ValidationException("不支持的模板类型{0}".L10nFormat(printInfo.FileName));
        }

        /// <summary>
        /// 打印（会自动在模板文件目录查找data.csv或data.xml文件并合并打印）
        /// </summary>
        /// <param name="templateFilePath">打印模板文件完整路径</param>
        /// <param name="isShowPreView">是否显示打印预览(SieDev打印才起作用)</param>
        /// <param name="copies">打印份数(BarTender打印才起作用)</param>
        public void Print(string templateFilePath, string printerName = "", bool isShowPreView = false, int copies = 1)
        {
            CheckTemplateFilePath(templateFilePath); //验证模板文件

            if (templateFilePath.ToLower().EndsWith(".btw"))
            {
                //验证BarTender执行文件路径
                CheckBarTenderExePath();

                //开始打印
                Process process = new Process
                {
                    EnableRaisingEvents = true
                };
                string arg = string.IsNullOrEmpty(printerName) ? string.Format("/AF=\"{0}\" /P /X /C={1}", templateFilePath, copies) 
                    : string.Format("/AF=\"{0}\" /P /X /C={1} /D=\"{2}\"", templateFilePath, copies, printerName);
                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    FileName = AppSettings.Instance.BarTenderExePath,
                    Arguments = arg,
                };
                process.StartInfo = startInfo;
                process.Start();
            }
            else if (templateFilePath.ToLower().EndsWith(".siedev"))
            {
                try
                {
                    XtraReport report = new XtraReport(); // 创建 XtraReport 实例并从 XML 文件加载报表布局
                    report.LoadLayoutFromXml(templateFilePath);
                    report.PaperKind = System.Drawing.Printing.PaperKind.A4;
                    report.XmlDataPath = this.SieDevDataFilePath;
                   
                    ReportPrintTool tool = new ReportPrintTool(report); // 使用 ReportPrintTool 执行报表打印操作

                    if (isShowPreView)
                        tool.ShowPreviewDialog();
                    else
                    {
                        if (string.IsNullOrEmpty(printerName))
                            tool.Print();
                        else
                            tool.Print(printerName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        #region 验证模板文件是否有效
        /// <summary>
        /// 验证模板文件是否有效
        /// </summary>
        /// <param name="templateFilePath"></param>
        private void CheckTemplateFilePath(string templateFilePath)
        {
            if (string.IsNullOrEmpty(templateFilePath))
                throw new ValidationException("打印模板文件参数不能为空".L10N());

            if (!File.Exists(templateFilePath))
                throw new ValidationException("打印模板文件{0}不存在".L10nFormat(templateFilePath));

            if (!templateFilePath.ToLower().EndsWith(".btw") && !templateFilePath.ToLower().EndsWith(".siedev"))
                throw new ValidationException("不支持的模板类型{0}".L10nFormat(templateFilePath));
        }
        #endregion

        #region 打印数据转data.csv
        /// <summary>
        /// 打印数据转data.csv
        /// </summary>
        /// <param name="datas"></param>
        private void ConvertDataToCSV(IEnumerable<object> datas)
        {
            var csvFiles = Directory.GetFiles(this.TemplatesDir, "*.csv");
            foreach (var csvFile in csvFiles)
            {
                File.Delete(csvFile);
            }

            if (datas == null || datas.Count() <= 0)
                return;

            const char SEP = '|';

            Type type = datas.FirstOrDefault().GetType();
            var propertys = GetPropertys(type);

            StringBuilder content = new StringBuilder();

            StringBuilder header = new StringBuilder();
            foreach (string propertyName in propertys)
            {
                header.Append(propertyName);
                header.Append(SEP);
            }
            content.Append($"{header.ToString().TrimEnd(SEP)}\n");

            foreach (object data in datas)
            {
                StringBuilder row = new StringBuilder();
                foreach (string propertyName in propertys)
                {
                    var propertyInfo = type.GetProperty(propertyName);
                    if (propertyInfo != null)
                    {
                        string val = propertyInfo.GetValue(data, null)?.ToString();
                        if (propertyInfo.PropertyType == typeof(decimal) || propertyInfo.PropertyType == typeof(double) || propertyInfo.PropertyType == typeof(float))
                        {
                            if (val.IndexOf(".") >= 0)
                                val = val.TrimEnd('0').TrimEnd('.');
                        }
                        row.Append(val);
                        row.Append(SEP);
                    }
                }
                content.Append($"{row.ToString().TrimEnd(SEP)}\n");
            }

            string contentStr = content.ToString() + "\r\n";

            File.WriteAllText(this.BarTenderDataFilePath, contentStr, Encoding.UTF8);
        }
        #endregion

        #region 打印数据转 data.xml
        /// <summary>
        /// 打印数据转 data.xml
        /// </summary>
        /// <param name="datas"></param>
        private void ConvertDateToXml(IEnumerable<object> datas)
        {
            var xmlFiles = Directory.GetFiles(this.TemplatesDir, "*.xml");
            foreach (var xmlFile in xmlFiles)
            {
                File.Delete(xmlFile);
            }

            foreach (DataTable table in dataSet.Tables)
                table.Constraints.Clear();
            dataSet.Relations.Clear();
            dataSet.Tables.Clear();
            dataSet.Clear();

            if (datas == null || datas.Count() <= 0)
                return;

            Type type = datas.FirstOrDefault().GetType();
            var propertys = GetPropertys(type);

            var dt = new DataTable(type.Name);
            foreach (string propertyName in propertys)
            {
                dt.Columns.Add(propertyName, typeof(string));
            }

            foreach (object data in datas)
            {
                List<string> rowValue = new List<string>();
                foreach (string propertyName in propertys)
                {
                    if (propertyName == "Qty")
                    { 
                    }
                    var propertyInfo = type.GetProperty(propertyName);
                    if (propertyInfo != null)
                    {
                        string val = propertyInfo.GetValue(data, null)?.ToString();
                        if (propertyInfo.PropertyType == typeof(decimal) || propertyInfo.PropertyType == typeof(double) || propertyInfo.PropertyType == typeof(float))
                        {
                            if (val.IndexOf(".") >= 0)
                                val = val.TrimEnd('0').TrimEnd('.');
                        }
                        rowValue.Add(val);
                    }
                }
                dt.Rows.Add(rowValue.ToArray());
            }
            dataSet.Tables.Add(dt);

            DataSetConvertHelper.ConvertDataSetToXMLFile(dataSet, this.SieDevDataFilePath);
        }
        #endregion

        #region 获取对象的属性名称列表
        /// <summary>
        /// 获取对象的属性名称列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private IEnumerable<string> GetPropertys(Type type)
        {
            var propertyNames = new List<string>();
            foreach (var property in type.GetProperties())
            {
                if (property.PropertyType.IsInterface)
                    continue;
                if (property.PropertyType.IsClass && property.PropertyType.Name != "String")
                    continue;
                propertyNames.Add(property.Name);
            }
            return propertyNames.Distinct();
        }
        #endregion

        #region 下载打印模板并保存到本地
        /// <summary>
        /// 下载打印模板并保存到本地
        /// </summary>
        /// <param name="templateId">模板ID</param>
        /// <param name="invOrgId">库存组织ID</param>
        /// <returns>下载保存后的本地模板文件路径</returns>
        public string DownloadTempalteWithFileName(double templateId, double invOrgId)
        {
            var result = ApiHelper.Post<Tuple<string, string>>("PrintsController", "GetPrintTemplate", templateId, invOrgId);
            if (!result.Success)
            {
                if(!string.IsNullOrEmpty(result.Message))
                    throw new ValidationException(result.Message);

                throw new ValidationException("打印模板下载失败{模板ID:}".L10nFormat(templateId));
            }

            string filePath = result.Result.Item1;
            if(string.IsNullOrEmpty(filePath))
                throw new ValidationException("保存打印模板文件失败，打印模板文件名称不能为空".L10N());


            byte[] fileContent = Convert.FromBase64String(result.Result.Item2);
            if (fileContent == null || fileContent.Length <= 0)
                throw new ValidationException("保存打印模板文件失败，打印模板文件内容不能为空".L10N());

            string localFilePath = Path.Combine(this.TemplatesDir, filePath);
            if (File.Exists(localFilePath))
                File.Delete(localFilePath);

            using (var stream = new FileStream(localFilePath, FileMode.OpenOrCreate))
            {
                stream.Write(fileContent, 0, fileContent.Length);
            }

            if (filePath.EndsWith(".siedev")) //siedev格式的需要移除
            {
                List<string> listXmlLine = new List<string>();
                string[] lines = File.ReadAllLines(localFilePath);
                bool isNeedSkip = false;
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Trim().Equals("<ComponentStorage>"))
                    {
                        isNeedSkip = true;
                        continue;
                    }
                    if (lines[i].Trim().Equals("</ComponentStorage>"))
                    {
                        isNeedSkip = false;
                        continue;
                    }

                    if (isNeedSkip)
                        continue;

                    listXmlLine.Add(lines[i]);
                }

                File.WriteAllLines(localFilePath, listXmlLine.ToArray());
            }

            return localFilePath;
        }
        #endregion

        #region 获取打印模板内容，不带文件名 -- 已废弃 PrintsController.GetPrintFileContent 
        /// <summary>
        /// 获取打印模板内容
        /// </summary>
        /// <param name="apiUrl">api地址</param>
        /// <param name="invOrgId">库存组织</param>
        /// <param name="printInfo">打印信息</param>
        /// <returns>打印模板内容</returns>
        //public string DownloadTempalte(double templateId, double invOrgId)
        //{
        //    var result = ApiHelper.Post<string>("PrintsController", "GetPrintFileContent", templateId, invOrgId);
        //    if (!result.Success)
        //        return string.Empty;


        //    byte[] fileContent = Convert.FromBase64String(result.Result);
        //    if (fileContent == null || fileContent.Length <= 0)
        //        throw new ValidationException("保存打印模板文件失败，打印模板文件内容不能为空".L10N());

        //    string localFilePath = Path.Combine(this.TemplatesDir, templateId.ToString() + ".siedev");
        //    if (File.Exists(localFilePath))
        //        File.Delete(localFilePath);

        //    using (var stream = new FileStream(localFilePath, FileMode.OpenOrCreate))
        //    {
        //        stream.Write(fileContent, 0, fileContent.Length);
        //    }

        //    return localFilePath;
        //}
        #endregion

        #region 验证BarTender执行文件路径
        /// <summary>
        /// 验证BarTender执行文件路径
        /// </summary>
        private void CheckBarTenderExePath()
        {
            if (string.IsNullOrEmpty(AppSettings.Instance.BarTenderExePath) || !File.Exists(AppSettings.Instance.BarTenderExePath))
            {
                var dialog = new OpenFileDialog
                {
                    Title = "选择BarTender打印程序".L10N(),
                    Filter = "BarTender程序文件|*.exe".L10N()
                };
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (!File.Exists(dialog.FileName))
                        throw new ValidationException("该路径找不到相应的Bartender打印程序，请重新配置路径".L10N());

                    AppSettings.Instance.BarTenderExePath = dialog.FileName;
                    AppSettings.Instance.SaveToFile();
                }
                else
                    throw new ValidationException("未选择Bartender打印程序路径，请配置路径".L10N());
            }
        }
        #endregion

        #region 打开BarTender设计器
        /// <summary>
        /// 打开BarTender设计器
        /// </summary>
        /// <param name="btwFilePath"></param>
        public void ShowBarTenderDesigner(string btwFilePath)
        {
            CheckTemplateFilePath(btwFilePath);
            CheckBarTenderExePath();

            Process process = new Process();
            process.EnableRaisingEvents = true;
            string arg = string.Format("/F=\"{0}\"", btwFilePath);
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "\"" + AppSettings.Instance.BarTenderExePath + "\"",
                Arguments = arg,
            };
            process.StartInfo = startInfo;
            try
            {
                process.Start();
            }
            catch (Exception exc)
            {
                throw new ValidationException(exc.Message);
            }
        }
        #endregion

        #region 从服务器获取BarTender打印任务（PRT_BC表）
        /// <summary>
        /// 异步从服务器上调用PrintsController.GetPrintInfo获取BarTender打印任务（PRT_BC表）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="onApiCallback">回调方法</param>
        /// <param name="printInfoId">任务ID，PrintInfo.Id</param>
        /// <param name="invOrgId">库存组织ID</param>
        public void GetBarTenderPrintInfoAsync(Control sender, ApiHelper.delegateApiCallback<PrintInfo> onApiCallback, double printInfoId, double invOrgId)
        {
            ApiHelper.PostAsync<PrintInfo>(sender, "PrintsController", "GetPrintInfo", onApiCallback, printInfoId, invOrgId);
        }


        /// <summary>
        /// 从服务器上调用PrintsController.GetPrintInfo获取BarTender打印任务（PRT_BC表）
        /// </summary>
        /// <param name="printInfoId">任务ID，PrintInfo.Id</param>
        /// <param name="invOrgId">库存组织ID</param>
        /// <returns>PrintInfo</returns>
        public PrintInfo GetBarTenderPrintInfo(double printInfoId, double invOrgId)
        {
            //string methodName = type == Type.BarTender ? "GetPrintInfo" : "GetSieDevPrintInfo";
            var result = ApiHelper.Post<PrintInfo>("PrintsController", "GetPrintInfo", printInfoId, invOrgId);
            if (result.Success)
            {
                return result.Result;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        #endregion

        #region 从服务器获取SieDev打印任务（PRT_BC表）
        /// <summary>
        /// 异步从服务器上调用PrintsController.GetPrintInfo获取SieDev打印任务（PRT_BC表）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="onApiCallback">回调方法</param>
        /// <param name="printInfoId">任务ID，PrintInfo.Id</param>
        /// <param name="invOrgId">库存组织ID</param>
        public void GetSieDevPrintInfoAsync(Control sender, ApiHelper.delegateApiCallback<PrintInfo> onApiCallback, double printInfoId, double invOrgId)
        {
            ApiHelper.PostAsync<PrintInfo>(sender, "PrintsController", "GetSieDevPrintInfo", onApiCallback, printInfoId, invOrgId);
        }

        /// <summary>
        /// 从服务器上调用PrintsController.GetPrintInfo获取SieDev打印任务（PRT_BC表）
        /// </summary>
        /// <param name="printInfoId">任务ID，PrintInfo.Id</param>
        /// <param name="invOrgId">库存组织ID</param>
        /// <returns>PrintInfo</returns>
        public PrintInfo GetSieDevPrintInfo(double printInfoId, double invOrgId)
        {
            var result = ApiHelper.Post<PrintInfo>("PrintsController", "GetSieDevPrintInfo", printInfoId, invOrgId);
            if (result.Success)
            {
                return result.Result;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        #endregion

    }

    /// <summary>
    /// 打印任务
    /// </summary>
    [Serializable]
    public class PrintInfo
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// 模板Id
        /// </summary>
        public double TemplateId { get; set; }

        /// <summary>
        /// 打印内容
        /// </summary>
        public string FileContent { get; set; }
    }
}
