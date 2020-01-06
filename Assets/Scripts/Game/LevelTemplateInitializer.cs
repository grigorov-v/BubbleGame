using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Controllers;

namespace Game {
    public class LevelTemplateInitializer : MonoBehaviour {
        void Start() {
            var levelInfo = ConfigsController.Instance.FindCurrentLevelInfo();
            var template = levelInfo.LevelTemplate;

            var goTemplate = Resources.Load<GameObject>(template);
            Instantiate(goTemplate);
        }
    }
}