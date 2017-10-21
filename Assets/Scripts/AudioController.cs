using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioController : MonoBehaviour {
    public float Volume = 0f;
    public float MusVolume = 0f;

    public float VolumeDeltaPerClick = 10f;
    private float volumeStart = 50f;

    public UnityEvent OnVolumeChanged = new UnityEvent();

    public static AudioController Instance
    {
        get { return instance; }
    }
    private static AudioController instance;

    void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(this);
            return;
        }

        instance = this;
    }

	void Start () {
        SetVolume(volumeStart);
        SetMusVolume(volumeStart);
    }

#region volume

    public void VolumeUp()
    {
        ShiftVolume(VolumeDeltaPerClick);
    }

    public void VolumeDown()
    {
        ShiftVolume(-VolumeDeltaPerClick);
    }

    private void ShiftVolume(float delta)
    {
        SetVolume(GetVolume() + delta);
    }

    private void SetVolume(float value)
    {
        //AkSoundEngine.SetRTPCValue("Volume", value, null);
        AkSoundEngine.SetRTPCValue(AK.GAME_PARAMETERS.MAINVOLUME, value, null);
        Volume = value;
        OnVolumeChanged.Invoke();
    }

    public float GetVolume()
    {
        float v = 0.5f;
        int vtype = (int)RTPCValue_type.RTPCValue_Global;
        uint pid = (uint)RTPCValue_type.RTPCValue_PlayingID;
        AkSoundEngine.GetRTPCValue(AK.GAME_PARAMETERS.MAINVOLUME, gameObject, pid, out v, ref vtype);

        return v;
    }

    public void MusVolumeUp()
    {
        ShiftMusVolume(VolumeDeltaPerClick);
    }

    public void MusVolumeDown()
    {
        ShiftMusVolume(-VolumeDeltaPerClick);
    }

    private void ShiftMusVolume(float delta)
    {
        SetMusVolume(GetMusVolume() + delta);
    }

    private void SetMusVolume(float value)
    {
        //AkSoundEngine.SetRTPCValue("Volume", value, null);
        AkSoundEngine.SetRTPCValue(AK.GAME_PARAMETERS.MUSICVOLUME, value, null);
        MusVolume = value;
        OnVolumeChanged.Invoke();
    }

    public float GetMusVolume()
    {
        float v = 0.5f;
        int vtype = (int)RTPCValue_type.RTPCValue_Global;
        uint pid = (uint)RTPCValue_type.RTPCValue_PlayingID;
        AkSoundEngine.GetRTPCValue(AK.GAME_PARAMETERS.MUSICVOLUME, gameObject, pid, out v, ref vtype);

        return v;
    }

    public void MusicLowPass()
    {
        AkSoundEngine.PostEvent(AK.EVENTS.MUSICLOWPASS, gameObject);
    }

    public void ResetMusicBus()
    {
        AkSoundEngine.PostEvent(AK.EVENTS.MUSICRESET, gameObject);
    }
    #endregion
}
