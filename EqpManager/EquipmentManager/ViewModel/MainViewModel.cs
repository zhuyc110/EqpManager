using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EquipmentManager.Infrastructure;
using EquipmentManager.Interact;
using EquipmentManager.ViewModel.Equipment;
using GongSolutions.Wpf.DragDrop;
using Prism.Commands;
using Prism.Mvvm;

namespace EquipmentManager.ViewModel
{
    [Export(typeof(MainViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MainViewModel : BindableBase, IDropTarget
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

        public DelegateCommand ResetScaleCommand { get; }

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

        public double ScaleValue
        {
            get => _scaleValue;
            set
            {
                SetProperty(ref _scaleValue, value);
                ResetScaleCommand.RaiseCanExecuteChanged();
            }
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
        public MainViewModel(IIOService ioService, IEquipmentLayoutManager equipmentLayoutManager)
        {
            _ioService = ioService;
            _equipmentLayoutManager = equipmentLayoutManager;
            SelectCommand = new DelegateCommand(ExecuteSelect);
            AddMockCommand = new DelegateCommand(ExecuteAddMock);
            ExportCommand = new DelegateCommand(async () => await ExecuteExport());
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            ResetScaleCommand = new DelegateCommand(() => ScaleValue = 1, () => ScaleValue != 1);
            Equipments = new ObservableCollection<EquipmentViewModel>();
            Equipments.CollectionChanged += EquipmentsCollectionChanged;
            _equipmentLayoutManager.DataInitialized += EquipmentLayoutManagerDataInitialized;
            _equipmentLayoutManager.EquipmentDataExported += EquipmentLayoutManagerEquipmentDataExported;
            RefreshData();
        }

        private void EquipmentLayoutManagerEquipmentDataExported(object sender, EventArgs e)
        {
            _ioService.ReleaseCursor();
            _ioService.ShowDialog("导出", "导出成功");
        }

        #region IDropTarget Members

        public void DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.Data == null)
            {
                return;
            }

            if (SelectedEquipment == dropInfo.Data && dropInfo.TargetItem == null)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Move;
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data as EquipmentViewModel;
            if (sourceItem != null)
            {
                sourceItem.Left = Math.Max((int) dropInfo.DropPosition.X - 25, 0);
                sourceItem.Top = Math.Max((int) dropInfo.DropPosition.Y - sourceItem.Height / 2, 0);
                SelectedEquipment = null;
            }
        }

        #endregion

        #region Private methods

        private void EquipmentLayoutManagerDataInitialized(object sender, EventArgs e)
        {
            _equipmentLayoutManager.Synchronize(Equipments);
        }

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
                var existingItem = Equipments.FirstOrDefault(x => x.EquipmentId == viewModel.EquipmentId);
                if (existingItem != null)
                {
                    existingItem.Height = viewModel.Height;
                    existingItem.Status = viewModel.Status;
                }
                else
                {
                    Equipments.Add(viewModel.ToEquipmentViewModel());
                }
            }
        }

        private async Task ExecuteExport()
        {
            _ioService.SetCursorBusy();
            await _equipmentLayoutManager.Export(Equipments);
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
        private readonly IEquipmentLayoutManager _equipmentLayoutManager;

        private EquipmentViewModel _selectedEquipment;
        private string _goalEquipmentId;

        private int _runningEquipmentAmout;
        private int _pmEquipmentAmout;
        private int _downEquipmentAmout;
        private int _offLineEquipmentAmout;
        private double _scaleValue = 1;

        #endregion
    }
}