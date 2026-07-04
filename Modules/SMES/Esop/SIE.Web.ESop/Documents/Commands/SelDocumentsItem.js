SIE.defineCommand('SIE.Web.ESop.Documents.Commands.SelDocumentsItem', {
    extend: 'SIE.Web.ESop.Common.Commands.SelCheckProjectCommand',
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    /**
         * canExecute 是否执行
         * @param {} view 当前视图
         * @returns {}
         */
    canExecute: function (view) {

        return view.getParent().getCurrent() != null &&!view.getParent().getCurrent().isNew();//&& view.getParent().getCurrent().getFilePath() != "";
    },
});

