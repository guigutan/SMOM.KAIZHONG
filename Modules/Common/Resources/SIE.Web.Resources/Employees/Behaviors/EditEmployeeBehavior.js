Ext.define('SIE.Web.Resources.Employees.Behaviors.EditEmployeeBehavior',
    {
        editEmployeeView: null,
        controller: null,


        /**
         * view生命周期函数--view聚合后
         * @param {*} view 生成的view
         */
        onViewReady: function (view) {
            //code here
            var me = this;
            var entity = view.getCurrent();

            me.editEmployeeView = view;

            if (entity.getUserId())
                view.getControl().getForm().findField("UserId").setReadOnly(true);
            else
                view.getControl().getForm().findField("UserId").setReadOnly(false);

           

        },
    });
