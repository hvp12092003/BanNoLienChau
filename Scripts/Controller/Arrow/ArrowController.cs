using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ArrowController : MonoBehaviour
{
    public float[] zone;
    public GameObject[] objScore;
    public float count = 0;
    public bool launch = false;
    //public GameObject audio_game;
    //public AudioManager audioManager;
    public Rigidbody rg;
    public BoxCollider box;
    GameManager gameManager;
    private void Start()
    {
        box = this.gameObject.GetComponent<BoxCollider>();
        rg = this.gameObject.GetComponent<Rigidbody>();
        //audio_game = GameObject.FindGameObjectWithTag("Audio");
        //audioManager = audio_game.GetComponent<AudioManager>();
    }
    private void Update()
    {
       
    }
    public void TimeDestroy()
    {
        if (!launch) return;
        count += Time.deltaTime;
        if (count >= 4) Destroy(this.gameObject);
    }
    public void setBox(Collision other)
    {
        //Debug.Log(123);
        this.transform.SetParent(other.transform);
        rg.isKinematic = true;
        box.enabled = false;
    }
    private void OnCollisionEnter(Collision other)
    {
        //Debug.Log(other.gameObject.tag);
        //if (other.gameObject.tag != "Target") return;

        //audioManager.PlayAudioPlusScore();
        AudioManager.AuM.PlayAudioPlusScore();
        GMNG.Instance.distance = Vector3.Distance(this.transform.position, other.transform.position);

        switch (other.gameObject.tag)
        {
            case "50":
                setBox(other);

                GMNG.Instance.scores += 50;
                Instantiate(objScore[1], this.transform.position, Quaternion.identity);
                GMNG.Instance.UpdateScores();
                break;
            case "40":
                setBox(other);
                GMNG.Instance.scores += 40;
                Instantiate(objScore[2], this.transform.position, Quaternion.identity);
                GMNG.Instance.UpdateScores();
                break;
            case "30":
                setBox(other);
                GMNG.Instance.scores += 30;
                Instantiate(objScore[3], this.transform.position, Quaternion.identity);
                GMNG.Instance.UpdateScores();
                break;
            case "20":
                setBox(other);
                GMNG.Instance.scores += 20;
                Instantiate(objScore[4], this.transform.position, Quaternion.identity);
                GMNG.Instance.UpdateScores();
                break;
            case "10":
                setBox(other);
                GMNG.Instance.scores += 10;
                Instantiate(objScore[5], this.transform.position, Quaternion.identity);
                GMNG.Instance.UpdateScores();
                break;
            case "1":
                setBox(other);
                GMNG.Instance.scores += 1;
                Instantiate(objScore[0], this.transform.position, Quaternion.identity);
                GMNG.Instance.UpdateScores();
                break;
        }       
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Target")
        {
            //Debug.Log(Vector3.Distance(this.transform.position, other.transform.position));
        }
    }

}
