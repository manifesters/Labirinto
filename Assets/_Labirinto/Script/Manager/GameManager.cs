using System.IO;
using DataPersistence;
using Helper;
using LootLocker.Requests;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    public override void Awake()
    {
        base.Awake();
    }

    public void Start()
    {
        if (!DataPersistenceManager.Instance.HasPlayerData())
        {
            DataPersistenceManager.Instance.NewPlayerData();
        }
    }
}
