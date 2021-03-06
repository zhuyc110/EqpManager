﻿using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EquipmentManager.Infrastructure;
using EquipmentManager.Interact;
using EquipmentManager.Resources;
using EquipmentManager.ViewModel.Equipment;
using Prism.Commands;
using Prism.Mvvm;

namespace EquipmentManager.ViewModel
{
    [Export(typeof(MainViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MainViewModel : BindableBase
    {
        public ObservableCollection<IEquipmentViewVisualModel> Equipments { get; }

        public IEquipmentViewVisualModel SelectedEquipment
        {
            get => _selectedEquipment;
            set
            {
                if (_selectedEquipment is EquipmentViewModel)
                {
                    ((EquipmentViewModel) _selectedEquipment).IsSelected = false;
                }

                SetProperty(ref _selectedEquipment, value);

                if (_selectedEquipment is EquipmentViewModel)
                {
                    ((EquipmentViewModel) _selectedEquipment).IsSelected = true;
                }
            }
        }

        public ICommand SelectCommand { get; }
        public ICommand ExportCommand { get; }
        public ICommand LayoutCommand { get; }

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

        public MainViewDragDropHandler DragDropHandler { get; }

        [ImportingConstructor]
        public MainViewModel(IIOService ioService, IEquipmentLayoutManager equipmentLayoutManager)
        {
            DragDropHandler = new MainViewDragDropHandler();
            _ioService = ioService;
            _equipmentLayoutManager = equipmentLayoutManager;
            SelectCommand = new DelegateCommand(ExecuteSelect);
            ExportCommand = new DelegateCommand(async () => await ExecuteExport());
            LayoutCommand = new DelegateCommand(ExecuteLayout);
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            ResetScaleCommand = new DelegateCommand(() => ScaleValue = 1, () => ScaleValue != 1);
            Equipments = new ObservableCollection<IEquipmentViewVisualModel>();
            Equipments.CollectionChanged += EquipmentsCollectionChanged;
            _equipmentLayoutManager.DataInitialized += EquipmentLayoutManagerDataInitialized;
            _equipmentLayoutManager.EquipmentDataExported += EquipmentLayoutManagerEquipmentDataExported;
            RefreshData();
        }

        #region Private methods

        private void EquipmentLayoutManagerDataInitialized(object sender, EventArgs e)
        {
            _equipmentLayoutManager.Synchronize(Equipments);
        }

        private void EquipmentLayoutManagerEquipmentDataExported(object sender, EventArgs e)
        {
            _ioService.ReleaseCursor();
            _ioService.ShowDialog("Export", "Export finished.");
        }

        private void EquipmentsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RefreshData();
        }

        private async Task ExecuteExport()
        {
            _ioService.SetCursorBusy();
            await _equipmentLayoutManager.Export(Equipments);
        }

        private void ExecuteLayout()
        {
            SelectedEquipment = null;
            var vm = new LayoutViewModel(Equipments, _ioService);
            _ioService.ShowDialog(vm, new DialogSetting
            {
                Width = Equipments.Select(x => x.Left).Max() + 200,
                Height = 600,
            });
        }

        private void ExecuteSelect()
        {
            var goal = Equipments.OfType<EquipmentViewModel>().FirstOrDefault(x => x.EquipmentId.Equals(GoalEquipmentId, StringComparison.CurrentCultureIgnoreCase));
            if (goal != null)
            {
                SelectedEquipment = goal;
            }
        }

        private void RefreshData()
        {
            RunningEquipmentAmout = Equipments.OfType<EquipmentViewModel>().Count(x => x.Status == EquipmentStatus.Running);
            PmEquipmentAmout = Equipments.OfType<EquipmentViewModel>().Count(x => x.Status == EquipmentStatus.Pm);
            DownEquipmentAmout = Equipments.OfType<EquipmentViewModel>().Count(x => x.Status == EquipmentStatus.Down);
            OffLineEquipmentAmout = Equipments.OfType<EquipmentViewModel>().Count(x => x.Status == EquipmentStatus.OffLine);
        }

        #endregion

        #region Fields

        private readonly IIOService _ioService;
        private readonly IEquipmentLayoutManager _equipmentLayoutManager;

        private IEquipmentViewVisualModel _selectedEquipment;
        private string _goalEquipmentId;

        private int _runningEquipmentAmout;
        private int _pmEquipmentAmout;
        private int _downEquipmentAmout;
        private int _offLineEquipmentAmout;
        private double _scaleValue = 1;

        #endregion
    }
}