using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePanelButton : MonoBehaviour
{
    private PanelManager _panelManager;

    void Start()
    {
        _panelManager = PanelManager.Instance;
    }

    public void DoHidePanel()
    {
        Debug.Log("The button is clicked");
        _panelManager.HideLastPanel();
    }
}
