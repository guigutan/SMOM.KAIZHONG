SIE.defineCommand('SIE.Web.MES.TaskManagement.Dispatchs.GenerateTaskBIllCommand', {
    extend: 'SIE.cmd.Command',
    executeIntervalMode: SIE.cmd.IntervalMode.Debounce.value,
    meta: { text: "生成任务单", group: "business", iconCls: "icon-ClipboardPaperCheck icon-blue" },
    canExecute: function (listView) {
        if (listView == null || listView.getCurrent() == null || listView.getSelection().length > 1) return false;
        var entity = listView.getCurrent();
        var eData = entity.data;
        return eData.IsPause == 0 && eData.State == 0;
    },
    /*business
   * @override 执行
   * @returns{}
   */
    execute: function (view, source) {
        var me = this;
        var current = view.getCurrent();
        var wo = current.data;
        var data = {};
        data.IsAccordConfig = true;
        data.WorkOrder = wo;

        if (wo.IsCommonMode && wo.IsMainMaterial) {
            SIE.invokeDataQuery({
                type: "SIE.Web.MES.TaskManagement.Dispatchs.DispatchDataQueryer",
                method: "IsCanSyntypeReport",
                params: [wo],
                async: false,
                token: view.token,
                callback: function (res) {
                    if (res.Success) {
                        var billInfo = res.Result;
                        if (billInfo.ErrMsg !== '') {
                            SIE.Msg.showError(billInfo.ErrMsg);
                            return;
                        }
                        else {
                            if (billInfo.OrgIsSyntype === true && billInfo.IsSyntype === false) {
                                SIE.Msg.askQuestion(Ext.String.format('共模主料工单的辅料工单已存在生成的任务单，如【确定】则生成不按共模比的任务单，否【取消】则不生成任务单？请确定！'.t()),
                                    function () {
                                        data.IsAccordConfig = false;
                                        me.generateBills(view, data);
                                    });
                            }
                            else
                                me.generateBills(view, data);
                        }
                    }
                }
            });
        }
        else
            me.generateBills(view, data);
    },

    /**
     * generateBills 生成任务单
     * @method generateBills
     * @param {view} view 当前视图
     * @param {data} data 当前数据
     */
    generateBills: function (view, data) {
        var indata = {};
        indata.Data = Ext.encode(data);
        SIE.Msg.wait("正在生成任务单......".t());
        view.execute({
            data: indata,
            success: function (res) {
                if (res.Result == true) {
                    SIE.Msg.showMessage("生成成功!".t());
                    if (view.getChildren().length <= 0) return;
                    var child = view.getChildren().first(function (m) { return m.config.model === 'SIE.MES.TaskManagement.Dispatchs.DispatchTask' });
                    if (child) child.reloadData();
                }
                else {
                    SIE.Msg.showMessage(res.Result);
                }
            }
        });
    },
});