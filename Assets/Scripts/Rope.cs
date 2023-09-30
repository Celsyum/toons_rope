using UnityEngine;

public class Rope : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent onRopeAnimationDone;

    SpriteRenderer spriteRenderer;
    
    float height = 0f;
    public float speed = 1f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();        
    }

    public void Animate(Vector3 start, Vector3 end)
    {
        height = Vector3.Distance(start, end);
        end.z = 0;
        start.z = 0;
        //rotate towards end
        Vector3 dir = start - end;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);
        
        Vector3 pos = transform.position;
        pos.z = transform.parent.position.z;
        transform.localPosition = pos;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spriteRenderer.size.y < height)
        {
            float ht = spriteRenderer.size.y + speed * Time.deltaTime;
            if (ht > height)
            {
                onRopeAnimationDone.Invoke();
                ht = height;
            }
            spriteRenderer.size = new Vector2(spriteRenderer.size.x, ht);
        }
    }

}
