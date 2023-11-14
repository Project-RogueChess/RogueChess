using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDBManager : MonoBehaviour
{
    [SerializeField] string CreepStageDB_CSV_Path = "CreepStageDB";

    static List<StageDB> list = new List<StageDB>();

    private void Awake()
    {
        ReadStageData(CreepStageDB_CSV_Path);
    }
    private void ReadStageData(string stageDBPath)
    {
        List<Dictionary<string, object>> stageData = new List<Dictionary<string, object>>();

        stageData = CSVReader.Read(stageDBPath);

        for (int i = 0; i < stageData.Count; i++)
        {
            StageDB stageDB = new StageDB();

            stageDB.stage = (int)stageData[i]["stage"];
            stageDB.id = (int)stageData[i]["creep"];
<<<<<<< HEAD
            stageDB.x = (float)stageData[i]["x"];
            stageDB.y = (float)stageData[i]["y"];
=======
            stageDB.x = (int)stageData[i]["x"];
            stageDB.y = (int)stageData[i]["y"];
>>>>>>> ddb6a9d63217bfcce53cb6fe2757965760d11437

            list.Add(stageDB);
        }
    }
}
