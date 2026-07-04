using SIE.Andon.Andons;
using SIE.Andon.Andons.Enum;
using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Web.Andon.Andons.Commands;
using SIE.Web.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.Andons
{
    /// <summary>
    /// 安灯维护视图配置
    /// </summary>
    public class AndonViewConfig : WebViewConfig<SIE.Andon.Andons.Andon>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(AndonAddCommand).FullName, "SIE.Web.Andon.Andons.Commands.AndonEditCommand", WebCommandNames.Delete, typeof(AndonSaveCommand).FullName);
            View.Property(p => p.AndonCode);
            View.Property(p => p.AndonName);
            View.Property(p => p.Desc);
            View.Property(p => p.AndonType).UseDataSource((s, p, k) =>
            {
                var source = s as SIE.Andon.Andons.Andon;
                if (source != null)
                {
                    return RT.Service.Resolve<AndonController>().GetEnableAndonType(p, k);
                }
                else
                {
                    return new EntityList<AndonType>();
                }
            });
            View.Property(p => p.AndonClass).Readonly();
            View.Property(p => p.Solution);
            View.Property(p => p.Priority)
                .UseCatalogEditor(p =>
                {
                    p.CatalogType = SIE.Andon.Andons.Andon.PriorityCatalogType; p.CatalogReloadData = true;
                }).UseListSetting(p => p.HelpInfo = "“来源快码ANDON_PRIORITY");
            View.Property(p => p.SuspectAndon);
            //View.Property(p => p.OrderNo).UseSpinEditor(p => p.MinValue = 1);
            View.Property(p => p.State).Readonly();
            View.Property(p => p.RepeatTrigger).ShowInList(width: 150).DefaultValue(false).UseListSetting(e => e.HelpInfo = "该安灯触发且未关闭或取消时，相同【安灯名称+工厂+车间+产线+工位+设备+班组】不允许再触发该安灯");
            View.Property(p => p.LineStop).DefaultValue(AndonYesOrNo.No);
            View.Property(p => p.DefaultType).UseListSetting(p => p.HelpInfo = "来源于[快码]功能中的异常类型(ABNORMAL_TYPE)").UseCatalogEditor(p => { p.CatalogReloadData = true; p.CatalogType = SIE.Equipments.Abnormal.AbnormalCause.AbnormalTypeCatalog; });
            View.Property(p => p.AskMaterial).DefaultValue(AndonYesOrNo.No);
            View.Property(p => p.Department).UseDataSource((e, p, k) =>
            {
                var source = e as SIE.Andon.Andons.Andon;
                if (source != null)
                {
                    return RT.Service.Resolve<AndonController>().GetOrganizations(p, k);
                }
                else
                {
                    return new EntityList<SIE.Andon.Andons.Andon>();
                }
            });
            View.Property(p => p.Charger);
            //View.Property(p => p.PushPlug);
            //View.Property(p => p.MessageTemplate).UseTextButtonFieldEditor(p => { p.ExtendJsObj = "SIE.Web.Andon.Andons.Scripts.AndonTypeMessageTemplateEditor"; p.Editable = false; });
            View.ChildrenProperty(p => p.MessageSendList).HasLabel("消息推送&升级机制").HasOrderNo(0);
            View.ChildrenProperty(p => p.AndonSespList).HasLabel("安灯清单").HasOrderNo(1).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.AndonPrepareProjectDetailList).HasOrderNo(2).Show(ChildShowInWhere.List);
            View.ChildrenProperty(p => p.AndonResponseDetailList).Show(ChildShowInWhere.All).HasOrderNo(5);
            View.ChildrenProperty(p => p.GeneralProbDtlList).Show(ChildShowInWhere.All).HasOrderNo(10);
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.AndonCode);
            View.Property(p => p.AndonName);
            View.Property(p => p.AndonType);
            View.Property(p => p.AndonClass);
            View.Property(p => p.State);
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
        }

        /// <summary>
        /// 下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.AndonCode);
            View.Property(p => p.AndonName);
        }
    }
}
