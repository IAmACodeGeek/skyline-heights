using TMPro;
using UnityEngine;

public class HighScoreLoad : MonoBehaviour{
    public float loadDuration = 1.5f;
    private float currentDuration = 0;

    public Vector2 targetPosition;
    private Vector2 fixPosition;
    private RectTransform rectTransform;

    public bool isLoading = false;
    private int score = 0;

    public void Start(){
        rectTransform = GetComponent<RectTransform>();
        fixPosition = rectTransform.localPosition;
    }

    public void Update(){
        if(isLoading && currentDuration < loadDuration){
            currentDuration += Time.smoothDeltaTime;
            rectTransform.localPosition = Vector2.Lerp(fixPosition, targetPosition, currentDuration/loadDuration);
        }
        else{
            isLoading = false;
        }
    }

    public void Load(float duration, int score){
        loadDuration = duration;
        isLoading = true;
        this.score = score;
        GetComponent<TextMeshProUGUI>().SetText("High Score: " + score.ToString());
    }
}
