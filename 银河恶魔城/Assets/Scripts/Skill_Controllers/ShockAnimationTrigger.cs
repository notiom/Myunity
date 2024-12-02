using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockAnimationTrigger : MonoBehaviour
{
	private void AnimationTrigger() => GetComponentInParent<ShockStrike_Controller>().DamageAndSelfDestory();
 
}
