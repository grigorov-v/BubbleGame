using System.Collections.Generic;
using System;

using UnityEngine;
using UnityEngine.SceneManagement;

using Core.Controller;
using Core.XML;

using LevelValues;
using Configs;

namespace Controllers {
    public class LevelController : BaseController<LevelController> {
        public LevelInfo FindCurrentLevelInfo() {
            var levelIndex = SaveController.Instance.GetCurrentProgress().Level;
            var levelConfig = ConfigsController.Instance.FindConfig<LevelConfig>();
            if ( levelConfig == null ) {
                return null;
            }

            var levels = levelConfig.Levels;
            if ( (levels == null) || (levelIndex > levels.Count - 1) ) {
                return null;
            }

            return levels[levelIndex];
        }        
    }
}