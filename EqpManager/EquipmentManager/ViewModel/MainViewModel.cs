using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using EquipmentManager.Infrastructure;
using EquipmentManager.Interact;
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
        public ICommand ExportCommand { get; }

        public string GoalEquipmentId
        {
            get => _goalEquipmentId;
            set => SetProperty(ref _goalEquipmentId, value);
        }

        public int RunningEquipmentAmout
        {
            get => _runningEquipmentAmout;
            set => SetProperty(ref _runningEquipmentAmout, value);
        }

        public int PmEquipmentAmout
        {
            get => _pmEquipmentAmout;
            set => SetProperty(ref _pmEquipmentAmout, value);
        }

        public int DownEquipmentAmout
        {
            get => _downEquipmentAmout;
            set => SetProperty(ref _downEquipmentAmout, value);
        }

        public int OffLineEquipmentAmout
        {
            get => _offLineEquipmentAmout;
            set => SetProperty(ref _offLineEquipmentAmout, value);
        }

        [ImportingConstructor]
        public MainViewModel(IIOService ioService, IEquipmentPositionManager equipmentPositionManager)
        {
            _ioService = ioService;
            _equipmentPositionManager = equipmentPositionManager;
            SelectCommand = new DelegateCommand(ExecuteSelect);
            AddMockCommand = new DelegateCommand(ExecuteAddMock);
            ExportCommand = new DelegateCommand(ExecuteExport);
            Equipments = new ObservableCollection<EquipmentViewModel>(_testItems);
            Equipments.CollectionChanged += EquipmentsCollectionChanged;

            RefreshData();
        }

        #region Private methods

        private void EquipmentsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RefreshData();
        }

        private void ExecuteAddMock()
        {
            var viewModel = new AddEquipmentViewModel(70 * Equipments.Count);
            _ioService.ShowDialog(viewModel);
            if (viewModel.Result)
            {
                Equipments.Add(viewModel.ToEquipmentViewModel());
            }
        }

        private void ExecuteExport()
        {
            _equipmentPositionManager.Export(Equipments);
        }

        private void ExecuteSelect()
        {
            var goal = Equipments.FirstOrDefault(x => x.EquipmentId.Equals(GoalEquipmentId, StringComparison.CurrentCultureIgnoreCase));
            if (goal != null)
            {
                SelectedEquipment = goal;
            }
        }

        private void RefreshData()
        {
            RunningEquipmentAmout = Equipments.Count(x => x.Status == EquipmentStatus.Running);
            PmEquipmentAmout = Equipments.Count(x => x.Status == EquipmentStatus.Pm);
            DownEquipmentAmout = Equipments.Count(x => x.Status == EquipmentStatus.Down);
            OffLineEquipmentAmout = Equipments.Count(x => x.Status == EquipmentStatus.OffLine);
        }

        #endregion

        #region Fields

        private readonly IIOService _ioService;
        private readonly IEquipmentPositionManager _equipmentPositionManager;

        private EquipmentViewModel _selectedEquipment;
        private string _goalEquipmentId;

        private readonly List<EquipmentViewModel> _testItems = new List<EquipmentViewModel>
        {
            new EquipmentViewModel("WR001")
            {
                PackageCode = "4KG",
                Status = EquipmentStatus.Running,
                Left = 5
            },
            new EquipmentViewModel("WR002", 100)
            {
                PackageCode = "2KG",
                Status = EquipmentStatus.OffLine,
                Left = 70
            },
        };

        private int _runningEquipmentAmout;
        private int _pmEquipmentAmout;
        private int _downEquipmentAmout;
        private int _offLineEquipmentAmout;

        #endregion
    }
}