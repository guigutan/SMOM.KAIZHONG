using SIE.Defects;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.BatchWIP
{
    /// <summary>
    /// 转入批次缺陷
    /// </summary>
    [ChildEntity, Serializable]
    [Label("转入批次缺陷")]
    public class InputBatchDefect : DataEntity
    {
        #region 转入批次 InputBatch
        /// <summary>
        /// 转入批次Id
        /// </summary>
        [Label("转入批次")]
        public static readonly IRefIdProperty InputBatchIdProperty =
            P<InputBatchDefect>.RegisterRefId(e => e.InputBatchId, ReferenceType.Parent);

        /// <summary>
        /// 转入批次Id
        /// </summary>
        public double InputBatchId
        {
            get { return (double)this.GetRefId(InputBatchIdProperty); }
            set { this.SetRefId(InputBatchIdProperty, value); }
        }

        /// <summary>
        /// 转入批次
        /// </summary>
        public static readonly RefEntityProperty<InputBatch> InputBatchProperty =
            P<InputBatchDefect>.RegisterRef(e => e.InputBatch, InputBatchIdProperty);

        /// <summary>
        /// 转入批次
        /// </summary>
        public InputBatch InputBatch
        {
            get { return this.GetRefEntity(InputBatchProperty); }
            set { this.SetRefEntity(InputBatchProperty, value); }
        }
        #endregion

        #region 缺陷 Defect
        /// <summary>
        /// 缺陷Id
        /// </summary>
        [Label("缺陷")]
        public static readonly IRefIdProperty DefectIdProperty =
            P<InputBatchDefect>.RegisterRefId(e => e.DefectId, ReferenceType.Normal);

        /// <summary>
        /// 缺陷Id
        /// </summary>
        public double DefectId
        {
            get { return (double)this.GetRefId(DefectIdProperty); }
            set { this.SetRefId(DefectIdProperty, value); }
        }

        /// <summary>
        /// 缺陷
        /// </summary>
        public static readonly RefEntityProperty<Defect> DefectProperty =
            P<InputBatchDefect>.RegisterRef(e => e.Defect, DefectIdProperty);

        /// <summary>
        /// 缺陷
        /// </summary>
        public Defect Defect
        {
            get { return this.GetRefEntity(DefectProperty); }
            set { this.SetRefEntity(DefectProperty, value); }
        }
        #endregion

        #region 缺陷描述 DefectDesc
        /// <summary>
        /// 缺陷描述
        /// </summary>
        [Label("缺陷描述")]
        public static readonly Property<string> DefectDescProperty = P<InputBatchDefect>.Register(e => e.DefectDesc);

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

    /// <summary>
    /// 
    /// </summary>
    public class InputBatchDefectConfig : EntityConfig<InputBatchDefect>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INPUT_BATCH_DEFECT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
