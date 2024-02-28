using System;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Block : MonoBehaviour{
    public static string SPAWNING = "Spawning";
    public static string ACTIVE = "Active";
    public static string DROP = "Drop";
    public static string INACTIVE = "Inactive";
    public string state = SPAWNING;
    
    private GameObject mainCamera;

    private Rigidbody2D rigidBody;
    private GameObject[] components = new GameObject[20];

    public ParticleSystem psLeft, psRight, psTop, psDestroy, psScore, psScoreLarge;
    
    [HideInInspector]
    public int[] scoreAdders = {2, 3, 5, 9};
    [HideInInspector]
    public int scoreIndex = 0;

    public AudioSource earthquake, gameOver;

    [HideInInspector]
    public int xDirection = 1;
    public float xSpeed = 10;
    
    private float inactiveX = 0, inactiveY = 0;
    [HideInInspector]
    public int startIndex, endIndex;

    [HideInInspector]
    public bool isFirstBlock = false;
    [HideInInspector]
    public static bool hasSpawned = false;

    [HideInInspector]
    public int score = 0;

    void Awake(){
        mainCamera = GameObject.FindGameObjectWithTag(Tags.MAIN_CAMERA);
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Start(){
        if(startIndex == -1){
            Destroy(gameObject);
        }
        // Get the children of block
        for(int i = 0; i < 20; i++){
            components[i] = gameObject.transform.Find("Square (" + i + ")").gameObject;
        }
        if(startIndex <= endIndex)
            trim();

        // Set the size of the particle system
        ShapeModule shape = psTop.shape;
        shape.scale = new Vector3(0.1f * (endIndex - startIndex + 1), 1, 0.5f);
        float startX = -0.475f + startIndex * 0.05f; float endX = -0.475f + endIndex * 0.05f;
        float midX = (startX + endX);
        shape.position = new Vector3(midX, 0, 0);
    }

    void Update(){
        if(state == Block.ACTIVE){
            // Horizontal Movement
            transform.Translate(new Vector2(xDirection * xSpeed * Time.smoothDeltaTime, 0));
        }
        else if(state == Block.DROP){
            rigidBody.gravityScale = 7;
        }
        else if(state == Block.INACTIVE){
            transform.position = new Vector2(inactiveX, inactiveY);
        }

        if(state != Block.INACTIVE) keepIntact();
    }

    private void setStatic(GameObject other){
        inactiveY = other.transform.position.y + other.transform.localScale.y/2 + gameObject.transform.localScale.y/2;
        inactiveX = transform.position.x;
        Destroy(rigidBody);
        psTop.Stop();
        state = Block.INACTIVE;
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.CompareTag(Tags.SIDE_BOUNDARY)){
            xDirection *= -1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag(Tags.GROUND)){
            if(!isFirstBlock){
                gameOver.Play();
                destroy();
            }
            setStatic(collision.gameObject);     
            mainCamera.GetComponent<CameraShake>().Shake();
            earthquake.Play();
        }
        else if(collision.gameObject.CompareTag(Tags.BLOCK) && state == Block.DROP){
            detatch(collision.gameObject);
            setStatic(collision.gameObject);
            mainCamera.GetComponent<CameraShake>().Shake();
            earthquake.Play();
        }
        else if(collision.gameObject.CompareTag(Tags.GARBAGE_COLLECTOR)){
            if(state == Block.DROP) gameOver.Play();
            destroy();
        }
    }

    private void detatch(GameObject below){
        float xOffset = transform.position.x - below.transform.position.x;
        int magXOffset = (int) (xOffset/0.1);
        
        if(Math.Abs(magXOffset) >= 5){
            scoreIndex = 0;
        }
        else if(Math.Abs(magXOffset) < 5 && Math.Abs(magXOffset) > 1){
            scoreIndex = 1;
        }
        else if(Math.Abs(magXOffset) == 1){
            scoreIndex = 2;
        }
        else{
            scoreIndex = 3;
        }

        psScore.textureSheetAnimation.SetSprite(0, psScore.textureSheetAnimation.GetSprite(scoreIndex + 1));
        psScoreLarge.textureSheetAnimation.SetSprite(0, psScoreLarge.textureSheetAnimation.GetSprite(scoreIndex + 1));
        
        if(magXOffset > 0){
            endIndex -= magXOffset;
            if(startIndex <= endIndex){
                psRight.Play();
                trim();
            }
            else{
                gameOver.Play();
                destroy();
            }
        }
        else if(magXOffset < 0){
            startIndex += -magXOffset;
            if(startIndex <= endIndex){
                psLeft.Play();
                trim();
            }
            else{
                gameOver.Play();
                destroy();
            }
        }
        if(startIndex <= endIndex){
            psRight.transform.localPosition = new Vector2(-0.475f + endIndex * 0.05f, 0);
            psLeft.transform.localPosition = new Vector2(-0.475f + startIndex * 0.05f, 0);
            psScore.Play();
            psScoreLarge.Play();
            updateScore();
        }
        
        transform.position = new Vector2(transform.position.x - xOffset % 0.1f, transform.position.y);
    }

    private void trim(){
        for(int i = 0; i < startIndex; i++){
            if(components[i] != null)
                components[i].SetActive(false);
        }
        for(int i = 19; i > endIndex; i--){
            if(components[i] != null)
                components[i].SetActive(false);
        }
    }

    private void keepIntact(){
        for(int i = startIndex; i <= endIndex; i++){
            if(components[i] != null)
                components[i].transform.localPosition = new Vector2(-0.475f + i * 0.05f, 0);
        }
    }

    public void destroy(){
        startIndex = -1; endIndex = -2;
        for(int i = 0; i < 20; i++){
            if(components[i] != null)
                components[i].SetActive(false);
        }
        psDestroy.Play();
        if(score > PlayerPrefs.GetInt(Tags.HIGH_SCORE)){
            PlayerPrefs.SetInt(Tags.HIGH_SCORE, score);
            PlayerPrefs.Save();
        }
    }

    public void updateScore(){
        score += scoreAdders[scoreIndex];
    }
}
