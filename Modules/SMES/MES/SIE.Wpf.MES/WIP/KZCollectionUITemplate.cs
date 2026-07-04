using DevExpress.Xpf.Core;
using Org.BouncyCastle.Asn1.Mozilla;
using SIE.Domain;
using SIE.MES.WIP.TaskExtensions;
using SIE.MetaModel;
using SIE.MetaModel.View;
using SIE.Wpf.MES.Controls.Messager;
using SIE.Wpf.MES.WIP.Inspects;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.MES.WIP
{
    /// <summary>
    /// KZ采集UI模板
    /// </summary>
    public class KZCollectionUITemplate : CollectionUITemplate
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        public KZCollectionUITemplate(Type entityType) : base(entityType)
        {
            ViewGroup = CollectionUIViewGroup;
        }

        /// <summary>
        /// 工作单元视图
        /// </summary>
        public DetailLogicalView kZWorkstationDetailLogicalView { get; set; }

        /// <summary>
        /// 创建操作控件(KZ)
        /// </summary>
        /// <param name="workstation"></param>
        /// <returns></returns>
        protected virtual FrameworkElement CreateOperationControl(KZWorkstation workstation)
        {
            var view = AutoUI.ViewFactory.CreateDetailView(typeof(KZWorkstation));
            view.Data = workstation;
            view.Current = workstation;
            //view.Control.Margin = new Thickness(5, -10, 5, -10);
            DockPanel.SetDock(view.Control, Dock.Bottom); //将工作站信息置底部\
            kZWorkstationDetailLogicalView = view;
            return view.Control;
        }
    }
}