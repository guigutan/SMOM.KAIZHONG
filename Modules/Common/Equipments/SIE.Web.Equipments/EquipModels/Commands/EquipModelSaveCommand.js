SIE.defineCommand('SIE.Web.Equipments.EquipModels.Commands.EquipModelSaveCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },

    execute: function (view, source) {
        var me = this;

        var current = view.getCurrent();
        var err = false;
        var errMsg = "";

        //if (Ext.isEmpty(current.getCode())) {
        //    errMsg += '【型号编码】';
        //    err = true;
        //}

        //if (Ext.isEmpty(current.getName())) {
        //    errMsg += '【型号名称】';
        //    err = true;
        //}

        //if (Ext.isEmpty(current.getTypeName())) {
        //    errMsg += '【设备类型编码】';
        //    err = true;
        //}

        if (err) {
            SIE.Msg.showInstantMessage(errMsg + '不能为空!'.t());
            return;
        }

        if (this.isExecuting) return;

        this.isExecuting = true;

        setTimeout(function () {
            me.isExecuting = false;
        }, 100, me);

        var isValidator = this.onSaving(view);

        if (isValidator)
            this.doSave(view);
        else
            SIE.Msg.showMessage("数据验证不通过，保存失败！".t())
    },
});