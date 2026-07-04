SIE.defineCommand('SIE.Web.Andon.Andons.Commands.AndonManageFormSaveCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    doSave: function (view) {
        var me = this;
        var entity = view.getCurrent();
        if (entity.getAndonManageClass() == 20) {
            if (entity.getEquipAccountId() == null) {
                SIE.Msg.showError('安灯大类为【机】时,设备编码不能为空'.t());
                return false;
            }
        }
        var itemDetail = entity.belongsView.findChild("SIE.Andon.Andons.AndonManageCallMaterial").getData().data.items;
        var andonManageCallMaterials = [];
        var wareEmpty = false;
        var repeat = false;
        var dic = [];
        if (entity.getAskMaterial() && itemDetail.length == 0) {
            SIE.Msg.showError('是否叫料勾选时，物料信息要有数据！'.t());
            return false;
        }
        itemDetail.forEach(function (item) {
            if (dic.indexOf(item.data.ItemId) > -1) {
                repeat = true;
            }
            else {
                dic.push(item.data.ItemId);
            }
            if (item.getConsumeType() == 0 && item.getWareHouseId() == 0) {
                wareEmpty = true;
            }
            andonManageCallMaterials.push(item.data);
        });
        if (repeat) {
            SIE.Msg.showError('添加物料唯一，不可重复！'.t());
            return false;
        }
        if (wareEmpty) {
            SIE.Msg.showError('物料消耗类型为【拉式物料】时,备料接收仓库不能为空！'.t());
            return false;
        }
        var indata = {};
        indata.data = Ext.encode({ AndonManage: entity.data, AndonManageCallMaterials: andonManageCallMaterials });
        var createStockMsg = '';
        if (andonManageCallMaterials.length != 0) {
            createStockMsg = '并生成备料单';
        }
        Ext.MessageBox.show({
            msg: '正在保存数据'.t()+ createStockMsg.t(),
            progressText: '...',
            width: 300,
            wait: {
                interval: 200
            }
        });
        view.execute({
            data: indata,
            success: function (res) {
                SIE.Msg.showInstantMessage('保存成功'.t());
                var current = view.getCurrent();
                current.markSaved();

                CRT.Event.fire(view.model + '_refresh', view.getCurrent().getId());
                CRT.Event.fire(view.model + '_' + view.getCurrent().getId() + '_refresh', view.getCurrent().getId());

                CRT.Workbench.closeCurrentTab();
            }
        });
    },
});
