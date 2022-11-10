using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteToggle : MonoBehaviour
{
    public Toggle toggle;
    // Start is called before the first frame update
    void Start()
    {
        SyncAudio();
    }

    public void SetMute(bool value)
    {
        AudioManager manager = GameSystem.Instance.audioManager;
        manager.Mute = value;
    }

    void SyncAudio()
    {
        AudioManager manager = GameSystem.Instance.audioManager;
        toggle.isOn = manager.Mute;
    }
}
