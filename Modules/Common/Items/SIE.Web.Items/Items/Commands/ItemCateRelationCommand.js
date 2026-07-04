SIE.defineCommand('SIE.Web.Items.Items.Commands.ItemCateRelationCommand', {
    meta: { text: "分类维护", group: "edit" },
    model : 'SIE.Web.Items.Items.ViewModels.ItemCategoryRelationViewModel',
    canExecute: function (listView) {
        return true;
    },
    execute: function (listView, source) {
        var me = this;
        SIE.AutoUI.getMeta({
            model: me.model,
            viewName: '',
            isAggt: true,
            callback: function (meta) {
                //meta.model = 'SIE.Web.Items.Items.ViewModels.ItemCategoryRelationViewModel';
                //var ui = SIE.App._createAggtControl(meta, true);
                //var control = ui.getControl();
                //var tabPanel = Ext.getCmp('centerTab');
                //var tab = {
                //    xtype: 'panel',
                //    border: 0,
                //    title: '分类维护',
                //    layout: 'fit',
                //    margin: 3,
                //    closable: true,
                //    autoScroll: true
                //};
                //tab.items = Ext.widget('container', {
                //    border: 0,
                //    layout: 'fit',
                //    autoScroll: true
                //});
                //tab.items.add(control);
                //tab = tabPanel.add(tab);
                //tabPanel.setActiveTab(tab);
                
                var view = SIE.AutoUI.createListView(meta);
                SIE.Window.show({
                    title: '分类维护'.t(),
                    width: 400,
                    items: view.getControl(),
                    callback: function (btn) {
                        var res = btn;
                    }
                });
            }
        });
    }
});