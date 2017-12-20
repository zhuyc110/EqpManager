using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using EquipmentManager.Config;
using EquipmentManager.Infrastructure;
using EquipmentManager.ViewModel.Equipment;
using log4net;

namespace EquipmentManager.Interact
{
    [Export(typeof(IEquipmentLayoutManager))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class EquipmentLayoutManager : IEquipmentLayoutManager
    {
        public event EventHandler DataInitialized;
        public event EventHandler EquipmentDataExported;

        [ImportingConstructor]
        public EquipmentLayoutManager(IIOService ioService, IAppSetting appSetting)
        {
            _ioService = ioService;
            _appSetting = appSetting;
            EquipmentPositionDatas = EquipmentPositionDataHolder.CreateDefault();
        }

        #region IEquipmentLayoutManager Members

        public async Task Export(IList<IEquipmentViewVisualModel> equipments)
        {
            var filePath = _ioService.OpenFileDialog("Select file", "export file|*.export;*.txt", false);
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return;
            }

            _appSetting.ExportFilePath = filePath;
            _appSetting.Save();

            EquipmentPositionDatas.EquipmentPositionDatas.Clear();
            EquipmentPositionDatas.EquipmentPositionDatas.AddRange(equipments.Select(x => new EquipmentPositionData
            {
                Id = x.Id,
                Left = x.Left,
                Top = x.Top,
                Size = x.Size,
                IsEquipment = x.IsEquipment,
                Orientation = x.Orientation
            }));

            await new TaskFactory().StartNew(() =>
                {
                    try
                    {
                        var xmlSerializer = new XmlSerializer(typeof(EquipmentPositionDataHolder));
                        using (var fileStream = new FileStream(_appSetting.ExportFilePath, FileMode.Create, FileAccess.Write))
                        {
                            using (var stream = new StreamWriter(fileStream))
                            {
                                xmlSerializer.Serialize(stream, EquipmentPositionDatas);
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        Log.Error($"Can not serialize file {_appSetting.ExportFilePath}", exception);
                    }
                    finally
                    {
                        EquipmentDataExported?.Invoke(null, EventArgs.Empty);
                    }
                }
            );
        }

        public void Initialize()
        {
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(EquipmentPositionDataHolder));
                using (var fileStream = new FileStream(_appSetting.ExportFilePath, FileMode.Open, FileAccess.Read))
                {
                    using (var stream = new StreamReader(fileStream))
                    {
                        EquipmentPositionDatas = (EquipmentPositionDataHolder) xmlSerializer.Deserialize(stream);
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error($"Can not deserialize file {_appSetting.ExportFilePath}", exception);
            }
            DataInitialized?.Invoke(null, EventArgs.Empty);
        }

        public void Synchronize(IList<IEquipmentViewVisualModel> equipments)
        {
            foreach (var positionData in EquipmentPositionDatas.EquipmentPositionDatas)
            {
                var viewModel = equipments.FirstOrDefault(x => x.Id == positionData.Id);
                if (viewModel != null)
                {
                    viewModel.Size = positionData.Size;
                    viewModel.Left = positionData.Left;
                    viewModel.Top = positionData.Top;
                }
                else
                {
                    if (positionData.IsEquipment)
                    {
                        equipments.Add(new EquipmentViewModel(positionData.Id, positionData.Size)
                        {
                            Left = positionData.Left,
                            Top = positionData.Top
                        });
                    }
                    else
                    {
                        equipments.Add(new BoundaryViewModel(positionData.Orientation, positionData.Size)
                        {
                            Id = BoundaryViewModel.GetId(),
                            Left = positionData.Left,
                            Top = positionData.Top
                        });
                    }
                }
            }
        }

        #endregion

        #region Fields

        private readonly IIOService _ioService;
        private readonly IAppSetting _appSetting;

        protected EquipmentPositionDataHolder EquipmentPositionDatas;

        private static readonly ILog Log = LogManager.GetLogger(typeof(EquipmentLayoutManager));

        #endregion
    }
}