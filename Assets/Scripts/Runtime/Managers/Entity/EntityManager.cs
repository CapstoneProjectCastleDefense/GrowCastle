namespace Runtime.Managers.Entity
{
    using System.Collections.Generic;
    using Runtime.Managers.Base;
    using Zenject;

    public class EntityManager : IInitializable
    {
        private List<IElementManager> elementManagers;

        public EntityManager(params IElementManager[] elementManagers) { this.elementManagers = elementManagers != null ? new(elementManagers) : new(); }

        public void Initialize() { }
    }
}