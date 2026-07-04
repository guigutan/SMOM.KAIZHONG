using SIE.XPCJ.Models.WIP;
using System.Drawing;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Controls
{
    public partial class XPTaskListCard : BaseUserControl
    {
        public ReportTask RepportTask { get; set; }

        public XPTaskListCard()
        {
            InitializeComponent();
            DoubleBuffered = true;
            this.tableLayoutPanel1.CellPaint += TableLayoutPanel1_CellPaint;
        }

        private void TableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            // 单元格重绘
            Pen pp = new Pen(Color.FromArgb(242, 242, 242));
            e.Graphics.DrawLine(pp, new Point(e.CellBounds.X, e.CellBounds.Y + e.CellBounds.Height - 1), new Point(e.CellBounds.X + e.CellBounds.Width - 1, e.CellBounds.Y + e.CellBounds.Height - 1));
        }

        private void TaskInfoCardCtr_Load(object sender, System.EventArgs e)
        {
            if (this.RepportTask != null)
            {
                this.lbBeginTime.Text = this.RepportTask.BeginTime.HasValue ? this.RepportTask.BeginTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
                this.lbEndTime.Text = this.RepportTask.EndTime.HasValue ? this.RepportTask.EndTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";

                this.lbNGQty.Text = this.RepportTask.NgQty.ToString();
                this.lbPassQty.Text = this.RepportTask.OkQty.ToString();
                this.lbProduceCode.Text = this.RepportTask.ProductCode;

                this.lbTaskNo.Text = this.RepportTask.No;
                this.lbToReportQty.Text = this.RepportTask.ToReportQty.ToString();
                this.lbWONo.Text = this.RepportTask.WorkOrderNo;
                this.lbTaskNum.Text = this.RepportTask.DispatchQty.ToString();
            }
        }

    }
}
