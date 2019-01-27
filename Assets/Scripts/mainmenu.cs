using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainmenu : MonoBehaviour
{
    public void Awake() {
        PlayerPrefs.DeleteAll(); // Clear   
    }

    public void OnUIClick() {
        SceneManager.LoadScene("minigame_selection");
    }
}
