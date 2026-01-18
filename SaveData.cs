using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData {

    public int level;
    public float masterVolume, sfxVolume, musicVolume, ambientVolume, uiVolume;

    public SaveData(int level, float masterVolume, float sfxVolume, float musicVolume, float ambientVolume,
        float uiVolume) {

        this.level = level;
        this.masterVolume = masterVolume;
        this.sfxVolume = sfxVolume;
        this.musicVolume = musicVolume;
        this.ambientVolume = ambientVolume;
        this.uiVolume = uiVolume;
    }

}
