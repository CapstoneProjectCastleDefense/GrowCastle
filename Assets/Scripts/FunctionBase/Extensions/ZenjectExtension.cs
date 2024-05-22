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
    }
}