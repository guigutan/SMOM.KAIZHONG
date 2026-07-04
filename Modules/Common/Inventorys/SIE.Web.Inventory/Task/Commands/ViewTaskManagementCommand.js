SIE.defineCommand('SIE.Web.Inventory.Task.Commands.ViewTaskManagementCommand', {
    meta: { text: "查看", group: "edit", iconCls: "icon-TextQuality icon-blue" },
    extend: 'SIE.cmd.Edit',

    canExecute: function (view) {
        if (view.getSelection() == null || view.getCurrent() == null || view.getSelection().length != 1) {
            return false;
        }

        return true;
    },
    edit: function (entity) {
        this.editInForm(entity);
    },
    showView: function (editEntity) {
        var me = this;
        if (!this.viewMeta) {
            SIE.AutoUI.getMeta({
                async: false,
                isDetail: true,
                isReadonly: true,
                ignoreQuery: true,
                model: this.view.model,
                viewGroup: "DetailsView",
                callback: function (meta) {
                    meta.token = me.view.token;
                    me.viewMeta = meta;
                }
            });
        }
        var cfg = {
            associateCmd: me,
            viewMeta: me.viewMeta,
            entity: editEntity,
            editMode: this.view.editMode,
            title: this.getEditViewTitle(editEntity),
        };
        SIE.App.showView(cfg);
        listView = this.view;
    },
});