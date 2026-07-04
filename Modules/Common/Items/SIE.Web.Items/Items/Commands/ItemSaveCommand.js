SIE.defineCommand('SIE.Web.Items.Items.Commands.ItemSaveCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    _viewReload: function (view) {
        var store = view.getData();
        store.load({
            url: "/api/DataPortal/Query",
            callback: function (records, operation, success) {
                if (!success && operation.error.status === 401) {
                    SIE.App.popupLogin(function () {

                    });
                    return;
                }
                store._loaded = success;
            }
        });
    },
});