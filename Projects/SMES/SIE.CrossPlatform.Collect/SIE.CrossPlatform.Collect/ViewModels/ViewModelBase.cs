using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.CrossPlatform.Collect.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        private bool _isBusy;

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _isBusy, value);
            }
        }
    }
}
