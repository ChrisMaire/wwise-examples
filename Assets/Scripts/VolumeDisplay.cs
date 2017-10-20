using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeDisplay : MonoBehaviour {
    private Image image;

	void Awake () {
        image = GetComponent<Image>();
        AudioController.Instance.OnVolumeChanged.AddListener(VolumeChanged);
    }

    void VolumeChanged()
    {
        image.fillAmount = AudioController.Instance.Volume / 100f;
    }
}
