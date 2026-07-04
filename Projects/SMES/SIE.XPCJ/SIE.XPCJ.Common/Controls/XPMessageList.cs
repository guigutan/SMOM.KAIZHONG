using SIE.XPCJ.Common.Properties;
using SIE.XPCJ.Models.WIP;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Controls
{
    public partial class XPMessageList : UserControl
    {
        public XPMessageList()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        /// <summary>
        /// 最大记录数
        /// </summary>
        public int AMaxRowCount { get; set; } = 80;

        private BindingList<MessageInfo> messageInfoList;

        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageType">1：成功 0-错误 2-警告</param>
        public void AddMessage(string message, XPMessageType messageType = XPMessageType.Success, bool isReplaceN = true)
        {
            if (string.IsNullOrEmpty(message))
                return;

            if (messageInfoList == null)
                messageInfoList = new BindingList<MessageInfo>();

            if (this.dataGridView1.DataSource == null)
                this.dataGridView1.DataSource = messageInfoList;

            if (this.messageInfoList.Count > this.AMaxRowCount)
            {
                this.messageInfoList.RemoveAt(this.messageInfoList.Count - 1); //移除
            }

            this.messageInfoList.Insert(0, new MessageInfo()
            {
                MessageType = (int)messageType,
                Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Message = isReplaceN ? message.Replace("\r\n", "").Replace("\n", "") : message
            });
            if (this.dataGridView1.Rows.Count > 0)
            {
                this.dataGridView1.Rows[0].Selected = true;
                this.dataGridView1.FirstDisplayedScrollingRowIndex = 0;
            }
        }


        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].DataPropertyName.Equals("MessageType"))
            {
                if (e.Value == null)
                {
                    return;
                }
                var type = (int)e.Value;
                switch (type)
                {
                    case 0:
                        e.Value = Resources.警示;
                        break;
                    case 1:
                        e.Value = Resources.成功;
                        break;
                    case 2:
                        e.Value = Resources.警示;
                        break;
                }
                e.FormattingApplied = true;
            }
        }

        private void MessageListCtr_Load(object sender, System.EventArgs e)
        {
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.GridColor = dataGridView1.BackColor;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            if (this.dataGridView1.Rows.Count > 0)
            {
                this.dataGridView1.Rows[0].Selected = true;
                this.dataGridView1.FirstDisplayedScrollingRowIndex =0;
                
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (this.dataGridView1.Rows.Count > 0)
            {
                this.dataGridView1.Rows[0].Selected = true;
                this.dataGridView1.FirstDisplayedScrollingRowIndex = 0;
            }
        }
    }

    public enum XPMessageType
    {
        Error = 0,
        Success = 1,
        Warn = 2
    }
}
