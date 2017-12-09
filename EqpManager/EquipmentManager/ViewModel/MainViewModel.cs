﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using EquipmentManager.Infrastructure;
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

        [ImportingConstructor]
        public MainViewModel(IIOService ioService)
        {
            _ioService = ioService;
            SelectCommand = new DelegateCommand(ExecuteSelect);
            AddMockCommand = new DelegateCommand(ExecuteAddMock);
            Equipments = new ObservableCollection<EquipmentViewModel>(_testItems);
        }

        #region Private methods

        private void ExecuteAddMock()
        {
            var viewModel = new AddEquipmentViewModel(70 * Equipments.Count);
            _ioService.ShowDialog(viewModel);
            if (viewModel.Result)
            {
                Equipments.Add(viewModel.ToEquipmentViewModel());
            }
        }

        private void ExecuteSelect()
        {
            var goal = Equipments.FirstOrDefault(x => x.EquipmentId.Equals(GoalEquipmentId, StringComparison.CurrentCultureIgnoreCase));
            if (goal != null)
            {
                SelectedEquipment = goal;
            }
        }

        #endregion

        #region Fields

        private readonly IIOService _ioService;

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

        #endregion
    }
}