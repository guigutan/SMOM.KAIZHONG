SIE.defineCommand('SIE.Web.AbnormalInfo.AbnormalInfos.Commands.AbnormalInfoDefHandlerSelectCommand',
    {
        extend: 'SIE.cmd.LookupCommandBase',
        userConfig: {
            dataParams: {
                specKeyPrototyName: 'HandlerId',
                targetClassName: 'SIE.Resources.Employees.Employee'
            }
        },
        meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
        /**
         * override 保存方法
         * @param {} win 
         * @returns {} 
         */
        save: function (win) {
            var me = this;
            var indata = {};
            var selections = this._targetView.getSelection();
            if (selections && selections.length > 0) {
                var operationDatas = [];
                SIE.each(selections,
                    function (item) {
                        var userId = item.getId();
                        if (me._sourceViewSelectItems.indexOf(userId) === -1) {
                            var defHandlers = {
                                AbnormalInfoDefinitionId: me._sourceId,
                                HandlerId: userId
                            };
                            operationDatas.push(defHandlers);
                        }

                    });
                indata = operationDatas;
                me._targetView.execute({
                    data: indata,
                    success: function (res) {
                        win.close(); //关闭模态窗口
                        me._ownerView.loadChildData(true); //重载视图数据
                    }
                },
                    me._ownerView);
            } else {
                SIE.Msg.showWarning('没有可提交的数据'.t());
            }
        }

    });

