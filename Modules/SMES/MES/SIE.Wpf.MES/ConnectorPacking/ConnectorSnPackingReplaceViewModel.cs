using DevExpress.DashboardCommon.DataProcessing;
using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MES.Engrave;
using SIE.MES.PackingQC;
using SIE.MES.PackRule;
using SIE.MES.TaskManagement.PackingQcs;
using SIE.ObjectModel;
using SIE.Wpf.MES.WIP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.ConnectorPacking
{
    /// <summary>
    /// 连接器单体包装采集替换ViewModel
    /// </summary>
    [RootEntity]
    [Label("连接器单体包装采集替换ViewModel")]
    public class ConnectorSnPackingReplaceViewModel: KZDataCollectionViewModel
    {
        public ConnectorSnPackingReplaceViewModel()
        {
        }

        #region 原刻码标签 OldBatchNo
        /// <summary>
        /// 原刻码标签
        /// </summary>
        [Label("原刻码标签")]
        public static readonly Property<string> OldBatchNoProperty = P<ConnectorSnPackingReplaceViewModel>.Register(e => e.OldBatchNo);

        /// <summary>
        /// 原刻码标签
        /// </summary>
        public string OldBatchNo
        {
            get { return this.GetProperty(OldBatchNoProperty); }
            set { this.SetProperty(OldBatchNoProperty, value); }
        }
        #endregion

        #region 新刻码标签 NewBatchNo
        /// <summary>
        /// 新刻码标签
        /// </summary>
        [Label("新刻码标签")]
        public static readonly Property<string> NewBatchNoProperty = P<ConnectorSnPackingReplaceViewModel>.Register(e => e.NewBatchNo);

        /// <summary>
        /// 新刻码标签
        /// </summary>
        public string NewBatchNo
        {
            get { return this.GetProperty(NewBatchNoProperty); }
            set { this.SetProperty(NewBatchNoProperty, value); }
        }
        #endregion

        #region 原刻码 OldEngraveLabel
        /// <summary>
        /// 原刻码Id
        /// </summary>
        [Label("原刻码")]
        public static readonly IRefIdProperty OldEngraveLabelIdProperty =
            P<ConnectorSnPackingReplaceViewModel>.RegisterRefId(e => e.OldEngraveLabelId, ReferenceType.Normal);

        /// <summary>
        /// 原刻码Id
        /// </summary>
        public double? OldEngraveLabelId
        {
            get { return (double?)this.GetRefNullableId(OldEngraveLabelIdProperty); }
            set { this.SetRefNullableId(OldEngraveLabelIdProperty, value); }
        }

        /// <summary>
        /// 原刻码
        /// </summary>
        public static readonly RefEntityProperty<EngraveLabel> OldEngraveLabelProperty =
            P<ConnectorSnPackingReplaceViewModel>.RegisterRef(e => e.OldEngraveLabel, OldEngraveLabelIdProperty);

        /// <summary>
        /// 原刻码
        /// </summary>
        public EngraveLabel OldEngraveLabel
        {
            get { return this.GetRefEntity(OldEngraveLabelProperty); }
            set { this.SetRefEntity(OldEngraveLabelProperty, value); }
        }
        #endregion

        /// <summary>
        /// 扫描变更事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnBarcodeChanged(ManagedPropertyChangedEventArgs e)
        {
            if (Barcode.IsNullOrEmpty()) return;
            Tips = string.Empty;
            Error = string.Empty;
            //当原刻码标签为空的时候，就说明在扫描原刻码阶段
            if (OldEngraveLabel == null)
            {
                EngraveSn engraveSn = RT.Service.Resolve<EngraveLabelController>().GetEngraveSn(Barcode);
                if (engraveSn == null)
                {
                    Error = "原刻码不存在!".L10N();
                    Barcode = "";
                    return;
                }
                PackingDetail packingDetail = RT.Service.Resolve<PackingQcController>().GetPackingDetailByProductLabel(Barcode);
                if (packingDetail == null)
                {
                    Error = "包装QC明细不存在!".L10N();
                    Barcode = "";
                    return;
                }
                OldEngraveLabel = engraveSn.EngraveLabel;
                OldBatchNo = Barcode;
                Tips = "请扫描替换刻码!".L10N();
                Barcode = string.Empty;
                return;
            }
            else
            {
                if (!NewBatchNo.IsNullOrEmpty())
                {
                    Error = "已扫描替换刻码，如需重新扫描请点击重新开始!".L10N();
                    Barcode = "";
                    return;
                }

                //原刻码不为空，就代码已经扫码原刻码了，现在要扫描替换刻码
                EngraveSn engraveSn = RT.Service.Resolve<EngraveLabelController>().GetEngraveSn(Barcode);
                if (engraveSn != null)
                {
                    Error = "替换刻码已存在!".L10N();
                    Barcode = "";
                    return;
                }

                try
                {
                    //校验二维码规则
                    RT.Service.Resolve<ConnectorSnPackingController>().ValidItemQRCodeRule(OldEngraveLabel.ProductId.Value, Barcode);
                }
                catch (Exception ex)
                {
                    Error = ex.GetBaseException().Message;
                    Barcode = "";
                    return;
                }

                NewBatchNo = Barcode;
                Tips = "已扫描替换刻码，请点击确认提交!".L10N();
                Barcode = string.Empty;
                return;
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="resetType"></param>
        public override void Reset(ResetType resetType)
        {
            base.Reset(resetType);
            Tips = "已重新开始，请扫描原刻码".L10N();
            Error = string.Empty;
            OldBatchNo = string.Empty;
            NewBatchNo = string.Empty;
            OldEngraveLabel = null;
            OldEngraveLabelId = null;
            Barcode = string.Empty;
        }
    }
}
