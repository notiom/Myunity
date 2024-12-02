using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{

     public virtual void AdjustPosition()
    {
		Vector2 mousePosition = Input.mousePosition;

		float xOffset = 0;
		float yOffset = 0;
		if (mousePosition.x > Screen.width / 2)
		{
			xOffset = -Screen.width / 10;
		}
		else
		{
			xOffset = Screen.width / 10;
		}

		if (mousePosition.y > Screen.height / 2)
		{
			yOffset = -Screen.height / 5;
		}
		else
		{
			yOffset = Screen.height / 5;
		}

		// ui.skillToolTip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
	}
}
