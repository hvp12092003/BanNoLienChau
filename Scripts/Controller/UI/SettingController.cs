using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
public class SettingController : MonoBehaviour
{
    private void Awake()
    {
       // QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 80;
    }

    public void UpdateAudio()
    {
    }

    public void ResetRankList()
    {
        DataType dataSave = new DataType();
        dataSave.scores = new float[10];
        for (int i = 0; i < dataSave.scores.Length; i++)
        {
            dataSave.scores[i] = 0;
        }
        string json = JsonUtility.ToJson(dataSave);
        File.WriteAllText(Application.streamingAssetsPath + "/RankList.json", json);
    }
    public void ButtonPlay()
    {
        SceneManager.LoadScene(1);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            SceneManager.LoadScene(1);
    }
}
