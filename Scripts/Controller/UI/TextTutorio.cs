using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextTutorio : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float time;
    public Color color;
    // Start is called before the first frame update
    void Start()
    {
        color = text.color;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            color.a -= Time.deltaTime ;
            text.color = color;
        }
    }
}
