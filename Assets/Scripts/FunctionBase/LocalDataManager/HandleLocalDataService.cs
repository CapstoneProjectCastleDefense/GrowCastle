namespace FunctionBase.LocalDataManager
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using FunctionBase.Extensions;
    using LocalData;
    using Newtonsoft.Json;
    using UnityEngine;
    using Zenject;

    public class HandleLocalDataService
    {
        private readonly DiContainer                  diContainer;
        private readonly Dictionary<Type, ILocalData> localDataCached = new();

        public HandleLocalDataService(DiContainer diContainer)
        {
            this.diContainer = diContainer;
        }

        public void LoadData(Type type)
        {
            if (!type.IsClass || type.IsAbstract) return;
            if (this.localDataCached.TryGetValue(type, out var localData)) return;

            var localDataKey = PlayerPrefs.GetString(nameof(type));
            if (localDataKey == null)
            {
                if (Activator.CreateInstance(type) is ILocalData newData)
                {
                    newData.Init();
                    this.localDataCached.Add(type, newData);
                }
            }
            var savedData = JsonConvert.DeserializeObject(PlayerPrefs.GetString(nameof(type)));
            this.localDataCached.Add(type, Convert.ChangeType(savedData, type) as ILocalData);
        }

        public T LoadData<T>() where T : ILocalData
        {
            this.LoadData(typeof(T));

            return (T)this.localDataCached[typeof(T)];
        }

        public void SaveData(Type type)
        {
            if (!type.IsClass || type.IsAbstract) return;
            if (!this.localDataCached.TryGetValue(type, out var localData)) return;
            var stringValue = JsonConvert.SerializeObject(localData);
            PlayerPrefs.SetString(nameof(type), stringValue);
        }

        public void SaveData<T>() where T : ILocalData => this.SaveData(typeof(T));

        public void SaveData<T>(T data) where T : ILocalData
        {
            if (!this.localDataCached.TryGetValue(typeof(T), out var localData))
            {
                this.localDataCached[typeof(T)] = data;
                this.SaveData<T>();

                return;
            }
            this.localDataCached.Add(typeof(T), data);
            var stringValue = JsonConvert.SerializeObject(data);
            PlayerPrefs.SetString(nameof(T), stringValue);
        }

        public void SaveAllData()
        {
            ReflectionExtension.GetAllDerivedTypes<ILocalData>().ForEach(this.SaveData);
        }

        public void LoadAllData()
        {
            var x = ReflectionExtension.GetAllDerivedTypes<ILocalData>();
            ReflectionExtension.GetAllDerivedTypes<ILocalData>().ForEach(type =>
            {
                Debug.Log("a"+typeof(TestLocalData).FullName);
                this.LoadData(type);
                this.diContainer.Bind(type).FromInstance(this.localDataCached[type]).AsCached();
            });
        }
    }
}