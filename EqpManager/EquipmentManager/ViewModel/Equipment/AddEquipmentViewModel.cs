using System.Windows.Input;
using EquipmentManager.Infrastructure;
using Prism.Commands;

namespace EquipmentManager.ViewModel.Equipment
{
    public class AddEquipmentViewModel : DialogViewModel
    {
        public ICommand ApllyCommand { get; }
        public ICommand CancelCommand { get; }

        public bool Result { get; private set; }

        public string EquipmentId { get; set; }

        public string PackageCode { get; set; }

        public int Left { get; set; }

        public int Top { get; set; }

        public int Height { get; set; }

        public EquipmentStatus Status { get; set; }

        public AddEquipmentViewModel(int left = 0)
        {
            ApllyCommand = new DelegateCommand(ExecuteApply);
            CancelCommand = new DelegateCommand(OnRequestClose);
            Top = 5;
            Left = left;
            Height = 56;
        }

        public EquipmentViewModel ToEquipmentViewModel()
        {
            return new EquipmentViewModel(EquipmentId, Height)
            {
                Left = Left,
                Top = Top,
                PackageCode = PackageCode,
                Status = Status
            };
        }

        #region Private methods

        private void ExecuteApply()
        {
            Result = true;
            OnRequestClose();
        }

        #endregion
    }
}