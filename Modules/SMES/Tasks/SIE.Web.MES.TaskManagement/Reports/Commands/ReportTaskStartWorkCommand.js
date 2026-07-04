SIE.defineCommand('SIE.Web.MES.TaskManagement.Reports.ReportTaskStartWorkCommand', {
    extend: 'SIE.cmd.Command',
    meta: { text: "开工", group: "edit", iconCls: "icon-Play icon-blue" },
    executeIntervalMode: SIE.cmd.IntervalMode.Debounce.value,
    canExecute: function (listView) {
        if (listView == null || listView.getCurrent() == null || listView.getSelection().length > 1) return false;
        var entity = listView.getCurrent();
        var eData = entity.data;
        //已派工状态且手动报工  未派工 派工中 已派工 
        return eData.ReportMode == 1 && (eData.TaskStatus === 20 || eData.TaskStatus == 0 || eData.TaskStatus==10)
    },
    /*
   * @override 执行
   * @returns{}
   */
    execute: function (view, source) {

        var current = view.getCurrent();
        var data = current.data;
        if (data.TaskStatus == 0 || data.TaskStatus == 10) {
            SIE.Msg.askQuestion("该任务单未派工/派工中，是否派工给当前用户并开工？".t(),
                function () {
                    //提交时，数据设置为脏，重新保存并校验所有内容。
                    SIE.Msg.wait("正在开工......".t());
                    view.execute({
                        data: current.data,
                        success: function (res) {
                            if (res.Result == true) {
                                SIE.Msg.showMessage("操作成功!".t());
                                view.reloadData();
                            }
                            else {
                                SIE.Msg.showMessage(res.Result);
                            }
                        }
                    });
                });
        } else if (data.TaskStatus ==20) {
            //提交时，数据设置为脏，重新保存并校验所有内容。
            SIE.Msg.wait("正在开工......".t());
            view.execute({
                data: current.data,
                success: function (res) {
                    if (res.Result == true) {
                        SIE.Msg.showMessage("操作成功!".t());
                        view.reloadData();
                    }
                    else {
                        SIE.Msg.showMessage(res.Result);
                    }
                }
            });
        }

        
    },
});