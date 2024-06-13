namespace Runtime.Managers.Entity
{
    using System;
    using System.Collections.Generic;
    using Runtime.Managers.Base;
    using Zenject;

    public class EntityManager
    {
        private readonly Dictionary<Type, IElementManager> presenterTypeToElementManager = new();

        public IElementManager GetElementManager(Type type)
        {
            this.presenterTypeToElementManager.TryGetValue(type, out var elementManager);
            return elementManager;
        }

        public void AddElementManager(Type type, IElementManager elementManager) { this.presenterTypeToElementManager.TryAdd(type, elementManager); }
    }
}