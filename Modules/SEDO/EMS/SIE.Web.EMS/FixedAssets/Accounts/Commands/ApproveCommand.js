SIE.defineCommand('SIE.Web.EMS.FixedAssets.Accounts.Commands.ApproveCommand', {
    meta: { text: "审核", group: "edit", iconCls: "icon-Check icon-blue" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length != 1) return false;
        var res = true;
        SIE.each(selectModels, function (model) {
            if (model.data.ReviewStatus !== 20) {
                res = false;
                return false;
            }
        });
        return res;
    },
    execute: function (view, source) {        
        var win = SIE.Window.show({
            title: "审核".t(),
            width: 500,
            height: 300,
            buttons: ['通过'.t(), '不通过'.t()],
            items: {
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: [{
                    xtype: 'textareafield',
                    grow: true,
                    id: 'ExaminePurOrder_Opinions',
                    name: 'Opinions',
                    margin: '5',
                    fieldLabel: '审核意见'.t(),                    
                    anchor: '100%'
                }]
            },
            callback: function (btn) {
                var isPass = null;

                if (btn == "通过".t())
                    isPass = true;
                else if (btn == "不通过".t())
                    isPass = false;
                else
                    return true;

                var opinions = Ext.getCmp('ExaminePurOrder_Opinions');
                var remark = opinions.value;
                if (remark == "") {
                    if (btn == "不通过".t()) {
                        SIE.Msg.showMessage("审核意见不允许为空!".t());
                        return false;
                    } else {
                        remark = '通过'.t();
                    }
                }

                var selectModels = view.getSelection();
                var selectIds = view.getSelectionIds(selectModels);

                SIE.invokeDataQuery({
                    type: "SIE.Web.EMS.FixedAssets.Accounts.AccountsDataQueryer",
                    method: "Approve",
                    params: [selectIds[0], remark, isPass],
                    async: false,
                    token: view.token,
                    callback: function (re) {
                        if (re.Success) {
                            win.close();
                            SIE.Msg.showMessage("审核完成!".t());
                            view.reloadData();
                        }
                        else {
                            SIE.Msg.showError(re.Message);
                        }
                    }
                });
                return false;
            }
        });
    }
});