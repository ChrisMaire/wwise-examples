using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeDisplay : MonoBehaviour {
    public bool Mus;

    private Image image;

	void Awake () {
        image = GetComponent<Image>();
        AudioController.Instance.OnVolumeChanged.AddListener(VolumeChanged);
    }

    void Start()
    {
        VolumeChanged();
    }

    void VolumeChanged()
    {
        if(Mus)
        {
            image.fillAmount = AudioController.Instance.MusVolume / 100f;
        }
        else
        {
            image.fillAmount = AudioController.Instance.Volume / 100f;
        }
    }
}
