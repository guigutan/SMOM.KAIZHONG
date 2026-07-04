SIE.defineCommand('SIE.Web.RedCardManagment.RedCards.Commands.ItemSnEnableRedCardCommand', {    meta: { text: "启用", group: "edit", iconCls: "icon-Play icon-red" },    executeIntervalMode: SIE.cmd.IntervalMode.Debounce.value,//使用防抖模式    canExecute: function (view) {        let isShow = true        var isEnableCount = 0;        if (view.getSelection().length > 0) {            for (const element of view.getSelection()) {
                if (element.getStatus() == SIE.RedCardManagment.RedCards.RedCardState.Enable) {
                    //isShow = false
                    isEnableCount++
                }
            }        } else {
            isShow = false
        }        if (view.getSelection().length == isEnableCount) {
            isShow = false
        }        return isShow;    },    execute: function (view) {        SIE.Msg.askQuestion('是否启用红牌'.t(), function () {            SIE.Msg.wait("数据处理中...".t());            var data = {}            var ids = [];            if (view.getSelection().length > 0) {                for (const element of view.getSelection()) {
                    ids.push(element.getId())
                }                data.Data = Ext.encode(ids)                view.execute({                    data: data,                    success: function (res) {                        if (res.Result) {                            SIE.Msg.hide();                            SIE.Msg.showInstantMessage("启用成功".t());                            view.getParent().reloadData()                            view.reloadData();                        } else {                            SIE.Msg.hide();                            SIE.Msg.showError("启用失败".t());                        }                    },                    error: function (res) {                        SIE.Msg.hide();                        SIE.Msg.showError("启用失败".t());                    },                });            }            //SIE.Msg.hide();        });    },}); 