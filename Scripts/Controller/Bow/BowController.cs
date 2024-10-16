using UnityEngine;
using DG.Tweening;
using System.Collections;

public class BowController : MonoBehaviour
{
    public float sacleArrows;
    private bool instantArrows = true;
    private bool launch = false;
    public float rotationSpeed;
    public float forceMagnitude;
    public float cooldown;
    private float count = 0;

    private GameObject arrows;
    public GameObject poniArrows;
    private GameObject arrowsPrefabs;
    private Rigidbody rgArrows;
    private BoxCollider boxArrows;
    private ArrowController arrowScrip;

    [SerializeField] SensorController sensorController;
    private GameObject stringBowR;
    private LineRender lineRenderScrip;
    public Vector3 newDirection;



    public float mouseSensitivityX = 5.0f;


    const float rotY = -0.19f;


    private void Start()
    {
        stringBowR = GameObject.FindGameObjectWithTag("StringBowR");
        lineRenderScrip = stringBowR.GetComponent<LineRender>();
        sensorController = GameObject.FindGameObjectWithTag("SensorCtrl").GetComponent<SensorController>();
        arrowsPrefabs = Resources.Load<GameObject>("Prefabs/Arrows");
        CreatArrows();
    }
    // Update is called once per frame
    private void Update()
    {
        if (!launch && GMNG.Instance.arrows > 0)
        {

            BowRotate();
            //TestBow();
            //TestBow2();


            //event shot
            if (Input.GetKeyDown(KeyCode.P))
            {
                AudioManager.AuM.PlayAudioArrow();
                lineRenderScrip.UpdateOriginPoint();
                arrowScrip.launch = true;
                boxArrows.enabled = true;
                rgArrows.AddForce(arrows.transform.up * forceMagnitude, ForceMode.Impulse);

                arrows.transform.DOScale(new Vector3(sacleArrows, sacleArrows, sacleArrows), 0.2f);

                launch = true;
                GMNG.Instance.UpdateArrows();
            }
        }
        else if (launch)
        {
            count += Time.deltaTime;
            if (count >= cooldown)
            {
                count = 0;
                launch = false;
                CreatArrows();
            }
        }
    }
    private void BowRotate()
    {
        if (-sensorController.angle <= -270 || -sensorController.angle >= -90)
        {

            Vector3 currentDirection = transform.forward;
            Vector3 targetDirection = Quaternion.Euler(0, -sensorController.angle, 0) * Vector3.forward;
            Vector3 newDirection = Vector3.RotateTowards(currentDirection, targetDirection, rotationSpeed * Mathf.Deg2Rad * Time.deltaTime, 0);
            // transform.rotation = Quaternion.LookRotation(newDirection);
            transform.DORotateQuaternion(Quaternion.LookRotation(newDirection), 0.1f).SetEase(Ease.Linear);
        }

    }
    private void CreatArrows()
    {
        lineRenderScrip.UpdateEndPonit();
        arrows = Instantiate(arrowsPrefabs, poniArrows.transform.position, Quaternion.identity);

        arrowScrip = arrows.GetComponent<ArrowController>();

        arrows.transform.SetParent(poniArrows.transform, true);
        arrows.transform.localScale = Vector3.one;
        arrows.transform.localRotation = Quaternion.identity;

        rgArrows = arrows.GetComponent<Rigidbody>();
        boxArrows = arrows.GetComponent<BoxCollider>();
        boxArrows.enabled = false;
    }

    void TestRotate()
    {
        if (Input.GetMouseButton(1))
        {
            float rotX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivityX;
            transform.localEulerAngles = new Vector3(-rotY, rotX, 0.0f);
        }
    }

    void TestBow()
    {
        float rotX = sensorController.angle;
        //transform.rotation = Quaternion.Euler(rotY, -rotX, 0f);

        Quaternion currentRot = transform.rotation;
        Quaternion targetRot = Quaternion.AngleAxis(-rotX, Vector3.up);

        float smooth = 10 * (1f - Mathf.Exp(-Time.deltaTime));
        transform.rotation = Quaternion.Slerp(currentRot, targetRot, smooth);
    }

    void TestBow2()
    {
        float rotX = sensorController.angle;
        Debug.Log(-rotX); //-90 0 -360 -270  
        if (-rotX <= -270 || -rotX >= -90)
        {
            Quaternion currentRot = transform.rotation;
            Quaternion targetRot = Quaternion.AngleAxis(-rotX, Vector3.up);


            float smooth = 10 * (1f - Mathf.Exp(-Time.deltaTime));
            transform.rotation = Quaternion.Slerp(currentRot, targetRot, smooth);
        }


    }

}