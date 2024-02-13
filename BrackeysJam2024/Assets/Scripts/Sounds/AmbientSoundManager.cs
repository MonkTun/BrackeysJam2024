using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class AmbientSoundManager : MonoBehaviour
{
    [SerializeField] private float _minTimeBetweenAmbiance;
    [SerializeField] private float _maxTimeBetweenAmbiance;
    private float _currentTimeBetweenAmbiance;
    private float _timeOfLastAmbiance = 0;
    [SerializeField] private MMF_Player _mmfPlayer;    
    // Start is called before the first frame update
    void Start()
    {
        if (_mmfPlayer == null) { _mmfPlayer = GetComponent<MMF_Player>(); }
        _currentTimeBetweenAmbiance = Random.Range(_minTimeBetweenAmbiance, _maxTimeBetweenAmbiance);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - _timeOfLastAmbiance>_currentTimeBetweenAmbiance) {
            _mmfPlayer.PlayFeedbacks();
            _currentTimeBetweenAmbiance = Random.Range(_minTimeBetweenAmbiance, _maxTimeBetweenAmbiance);

        }
    }
}
