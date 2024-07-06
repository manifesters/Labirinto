using UnityEngine;

public class ShowPanelButton : MonoBehaviour
{
    public string PanelID;

    public PanelShowBehaviour Behaviour;

    private PanelManager _panelManager;

    void Start()
    {
        _panelManager = PanelManager.Instance;
    }

    public void DoShowPanel()
    {
        Debug.Log("The button is clicked");
        _panelManager.ShowPanel(PanelID, Behaviour);
    }
}
