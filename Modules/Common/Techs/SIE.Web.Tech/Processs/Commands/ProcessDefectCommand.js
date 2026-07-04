SIE.defineCommand('SIE.Web.Tech.Processs.Commands.ProcessDefectSelectCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'DefectId', targetClassName: 'SIE.Defects.Defect' },
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    canExecute: function (view) {
        var p = view.getParent();
        if (!p) { return true; }
        var entity = p.getCurrent();
        if (entity === null) {
            return false;
        }

        //工序类型必须为 0 检验；5 终检；30 批次检验；15 装配 时，才可以选择缺陷代码
        return entity.data.CreateBy > 0 && (entity.data.Type === 0 || entity.data.Type === 5 || entity.data.Type === 30 || entity.data.Type === 10 || entity.data.Type === 15);
    },
    save: function (win) {
        var me = this;
        var indata = {};
        var selections = this._targetView.getSelection();
        if (selections && selections.length > 0) {
            var operationDatas = [];
            SIE.each(selections, function (item) {
                var defectId = item.getId();
                if (me._sourceViewSelectItems.indexOf(defectId) === -1) {
                    var defect = { ProcessId: me._sourceId, DefectId: defectId };
                    operationDatas.push(defect);
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