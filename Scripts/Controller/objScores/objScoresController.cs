using UnityEngine;
using DG.Tweening;
public class objScoresController : MonoBehaviour
{
    public float y;
    public float initialAlpha = 1f;
    public float count = 0;
    public SpriteRenderer ren;
    public UnityEngine.Color color;
    // Start is called before the first frame update
    void Start()
    {
        ren = this.GetComponent<SpriteRenderer>();
        color = ren.color;
        color.a = initialAlpha;
    }
    // Update is called once per frame
    void Update()
    {
        color.a -= Time.deltaTime/2;
        ren.color = color;
        count += Time.deltaTime;
        if (count > 5) Destroy(this.gameObject);

        this.transform.DOMoveY(y, 2f);
    }
}
