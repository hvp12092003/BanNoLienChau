using UnityEngine;
using UnityEngine.SceneManagement;
public class UIcontroller : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            SceneManager.LoadScene(2);
    }

    //private void Awake()
    //{
    //    PlayAudioMain();
    //}

    //public void PlayAudioMain()
    //{
    //    //voice = AudioManager.AuM.onAudio;
    //    if (AudioManager.AuM.onAudio)
    //    {
    //        AudioManager.AuM.PlayAudioMain();
    //        Debug.Log("Bat am ne");
    //    }
            
    //}
}
