using System;
using Prism.Mvvm;

namespace EquipmentManager.ViewModel.Equipment
{
    public class EquipmentViewModel : BindableBase
    {
        public string EquipmentId { get; }

        public string PackageCode
        {
            get => _packageCode;
            set => SetProperty(ref _packageCode, value);
        }

        public int Height { get; }

        public string StatusChangedTime => _statusChangedTime.ToShortTimeString();

        public EquipmentStatus Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public EquipmentViewModel(string equipmentId, int height = 56)
        {
            EquipmentId = equipmentId;
            _statusChangedTime = DateTime.Now;
            Height = height;
        }

        #region Fields

        private DateTime _statusChangedTime;

        private EquipmentStatus _status;

        private string _packageCode;

        #endregion
    }
}