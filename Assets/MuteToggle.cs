// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class MuteToggle : MonoBehaviour
// {
//   public Toggle toggle;

//   public void Start()
//   {
//     SyncToggle();
//   }

//   public void Toggle()
//   {
//     AudioManager audioManager = GameSys.Instance.audioManager;
//     audioManager.isMute = !audioManager.isMute;
//     SyncToggle();
//   }

//   void SyncToggle()
//   {
//     bool isMute = GameSys.Instance.audioManager.isMute;
//     if (toggle) toggle.isOn = isMute;
//   }
// }