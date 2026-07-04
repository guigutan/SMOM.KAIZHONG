using SIE.Domain;
using SIE.Domain.Query;
using SIE.MetaModel;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 物料 视图
    /// </summary>
    [RootEntity, Serializable]
    public class SimpleItemViewModel : Entity<double>
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        public static readonly Property<string> CodeProperty = P<SimpleItemViewModel>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        public static readonly Property<string> NameProperty = P<SimpleItemViewModel>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 规格型号 SpecificationModel
        /// <summary>
        /// 规格型号
        /// </summary>
        public static readonly Property<string> SpecificationModelProperty = P<SimpleItemViewModel>.Register(e => e.SpecificationModel);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string SpecificationModel
        {
            get { return GetProperty(SpecificationModelProperty); }
            set { SetProperty(SpecificationModelProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    internal class SimpleItemViewModelEntityConfig : EntityConfig<SimpleItemViewModel>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Func<IQuery> view = () => DB.Query<Item>("b1")
                    .Select((b1) => new
                    {
                        b1.Id,
                        Code = b1.Code,
                        Name = b1.Name,
                        Specification_Model = b1.SpecificationModel,
                    })
            .ToQuery();

            Meta.MapView(view).MapAllProperties();
            Meta.IsTreeEntity = false;
        }
    }
}