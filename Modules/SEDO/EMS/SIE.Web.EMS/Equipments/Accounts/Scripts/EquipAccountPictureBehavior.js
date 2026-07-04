Ext.define('SIE.Web.EMS.Equipments.Accounts.Scripts.EquipAccountPictureBehavior', {
    onViewReady: function (view) {
        view.mon(view, 'currentChanged', this.currentChanged, view);
    },
    currentChanged: function () {
        if (this.getCurrent() && this.getCurrent() != null) {
            var picture = document.getElementById("equipAccountPicture");
            if (picture && picture != null) {
                var photo = this.getCurrent().getPhoto();
                if (photo && photo !== null) {
                    picture.src = photo;
                } else {
                    picture.src = "";
                }
            }
        }
    }
});