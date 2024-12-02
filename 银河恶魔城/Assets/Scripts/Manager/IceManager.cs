using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class IceManager : MonoBehaviour
{
    [SerializeField] private Transform[] icePosition;

    [SerializeField] private GameObject icePrefab;

    [SerializeField] private float iceCooldown;
    private float stateTimer;

	private void Update()
	{
        stateTimer -= Time.deltaTime;
		if(stateTimer < 0)
        {
            StartCoroutine(GenerateIce(0.2f));
            stateTimer = iceCooldown;
        }
	}

	private IEnumerator GenerateIce(float _seconds)
    {
        for(int i = 0;i < icePosition.Length;i++) 
        {
            yield return new WaitForSeconds(_seconds);
            Instantiate(icePrefab, icePosition[i].position, Quaternion.identity);
		}
    }
}
