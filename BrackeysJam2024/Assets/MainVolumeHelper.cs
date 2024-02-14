using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class MainVolumeHelper : MonoBehaviour
{
    public MMSoundManager.MMSoundManagerTracks targetTrack;
    public void SetMainVolume(float val)
    {
        MMSoundManager.Current.SetTrackVolume(targetTrack, val);
    }
}
