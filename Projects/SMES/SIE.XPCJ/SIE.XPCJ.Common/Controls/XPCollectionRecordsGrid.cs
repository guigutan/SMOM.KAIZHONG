using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Models.Enums;
using SIE.XPCJ.Models.WIP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Controls
{
    public partial class XPCollectionRecordsGrid : UserControl
    {
        /// <summary>
        /// 最大记录数
        /// </summary>
        public int AMaxRowCount { get; set; } = 1000;

        /// <summary>
        /// 采集记录列表
        /// </summary>
        private BindingList<CollectDetail> collectDetailList;

        /// <summary>
        /// 添加采集结果记录
        /// </summary>
        /// <param name="barCode">条码</param>
        /// <param name="barcodeType">类型</param>
        /// <param name="result">结果类型</param>
        public void AddRecord(string barCode, BarcodeType? barcodeType, ResultType result = ResultType.Pass)
        {
            if (collectDetailList == null)
                collectDetailList = new BindingList<CollectDetail>();

            if (this.dataGridView1.DataSource == null)
                this.dataGridView1.DataSource = collectDetailList;

            if (this.collectDetailList.Count > this.AMaxRowCount)
            {
                this.collectDetailList.RemoveAt(this.collectDetailList.Count - 1); //移除
            }

            this.collectDetailList.Insert(0, new CollectDetail
            {
                Barcode = barCode,
                BarcodeType = barcodeType.ToLabel(),
                CollectDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Result = result.ToLabel()
            });

            if (this.dataGridView1.Rows.Count > 0)
            {
                this.dataGridView1.Rows[0].Selected = true;
            }
        }

        /// <summary>
        /// 添加采集结果记录
        /// </summary>
        /// <param name="barcode">采集条码</param>
        /// <param name="result">结果类型</param>
        public void AddRecord(CollectBarcode barcode, ResultType result = ResultType.Pass)
        {
            if (barcode == null || string.IsNullOrEmpty(barcode.Code))
                return;

            AddRecord(barcode.Code, barcode.Type, result);
        }

        public XPCollectionRecordsGrid()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        private void CollectionRecordsGridCtr_Load(object sender, EventArgs e)
        {
        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (i % 2 != 0)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(238, 238, 238);
                }
            }
        }
    }
}
