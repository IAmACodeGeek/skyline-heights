using UnityEngine;

public class SpawnEffect : MonoBehaviour{
    public double spawnTime = 1.0f;
    private double currentTime = 0;

    private Vector2 originalScale;

    void Start(){
        originalScale = transform.localScale;
        transform.localScale = new Vector2(0.1f, 0.1f);
    }

    void Update(){
        currentTime += Time.smoothDeltaTime;

        double scaleFactor = (currentTime / spawnTime);
        transform.localScale = new Vector2((float)scaleFactor * originalScale.x, (float)scaleFactor * originalScale.y);
        
        if(currentTime >= spawnTime){
            transform.localScale = originalScale;
            if(!GetComponent<Block>().isFirstBlock){
                Block.hasSpawned = true;
                gameObject.GetComponent<Block>().state = Block.ACTIVE;
            }
            gameObject.GetComponent<SpawnEffect>().enabled = false;
        }
    }
    
    void LateUpdate(){
        if(gameObject.GetComponent<Block>().isFirstBlock){
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
    }
}
