using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core.Controller;

namespace Controllers {
    public class ControllersInitializer : MonoBehaviour {
        List<IController> _controllersRegister = new List<IController>(){
            new ConfigsController(),
            new SaveController()
        };

        private void Awake() {
            _controllersRegister.ForEach(controller => controller.Init());
            _controllersRegister.ForEach(controller => controller.PostInit());
        }

        void OnDestroy() {
            _controllersRegister.ForEach(controller => controller.Reinit());
        }
    }
}