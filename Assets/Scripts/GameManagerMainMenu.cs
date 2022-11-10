using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerMainMenu : MonoBehaviour
{
    [SerializeField] Canvas mainMenu;
    [SerializeField] Canvas settingMenu;
    [SerializeField] GameObject connectPanel;
    [SerializeField] TMP_Dropdown dropdown;
    private void Awake(){
        mainMenu.enabled = true;
        settingMenu.enabled = false;
        connectPanel.SetActive(false);
    }
    public void LoadScene(int sceneIndex){
        SceneManager.LoadScene(sceneIndex);
    }
    public void LoadScene(string sceneName){
        SceneManager.LoadScene(sceneName);
        Debug.Log("Anda sedang di halaman " + sceneName);
    }
    public void SettingGame(){
        mainMenu.enabled = false;
        settingMenu.enabled = true;
        Debug.Log("Tombol Setting Ditekan");
    }
    public void GetOptionResolusi(int value){
        Debug.Log("Resolusi Sekarang: " + dropdown.options[value].text);
    } 
    public void GetConnect(){
        Debug.Log("Running API Connection..");
    } 
    public void BackMenu(){
        mainMenu.enabled = true;
        settingMenu.enabled = false;
        Debug.Log("Tombol Back Ditekan");
    }
    public void QuitGame(){
        Application.Quit();
        Debug.Log("Tombol Exit Ditekan");
    }
}
