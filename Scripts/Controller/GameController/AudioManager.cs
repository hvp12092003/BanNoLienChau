using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AudioManager : MonoBehaviour
{
    public static AudioManager AuM;

    public bool onAudio;

    [SerializeField] TextMeshProUGUI btnSoundName = null;

    public AudioSource audioMain;
    public AudioSource audioPlusScore;
    public AudioSource audioArrow;
    // Start is called before the first frame update

   

    void Awake()
    {
        AuM = this;
        DontDestroyOnLoad(this.gameObject);

    }


    private void Update()
    {
        TogSound();
    }

    public void TogSound()
    {
        if(btnSoundName != null)
        {
            if (onAudio)
            {
                btnSoundName.text = "ÂM THANH";
            }
            else
            {
                btnSoundName.text = "TẮT ÂM";
            }
        }
    }

    public void SoundToggle(bool newVal)
    {
        onAudio = newVal;
        if (onAudio)
        {
            audioMain.Play();

        }
        else
        {
            audioMain.Stop();

        }
    }

    public void PlayAudioMain()
    {
        audioMain.Play();
    }
    public void PlayAudioPlusScore()
    {
        if (onAudio) audioPlusScore.Play();
    }
    public void PlayAudioArrow()
    {
        if (onAudio) audioArrow.Play();
    }
}
