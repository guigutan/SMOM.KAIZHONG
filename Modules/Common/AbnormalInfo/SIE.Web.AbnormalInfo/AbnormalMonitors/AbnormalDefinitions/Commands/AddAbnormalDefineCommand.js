SIE.defineCommand('SIE.Web.AbnormalInfo.AbnormalMonitors.AbnormalDefinitions.Commands.AddAbnormalDefineCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },

    execute: function (view, source) {
        var me = this;
        SIE.invokeDataQuery({
            type: "SIE.Web.AbnormalInfo.AbnormalMonitors.AbnormalDefinitions.DataQuerys.AbnormalDefineDataQuery",
            method: "GenerateCode",
            params: [],
            token: me.view.getToken(),
            async: false,
            callback: function callback(res) {
                if (res.Success) {
                    var editEntity = me.getEditEntity();
                    editEntity.setCode(res.Result);
                    me.onEditting(editEntity);
                    me.edit(editEntity);
                    me.onEdited(editEntity);
                }
            }
        });
    },

    onItemCreated: function (entity) {
        var me = this;
        var view = this.view;
        view.fireEvent("newentityadded", entity);
        me.callParent(arguments);

    },
});