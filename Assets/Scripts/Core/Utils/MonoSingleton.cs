using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Utils {
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour {
        static T _instance = null;

        public static T Instance {
            get {
                _instance = !_instance ? FindObjectOfType<T>() : _instance;
                return _instance;
            }
        }
    }
}