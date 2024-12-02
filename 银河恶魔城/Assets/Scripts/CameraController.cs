using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private GameObject cam;

	[SerializeField] private float parallaxEffect;
	private float xPosition;
	private float camxPosition;
	// Start is called before the first frame update
	void Start()
	{
		cam = GameObject.Find("Main Camera");
		camxPosition = cam.transform.position.x;
		xPosition = transform.position.x;
	}


	// Update is called once per frame
	void Update()
    {
		float distanceToMove = (cam.transform.position.x - camxPosition) * parallaxEffect;
		transform.position = new Vector3(xPosition + distanceToMove,transform.position.y);
    }
}
