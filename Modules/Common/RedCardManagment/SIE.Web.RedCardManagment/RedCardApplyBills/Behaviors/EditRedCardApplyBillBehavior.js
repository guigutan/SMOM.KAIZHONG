Ext.define('SIE.Web.RedCardManagment.RedCardApplyBills.Behaviors.EditRedCardApplyBillBehavior',
    {
        view: null,

        beforeCreate: function (meta, curEntity) {
            me = this;
            if (!meta) {
                return;
            }
            var formConfig = Array.from(meta.formConfig.items);
            var problemFormConfig = formConfig.find(function (item) { return item.title == "问题描述" });
            if (problemFormConfig) {
                problemFieldConfig = problemFormConfig.items.find(function (item) { return item.fieldLabel == "问题描述" });
                problemFieldConfig.height = 150;
            }
        },

        /**
         * view生命周期函数--view聚合后
         * @param {*} view 生成的view
         */
        onViewReady: function (view) {
            //code here
            this.view = view;
            var entity = view.getCurrent();
            if (this.view.viewGroup == "DetailsView") {
                var params = CRT.Context.PageContext.getParams();
                if (params && params.IsNew) {
                    entity.data.No = params.No;
                    entity.data.ApplySource = params.ApplySource;
                    entity.data.ApplyType = params.ApplyType;
                }
            }
        }
    });