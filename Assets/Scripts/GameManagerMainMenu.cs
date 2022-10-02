using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerMainMenu : MonoBehaviour
{
    private void Awake(){
        #if UNITY_STANDALONE
            Screen.SetResolution(540, 960, false);
            Screen.fullScreen = false;
        #endif
    }
    public void LoadScene(int sceneIndex){
        SceneManager.LoadScene(sceneIndex);
    }
    public void QuitGame(){
        Application.Quit();
    }
}
