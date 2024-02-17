using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using UnityEngine.UI;

public class MainVolumeHelper : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private void Awake()
    {
        if (slider == null) { slider = GetComponent<Slider>(); }
        slider.value = MMSoundManager.Current.GetTrackVolume(targetTrack,false);
    }
    public MMSoundManager.MMSoundManagerTracks targetTrack;
    public void SetMainVolume(float val)
    {
        MMSoundManager.Current.SetTrackVolume(targetTrack, val);
    }
}
