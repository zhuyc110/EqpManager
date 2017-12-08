using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using EquipmentManager.ViewModel.Equipment;
using Prism.Commands;
using Prism.Mvvm;

namespace EquipmentManager.ViewModel
{
    [Export(typeof(MainViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MainViewModel : BindableBase
    {
        public ObservableCollection<EquipmentViewModel> Equipments { get; }

        public EquipmentViewModel SelectedEquipment
        {
            get => _selectedEquipment;
            set => SetProperty(ref _selectedEquipment, value);
        }

        public ICommand SelectCommand { get; }
        public ICommand AddMockCommand { get; }

        public string GoalEquipmentId
        {
            get => _goalEquipmentId;
            set => SetProperty(ref _goalEquipmentId, value);
        }

        public MainViewModel()
        {
            SelectCommand = new DelegateCommand(ExecuteSelect);
            AddMockCommand = new DelegateCommand(ExecuteAddMock);
            Equipments = new ObservableCollection<EquipmentViewModel>(_testItems);
        }

        #region Private methods

        private void ExecuteSelect()
        {
            var goal = Equipments.FirstOrDefault(x => x.EquipmentId.Equals(GoalEquipmentId, StringComparison.CurrentCultureIgnoreCase));
            if (goal != null)
            {
                SelectedEquipment = goal;
            }
        }

        private void ExecuteAddMock()
        {
            var random = new Random(DateTime.Now.Millisecond);
            var equipmentName = "WR00" + random.Next(10);
            Equipments.Add(new EquipmentViewModel(equipmentName, random.Next(50, 100))
            {
                PackageCode = "4KG",
                Status = (EquipmentStatus)(random.Next(10)/4)
            });
        }

        #endregion

        #region Fields

        private EquipmentViewModel _selectedEquipment;
        private string _goalEquipmentId;

        private readonly List<EquipmentViewModel> _testItems = new List<EquipmentViewModel>
        {
            new EquipmentViewModel("WR001")
            {
                PackageCode = "4KG",
                Status = EquipmentStatus.Running
            },
            new EquipmentViewModel("WR002",100)
            {
                PackageCode = "2KG",
                Status = EquipmentStatus.OffLine
            },
        };

        #endregion
    }
}