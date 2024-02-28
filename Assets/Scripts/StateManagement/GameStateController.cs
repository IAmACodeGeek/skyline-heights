using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateController : MonoBehaviour{
    public static GameStateController gsc;

    public AudioSource bgm;
    private bool audio_on = false;
    public Sprite on, off;
    public Button soundButton;

    void Awake(){
        if(gsc == null){
            gsc = this;
            bgm = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);

            audio_on = (PlayerPrefs.GetInt(Tags.AUDIO) == 0)? false : true;
            initAudioSettings();
        }
        else{
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }

    public void LoadScene(string sceneName){
        if(sceneName == Tags.MAIN_MENU_STATE){
            bgm.Stop();
            bgm.Play();
        }
        SceneManager.LoadScene(sceneName);
    }

    public void clickSoundButton(){
        audio_on = !audio_on;
        initAudioSettings();
    }

    public void initAudioSettings(){
        if(soundButton == null){
            GameObject btn = GameObject.FindGameObjectWithTag(Tags.SOUND_BUTTON);
            soundButton = btn.GetComponent<Button>();
            soundButton.onClick.AddListener(clickSoundButton);
        }
        if(audio_on){
            soundButton.image.sprite = on;
            bgm.volume = 0.1f;
            PlayerPrefs.SetInt(Tags.AUDIO, 1);
        }
        else{
            soundButton.image.sprite = off;
            bgm.volume = 0;
            PlayerPrefs.SetInt(Tags.AUDIO, 0);
        }
        PlayerPrefs.Save();
    }
}
