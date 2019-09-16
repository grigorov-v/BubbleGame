using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonReload : MonoBehaviour {
    [SerializeField] Button _button = null;

    private void Awake() {
        _button.onClick.AddListener(Reload);    
    }

    public void Reload() {
        var curScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(curScene);
    }
}
