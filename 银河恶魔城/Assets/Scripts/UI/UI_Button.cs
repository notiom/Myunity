using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Button : MonoBehaviour
{
    [SerializeField] private UI ui;
    [SerializeField] private GameObject menu;

    public void clickButtonSwitchTo() => ui.SwitchWithKeyTo(menu);
}
