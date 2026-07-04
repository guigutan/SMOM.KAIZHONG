SIE.defineCommand('SIE.Web.MES.TaskManagement.Reports.Commands.ReportFirstInspCommand', {
    extend: 'SIE.cmd.Command',
    meta: { text: "首件报检", group: "edit", iconCls: "icon-TextRelease icon-blue" },
    canExecute: function (view) {
        return view._parent.getSelection().length == 1 && view._parent._current.data.TaskStatus == 30 && view._parent._current.data.ReportMode == 1
            && !view._parent._current.data.IsVirtualPart && Ext.isEmpty(view._parent._current.data.SpecificationCode);
    },
    /*
   * @override 执行
   * @returns{}
   */
    execute: function (view, source) {
        
        var parentEntity = view._parent.getCurrent();
        SIE.Msg.wait("正在首件报检......".t());
        parentEntity.data.InspQty = 0;
        view.execute({
            data: parentEntity.data,
            success: function (res) {
                if (res.Result == true) {

                    SIE.AutoUI.getMeta({
                        isDetail: true,
                        ignoreQuery: true,
                        model: view._parent.model,
                        viewGroup: "DispatchTaskInspView",
                        callback: function (meta) {

                            var detailView = SIE.AutoUI.generateAggtControl(meta);
                            detailView._view.setData(parentEntity);

                            var win = SIE.Window.show({
                                title: '首件报检'.t(),
                                width: 400,
                                height: 200,
                                items: detailView.getControl(),
                                callback: function (btn) {
                                    if (btn === "确定".t()) {

                                        if (parentEntity.data.InspQty <= 0)
                                        {
                                            SIE.Msg.showError('报检数量必须为正整数！'.t());
                                            return false;
                                        }

                                        view.execute({
                                            data: parentEntity.data,
                                            success: function (res) {
                                                if (res.Result == true) {
                                                    SIE.Msg.showMessage('报检成功！'.t());
                                                    view._parent.reloadData();
                                                    win.close();
                                                }

                                            },
                                            error: function (res) {
                                                SIE.Msg.showError(res.Result);
                                            }
                                        });
                                    }
                                }
                            });
                        }
                    });
                }
                else {
                    SIE.Msg.showError(res.Result);
                }
            }
        });
    },
});