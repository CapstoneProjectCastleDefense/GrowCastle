namespace FunctionBase.BlueprintManager
{
    using System.Linq;
    using System.Threading.Tasks;
    using Cysharp.Threading.Tasks;
    using FunctionBase.BlueprintManager.BlueprintBase;
    using FunctionBase.Extensions;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using Zenject;

    public class BlueprintDataManager
    {
        private readonly SignalBus signalBus;
        public BlueprintDataManager(SignalBus signalBus) { this.signalBus = signalBus; }
        public           float     CurrentLoadedProgress;
        
        public async void LoadAllData()
        {
            var listAllBlueprint = ReflectionExtension.GetAllDerivedTypes<IBlueprintData>();
            foreach (var blueprintData in listAllBlueprint.Where(data=>!data.IsAbstract).Select(dataType => (IBlueprintData)this.GetCurrentContainer().Resolve(dataType)))
            {
                var rawData = await this.GetRawData(blueprintData);
                blueprintData.ConvertData(rawData);
            }
            this.signalBus.Fire<LoadedAllBlueprintDataSignal>();
        }
        
        private async Task<string> GetRawData(IBlueprintData blueprint)
        {
            var dataPath = blueprint.GetCustomAttribute<DataInfoAttribute>().DataPath; 
            var rawData  = (await Addressables.LoadAssetAsync<TextAsset>(dataPath).ToUniTask(Progress.CreateOnlyValueChanged<float>(progress =>
            {
                this.CurrentLoadedProgress += progress;
            }))).text;
            return rawData;
        }
    }
}