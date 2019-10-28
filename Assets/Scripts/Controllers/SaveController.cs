using System.Collections.Generic;
using System.Xml;

using UnityEngine;

using Core.Controller;

namespace Controllers {

    struct SaveInfo {
        public object Save;
        public string Key;
        public bool AutoSave;
    }

    public class SaveController : BaseController<SaveController> {
        List<SaveInfo> _xmlLoadableInfoList = new List<SaveInfo>() {
            new SaveInfo() {
                Save = null,
                Key  = "{SceneName}",
            }   
        };

        public override void PostInit() {
        }

        void Load (SaveInfo saveInfo) {
            if ( saveInfo.Save == null ) {
                return;
            }

            var json = PlayerPrefs.GetString(saveInfo.Key, string.Empty);
            JsonUtility.FromJsonOverwrite(json, saveInfo.Save);
        }

        void Save (SaveInfo saveInfo) {
            if ( saveInfo.Save == null ) {
                return;
            }

            var json = JsonUtility.ToJson(saveInfo.Save);
            PlayerPrefs.SetString(saveInfo.Key, json);
        }
    }
}