using DG.Tweening;
using UnityEngine;
using static UnityEngine.UI.Image;

public class LineRender : MonoBehaviour
{
    public bool updateDone = true;
    public LineRenderer lineRenderer;
    public GameObject stringPowR, stringPowL, stringCenter;
    public GameObject endPoint, originPoint;

    public Vector3 origin;
    void Start()
    {
        lineRenderer = this.GetComponent<LineRenderer>();
        stringCenter = GameObject.FindGameObjectWithTag("StringCenter");
        stringPowR = GameObject.FindGameObjectWithTag("StringBowR");
        stringPowL = GameObject.FindGameObjectWithTag("StringBowL");

        endPoint = GameObject.FindGameObjectWithTag("EndPoint");
        originPoint = GameObject.FindGameObjectWithTag("OriginPoint");

        origin = stringCenter.transform.position;
        UpdateEndPonit();
    }
    private void Update()
    {
        lineRenderer.positionCount = 3;
        lineRenderer.SetPosition(0, stringPowR.transform.position);
        lineRenderer.SetPosition(1, stringCenter.transform.position);
        lineRenderer.SetPosition(2, stringPowL.transform.position);

        if (updateDone) stringCenter.transform.position = endPoint.transform.position;
    }
    public void UpdateOriginPoint()
    {
        stringCenter.transform.DOMove(new Vector3(origin.x, origin.y, origin.z), 0.1f);
    }
    public void UpdateEndPonit()
    {
        updateDone = false;
        stringCenter.transform.DOMove(new Vector3(endPoint.transform.position.x, endPoint.transform.position.y, endPoint.transform.position.z), 1).OnComplete(() =>
        {
            updateDone = true;
        });
    }
}
