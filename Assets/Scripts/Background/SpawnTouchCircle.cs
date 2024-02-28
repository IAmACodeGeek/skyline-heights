using UnityEngine;

public class SpawnTouchCircle : MonoBehaviour{
    public GameObject touchCircle;

    void Update(){
        if(Input.GetMouseButtonDown(0)){
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject newTouchCircle = Instantiate(touchCircle, new Vector3(worldPosition.x, worldPosition.y, -1), Quaternion.identity);
        }
    }
}
