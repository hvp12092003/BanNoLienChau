using DG.Tweening;
using UnityEngine;
using System.Collections;

public class objTargetController : MonoBehaviour
{
    public bool onTouchArrow = false;
    public Vector3[] pos;
    public float time;
    public int x = 0;
    private Tween moveTween;
    // Start is called before the first frame update
    void Start()
    {
        if (GMNG.Instance.arrows > 0) OnTweenStepComplete();
    }
    void OnTweenStepComplete()
    {
        if (GMNG.Instance.arrows > 0)
        {
            //Debug.Log(x);
            System.Random random = new System.Random();
            switch (x)
            {
                case 0:
                    x = random.Next(0, 2);
                    if (x == 0) x = 1;
                    else x = 2;
                    break;
                case 1:
                    x = random.Next(0, 2);
                    if (x == 0) x = 0;
                    else x = 2;
                    break;
                case 2:
                    x = random.Next(0, 2);
                    if (x == 0) x = 0;
                    else x = 1;
                    break;
            }

            //moveTween = this.transform.DOMove(pos[x], 5f).SetEase(Ease.InOutSine).OnStepComplete(OnTweenStepComplete);
            moveTween = this.transform.DOMove(pos[x], 5f).SetEase(Ease.Unset).OnStepComplete(OnTweenStepComplete);
        }

    }
}
