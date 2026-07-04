SIE.defineCommand('SIE.Web.Tech.Processs.Commands.ProcessPackingUnitCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'PackageUnitId', targetClassName: 'SIE.Packages.Packages.PackingUnit' },
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    canExecute: function (view) {
        var p = view.getParent();
        if (!p) { return true; }
        var entity = p.getCurrent();
        if (entity === null) {
            return false;
        };
        return entity.data.CreateBy > 0 && (entity.data.Type === 20 || entity.data.Type === 40);
    },
    save: function (win) {
        var me = this;
        var indata = {};
        var selections = this._targetView.getSelection();
        if (selections && selections.length > 0) {
            var operationDatas = [];
            SIE.each(selections, function (item) {
                var packageUnitId = item.getId();
                if (me._sourceViewSelectItems.indexOf(packageUnitId) === -1) {
                    var PackageUnit = { ProcessId: me._sourceId, PackageUnitId: packageUnitId };
                    operationDatas.push(PackageUnit);
                }
            });
            indata = operationDatas;
            me._targetView.execute({
                data: indata,
                success: function (res) {
                    win.close();
                    me._ownerView.loadChildData(true);
                }
            }, me._ownerView);
        }
        else {
            SIE.Msg.showWarning('没有可提交的数据'.t());
        }
    },
});