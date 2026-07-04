using DevExpress.XtraReports.UI;
using SIE.Common.Prints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Common.Prints
{
    /// <summary>
    /// 报表接口
    /// </summary>
    public interface IHostReport
    {
        /// <summary>
        /// 报表文件扩展名
        /// </summary>
        string Extension { get; }
        /// <summary>
        /// 报表名
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 弹出设计窗口
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="completeCallBack">设计完成回调</param>
        //void ShowDesigner(string filePath, Action completeCallBack);
        /// <summary>
        /// 获取文件全名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        string GetFileFullName(string fileName);
        /// <summary>
        /// 生成新的模板
        /// </summary>
        /// <param name="printable">打印提供者</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        string GenerateNewTemplate(IPrintable printable, string fileName);
        /// <summary>
        /// 生成数据文件
        /// </summary>
        /// <param name="printable">打印提供者</param>
        /// <param name="datas">打印的数据集合</param>
        /// <returns>生成的数据文件路径</returns>
        string GenerateDataFile(IPrintable printable, IEnumerable<object> datas);
        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="printable">打印提供者</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="printerName">打印名称</param>
        /// <param name="datas">打印的数据集合</param>
        /// <param name="completeCallBack">打印完成回调方法</param>
        /// <param name="copies"></param>
        void PrintProcess(IPrintable printable, string filePath, string printerName, Func<IEnumerable<object>> datas, Action completeCallBack, short copies);
        /// <summary>
        /// 更新 CSV 数据文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        void UpdateCsvDataPath(string filePath);
        /// <summary>
        /// 更新 CSV 数据文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        void UpdateCsvDataPath(XtraReport filePath);
    }
}
