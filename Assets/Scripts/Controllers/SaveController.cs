using System.Collections.Generic;
using System.Xml;

using UnityEngine;
using UnityEngine.SceneManagement;

using Core.Controller;
using Configs;

namespace Controllers {
    public class SaveController : BaseController<SaveController> {
        public int LoadLevelValue() {
            var worldInfo = ConfigsController.Instance.FindCurrentWorldInfo();
            var saveInfo = worldInfo.SaveInfo;
            return PlayerPrefs.GetInt(saveInfo.LevelKey, 0);
        }
    }
}