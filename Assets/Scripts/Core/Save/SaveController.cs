using System.Collections.Generic;
using System.Xml;

using UnityEngine;
using UnityEngine.SceneManagement;

using Core.Controller;

namespace Controllers {

    struct SaveInfo {
        public object Save;
        public string Key;
        public bool AutoSave;

        public bool IsEmpty {
            get {
                return (Save == null) && string.IsNullOrEmpty(Key);
            }
        }
    }

    public class SaveController : BaseController<SaveController> {
        List<SaveInfo> _saveInfoList = new List<SaveInfo>() {
            new SaveInfo() {
                Save = null,
                Key  = "{SceneName}",
            }   
        };

        public override void PostInit() {
            _saveInfoList.ForEach(saveInfo => Load(saveInfo));
        }

        string ParseKey(string key) {
            var sceneName = SceneManager.GetActiveScene().name;
            var newKey = key.Replace("{SceneName}", sceneName);
            return newKey;
        }

        void Load(SaveInfo saveInfo) {
            if ( saveInfo.Save == null ) {
                return;
            }

            var key = ParseKey(saveInfo.Key);
            var json = PlayerPrefs.GetString(key, string.Empty);
            JsonUtility.FromJsonOverwrite(json, saveInfo.Save);
        }

        void Save(SaveInfo saveInfo) {
            if ( saveInfo.Save == null ) {
                return;
            }

            var key = ParseKey(saveInfo.Key);
            var json = JsonUtility.ToJson(saveInfo.Save);
            PlayerPrefs.SetString(key, json);
        }

        public void Save<T>() {
            var save = _saveInfoList.Find(saveInfo => saveInfo.Save is T);
            if ( save.IsEmpty ) {
                return;
            }

            Save(save);
        }

        public T Load<T>() where T: class {
            var save = _saveInfoList.Find(saveInfo => saveInfo.Save is T);
            if ( save.IsEmpty ) {
                return null;
            }

            Load(save);
            return save.Save as T;
        }

        public T FindSave<T>() where T: class {
            var save = _saveInfoList.Find(saveInfo => saveInfo.Save is T);
            if ( save.IsEmpty ) {
                return null;
            }

            return save.Save as T;
        }
    }
}