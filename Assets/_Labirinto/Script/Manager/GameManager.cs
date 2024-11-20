using DataPersistence;
using Helper;
using UnityEngine;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    public override void Awake()
    {
        base.Awake();
    }

    public void Start()
    {
        DataPersistenceManager.Instance.LoadPlayer();

        if (!DataPersistenceManager.Instance.HasPlayerData())
        {
            Debug.Log("No player data found. Creating new player data.");
            DataPersistenceManager.Instance.NewPlayerData();
            DataPersistenceManager.Instance.SavePlayer();
        }
    }
}
