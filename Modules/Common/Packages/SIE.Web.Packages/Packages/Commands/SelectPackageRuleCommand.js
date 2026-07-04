SIE.defineCommand('SIE.Web.Packages.Packages.Commands.SelectPackageRuleCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'Id', targetClassName: 'SIE.Packages.PackageRule' },
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    save: function (win) {
        /// <summary>
        /// 保存选择的操作列表。
        /// </summary>
        var me = this;
        /* post数据结构*/
        var indata = {};
        /* post数据结构*/
        var selections = this._targetSelectItems.items;
        if (selections && selections.length > 0) {
            var operationDatas = [];
            SIE.each(selections, function (item) {
                var packageRuleId = item.getId();
                if (me._sourceViewSelectItems.indexOf(packageRuleId) === -1) {
                    var packageRuleList = { ItemId: me._sourceId, PackageRuleId: packageRuleId };
                    operationDatas.push(packageRuleList);
                }
            });
            var signdata = {
                command: me.meta.command,
                entityType: me.view.model,
                parentType: me.view.getParent() ? me.view.getParent().model : ""
            }
            indata = operationDatas;
            SIE.invokeDataQuery({
                method: 'SetItemPackageRule',
                action: 'queryer',
                params: [indata],
                logInfo: signdata,
                type: 'SIE.Web.Packages.Packages.PackageRuleDataQuery',
                token: me._ownerView.getToken(),
                success: function (res) {
                    me._ownerView.loadChildData(true);
                    win.close();
                }
            });
        }
        else {
            SIE.Msg.showWarning('没有可提交的数据'.t());
        }
    }
    // end 
});