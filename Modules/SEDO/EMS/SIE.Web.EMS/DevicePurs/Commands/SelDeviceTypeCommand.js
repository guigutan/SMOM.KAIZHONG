SIE.defineCommand('SIE.Web.EMS.DevicePurs.Commands.SelDeviceTypeCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'EquipTypeId', targetClassName: 'SIE.Equipments.EquipTypes.EquipType' },
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    save: function (win) {
        var me = this;
        var indata = {};
        var selections = me._targetSelectItems;
        if (selections && selections.items.length > 0) {
            var operationDatas = [];
            SIE.each(selections.items, function (item) {
                var itemId = item.getId();
                if (me._sourceViewSelectItems.indexOf(itemId) === -1) {
                    var deviceType = { DevicePurId: me._sourceId, EquipTypeId: itemId };
                    operationDatas.push(deviceType);
                }
            });
            if (operationDatas.length > 0) {
                indata = operationDatas;
                me._targetView.execute({
                    data: indata,
                    success: function (res) {
                        win.close();
                        me._ownerView.loadChildData(true);
                    }
                }, me._ownerView);
                return true;
            }
        }
        else {
            SIE.Msg.showWarning('没有可提交的数据'.t());
        }
    }
});