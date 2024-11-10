using System.Collections.Generic;
using Helper;
using UnityEngine;

namespace Panel
{
     public class PanelManager : SingletonMonobehaviour<PanelManager>
    {
        private List<PanelInstanceModel> _listInstances = new List<PanelInstanceModel>();

        private ObjectPool _objectPool;

        private void Start() {
            _objectPool = ObjectPool.Instance;
            Debug.Log("Creata an instance of object pool");
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
                panelInstance.SetActive(true);
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

        public void DestroyLastPanel()
        {
            if (AnyPanelShowing())
            {
                var lastPanel = _listInstances[_listInstances.Count - 1];
                _listInstances.Remove(lastPanel);
                // Destroy the last panel instead of pooling it
                Destroy(lastPanel.PanelInstance);

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

        public PanelInstanceModel GetLastPanel()
        {
            return _listInstances.Count > 0 ? _listInstances[_listInstances.Count - 1] : null;
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
}