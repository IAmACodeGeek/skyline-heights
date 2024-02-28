using UnityEngine;
using UnityEngine.UI;

public class MainMenuLoader : MonoBehaviour{
    public GameObject logo, title, highScore;
    public float loadDuration = 1f;
    private bool isLoading = true;
    private float currentDuration = 0f;
    public Button soundButton;

    void Start(){
        logo.GetComponent<LogoLoad>().Load(loadDuration);
        title.GetComponent<TitleLoad>().Load(loadDuration);
        int score = PlayerPrefs.GetInt(Tags.HIGH_SCORE);
        highScore.GetComponent<HighScoreLoad>().Load(loadDuration, score);

        GameStateController.gsc.initAudioSettings();
    }

    void Update(){
        if(isLoading && currentDuration <= loadDuration){
            currentDuration += Time.smoothDeltaTime;
        }
        else{
            isLoading = false;
        }
    }

    void LateUpdate(){
        if(Input.GetMouseButtonDown(0)){
            if(!isLoading && !IsMouseOverButton(soundButton.gameObject)){
                GameStateController.gsc.LoadScene(Tags.GAME_PLAY_STATE);
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape)){
            QuitGame();
        }
    }

    bool IsMouseOverButton(GameObject obj){
        return obj == UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
    }

    private void QuitGame(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
