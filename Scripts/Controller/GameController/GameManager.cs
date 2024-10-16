using UnityEngine;
using LTAUnityBase.Base.DesignPattern;
using TMPro;
using System;
using System.IO;
using SimpleJSON;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Unity.VisualScripting;
public class GameManager : MonoBehaviour
{
    //0.88 +50
    //1.122 +40 
    //1.425 +30
    //1.788 +20
    //2.146 +30
    //++ +1
    public float arrows;
    public float scores = 0;

    public TextMeshProUGUI textCore;
    public TextMeshProUGUI textArrows;

    public GameObject panelRankList;
    public GameObject bow;
    public GameObject modleBow;
    [SerializeField]  GameObject wall;
    private GameObject stringBow;
    public GameObject target;

    public DataType dataSave;

    public float distance = 0;
    private float count;
    private float count2;
    private float count3;
    private bool end = false;
    public bool moveTarget;
    private GameObject audio_game;
    private AudioManager audioManager;
    private void Start()
    {
        audio_game = GameObject.FindGameObjectWithTag("Audio");
        audioManager = audio_game.GetComponent<AudioManager>();

        dataSave = new DataType();

        textCore = GameObject.FindGameObjectWithTag("textScores").GetComponent<TextMeshProUGUI>();
        textArrows = GameObject.FindGameObjectWithTag("textArrows").GetComponent<TextMeshProUGUI>();
        textCore.text = "0";
        textArrows.text = arrows.ToString();
        LoadDataType();
        //SaveDataType();
    }
    private void Update()
    {

        if (moveTarget)
        {
            MoveTarget();
        }
        else
        {
            ShowRankList();
        }
    }
    public void Timer()
    {
        count += Time.deltaTime;
        if (Input.anyKeyDown) count = 0;
        if (count >= 30) SceneManager.LoadScene(0);
    }
    public void UpdateScores()
    {
        textCore.text = scores.ToString();
    }
    public void UpdateArrows()
    {
        arrows--;
        textArrows.text = arrows.ToString();
    }
    public void ShowRankList()
    {
        if (arrows == 0)
        {
            count2 += Time.deltaTime;
            if (count2 >= 3)
            {
                bow.transform.DOLocalRotate(new Vector3(0f, 0f, 0f), 1f);
                modleBow.SetActive(false);
                panelRankList.SetActive(true);
                wall.SetActive(false);
            }
        }
    }
    public void MoveTarget()
    {
        if (arrows == 0) count3 += Time.deltaTime;

        if (count3 >= 3)
        {
            bow.transform.DOLocalRotate(new Vector3(0f, 0f, 0f), 1f);
            target.transform.DOMove(new Vector3(0, 5, 9), 1f);
            ShowRankList();
        }
    }
    public void LoadDataType()
    {
        string filePath = Application.streamingAssetsPath + "/RankList.json";
        if (File.Exists(filePath))
        {
            string path = File.ReadAllText(filePath);

            JSONNode data = JSONNode.Parse(path);

            dataSave = JsonUtility.FromJson<DataType>(data.ToString());

            if (dataSave != null && dataSave.scores != null)
            {
                foreach (int value in dataSave.scores)
                {
                }
            }
            else
            {
                Debug.LogError("Failed to load array from JSON.");
            }
        }
        else
        {
            Debug.LogError("JSON file not found.");
        }
    }
    public void SaveDataType()
    {
        string json = JsonUtility.ToJson(dataSave);
        File.WriteAllText(Application.streamingAssetsPath + "/RankList.json", json);
    }
}
public class GMNG : SingletonMonoBehaviour<GameManager> { }
