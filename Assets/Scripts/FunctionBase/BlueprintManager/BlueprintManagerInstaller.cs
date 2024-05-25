namespace UnityFoundation.Scripts.BlueprintManager
{
    using FunctionBase.BlueprintManager.BlueprintBase;
    using FunctionBase.Extensions;
    using Zenject;

    public class BlueprintManagerInstaller: Installer<BlueprintManagerInstaller>
    {
        public override void InstallBindings()
        {
            this.Container.BindAllDerivedTypes<IBlueprintData>();
            this.Container.Bind<BlueprintDataManager>().AsCached();
            this.Container.DeclareSignal<LoadedAllBlueprintDataSignal>();
        }
    }
}