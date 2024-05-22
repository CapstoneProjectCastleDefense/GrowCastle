namespace FunctionBase.LocalDataManager
{
    using System;
    using System.Collections.Generic;
    using FunctionBase.Extensions;
    using ModestTree;
    using Newtonsoft.Json;
    using UnityEngine;
    using Zenject;

    public class HandleLocalDataService
    {
        private readonly DiContainer                  diContainer;
        private readonly Dictionary<Type, ILocalData> localDataCached = new();

        private readonly JsonSerializerSettings jsonSetting = new()
        {
            TypeNameHandling      = TypeNameHandling.Auto,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        };

        public HandleLocalDataService(DiContainer diContainer)
        {
            this.diContainer = diContainer;
        }

        public ILocalData GetDataFromCached(Type type) => this.localDataCached[type];

        public void LoadData(Type type)
        {
            if (!type.IsClass || type.IsAbstract) return;
            if (this.localDataCached.TryGetValue(type, out var localData)) return;

            var localDataKey = PlayerPrefs.GetString(nameof(type));
            if (localDataKey == null || localDataKey.IsEmpty())
            {
                if (Activator.CreateInstance(type) is ILocalData newData)
                {
                    newData.Init();
                    this.localDataCached.Add(type, newData);

                    return;
                }
            }
            var savedData = JsonConvert.DeserializeObject(PlayerPrefs.GetString(nameof(type)),type,this.jsonSetting);
            this.localDataCached.Add(type, (ILocalData)savedData);
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
            ReflectionExtension.GetAllDerivedTypes<ILocalData>().ForEach(this.LoadData);
        }
    }

}