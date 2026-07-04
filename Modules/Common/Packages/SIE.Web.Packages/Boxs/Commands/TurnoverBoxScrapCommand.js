SIE.defineCommand('SIE.Web.Packages.Boxs.Commands.TurnoverBoxScrapCommand', {
    meta: { text: "报废", group: "edit" },

    canExecute: function (view) {
        if (view.getSelection() === null || view.getSelection().length === 0) {
            return false;
        }

        var sel = view.getSelection();
        for (i = 0; i < sel.length; i++) {
            var item = sel[i].data;
            if (item.State === 0 || item.State === 3) {
                return false;
            }
        }

        return true;
    },
    execute: function (view, source) {
        var boxs = this.view.getSelection();
        boxs.forEach(function (box) {
            box.setState(3);
            box.dirty = true;
        });
        view.syncCmdState(view, true);
    }
});