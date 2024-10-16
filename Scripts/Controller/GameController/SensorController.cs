using UnityEngine;
using IMU;
using System.IO.Ports;
using static IMU.HWT101CT;
public class SensorController : MonoBehaviour
{
    private HWT101CT imuSensor;
    [SerializeField] string Portname = null;
    public GameObject bow;
    public string[] PortnameArray;
    public float angle;
    bool connected;
    public float rotationSpeed;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        PortnameArray = new string[] { "COM3", "COM4", "COM5", "COM6" };
        imuSensor = new HWT101CT();
        ConnectToIMUSensor();
    }

    async void ConnectToIMUSensor()
    {
        for (int i = 0; i < PortnameArray.Length; i++)
        {
            // Replace "COMX" with the appropriate COM port name
            connected = await imuSensor.ConnectComport(PortnameArray[i]);
            if (connected)
            {
                Portname = PortnameArray[i];
                await imuSensor.PollData(HandleIMUData);
                break;
            }
        }
    }
    // Callback method to handle IMU data
    void HandleIMUData(object sender, float yawAngle)
    {
        angle = yawAngle;
        Debug.Log("Yaw Angle: " + yawAngle );
    }
    void OnDestroy()
    {
        imuSensor.ClosePort();
    }
}

