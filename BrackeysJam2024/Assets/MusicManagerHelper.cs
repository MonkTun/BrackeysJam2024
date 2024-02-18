using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class MusicManagerHelper : MonoBehaviour
{
    public static MusicManagerHelper instance;
    public List<EnemyBase> enemies;
    private bool wasLastFrameAggroed;
    public MMF_Player standardMusicPlayer;

    public MMF_Player combatMusicPlayer;
    private float standardMusicPlayerVolume=0.5f;
    [SerializeField] private float volumeShiftingSpeed;
    private MMF_MMSoundManagerSound _standardMusic;
    public bool aggroedEnemyExists;
    private void Awake()
    {
        _standardMusic = standardMusicPlayer.GetFeedbackOfType<MoreMountains.Feedbacks.MMF_MMSoundManagerSound>();
        instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        aggroedEnemyExists = false;
        foreach (EnemyBase e in enemies) { if (e.currentState == EnemyState.aggressive) { aggroedEnemyExists = true;break; } }

        standardMusicPlayerVolume = Mathf.MoveTowards(standardMusicPlayerVolume, (aggroedEnemyExists ? 0f : 0.5f), volumeShiftingSpeed * Time.deltaTime);
        _standardMusic.MaxVolume = standardMusicPlayerVolume;
        _standardMusic.MinVolume = standardMusicPlayerVolume;
        if (!wasLastFrameAggroed && aggroedEnemyExists)
        {
            combatMusicPlayer.PlayFeedbacks();
        }
        else if (!aggroedEnemyExists)
        {
            Invoke("StopCombatMusic", 3f);
        }
        wasLastFrameAggroed = aggroedEnemyExists;
    }
    private void StopCombatMusic()
    {
        combatMusicPlayer.StopFeedbacks();
    }
    
}
