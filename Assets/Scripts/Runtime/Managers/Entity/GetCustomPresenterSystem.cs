namespace Runtime.Managers.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Runtime.Elements.Base;
    using Runtime.Managers.Base;

    public class GetCustomPresenterSystem
    {
        private readonly Dictionary<Type, IElementManager> presenterTypeToElementManager = new();

        public GetCustomPresenterSystem(List<IElementManager> listElementManagers)
        {
            this.presenterTypeToElementManager = listElementManagers.ToDictionary(element => element.GetType(), element => element);
        }
        public IElementManager GetElementManager(Type type)
        {
            this.presenterTypeToElementManager.TryGetValue(type, out var elementManager);
            return elementManager;
        }
        public List<IElementPresenter> GetAllElementPresenter()
        {
            var result = new List<IElementPresenter>();
            foreach (var elementManager in this.presenterTypeToElementManager.Values)
            {
                result.AddRange(elementManager.GetAllElementPresenter<IElementPresenter>());
            }

            return result;
        }
    }
}