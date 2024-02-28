using UnityEngine;

public class CameraShake : MonoBehaviour{
    public float shakeDuration = 0.1f;
    public float shakeOffset = 0.1f;
    public float timeFactor = 1f;

    private float state = 0;
    private Vector3 originalPosition;
    
    void Start(){
        originalPosition = transform.localPosition;    
    }

    void Update(){
        if(state > 0){
            transform.localPosition = originalPosition + Random.insideUnitSphere * shakeOffset;
            state -= Time.smoothDeltaTime * timeFactor;
        }
        else{
            state = 0;
            transform.localPosition = originalPosition;
        }
    }

    public void Shake(){
        state = shakeDuration;
    }
}
