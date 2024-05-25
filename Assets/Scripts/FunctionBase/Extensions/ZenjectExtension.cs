namespace FunctionBase.Extensions
{
    using UnityEngine;
    using Zenject;

    public static class ZenjectExtension
    {

        public static DiContainer GetCurrentContainer(this object obj)
        {
            return GetCurrentContainer();
        }

        public static DiContainer GetCurrentContainer()
        {
            return Object.FindObjectOfType<SceneContext>().Container;
        }
        
        public static void BindAllDerivedTypes<T>(this DiContainer container, bool nonLazy = false, bool sameAssembly = false)
        {
            foreach (var type in ReflectionExtension.GetAllDerivedTypes<T>())
            {
                if (nonLazy)
                {
                    container.Bind(type).AsSingle().NonLazy();
                }
                else
                {
                    container.Bind(type).AsSingle();
                }
            }
        }
    }
}