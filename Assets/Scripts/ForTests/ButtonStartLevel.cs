using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class ButtonStartLevel : MonoBehaviour {
    [SerializeField] Button _btn       = null;
    [SerializeField] string _nameScene = "CandyWorld";

    private void Awake() {
        _btn.onClick.AddListener(() => {
            SceneManager.LoadScene(_nameScene);
        });
    }
}
