using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode_Trigger : MonoBehaviour
{ 
	private Enemy_Night enemy => GetComponentInParent<Enemy_Night>();

	private void ExplodeTrigger() => enemy.Explode();

}
