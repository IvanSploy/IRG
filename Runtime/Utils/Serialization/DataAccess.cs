using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace IRG
{
    public abstract class DataAccess<TDataAccess, TData> : DataAccess<TData>
        where TDataAccess : DataAccess<TData>, new()
    {
        private static TDataAccess _instance;

        public static TDataAccess Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TDataAccess();
                    _instance.Load();
                }

                return _instance;
            }
        }
    }

    public abstract class DataAccess<TData>
    {
        private const string Path = "Content/Scenario";
        protected abstract string GetFileName();
        private string GetFullPath() => $"{Path}/{GetFileName()}.json";

        protected List<TData> _data = new();

        public void Load()
        {
            var json = LoadJson();
            DeSerialize(json);
            OnLoad();
        }

        public virtual void OnLoad()
        {
        }

        public void Save()
        {
            var json = Serialize();
            SaveJson(json);
            OnSave();
        }

        public virtual void OnSave()
        {
        }

        public List<TData> GetAll() => _data.ToList();

        protected virtual string Serialize()
        {
            return JsonConvert.SerializeObject(_data, Formatting.Indented);
        }

        protected virtual void DeSerialize(string json)
        {
            _data = JsonConvert.DeserializeObject<List<TData>>(json);
            _data ??= new List<TData>();
        }

        private string LoadJson()
        {
            var path = GetFullPath();
            if (!File.Exists(path)) return "";
            return File.ReadAllText(path);
        }

        private void SaveJson(string json)
        {
            var path = GetFullPath();
            if (!File.Exists(path))
            {
                var lastIndex = path.LastIndexOf("/", StringComparison.InvariantCulture);
                var directory = path.Substring(0, lastIndex);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }

            File.WriteAllText(path, json);
        }
    }
}