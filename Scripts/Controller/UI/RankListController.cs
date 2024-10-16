using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class RankListController : MonoBehaviour
{
    public GameObject[] boxRank;
    public TextMeshProUGUI[] score;

    public TextMeshProUGUI box11_score;
    public GameObject box11;

    public float rank, count = 5;
    public GameObject audio_game;
    public AudioManager audioManager;
    private void Start()
    {

        audio_game = GameObject.FindGameObjectWithTag("Audio");
        audioManager = audio_game.GetComponent<AudioManager>();

        //audio

        if (GMNG.Instance.scores < GMNG.Instance.dataSave.scores[0])
        {
            box11.gameObject.SetActive(true);
            box11.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.5f).SetLoops(-1, LoopType.Yoyo);
            box11_score.text = GMNG.Instance.scores.ToString();
            ShowText();
        }
        else
        {
            GMNG.Instance.dataSave.scores[0] = GMNG.Instance.scores;
            Array.Sort(GMNG.Instance.dataSave.scores);

            for (int i = 0; i < GMNG.Instance.dataSave.scores.Length; i++)
            {
                if (GMNG.Instance.scores > GMNG.Instance.dataSave.scores[i]) rank--;
            }
            ShowText();
            ScaleBoxRank();
        }

    }
    public void ShowText()
    {

        for (int i = 0; i < GMNG.Instance.dataSave.scores.Length; i++)
        {
            score[i].text = GMNG.Instance.dataSave.scores[i].ToString();
            GMNG.Instance.SaveDataType();
        }
    }
    public void ScaleBoxRank()
    {
        for (int i = 0; i < GMNG.Instance.dataSave.scores.Length; i++)
        {
            if (GMNG.Instance.scores >= GMNG.Instance.dataSave.scores[i]) rank = i;
        }
        boxRank[(int)rank].transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.5f).SetLoops(-1, LoopType.Yoyo);
    }
    public void Update()
    {
        count -= Time.deltaTime;
        if (count <= 0) SceneManager.LoadScene(1);
    }
}
