using UnityEngine;

public class ContainerMovement : MonoBehaviour{
    private Vector2 targetPosition, fixPosition;
    public bool isMoving = false;
    private float currentTime = 0;
    public float moveDuration = 1.5f;

    public void Update(){
        if(isMoving){
            currentTime += Time.smoothDeltaTime;
            transform.position = Vector2.Lerp(fixPosition, targetPosition, currentTime/moveDuration);
        }
        if(currentTime >= moveDuration){
            stopMoving();
        }
    }
    
    public void startMoving(Vector2 position){
        fixPosition = transform.position;
        targetPosition = position;
        isMoving = true;
        currentTime = 0;
    }

    public void stopMoving(){
        isMoving = false;
        transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
        Block.hasSpawned = true;
    }

    public void LateUpdate(){
        if(isMoving){
            Block.hasSpawned = false;
        }
    }
}
