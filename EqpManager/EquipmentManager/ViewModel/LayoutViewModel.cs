using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EquipmentManager.Infrastructure;
using EquipmentManager.Interact;
using EquipmentManager.Resources;
using EquipmentManager.ViewModel.Equipment;
using GongSolutions.Wpf.DragDrop;
using Prism.Commands;

namespace EquipmentManager.ViewModel
{
    public class LayoutViewModel : DialogViewModel, IDropTarget
    {
        public MainViewDragDropHandler DragDropHandler { get; }

        public ObservableCollection<IEquipmentViewVisualModel> Equipments { get; }

        public IEquipmentViewVisualModel SelectedEquipment
        {
            get => _selectedEquipment;
            set
            {
                SetProperty(ref _selectedEquipment, value);
                RaisePropertyChanged(nameof(ShowEquipmentInfoBoard));
                ChangeOrientationCommand.RaiseCanExecuteChanged();
            }
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

        public DelegateCommand ResetScaleCommand { get; }

        public bool EquipmentInfoBoardVisibility
        {
            get => _equipmentInfoBoardVisibility;
            set => SetProperty(ref _equipmentInfoBoardVisibility, value);
        }

        public bool ShowEquipmentInfoBoard => SelectedEquipment is EquipmentViewModel;

        public ICommand UpdateEquipmentInfoBoardPositionCommand { get; }
        public ICommand AddMockCommand { get; }
        public ICommand AddMockLineCommand { get; }

        public ICommand DeleteCommand { get; }
        public DelegateCommand ChangeOrientationCommand { get; }
        public DelegateCommand RevertCommand { get; }

        public LayoutViewModel(ObservableCollection<IEquipmentViewVisualModel> equipments, IIOService ioService)
        {
            _ioService = ioService;
            DragDropHandler = new MainViewDragDropHandler();
            Equipments = equipments;
            ResetScaleCommand = new DelegateCommand(() => ScaleValue = 1, () => Math.Abs(ScaleValue - 1) > 0.1);
            UpdateEquipmentInfoBoardPositionCommand = new DelegateCommand(() => EquipmentInfoBoardVisibility = !EquipmentInfoBoardVisibility);
            AddMockCommand = new DelegateCommand(ExecuteAddMock);
            AddMockLineCommand = new DelegateCommand(ExecuteAddMockLine);
            DeleteCommand = new DelegateCommand(ExecuteDelete, () => SelectedEquipment != null);
            ChangeOrientationCommand = new DelegateCommand(ExecuteChangeOrientation, () => SelectedEquipment is BoundaryViewModel);
            RevertCommand = new DelegateCommand(ExecuteRevert, () => _deletedVisualModels.Any());
        }

        #region IDropTarget Members

        public void DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.Data == null)
            {
                return;
            }

            if (SelectedEquipment == dropInfo.Data && (dropInfo.TargetItem == null || dropInfo.TargetItem == dropInfo.Data))
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Move;
                var sourceItem = dropInfo.Data as IEquipmentViewVisualModel;
                if (sourceItem != null)
                {
                    var targetPosition = dropInfo.DropPosition - DragDropHandler.Delta;
                    sourceItem.Left = Math.Max((int) targetPosition.X, 0);
                    sourceItem.Top = Math.Max((int) targetPosition.Y, 0);
                }
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data as IEquipmentViewVisualModel;
            if (sourceItem != null)
            {
                SelectedEquipment = null;
            }
        }

        #endregion

        #region Private methods

        private void ExecuteAddMock()
        {
            var viewModel = new AddEquipmentViewModel(70 * Equipments.Count);
            _ioService.ShowDialog(viewModel);
            if (viewModel.Result)
            {
                var existingItem = Equipments.OfType<EquipmentViewModel>().FirstOrDefault(x => x.EquipmentId == viewModel.EquipmentId);
                if (existingItem != null)
                {
                    existingItem.Size = viewModel.Height;
                    existingItem.Status = viewModel.Status;
                }
                else
                {
                    Equipments.Add(viewModel.ToEquipmentViewModel());
                }
            }
        }

        private void ExecuteAddMockLine()
        {
            Equipments.Add(new BoundaryViewModel(Orientation.Horizontal, 100)
            {
                Id = BoundaryViewModel.GetId(),
                Left = 100,
                Top = 100
            });
        }

        private void ExecuteChangeOrientation()
        {
            (SelectedEquipment as BoundaryViewModel)?.ChangeOrientation();
        }

        private void ExecuteDelete()
        {
            if (SelectedEquipment != null)
            {
                if (_deletedVisualModels.Count > MAX_REVERT_SIZE)
                {
                    _deletedVisualModels.RemoveAt(0);
                }
                _deletedVisualModels.Add(SelectedEquipment);
                Equipments.Remove(SelectedEquipment);
                SelectedEquipment = null;
                RevertCommand.RaiseCanExecuteChanged();
            }
        }

        private void ExecuteRevert()
        {
            if (_deletedVisualModels.Any())
            {
                SelectedEquipment = _deletedVisualModels.Last();
                Equipments.Add(SelectedEquipment);
                _deletedVisualModels.Remove(SelectedEquipment);
                RevertCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Fields

        private IEquipmentViewVisualModel _selectedEquipment;
        private double _scaleValue = 1;
        private bool _equipmentInfoBoardVisibility = true;
        private readonly IIOService _ioService;

        private readonly List<IEquipmentViewVisualModel> _deletedVisualModels = new List<IEquipmentViewVisualModel>();
        private const int MAX_REVERT_SIZE = 4;

        #endregion
    }
}