using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShowFPS : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _Fps;
    float fps;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("GetFPS", 1, .5f);
    }


    void GetFPS()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);
        _Fps.text = "FPS: " + fps.ToString();
    }
}
