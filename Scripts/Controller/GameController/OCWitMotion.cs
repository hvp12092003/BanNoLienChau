using UnityEngine;
using System.Diagnostics;
using System.Collections;
using System;

public class OCWitMotion : MonoBehaviour
{
    private Process process;
    public float deylayBeforClosing = 7f;
    // Start is called before the first frame update
    void Start()
    {
         string filePath = @"C:\DATA\WitMotion(V2.6.1.0)\WitMotion.exe";
        //string filePath = @"C:\DATA\WORK\unity\Sensor\Cambiengoc\WitMotion New Software\WitMotion(V2.6.1.0)\WitMotion.exe";

        //process = new Process();
        //process.StartInfo.FileName = filePath;

        //process.Start();

        //StartCoroutine(CloseAppAfterDelay());
    }

    IEnumerator CloseAppAfterDelay()
    {
        yield return new
            WaitForSeconds(deylayBeforClosing);
        process.Kill();
        process.WaitForExit();
       // CloseExternalApp();
    }
    void CloseExternalApp()
    {
        try
        {
            if (process != null && !process.HasExited)
            {
                process.Kill();
                process.WaitForExit();
                UnityEngine.Debug.Log("Close App");
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e.Message);
        }
    }

}
