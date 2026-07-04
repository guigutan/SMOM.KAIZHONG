using SIE.Domain;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Utils;
using System;
using System.Linq;

namespace SIE.xUnit.Resources
{
    public partial class ResTestController : DomainController
    {
        /// <summary>
        /// 创建企业模型（车间）
        /// </summary>
        /// <param name="isNew">是否新增</param>
        /// <returns></returns>
        public virtual Enterprise CreateShop(bool isNew = true)
        {
            Enterprise shop = null;
            var list = RT.Service.Resolve<EnterpriseController>().GetEnterprises(EnterpriseType.Group);
            if (list.Count == 0)
            {
                var level = new EnterpriseLevel();
                level.Type = EnterpriseType.Group;
                level.Code = level.Type.ToString();
                level.Name = EnumViewModel.EnumToLabel(level.Type);
                level.IsByHand = YesNo.No;
                level.InvOrgId = 0;
                RF.Save(level);

                var enterprise = new Enterprise();
                enterprise.Level = level;
                enterprise.Name = "SIE集团";
                enterprise.Code = "SIE_GROUP";
                enterprise.IsByHand = YesNo.No;
                enterprise.InvOrgId = 0;
                RF.Save(enterprise);

                //车间
                var shoplevel = new EnterpriseLevel();
                shoplevel.Type = EnterpriseType.Shop;
                shoplevel.Code = shoplevel.Type.ToString();
                shoplevel.Name = EnumViewModel.EnumToLabel(EnterpriseType.Shop);
                shoplevel.IsByHand = YesNo.No;
                shoplevel.InvOrgId = RT.InvOrg;
                shoplevel.TreePId = level.Id;
                shoplevel.IsResource = true;
                RF.Save(shoplevel);

                shop = new Enterprise();
                shop.GenerateId();
                shop.Level = shoplevel;
                shop.Name = "ShopName" + shop.Id;
                shop.Code = "ShopCode" + shop.Id;
                shop.IsByHand = YesNo.Yes;
                shop.InvOrgId = 1;
                shop.IsResource = true;
                shop.TreePId = enterprise.Id;
                RF.Save(shop);

            }
            else
            {
                var oldShop = RT.Service.Resolve<EnterpriseController>().GetEnterprises(EnterpriseType.Shop).LastOrDefault();
                if (isNew)
                {
                    shop = new Enterprise();
                    shop.GenerateId();
                    shop.Level = oldShop.Level;
                    shop.Name = "ShopName" + shop.Id;
                    shop.Code = "ShopCode" + shop.Id;
                    shop.IsByHand = YesNo.Yes;
                    shop.InvOrgId = 1;
                    shop.IsResource = true;
                    shop.TreePId = oldShop.TreePId;
                    RF.Save(shop);
                }
                else
                {
                    shop = oldShop;
                }
            }

            return shop;
        }

        /// <summary>
        /// 创建班组
        /// </summary>
        /// <returns></returns>
        public virtual WorkGroup CreateWorkGroup()
        {
            var workGroup = new WorkGroup();
            workGroup.GenerateId();
            double id = workGroup.Id;
            workGroup.Code = $"Code{id}";
            workGroup.Name = $"Name{id}";
            workGroup.DemandQty = Convert.ToInt32(id);
            workGroup.ActualQty = Convert.ToInt32(id);
            workGroup.DepartmentId = CreateShop().Id;
            RF.Save(workGroup);

            return workGroup;
        }
    }
}
