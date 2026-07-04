using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Controls
{
    public class XPCardListPanel : Panel
    {
        public XPCard ACard { get; set; }

        /// <summary>
        /// The m data source
        /// </summary>
        private object m_dataSource = null;
        /// <summary>
        /// 数据源,支持列表或table，如果使用翻页控件，请使用翻页控件的DataSource
        /// </summary>
        /// <value>The data source.</value>
        /// <exception cref="Exception">数据源不是有效的数据类型，请使用Datatable或列表</exception>
        /// <exception cref="System.Exception">数据源不是有效的数据类型，请使用Datatable或列表</exception>
        [Description("数据源,支持列表或table，如果使用翻页控件，请使用翻页控件的DataSource"), Category("自定义")]
        public object DataSource
        {
            get { return m_dataSource; }
            set
            {
                if (value != null)
                {
                    if (!(value is DataTable) && (!typeof(IList).IsAssignableFrom(value.GetType())))
                    {
                        throw new Exception("数据源不是有效的数据类型，请使用Datatable或列表");
                    }
                }
                m_dataSource = value;
                ReloadSource();
            }
        }

        public void ReloadSource()
        {
            if (DesignMode)
                return;

            try
            {
                TreeGrid.ControlHelper.FreezeControl(this, true);
                if (m_dataSource != null)
                {
                    int intIndex = 0;

                    int intSourceCount = 0;
                    if (m_dataSource is DataTable)
                    {
                        intSourceCount = (m_dataSource as DataTable).Rows.Count;
                    }
                    else if (typeof(IList).IsAssignableFrom(m_dataSource.GetType()))
                    {
                        intSourceCount = (m_dataSource as IList).Count;
                    }

                    foreach (Control item in this.Controls)
                    {
                        if (intIndex >= intSourceCount)
                        {
                            item.Visible = false;
                        }
                        else
                        {
                            var row = (item as XPCard);
                            if (m_dataSource is DataTable)
                                row.BindData((m_dataSource as DataTable).Rows[intIndex]);
                            else
                                row.BindData((m_dataSource as IList)[intIndex]);
                            item.Visible = true;
                        }
                        intIndex++;
                    }

                    if (intIndex < intSourceCount)
                    {
                        for (int i = intIndex; i < intSourceCount; i++)
                        {
                            XPCard row = (XPCard)Activator.CreateInstance(this.ACard.GetType());
                            if (m_dataSource is DataTable)
                                row.BindData((m_dataSource as DataTable).Rows[intIndex]);
                            else
                                row.BindData((m_dataSource as IList)[intIndex]);
                            row.Dock = DockStyle.Top;
                            this.Controls.Add(row);

                        }
                    }
                }
                else
                {
                    foreach (Control item in this.Controls)
                    {
                        item.Visible = false;
                    }
                }
            }
            finally
            {
                TreeGrid.ControlHelper.FreezeControl(this, false);
            }
        }
    }
}
