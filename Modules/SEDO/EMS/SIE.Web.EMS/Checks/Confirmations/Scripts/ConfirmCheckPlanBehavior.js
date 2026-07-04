Ext.define('SIE.Web.EMS.Checks.Confirmations.Scripts.ConfirmCheckPlanBehavior',
    {
        /**
         * view生命周期函数--view生成前
         * @param {*} meta 实体视图元数据
         * @param {*} curEntity 当前操作实体(可空)
         */
        beforeCreate: function (meta, curEntity) {
            //...
        },

        /**
        * view生命周期函数--view生成后
        * @param {*} view 生成的view
        */
        onCreated: function (view) {
            var entity = CRT.Context.PageContext.getCurrentRecord();
            var params = CRT.Context.PageContext.getParams();

            //如果已经确认过，则置灰。
            if (params.ConfirmResult !== null) { view.setIsReadonly(true); }

            if (params) {
                entity.setConfirmResult(params.ConfirmResult);
                entity.setConfirmNote(params.ConfirmNote);
                entity.setExeState(params.CheckExeState);
                entity.setConfirmDeptId(params.DepartmentId);
                entity.setConfirmDeptDisplay(params.DepartmentName);
            }

            // 如果状态为已确认和已评分，只读
            if (entity.getExeState() == 3 || entity.getExeState() == 7) {
                entity.markSaved();
            }
        },

        /**
        * view生命周期函数--view准备完成
        * @param {ListLogicView} view 生成的view
        */
        onViewReady: function (view) {
            var me = this;
            var current = view.getCurrent();
            view.mon(current, "propertyChanged", me.onPropertyChanged, view);

            new Promise(function (resolve, reject) {
                SIE.invokeDataQuery({
                    method: 'IsNeedMarkScore',
                    params: [],
                    action: 'queryer',
                    type: 'SIE.Web.EMS.Checks.Confirmations.DataQuery.CheckConfirmationQueryer',
                    token: view.token,
                    success: function (res) {
                        if (res.Success) {
                            resolve(res.Result)
                        } else {
                            reject(res.Message);
                        }
                    }
                })
            }).then(function (rs) { rs === false ? me.hideTab(view) : null; }).catch(function (err) { console.log(err); });
            // 隐藏评分项页签,否则加载数据
            

        },
        hideTab: function (view) {
            view.findChild("SIE.EMS.Checks.Confirmations.CheckConfirmation").getControl().ownerLayout.owner.tab.hide();
            view.findChild("SIE.EMS.Checks.Projects.CheckProject").getControl().up("tabpanel").setActiveTab(1)
        },
        /**
         * view生命周期函数--数据加载后
         * @param {any} view 逻辑视图
         */
        onDataLoaded: function (view) {
            var me = this;
        },

        /**
        * 属性变更处理
        * */
        onPropertyChanged: function (e) {
            var me = this;
            var entity = e.entity;
        },
    });
