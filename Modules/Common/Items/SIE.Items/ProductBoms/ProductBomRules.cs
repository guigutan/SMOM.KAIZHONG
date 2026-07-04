using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.Common.Items;
using SIE.Items.ProductBoms;
using SIE.MetaModel;
using System;

namespace SIE.Items
{
    #region 产品BOM编码与版本号非重验证规则
    /// <summary>
    /// 产品BOM编码与版本号非重验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("产品BOM编码与版本号非重验证规则")]
    [System.ComponentModel.Description("产品BOM编码与版本号非重验证规则")]
    public class NotDuplicateProductBom : NotDuplicateRule<ProductBom>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NotDuplicateProductBom()
        {
            Properties.Add(ProductBom.CodeProperty);
            Properties.Add(ProductBom.VersionProperty);
            MessageBuilder = (e) =>
            {
                var entity = e as ProductBom;
                return "存在产品编码[{0}]、版本[{1}]的产品BOM".L10nFormat(entity.Code, entity.Version);
            };
        }
    }
    #endregion

    #region 产品BOM产品与项目号非重复校验规则
    /// <summary>
    /// 产品BOM产品与项目号非重复校验规则
    /// </summary>
    [System.ComponentModel.DisplayName("产品BOM产品与项目号非重复校验规则")]
    [System.ComponentModel.Description("产品BOM产品与项目号非重复校验规则")]
    public class NotDuplicateProductBomProject: NotDuplicateRule<ProductBom>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NotDuplicateProductBomProject()
        {
            Properties.Add(ProductBom.ProductIdProperty);
            Properties.Add(ProductBom.ProjectMaintainIdProperty);
            MessageBuilder = (e) =>
            {
                var entity = e as ProductBom;
                return "存在产品编码[{0}]、项目号编码[{1}]的产品BOM".L10nFormat(entity.ProductCode, entity.ProjectMaintainCode);
            };
        }
    }
    #endregion

    #region 产品BOM明细的物料不能重复
    /// <summary>
    /// 产品BOM明细的物料不能重复
    /// </summary>
    [System.ComponentModel.DisplayName("产品BOM明细的物料不能重复")]
    [System.ComponentModel.Description("产品BOM明细的物料不能重复")]
    public class NotDuplicateProductBomDetail : NotDuplicateRule<ProductBomDetail>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NotDuplicateProductBomDetail()
        {
            Properties.Add(ProductBomDetail.ProductBomIdProperty);
            Properties.Add(ProductBomDetail.ItemIdProperty);
            MessageBuilder = (e) =>
            {
                var entity = e as ProductBomDetail;
                return "产品BOM[{0}],存在相同的物料编码[{1}]".L10nFormat(entity.ProductBom.Code, entity.Item.Code);
            };

        }
    }
    #endregion



    #region 产品BOM明细物料不能与产品一样
    /// <summary>
    /// 产品BOM明细物料不能与产品一样
    /// </summary>
    [System.ComponentModel.DisplayName("产品BOM明细物料验证规则")]
    [System.ComponentModel.Description("产品BOM明细物料不能与产品一样")]
    public class BomDetailItemRule : EntityRule<ProductBomDetail>
    {
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">验证规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var detail = entity as ProductBomDetail;
            var bom = detail.ProductBom;
            if (bom.ProductId == detail.ItemId)
            {
                e.BrokenDescription = "产品BOM产品[{0}]与BOM明细物料[{1}]不能一样".L10nFormat(bom.Product.Code, detail.Item.Code);
            }
        }
    }
    #endregion

    #region 产品BOM有子项时不允许删除验证
    /// <summary>
    /// 产品BOM有子项时不允许删除验证
    /// </summary>
    [System.ComponentModel.DisplayName("产品BOM验证规则")]
    [System.ComponentModel.Description("产品BOM下面有子项时不允许删除")]
    public class UnDeleteProductBomHasDetail : EntityRule<ProductBom>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UnDeleteProductBomHasDetail()
        {
            Scope = EntityStatusScopes.Delete;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">验证规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var productBom = entity as ProductBom;
            var values = RT.Service.Resolve<ProductBomController>().GetProductBomPropertyValues(productBom.Id);
            var details = RT.Service.Resolve<ItemController>().GetProductBomDetails(productBom.Id);
            if (values.Count > 0 || details.Count > 0)
            {
                e.BrokenDescription = "数据有子项不允许删除".L10N();
            }
        }
    }
    #endregion

    #region 产品BOM明细下面有子项时不允许删除
    /// <summary>
    /// 产品BOM明细下面有子项时不允许删除
    /// </summary>
    [System.ComponentModel.DisplayName("产品BOM明细验证规则")]
    [System.ComponentModel.Description("产品BOM明细下面有子项时不允许删除")]
    public class UnDeleteProductBomDetailHasProperty : EntityRule<ProductBomDetail>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UnDeleteProductBomDetailHasProperty()
        {
            Scope = Scope = EntityStatusScopes.Add | EntityStatusScopes.Update | EntityStatusScopes.Delete;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">验证规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var detail = entity as ProductBomDetail;
            if (Scope != EntityStatusScopes.Delete && RT.Service.Resolve<ProductBomController>().CountProductBomDetailWithExtProp(detail) > 0)
            {
                e.BrokenDescription = "存在物料[{0}]+扩展属性[{1}]的产品BOM明细".L10nFormat(detail.Item.Name, detail.ItemExtPropName);
            }
        }
    }
    #endregion

    #region 产品BOM属性值验证
    /// <summary>
    /// 产品BOM属性的属性值不能相同
    /// </summary>
    [System.ComponentModel.DisplayName("产品BOM属性值验证规则")]
    [System.ComponentModel.Description("属性值不能相同")]
    public class ProductBomValueNotDuplicateRule : NotDuplicateRule<ProductBomPropertyValue>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductBomValueNotDuplicateRule()
        {
            Properties.Add(ProductBomPropertyValue.DefinitionIdProperty);
            Properties.Add(ProductBomPropertyValue.ValueProperty);
            Properties.Add(ProductBomPropertyValue.ProductBomIdProperty);
            MessageBuilder = (e) =>
            {
                return "该产品BOM属性的属性值已经存在".L10N();
            };
        }
    }
    #endregion

    #region 产品BOM明细属性值验证
    /// <summary>
    /// 产品BOM明细属性的属性值不能相同
    /// </summary>
    [System.ComponentModel.DisplayName("产品BOM明细属性值验证规则")]
    [System.ComponentModel.Description("属性值不能相同")]
    public class ProductBomDetailValueNotDuplicateRule : NotDuplicateRule<ProductBomDetailPropertyValue>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductBomDetailValueNotDuplicateRule()
        {
            Properties.Add(ProductBomDetailPropertyValue.DefinitionIdProperty);
            Properties.Add(ProductBomDetailPropertyValue.ValueProperty);
            Properties.Add(ProductBomDetailPropertyValue.DetailIdProperty);
            MessageBuilder = (e) =>
            {
                return "该产品BOM明细属性的属性值已经存在".L10N();
            };
        }
    }
    #endregion

    #region BOM明细单位耗用量必须大于0
    /// <summary>
    /// BOM明细单位耗用量必须大于0
    /// </summary>
    [System.ComponentModel.DisplayName("BOM明细验证规则")]
    [System.ComponentModel.Description("BOM明细单位耗用量必须大于0")]
    public class BomDetailUnitQtyRule : EntityRule<ProductBomDetail>
    {
        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="entity">IEntity</param>
        /// <param name="e">RuleArgs</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var productBomDtl = entity as ProductBomDetail;
            if (productBomDtl != null && productBomDtl.Item != null && productBomDtl.UnitQty <= 0)
            {
                e.BrokenDescription = "物料[{0}]单位耗用量必须大于0".L10nFormat(productBomDtl.Item.Name);
            }
        }
    }
    #endregion

    #region 产品BOM被生产订单或需求管理引用时不许删除
    /// <summary>
    /// 产品BOM被生产订单或需求管理引用时不许删除
    /// </summary>
    [System.ComponentModel.DisplayName("产品BOM验证规则")]
    [System.ComponentModel.Description("产品BOM被生产订单或需求管理引用时不许删除")]
    public class UnDeleteProductBomApponited : EntityRule<ProductBom>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UnDeleteProductBomApponited()
        {
            Scope = EntityStatusScopes.Delete;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">验证规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var productBom = entity as ProductBom;
            var dm = RT.Service.Resolve<IAppointedDemandManagement>().ExsitAppointedProductBomDemandManagement(productBom.Id);
            var po = RT.Service.Resolve<IAppointedProductOrder>().ExsitAppointedProductBomProductOrder(productBom.Id);
            if (dm || po)
            {
                e.BrokenDescription = "产品BOM被生产订单或需求管理引用为指定BOM，不许删除".L10N();
            }
        }
    }
    #endregion
}
