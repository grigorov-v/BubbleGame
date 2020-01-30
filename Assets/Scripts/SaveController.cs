using System.Collections.Generic;
using System.Xml;

using UnityEngine;
using UnityEngine.SceneManagement;

using Core.Controller;
using SaveValues;

namespace Controllers {
    public class SaveController : BaseController<SaveController> {
        public ProgressId GetCurrentProgress() {
            var worldInfo = ConfigsController.Instance.FindCurrentWorldInfo();
            
            var defaultValue = new ProgressId() {
                WorldName = worldInfo.Name,
                Level = 0
            };

            var saveInfo = worldInfo.SaveInfo;
            var key = saveInfo.LevelKey;

            return Load(key, defaultValue);
        }

        void Save<T>(string key, T data) {
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(key, json);
        }

        T Load<T>(string key, T def) {
            if ( !PlayerPrefs.HasKey(key) ) {
                return def;
            }

            var json = PlayerPrefs.GetString(key);
            return JsonUtility.FromJson<T>(json);
        }
    }
}