using UnityEngine;

public class LogoLoad : MonoBehaviour{
    public float loadDuration = 1.5f;
    private float currentDuration = 0;

    public Vector2 targetPosition;
    private Vector2 fixPosition;

    public bool isLoading = false;

    public void Start(){
        fixPosition = transform.position;
    }

    public void Update(){
        if(isLoading && currentDuration < loadDuration){
            currentDuration += Time.smoothDeltaTime;
            transform.position = Vector2.Lerp(fixPosition, targetPosition, currentDuration/loadDuration);
        }
        else{
            isLoading = false;
        }
    }

    public void Load(float duration){
        loadDuration = duration;
        isLoading = true;
    }
}
