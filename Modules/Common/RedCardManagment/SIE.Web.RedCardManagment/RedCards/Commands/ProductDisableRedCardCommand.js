SIE.defineCommand('SIE.Web.RedCardManagment.RedCards.Commands.ProductDisableRedCardCommand', {    meta: { text: "禁用", group: "edit", iconCls: "icon-Cancel icon-green" },    executeIntervalMode: SIE.cmd.IntervalMode.Debounce.value,//使用防抖模式    canExecute: function (view) {        let isShow = true        var isDisableCount = 0;        if (view.getSelection().length > 0) {            for (const element of view.getSelection()) {
                if (element.getStatus() == SIE.RedCardManagment.RedCards.RedCardState.Disable) {
                    //isShow = false
                    isDisableCount++;
                }
            }        } else {
            isShow = false
        }        if (view.getSelection().length == isDisableCount) {
            isShow = false
        }        return isShow;    },    execute: function (view) {        SIE.Msg.askQuestion('是否禁用红牌'.t(), function () {            SIE.Msg.wait("数据处理中...".t());            var data = {}            var ids = [];            if (view.getSelection().length > 0) {                for (const element of view.getSelection()) {                    ids.push(element.getId())                }                data.Data = Ext.encode(ids)                view.execute({                    data: data,                    success: function (res) {                        if (res.Result) {
                            SIE.Msg.hide();
                            view.reloadData();                            SIE.Msg.showInstantMessage("禁用成功".t());
                        } else {                            SIE.Msg.hide();
                            SIE.Msg.showError("禁用失败".t());
                        }                    },                    error: function (res) {
                        SIE.Msg.hide();
                        SIE.Msg.showError("禁用失败".t());

                    },                });            }
            //SIE.Msg.hide();
        });            },}); 