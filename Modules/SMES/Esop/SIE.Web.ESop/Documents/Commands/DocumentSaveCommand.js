SIE.defineCommand('SIE.Web.ESop.Documents.Commands.DocumentSaveCommand',
    {
        extend: 'SIE.cmd.Save',
        meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
        canExecute: function (view) {
            var result = view.getData().isDirty() || view.getData().dirty;
            return result;
        },
        execute: function (view, source) {

            var me = this;
            me.doSave(view);
        },
        onSaved: function (view, res) {
            var me = this;
            me.onSavedMsg();
            me.view.getCurrent().markSaved();
        },
        onSavedMsg: function (view, res) {

            Ext.Msg.show({
                title: '提示'.t(),
                message: '保存成功'.t(),
                buttons: Ext.MessageBox.OK,
                icon: Ext.Msg.INFO,
                callback: function () {
                    //CRT.Workbench.closeCurrentTab();
                    CRT.Event.fire("SIE.ESop.Documents.DocumentCollection_refresh");
                }
            });
        }
    });