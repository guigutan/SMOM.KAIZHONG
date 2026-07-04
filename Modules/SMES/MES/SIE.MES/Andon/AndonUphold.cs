using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.Fixture;
using SIE.MES.ProcessProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Andon
{
    /// <summary>
    /// 安灯区域
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(AndonUpholdCriterial))]
    [DisplayMember(nameof(AndonDesc))]
    [Label("安灯区域")]
    public class AndonUphold : DataEntity
    {
        #region 区域描述 AndonDesc
        /// <summary>
        /// 区域描述
        /// </summary>
        [Required]
        [Label("区域描述")]
        public static readonly Property<string> AndonDescProperty = P<AndonUphold>.Register(e => e.AndonDesc);

        /// <summary>
        /// 区域描述
        /// </summary>
        public string AndonDesc
        {
            get { return this.GetProperty(AndonDescProperty); }
            set { this.SetProperty(AndonDescProperty, value); }
        }
        #endregion

        #region 安灯编码 AndonCode
        /// <summary>
        /// 安灯编码
        /// </summary>
        [Required]
        [Label("安灯编码")]
        public static readonly Property<string> AndonCodeProperty = P<AndonUphold>.Register(e => e.AndonCode);

        /// <summary>
        /// 安灯编码
        /// </summary>
        public string AndonCode
        {
            get { return this.GetProperty(AndonCodeProperty); }
            set { this.SetProperty(AndonCodeProperty, value); }
        }
        #endregion

        #region 厂部编码 PlantCode
        /// <summary>
        /// 厂部编码
        /// </summary>
        [Label("厂部编码")]
        public static readonly Property<string> PlantCodeProperty = P<AndonUphold>.Register(e => e.PlantCode);

        /// <summary>
        /// 安灯编码
        /// </summary>
        public string PlantCode
        {
            get { return this.GetProperty(PlantCodeProperty); }
            set { this.SetProperty(PlantCodeProperty, value); }
        }
        #endregion

        #region 厂部名称 PlantName
        /// <summary>
        /// 厂部名称
        /// </summary>
        [Label("厂部名称")]
        public static readonly Property<string> PlantNameProperty = P<AndonUphold>.Register(e => e.PlantName);

        /// <summary>
        /// 安灯编码
        /// </summary>
        public string PlantName
        {
            get { return this.GetProperty(PlantNameProperty); }
            set { this.SetProperty(PlantNameProperty, value); }
        }
        #endregion

        #region IOT指令 AndonOrder
        /// <summary>
        /// IOT指令
        /// </summary>
        [Required]
        [Label("IOT指令")]
        public static readonly Property<string> AndonOrderProperty = P<AndonUphold>.Register(e => e.AndonOrder);

        /// <summary>
        /// IOT指令
        /// </summary>
        public string AndonOrder
        {
            get { return this.GetProperty(AndonOrderProperty); }
            set { this.SetProperty(AndonOrderProperty, value); }
        }
        #endregion

        #region IOT实体 AndonEntity
        /// <summary>
        /// IOT实体
        /// </summary>
        [Required]
        [Label("IOT实体")]
        public static readonly Property<string> AndonEntityProperty = P<AndonUphold>.Register(e => e.AndonEntity);

        /// <summary>
        /// IOT实体
        /// </summary>
        public string AndonEntity
        {
            get { return this.GetProperty(AndonEntityProperty); }
            set { this.SetProperty(AndonEntityProperty, value); }
        }
        #endregion
    }
    /// <summary>
    /// 安灯区域 实体配置
    /// </summary>
    internal class AndonUpholdConfig : EntityConfig<AndonUphold>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(AndonUphold.AndonDescProperty, new NotDuplicateRule());
            rules.AddRule(AndonUphold.AndonCodeProperty, new NotDuplicateRule());
            rules.AddRule(new NotDuplicateRule()
            {
                Properties =
                {
                    AndonUphold.AndonDescProperty,
                    AndonUphold.AndonCodeProperty,
                    AndonUphold.AndonOrderProperty,
                    AndonUphold.AndonEntityProperty
                },
                MessageBuilder = (e) =>
                {
                    return "数据已存在!".L10N();
                }
            });
            base.AddValidations(rules);
        }
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ANDON_UPHOLD").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
