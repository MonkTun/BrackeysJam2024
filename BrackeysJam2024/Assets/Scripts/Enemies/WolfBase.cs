using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfBase : MonoBehaviour
{
    public List<WolfEnemy> wolves;
    [SerializeField] private float _maxCallDistance;
    [SerializeField] private GameObject _wolfPrefab;
    [SerializeField] private int _wolfAmount;
    private void Awake()
    {
        for(int i = 0; i < _wolfAmount;i++)
        {
            WolfEnemy w=Instantiate(_wolfPrefab, (Vector2)transform.position + Random.insideUnitCircle, Quaternion.identity).GetComponent<WolfEnemy>();
            wolves.Add(w);
           
            w.baseLocation = transform;
            w.wolfBase = this;
        }
    }
    public void SetPackTarget(WolfEnemy trigger, Vector2 target)
    {
        Debug.Log(trigger.name + " howled!");
        foreach (WolfEnemy w in wolves)
        {
            if (w != null && Vector2.Distance(w.transform.position, trigger.transform.position) < _maxCallDistance)
            {
                w.ReceivePackTarget(trigger, target);
            }
        }
    }
}
