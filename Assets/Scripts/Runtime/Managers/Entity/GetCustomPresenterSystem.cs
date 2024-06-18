namespace Runtime.Managers.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Runtime.Elements.Base;
    using Runtime.Managers.Base;

    public class GetCustomPresenterSystem
    {
        private readonly Dictionary<Type, IElementManager> typeToElementManager = new();

        public GetCustomPresenterSystem(List<IElementManager> listElementManagers)
        {
            this.typeToElementManager = listElementManagers.ToDictionary(element => element.GetType(), element => element);
        }
        public IElementManager GetElementManager(Type type)
        {
            this.typeToElementManager.TryGetValue(type, out var elementManager);
            return elementManager;
        }
        public List<IElementPresenter> GetAllElementPresenter()
        {
            var result = new List<IElementPresenter>();
            foreach (var elementManager in this.typeToElementManager.Values)
            {
                result.AddRange(elementManager.GetAllElementPresenter<IElementPresenter>());
            }

            return result;
        }
        
        public List<IElementPresenter> GetAllElementPresenters(params Type[] types)
        {
            var result = new List<IElementPresenter>();
            foreach (var type in types)
            {
                this.typeToElementManager.TryGetValue(type, out var elementManager);
                if (elementManager != null)
                {
                    result.AddRange(elementManager.GetAllElementPresenter<IElementPresenter>());
                }
            }

            return result;
        }
    }
}