using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
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

        public LayoutViewModel(IEnumerable<IEquipmentViewVisualModel> equipments)
        {
            DragDropHandler = new MainViewDragDropHandler();
            Equipments = new ObservableCollection<IEquipmentViewVisualModel>(equipments);
            ResetScaleCommand = new DelegateCommand(() => ScaleValue = 1, () => Math.Abs(ScaleValue - 1) > 0.1);
            UpdateEquipmentInfoBoardPositionCommand = new DelegateCommand(() => EquipmentInfoBoardVisibility = !EquipmentInfoBoardVisibility);
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

        #region Fields

        private IEquipmentViewVisualModel _selectedEquipment;
        private double _scaleValue = 1;
        private bool _equipmentInfoBoardVisibility = true;

        #endregion
    }
}