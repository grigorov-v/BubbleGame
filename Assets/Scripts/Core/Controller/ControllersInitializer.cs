using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Controller {
    public class ControllersInitializer : MonoBehaviour {
        List<IController> _controllersRegister = new List<IController>(){
            //new MainController()
        };

        private void Awake() {
            foreach (var controller in _controllersRegister) {
                controller.Init();
            }
        }
    }
}