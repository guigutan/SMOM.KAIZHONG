using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.PrepareProducts;
using SIE.MES.PrepareProducts.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons
{
    /// <summary>
    /// 产前准备项目与安灯对应关系
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产前准备项目与安灯对应关系")]
    public class AndonPrepareProjectDetail : DataEntity
    {
        #region 安灯维护 Andon
        /// <summary>
        /// 安灯维护Id
        /// </summary>
        [Label("安灯维护")]
        public static readonly IRefIdProperty AndonIdProperty =
            P<AndonPrepareProjectDetail>.RegisterRefId(e => e.AndonId, ReferenceType.Parent);

        /// <summary>
        /// 安灯维护Id
        /// </summary>
        public double AndonId
        {
            get { return (double)this.GetRefId(AndonIdProperty); }
            set { this.SetRefId(AndonIdProperty, value); }
        }

        /// <summary>
        /// 安灯维护
        /// </summary>
        public static readonly RefEntityProperty<Andon> AndonProperty =
            P<AndonPrepareProjectDetail>.RegisterRef(e => e.Andon, AndonIdProperty);

        /// <summary>
        /// 安灯维护
        /// </summary>
        public Andon Andon
        {
            get { return this.GetRefEntity(AndonProperty); }
            set { this.SetRefEntity(AndonProperty, value); }
        }
        #endregion

        #region 产前准备项目维护 PrepareProject
        /// <summary>
        /// 产前准备项目维护Id
        /// </summary>
        [Label("产前准备项目维护")]
        public static readonly IRefIdProperty PrepareProjectIdProperty =
            P<AndonPrepareProjectDetail>.RegisterRefId(e => e.PrepareProjectId, ReferenceType.Normal);

        /// <summary>
        /// 产前准备项目维护Id
        /// </summary>
        public double PrepareProjectId
        {
            get { return (double)this.GetRefId(PrepareProjectIdProperty); }
            set { this.SetRefId(PrepareProjectIdProperty, value); }
        }

        /// <summary>
        /// 产前准备项目维护
        /// </summary>
        public static readonly RefEntityProperty<PrepareProject> PrepareProjectProperty =
            P<AndonPrepareProjectDetail>.RegisterRef(e => e.PrepareProject, PrepareProjectIdProperty);

        /// <summary>
        /// 产前准备项目维护
        /// </summary>
        public PrepareProject PrepareProject
        {
            get { return this.GetRefEntity(PrepareProjectProperty); }
            set { this.SetRefEntity(PrepareProjectProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 项目编码 ProCode
        /// <summary>
        /// 项目编码
        /// </summary>
        [Label("项目编码")]
        public static readonly Property<string> ProCodeProperty = P<AndonPrepareProjectDetail>.RegisterView(e => e.ProCode, p => p.PrepareProject.ProCode);

        /// <summary>
        /// 项目编码
        /// </summary>
        public string ProCode
        {
            get { return this.GetProperty(ProCodeProperty); }
        }
        #endregion

        #region 项目名称 ProName
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("项目名称")]
        public static readonly Property<string> ProNameProperty = P<AndonPrepareProjectDetail>.RegisterView(e => e.ProName, p => p.PrepareProject.ProName);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProName
        {
            get { return this.GetProperty(ProNameProperty); }
        }
        #endregion

        #region 项目类型 ProType
        /// <summary>
        /// 项目类型
        /// </summary>
        [Label("项目类型")]
        public static readonly Property<PrepareProjectType?> ProTypeProperty = P<AndonPrepareProjectDetail>.RegisterView(e => e.ProType, p => p.PrepareProject.ProType);

        /// <summary>
        /// 项目类型
        /// </summary>
        public PrepareProjectType? ProType
        {
            get { return this.GetProperty(ProTypeProperty); }
        }
        #endregion

        #region 项目描述 ProDesc
        /// <summary>
        /// 项目描述
        /// </summary>
        [Label("项目描述")]
        public static readonly Property<string> ProDescProperty = P<AndonPrepareProjectDetail>.RegisterView(e => e.ProDesc, p => p.PrepareProject.ProDesc);

        /// <summary>
        /// 项目描述
        /// </summary>
        public string ProDesc
        {
            get { return this.GetProperty(ProDescProperty); }
        }
        #endregion

        #endregion
    }

    internal class AndonPrepareProjectDetailConfig : EntityConfig<AndonPrepareProjectDetail>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new NotDuplicateRule()
            {
                Properties =
                {
                    AndonPrepareProjectDetail.PrepareProjectIdProperty,
                    AndonPrepareProjectDetail.AndonIdProperty

                },
                MessageBuilder = (e) =>
                {
                    return "数据已存在!".L10N();
                }
            });
            base.AddValidations(rules);
        }
        protected override void ConfigMeta()
        {
            Meta.MapTable("ANDON_MES_PREPRO_DTL").MapAllProperties();
        }
    }

}
