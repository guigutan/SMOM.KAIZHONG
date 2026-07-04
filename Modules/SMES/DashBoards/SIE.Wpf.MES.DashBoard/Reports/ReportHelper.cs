using DevExpress.Xpf.PivotGrid;
using DevExpress.Xpf.Printing;
using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MES.DashBoard.Reports.Commons;
using SIE.MES.Statistics.Fpy;
using SIE.ObjectModel;
using SIE.Reflection;
using SIE.Wpf.Controls.WaitProgress;
using SIE.Wpf.MES.DashBoard.Reports.Commons;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using SummaryType = SIE.MES.DashBoard.Reports.Commons.SummaryType;

namespace SIE.Wpf.MES.DashBoard.Reports
{
    /// <summary>
    /// 报表导出帮助类
    /// </summary>
    public class ReportHelper
    {
        /// <summary>
        /// 报表Field名字后缀
        /// </summary>
        public const string _fieldNameExtension = "Field";

        #region 布局保存恢复
        /// <summary>
        /// 保存布局设置
        /// </summary>
        /// <param name="pivotGridControl">报表控件</param>
        /// <param name="fileName">文件名</param>
        internal static void SaveLayoutToXml(PivotGridControl pivotGridControl, string fileName)
        {
            try
            {
                if (fileName.IsNullOrEmpty())
                {
                    CRT.MessageService.ShowWarning("保存设置失败:报表未设置保存布局文件名(LayoutFileName),请联系系统管理员！".L10N());
                    return;
                }

                string path = GetPath(fileName);
                pivotGridControl.SaveLayoutToXml(path);

                //由于样式也会被保存到xml，
                //删掉样式规则节点
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(path);
                XmlNodeList xnlist = xdoc.DocumentElement.ChildNodes[1].ChildNodes;
                for (int i = 0; i < xnlist.Count; i++)
                {
                    XmlElement xe = (XmlElement)xnlist.Item(i);
                    if (xe.Attributes["name"].Value == "FormatConditions")
                    {
                        xdoc.DocumentElement.ChildNodes[1].RemoveChild(xe);
                        break;
                    }
                }

                xdoc.Save(path);

                CRT.MessageService.ShowMessage("保存设置成功！".L10N());
            }
            catch (Exception exc)
            {
                CRT.MessageService.ShowWarning("保存设置失败:{0}".L10nFormat(exc.Message));
            }
        }

        /// <summary>
        /// 恢复布局设置
        /// </summary>
        /// <param name="pivotGridControl">报表控件</param>
        /// <param name="fileName">文件名</param>
        internal static void ResetLayoutToDefault(PivotGridControl pivotGridControl, string fileName)
        {
            try
            {
                if (fileName.IsNullOrEmpty())
                {
                    CRT.MessageService.ShowWarning("恢复默认设置失败:报表未设置保存布局文件名(LayoutFileName),请联系系统管理员！".L10N());
                    return;
                }

                string path = GetPath(fileName);
                File.Delete(path);
                CRT.MessageService.ShowMessage("恢复默认设置成功，重新打开页面后生效！".L10N());
            }
            catch (Exception exc)
            {
                CRT.MessageService.ShowWarning("恢复默认设置失败:{0}".L10nFormat(exc.Message));
            }
        }

        /// <summary>
        /// 获取报表布局配置路径
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>报表表格配置路径</returns>
        internal static string GetPath(string fileName)
        {
            var path = Environment.CurrentDirectory + "\\Customize\\";
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            return System.IO.Path.Combine(path, fileName + ".xml");
        }

        /// <summary>
        /// 恢复报表表格配置
        /// </summary>
        /// <param name="pivotGridControl">报表控件</param>
        /// <param name="fileName">布局文件名</param>
        internal static void RestoreLayoutFromXml(PivotGridControl pivotGridControl, string fileName)
        {
            try
            {
                if (fileName.IsNullOrEmpty())
                {
                    return;
                }

                string path = GetPath(fileName);
                if (!File.Exists(path))
                {
                    return;
                }

                pivotGridControl.RestoreLayoutFromXml(path);
            }
            catch (Exception exc)
            {
                CRT.MessageService.ShowWarning("加载报表设置失败:{0}".L10nFormat(exc.Message));
            }
        }
        #endregion

        #region 导出报表
        /// <summary>
        /// 导出报表到Excel
        /// </summary>
        /// <param name="printableControls">导出控件集合</param>
        public void ExportToExcel(List<IPrintableControl> printableControls)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Title = "导出数据信息".L10N();
#pragma warning disable S4055 // Literals should not be passed as localized parameters
            fileDialog.Filter = "Excel (2010) (.xlsx)|*.xlsx |Excel (2003)(.xls)|*.xls";
#pragma warning restore S4055 // Literals should not be passed as localized parameters
            if ((fileDialog.ShowDialog()) == DialogResult.OK)
            {
                var win = new WaitDialog();
                win.Width = 500;
                win.ShowInTaskbar = false;
                win.Text = "正在导出，请稍等……".L10N();
                win.ProgressValue = new ProgressValue() { Percent = 100 };

                Exception exception = null;
                string exportFilePath = fileDialog.FileName;
                string fileExtenstion = new FileInfo(exportFilePath).Extension;
                ThreadPool.QueueUserWorkItem(p =>
                {
                    try
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke(() =>
                        {
                            switch (fileExtenstion)
                            {
                                case ".xls":
                                    CreatePrintableLink(printableControls).ExportToXls(fileDialog.FileName);
                                    break;
                                case ".xlsx":
                                    CreatePrintableLink(printableControls).ExportToXlsx(fileDialog.FileName);
                                    break;
                                default:
                                    break;
                            }
                        });
                    }
                    catch (Exception exc)
                    {
                        exception = exc;
                    }
                    finally
                    {
                        Action ac = () => win.DialogResult = true;
                        win.Dispatcher.BeginInvoke(ac);
                    }
                });

                win.Topmost = true;
                win.ShowDialog();

                if (exception != null)
                {
                    CRT.MessageService.ShowException(exception);
                }
                else if (CRT.MessageService.AskQuestion("文件已成功导出，是否打开文件?".L10N(), "提示".L10N()))
                {
                    System.Diagnostics.Process.Start(exportFilePath);
                }
                else
                {
                    //
                }
            }
        }

        /// <summary>
        /// 创建打印Componet
        /// </summary>
        /// <param name="printableControls">需打印控件列表</param>
        /// <returns>返回打印Componet</returns>
        CompositeLink CreatePrintableLink(List<IPrintableControl> printableControls)
        {
            List<TemplatedLink> links = new List<TemplatedLink>();
            printableControls.ForEach(p => links.Add(new PrintableControlLink(p)
            {
                Landscape = true
            }));

            CompositeLink compositeLink = new CompositeLink(links);
            compositeLink.PrintingSystem.PageSettings.Landscape = true;
            return compositeLink;
        }
        #endregion

        /// <summary>
        /// 根据类型生成Field
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <returns>PivotGridField列表</returns>
        internal List<PivotGridField> InitPivotGridFields(Type type)
        {
            var fields = new List<PivotGridField>();

            var fieldInfoes = TypeHelper.GetHierarchy(type).SelectMany(t => t.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)).Where(t => typeof(IManagedProperty).IsAssignableFrom(t.FieldType)).ToList();
            foreach (var p in fieldInfoes)
            {
                var managedProperty = p.GetValue(null) as IManagedProperty;
                var attributes = p.GetCustomAttributes(typeof(FieldSettingAttribute), false);
                if (attributes == null || attributes.Length <= 0)
                {
                    continue;
                }

                var attrInfo = attributes[0] as FieldSettingAttribute;

                if (fields.Any(q => q.Area == DevExpress.Xpf.PivotGrid.FieldArea.DataArea) && attrInfo.Area == SIE.MES.DashBoard.Reports.Commons.FieldArea.DataArea)
                {
                    continue;
                }

                PivotGridField field = CreateField(managedProperty.Name, attrInfo);
                fields.Add(field);
            }

            return fields;
        }

        /// <summary>
        /// 创建Field
        /// </summary>
        /// <param name="fieldName">属性名称</param>
        /// <param name="attrInfo">配置信息</param>
        /// <returns>Pivot Grid属性</returns>
        private static PivotGridField CreateField(string fieldName, FieldSettingAttribute attrInfo)
        {
            DevExpress.Xpf.PivotGrid.FieldArea area;
            Enum.TryParse(Enum.GetName(typeof(DevExpress.Xpf.PivotGrid.FieldArea), attrInfo.Area), out area);
            PivotGridField field = new PivotGridField()
            {
                FieldName = fieldName,
                Name = fieldName + _fieldNameExtension,
                Area = area,
                AreaIndex = attrInfo.AreaIndex,
                Caption = attrInfo.Caption.L10N(),
                Visible = true,
                ShowTotals = true,
            };

            //格式化数据值为百分数
            if (field.Area == DevExpress.Xpf.PivotGrid.FieldArea.RowArea)
            {
                field.AllowDrag = false;
                field.ShowGrandTotal = false;
                field.SummaryType = (FieldSummaryType)Enum.Parse(typeof(SummaryType), attrInfo.SummaryType.ToString());
                field.TotalCellFormat = "p2";
                field.GrandTotalCellFormat = "p2";
            }
            else if (field.Area == DevExpress.Xpf.PivotGrid.FieldArea.DataArea)
            {
                field.EmptyCellText = " ".L10N();
                field.SummaryType = FieldSummaryType.Average;
                field.AllowDrag = false;
                field.CellFormat = "p2";
                field.TotalCellFormat = "p2";
                field.GrandTotalCellFormat = "p2";
            }
            else
            {
                field.ShowTotals = false;
                field.ShowGrandTotal = false;
                field.GrandTotalCellFormat = "p2";
                field.AllowedAreas = FieldAllowedAreas.FilterArea | FieldAllowedAreas.ColumnArea;
            }

            return field;
        }

        /// <summary>
        /// 打开直通率配置视图
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="viewGroup">视图组</param>
        /// <param name="moduleKey">模块键</param>
        internal void ShowSettingView(Type type, string viewGroup = ViewConfig.ListView, string moduleKey = null)
        {
            var key = CRT.Workbench.CreateKey(viewGroup, type, null);
            var ui = new ListUITemplate(type, viewGroup, moduleKey).CreateUI();

            CRT.Workbench.ShowView(key, (v) =>
            {
                var attributes = type.GetCustomAttributes(typeof(LabelAttribute), true);
                v.Title = attributes == null || attributes.Length <= 0 ? "直通率设置".L10N() : (attributes[0] as LabelAttribute).Label;

                return ui;
            });

            ui.MainView.QueryView?.TryExecuteQuery();
        }

        /// <summary>
        /// 显示工序直通率报表视图
        /// </summary>
        /// <param name="type">报表类型</param>
        /// <param name="fpySouece">直通率报表数据源</param>
        /// <param name="topFiveSouece">缺陷TOP5数据源</param>
        /// <param name="title">标题</param>
        internal void ShowProcessFpyView(Type type, EntityList<DefectStatistics> topFiveSouece, EntityList<ProcessFpyStatistics> fpySouece, string title = "")
        {
            var control = new ProceessDirectRateStatistics(topFiveSouece, fpySouece);
            var key = CRT.Workbench.CreateKey(ViewConfig.DetailsView, type, null);
            CRT.Workbench.Close(key);
            CRT.Workbench.ShowView(key, v =>
            {
                v.Title = "{0}工序直通率统计".L10nFormat(title);
                return control;
            });
        }

        /// <summary>
        /// 显示工序直通率报表视图
        /// </summary>
        /// <param name="model">报表基类</param>
        /// <param name="criteria">查询实体</param>
        /// <param name="title">Tab页标题</param>
        /// <param name="fpyTitle">直通率标题</param>
        internal void ShowProcessFpyView(ReportBaseViewModel model, DefectStatisticsCriteria criteria, string title = "", string fpyTitle = "")
        {
            var topFiveSouece = model.GetDefectTopDataSource(criteria);
            var fpySouece = model.GetProcessFpyDataSource(criteria);
            var type = model.GetType();

            var control = new ProceessDirectRateStatistics(topFiveSouece, fpySouece, fpyTitle);
            var key = CRT.Workbench.CreateKey(ViewConfig.DetailsView, type, null);
            CRT.Workbench.Close(key);
            CRT.Workbench.ShowView(key, v =>
            {
                v.Title = "{0}工序直通率统计".L10nFormat(title);
                return control;
            });
        }
    }
}
