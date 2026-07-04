using SIE.Domain;
using SIE.MES.BatchWIP.Products;
using SIE.MES.BatchWIP.Products.SplitAndMerge;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Report.BatchWipProducts
{
    /// <summary>
    /// 批次采集记录(出站入站分离)
    /// </summary>
    [ChildEntity, Serializable]
    [Label("批次采集记录")]
    public class BatchWipReportRecord : BatchWipRecord
    {
        #region 生产通用报表 BatchVersion
        /// <summary>
        /// 生产通用报表Id
        /// </summary>
        [Label("生产通用报表")]
        public static new readonly IRefIdProperty BatchVersionIdProperty =
            P<BatchWipReportRecord>.RegisterRefId(e => e.BatchVersionId, ReferenceType.Parent);

        /// <summary>
        /// 生产通用报表Id
        /// </summary>
        public new double BatchVersionId
        {
            get { return (double)this.GetRefId(BatchVersionIdProperty); }
            set { this.SetRefId(BatchVersionIdProperty, value); }
        }

        /// <summary>
        /// 生产通用报表
        /// </summary>
        public static new readonly RefEntityProperty<BatchWipProductVersionReport> BatchVersionProperty =
            P<BatchWipReportRecord>.RegisterRef(e => e.BatchVersion, BatchVersionIdProperty);

        /// <summary>
        /// 生产通用报表
        /// </summary>
        public new BatchWipProductVersionReport BatchVersion
        {
            get { return this.GetRefEntity(BatchVersionProperty); }
            set { this.SetRefEntity(BatchVersionProperty, value); }
        }
        #endregion

        #region 关键件列表 KeyItemList
        /// <summary>
        /// 关键件列表
        /// </summary>
        public static new readonly ListProperty<EntityList<BatchWipProductProcessKeyItemReport>> KeyItemListProperty = P<BatchWipReportRecord>.RegisterList(e => e.KeyItemList);

        /// <summary>
        /// 关键件列表
        /// </summary>
        public new EntityList<BatchWipProductProcessKeyItemReport> KeyItemList
        {
            get { return this.GetLazyList(KeyItemListProperty); }
        }
        #endregion
    }
}
