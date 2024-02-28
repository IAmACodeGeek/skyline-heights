using UnityEngine;

public class TouchCircle : MonoBehaviour{
    private float currentTime = 0;
    public float duration = 0.5f;
    public float scaleSize = 2f;

    private SpriteRenderer sprite;
    public Color startColor;
    public Color endColor;

    void Awake(){
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update(){
        if(currentTime <= duration){
            transform.localScale = new Vector2(currentTime/duration * scaleSize, currentTime/duration * scaleSize);
            currentTime += Time.smoothDeltaTime;
            sprite.color = Color.Lerp(startColor, endColor, currentTime/duration);
        }
        else{
            Destroy(gameObject);
        }
    }
}
