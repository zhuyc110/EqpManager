using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using EquipmentManager.Config;
using EquipmentManager.Infrastructure;
using EquipmentManager.ViewModel.Equipment;

namespace EquipmentManager.Interact
{
    [Export(typeof(IEquipmentPositionManager))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class EquipmentPositionManager : IEquipmentPositionManager
    {
        [ImportingConstructor]
        public EquipmentPositionManager(IIOService ioService, IAppSetting appSetting)
        {
            _ioService = ioService;
            _appSetting = appSetting;
            EquipmentPositionDatas = EquipmentPositionDataHolder.CreateDefault();
        }

        #region IEquipmentPositionManager Members

        public void Export(IList<EquipmentViewModel> equipments)
        {
            var filePath = _ioService.OpenFileDialog("Select file", "export file|*.export;*.txt", false);
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return;
            }

            _appSetting.SetExportFilePath(filePath);

            EquipmentPositionDatas.EquipmentPositionDatas.Clear();
            EquipmentPositionDatas.EquipmentPositionDatas.AddRange(equipments.Select(x => new EquipmentPositionData
            {
                EquipmentId = x.EquipmentId,
                Left = x.Left,
                Top = x.Top,
                Size = x.Height
            }));

            try
            {
                var xmlSerializer = new XmlSerializer(typeof(EquipmentPositionDataHolder));
                using (var fileStream = new FileStream(_appSetting.ExportFilePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (var stream = new StreamWriter(fileStream))
                    {
                        xmlSerializer.Serialize(stream, EquipmentPositionDatas);
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        public void Synchronize(IList<EquipmentViewModel> equipments)
        {
            foreach (var positionData in EquipmentPositionDatas.EquipmentPositionDatas)
            {
                var viewModel = equipments.FirstOrDefault(x => x.EquipmentId == positionData.EquipmentId);
                if (viewModel != null)
                {
                    viewModel.Height = positionData.Size;
                    viewModel.Left = positionData.Left;
                    viewModel.Top = positionData.Top;
                }
            }
        }

        #endregion

        #region Fields

        private readonly IIOService _ioService;
        private readonly IAppSetting _appSetting;

        protected EquipmentPositionDataHolder EquipmentPositionDatas;

        #endregion
    }
}