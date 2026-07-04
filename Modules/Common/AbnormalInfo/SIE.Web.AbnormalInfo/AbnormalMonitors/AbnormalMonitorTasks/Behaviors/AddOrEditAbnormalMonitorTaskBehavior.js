Ext.define('SIE.Web.AbnormalInfo.AbnormalMonitors.Behaviors.AddOrEditAbnormalMonitorTaskBehavior',
    {
        view: null,
        beforeCreate: function (meta) {
            if (!meta || !meta.formConfig) return;
            var columns = meta.formConfig.items;
            if (Ext.isEmpty(columns)) return;
            var column = columns.first(function (c) { return c.name == "ProblemDescription"; });
            if (Ext.isEmpty(column)) return;
            //delete column.maxWidth;
            //column.height = 200;
        },
        onViewReady: function (view) {
            this.view = view;
            var entity = view.getCurrent();
            var params = CRT.Context.PageContext.getParams();
            if (params) {
                if (params.IsAdd) {
                    entity.data.Code = params.Code;
                    entity.data.TaskType = params.TaskType;
                    entity.data.TaskState = params.TaskState;
                }
                else {

                }

            }
            view.mon(view.getCurrent(), "propertyChanged", this._onEntityPropertyChanged, view);
        },

        //属性变更处理
        _onEntityPropertyChanged: function (e) {
            if (e.property.length > 0 && e.property === 'WorkShopId') {
                e.entity.setLineId(null)
            }
        }
    });