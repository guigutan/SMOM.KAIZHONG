using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DispoLookups
{
    /// <summary>
    /// MRP控制者对照表
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("MRP控制者对照表")]
    public class DispoLookup : DataEntity
    {
        #region MRB控制者 Dispo
        /// <summary>
        /// MRB控制者
        /// </summary>
        [Label("MRB控制者")]
        public static readonly Property<string> DispoProperty = P<DispoLookup>.Register(e => e.Dispo);

        /// <summary>
        /// MRB控制者
        /// </summary>
        public string Dispo
        {
            get { return this.GetProperty(DispoProperty); }
            set { this.SetProperty(DispoProperty, value); }
        }
        #endregion

        #region 生产管理者 Fevor
        /// <summary>
        /// 生产管理者
        /// </summary>
        [Label("生产管理者")]
        public static readonly Property<string> FevorProperty = P<DispoLookup>.Register(e => e.Fevor);

        /// <summary>
        /// 生产管理者
        /// </summary>
        public string Fevor
        {
            get { return this.GetProperty(FevorProperty); }
            set { this.SetProperty(FevorProperty, value); }
        }
        #endregion

        #region 材料MRB控制者 MaterialDispo
        /// <summary>
        /// 材料MRB控制者
        /// </summary>
        [Label("材料MRB控制者")]
        public static readonly Property<string> MaterialDispoProperty = P<DispoLookup>.Register(e => e.MaterialDispo);

        /// <summary>
        /// 材料MRB控制者
        /// </summary>
        public string MaterialDispo
        {
            get { return this.GetProperty(MaterialDispoProperty); }
            set { this.SetProperty(MaterialDispoProperty, value); }
        }
        #endregion

        #region 物料类型 Mtart
        /// <summary>
        /// 物料类型
        /// </summary>
        [Label("物料类型")]
        public static readonly Property<string> MtartProperty = P<DispoLookup>.Register(e => e.Mtart);

        /// <summary>
        /// 物料类型
        /// </summary>
        public string Mtart
        {
            get { return this.GetProperty(MtartProperty); }
            set { this.SetProperty(MtartProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<DispoLookup>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion
    }

    internal class DispoLookupConfig : EntityConfig<DispoLookup>
    {

        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new NotDuplicateRule()
            {
                Properties = {
                DispoLookup.MaterialDispoProperty,
                DispoLookup.MtartProperty
                },
                MessageBuilder = (e) => {
                    return "已存在相同材料MRB控制者、物料类型的数据".L10N();
                }
            });
            base.AddValidations(rules);
        }

        protected override void ConfigMeta()
        {
            Meta.MapTable("DISPO_LOOKUP").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
