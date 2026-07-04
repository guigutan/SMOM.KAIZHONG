SIE.defineCommand('SIE.Web.EMS.SpareParts.OutDepots.Commands.SendOutDepotDetailCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "发货", group: "edit", iconCls: "icon-Release icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null) {
            return false;
        }
        return view.getSelection().length == 1
            && (view.getSelection()[0].data.OutDepotState == 0 || view.getSelection()[0].data.OutDepotState == 2);
    },
    execute: function (view, source) {

        var entity = view.getCurrent();
        entity.setOutDepotDate(new Date());

        SIE.AutoUI.getMeta({
            async: false,
            ignoreCommands: false,
            isDetail: true,
            ignoreQuery: true,
            model: view.model,
            viewGroup: "SendOutDepotDetailsViewGroup",
            callback: function (meta) {

                var detailView = SIE.AutoUI.generateAggtControl(meta); 
                detailView._view.setData(entity);

                var win = SIE.Window.show({
                    title: '发货-备件出库单'.t(),
                    width: '80%',
                    height: '80%',
                    items: detailView.getControl(),
                    callback: function (btn) {
                        if (btn === "确定".t()) {

                            var dataArr = [];
                            var editEntity = detailView._view.getCurrent();
                            var outDepotChildView = detailView._view.findChild('SIE.EMS.SpareParts.OutDepots.Details.PartOutDepotDetail');
                            var selections = outDepotChildView.getSelection();

                            if (selections.length == 0) {
                                SIE.Msg.showError('请先选择待发货记录！'.t());
                                return false;
                            }

                            for (var i = 0; i < selections.length; i++) {
                                selections[i].data.OutDepotDate = editEntity.data.OutDepotDate;
                                dataArr.push(selections[i].data);
                            }

                            var indata = { Data: Ext.encode(dataArr)};
                            view.execute({
                                data: indata,
                                success: function (res) {
                                    SIE.Msg.showMessage('发货完成'.t());
                                    view.reloadData();
                                    win.close();
                                },
                                error: function (res) {
                                    SIE.Msg.showError(res.Message);
                                }
                            });
                        }
                    }
                });
            }
        });
    }
});