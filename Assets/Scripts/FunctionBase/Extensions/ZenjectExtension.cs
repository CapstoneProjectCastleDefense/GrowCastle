namespace FunctionBase.Extensions
{
    using UnityEngine;
    using Zenject;

    public static class ZenjectExtension
    {

        private static SceneContext currentSceneContext;

        public static DiContainer GetCurrentContainer(this object obj)
        {
            return GetCurrentContainer();
        }

        public static DiContainer GetCurrentContainer()
        {
            if (currentSceneContext == null)
            {
                currentSceneContext = Object.FindObjectOfType<SceneContext>();
            }

            return currentSceneContext.Container;
        }
    }
}