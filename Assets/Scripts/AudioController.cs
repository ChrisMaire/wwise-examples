using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioController : MonoBehaviour {
    public float Volume = 0f;
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
    }

#region volume

    public void VolumeUp()
    {
        ShiftVolume(10f);
    }

    public void VolumeDown()
    {
        ShiftVolume(-10f);
    }

    private void ShiftVolume(float delta)
    {
        SetVolume(GetVolume() + delta);
    }

    private void SetVolume(float value)
    {
        //AkSoundEngine.SetRTPCValue("Volume", value, null);
        AkSoundEngine.SetRTPCValue(AK.GAME_PARAMETERS.VOLUME, value, null);
        Volume = value;
        OnVolumeChanged.Invoke();
    }

    public float GetVolume()
    {
        float v = 0.5f;
        int vtype = (int)RTPCValue_type.RTPCValue_Global;
        uint pid = (uint)RTPCValue_type.RTPCValue_PlayingID;
        AkSoundEngine.GetRTPCValue(AK.GAME_PARAMETERS.VOLUME, gameObject, pid, out v, ref vtype);

        return v;
    }
#endregion
}
