using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PanelManager : BasicSingleton<PanelManager>
{
    private List<PanelInstanceModel> _listInstances = new List<PanelInstanceModel>();

    private ObjectPool _objectPool;

    private void Start() {
        _objectPool = ObjectPool.Instance;
    }

    public void ShowPanel(string panelID, PanelShowBehaviour behaviour = PanelShowBehaviour.KEEP_PREVIOUS)
    {
        GameObject panelInstance = _objectPool.GetObjectFromPool(panelID);

        if (panelInstance != null)
        {
            if (behaviour == PanelShowBehaviour.HIDE_PREVIOUS && GetAmountPanelsInQueue() > 0)
            {
                var lastPanel = GetLastPanel();
                if(lastPanel != null)
                {
                    lastPanel.PanelInstance.SetActive(false);
                }
            }

            _listInstances.Add(new PanelInstanceModel
            {
                PanelID = panelID,
                PanelInstance = panelInstance
            });
            Debug.Log($"Panel {panelID} instantiated and added to the queue");
        }
        else
        {
            Debug.LogWarning($"Panel ID {panelID} not found");
        }
    }

    public void HideLastPanel()
    {
        if(AnyPanelShowing())
        {
            var lastPanel = _listInstances[_listInstances.Count - 1];
            _listInstances.Remove(lastPanel);
            _objectPool.PoolObject(lastPanel.PanelInstance);

            if (GetAmountPanelsInQueue() > 0)
            {
                lastPanel = GetLastPanel();
                if (lastPanel != null && !lastPanel.PanelInstance.activeInHierarchy)  
                {
                    lastPanel.PanelInstance.SetActive(true);
                }
            }
        }
    }

    PanelInstanceModel GetLastPanel()
    {
        return _listInstances[_listInstances.Count - 1];
    }

    public bool AnyPanelShowing()
    {
        return GetAmountPanelsInQueue() > 0;
    }

    public int GetAmountPanelsInQueue()
    {
        return _listInstances.Count;
    }
}
