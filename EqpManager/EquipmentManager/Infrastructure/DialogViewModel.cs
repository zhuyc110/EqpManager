using System;
using Prism.Mvvm;

namespace EquipmentManager.Infrastructure
{
    public abstract class DialogViewModel : BindableBase
    {
        public event EventHandler RequestClose;

        #region Protected methods

        protected void OnRequestClose()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}