using SIE.Defects;
using SIE.Domain;
using SIE.Domain.Query;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.TaskManagement.SuspectProductLabels;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Report.BatchTracebacks
{
    /// <summary>
    /// 缺陷代码
    /// </summary>
    [RootEntity, Serializable]
    [Label("缺陷代码")]
    public class BatchTracebackDefetctDtl : Entity<double>
    {
        #region 可疑品标签 SuspectProductLabel
        /// <summary>
        /// 可疑品标签Id
        /// </summary>
        [Label("可疑品标签")]
        public static readonly IRefIdProperty SuspectProductLabelIdProperty =
            P<BatchTracebackDefetctDtl>.RegisterRefId(e => e.SuspectProductLabelId, ReferenceType.Normal);

        /// <summary>
        /// 可疑品标签Id
        /// </summary>
        public double SuspectProductLabelId
        {
            get { return (double)this.GetRefId(SuspectProductLabelIdProperty); }
            set { this.SetRefId(SuspectProductLabelIdProperty, value); }
        }

        /// <summary>
        /// 可疑品标签
        /// </summary>
        public static readonly RefEntityProperty<SuspectProductLabel> SuspectProductLabelProperty =
            P<BatchTracebackDefetctDtl>.RegisterRef(e => e.SuspectProductLabel, SuspectProductLabelIdProperty);

        /// <summary>
        /// 可疑品标签
        /// </summary>
        public SuspectProductLabel SuspectProductLabel
        {
            get { return this.GetRefEntity(SuspectProductLabelProperty); }
            set { this.SetRefEntity(SuspectProductLabelProperty, value); }
        }
        #endregion

        #region 缺陷代码 DefectCode
        /// <summary>
        /// 缺陷代码
        /// </summary>
        [Label("缺陷代码")]
        public static readonly Property<string> DefectCodeProperty = P<BatchTracebackDefetctDtl>.Register(e => e.DefectCode);

        /// <summary>
        /// 缺陷代码
        /// </summary>
        public string DefectCode
        {
            get { return this.GetProperty(DefectCodeProperty); }
            set { this.SetProperty(DefectCodeProperty, value); }
        }
        #endregion

        #region 缺陷描述 DefectDesc
        /// <summary>
        /// 缺陷描述
        /// </summary>
        [Label("缺陷描述")]
        public static readonly Property<string> DefectDescProperty = P<BatchTracebackDefetctDtl>.Register(e => e.DefectDesc);

        /// <summary>
        /// 缺陷描述
        /// </summary>
        public string DefectDesc
        {
            get { return this.GetProperty(DefectDescProperty); }
            set { this.SetProperty(DefectDescProperty, value); }
        }
        #endregion
    }

    internal class BatchTracebackDefetctDtlConfig : EntityConfig<BatchTracebackDefetctDtl>
    {
        protected override void ConfigMeta()
        {
            Func<IQuery> view = () => DB.Query<SuspectProductLabelDetail>("spld")
            .Join<Defect>("d", (x, y) => x.DefectId == y.Id && y.SQL<int>("d.IS_PHANTOM") == 0)
.Select<Defect>((spld, d) => new
{
    Id = spld.Id,
    Suspect_Product_Label_Id = spld.SuspectProductLabelId,
    Defect_Code = d.Code,
    Defect_Desc = d.Description
})
.Where(spld => spld.SQL<int?>("spld.INV_ORG_ID") == RT.InvOrg && spld.SQL<int>("spld.IS_PHANTOM") == 0)
.ToQuery();
            Meta.MapView(view).MapAllProperties();
        }
    }
}
