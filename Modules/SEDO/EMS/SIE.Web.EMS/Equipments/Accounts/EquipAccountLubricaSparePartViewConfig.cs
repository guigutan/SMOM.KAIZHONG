using SIE.Domain;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.SpareParts;
using SIE.MetaModel.View;
using System.Collections.Generic;

namespace SIE.Web.EMS.Equipments.Accounts
{
    /// <summary>
    /// 设备台账润滑项目备件视图
    /// </summary>
    public class EquipAccountLubricaSparePartViewConfig : WebViewConfig<EquipAccountLubricaSparePart>
    {
        /// <summary>
        /// 查看设备台账润滑项目的备件清单
        /// </summary>
        public const string SeeViewGroup = "SeeView";
        /// <summary>
        /// 显示宽度
        /// </summary>
        private const int displayCoulmnWidth = 20;

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(SeeViewGroup);
            if (ViewGroup == SeeViewGroup) {
                SeeView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save);
            using (View.OrderProperties())
            {
                View.Property(p => p.SparePartId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var item = source as EquipAccountLubricaSparePart;
                    if (item != null)
                    {
                        var list = RT.Service.Resolve<SparePartController>().GetSparePartsToState(pagingInfo, keyword);
                        return list;
                    }
                    else
                    {
                        return new EntityList<SparePart>();
                    }
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.SparePartName), nameof(e.SparePart.SparePartName));
                    dic.Add(nameof(e.Specification), nameof(e.SparePart.Specification));
                    dic.Add(nameof(e.UnitName), nameof(e.SparePart.UnitName));
                    m.DicLinkField = dic;
                }).HasLabel("备件编码").ShowInList(displayCoulmnWidth * 4); 
                View.Property(p => p.SparePartName).HasLabel("备件名称").Readonly().ShowInList(displayCoulmnWidth * 5); 
                View.Property(p => p.Specification).HasLabel("型号规格").Readonly().ShowInList(displayCoulmnWidth * 5); 
                View.Property(p => p.UnitName).Readonly().ShowInList(displayCoulmnWidth * 2);
                View.Property(p => p.Qty).UseSpinEditor(p =>
                {
                    p.AllowNegative = false;
                    p.AllowDecimals = false;
                    p.MinValue = 1;
                }).DefaultValue(1).ShowInList(displayCoulmnWidth * 3); 
                View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            }
        }


        /// <summary>
        /// 查看备件清单
        /// </summary>
        protected void SeeView() {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.SparePartId).HasLabel("备件编码").Readonly().ShowInList(displayCoulmnWidth * 4).Show(ShowInWhere.All) ;
                View.Property(p => p.SparePartName).HasLabel("备件名称").Readonly().ShowInList(displayCoulmnWidth * 5).Show(ShowInWhere.All); 
                View.Property(p => p.Specification).HasLabel("型号规格").Readonly().ShowInList(displayCoulmnWidth * 5).Show(ShowInWhere.All); 
                View.Property(p => p.UnitName).Readonly().ShowInList(displayCoulmnWidth * 2).Show(ShowInWhere.All); 
                View.Property(p => p.Qty).ShowInList(displayCoulmnWidth * 3).Show(ShowInWhere.All);
                View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            }
        }
    }
}
