using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStatus {
    NO_STATUS = 0,
    LOST_STATUS = 1,
    WON_STATUS = 2
}

[System.Serializable]
public struct game_settings
{
    public string title;
    public string description;
    public float warmup;
    public float maxTime;
}

public class GameController : MonoBehaviour
{
    [Header("Minigame Settings")]
    [SerializeField]
    public game_settings settings;

    // EVENTS
    public delegate void onMinigameStart();
    public static event onMinigameStart OnMinigameStart;

    public delegate void onMinigameAwake(game_settings settings);
    public static event onMinigameAwake OnMinigameAwake;

    public delegate void onMinigameStatusUpdate(GameStatus status);
    public static event onMinigameStatusUpdate OnMinigameStatusUpdate;

    private GameStatus status;
    
    private float _timer;

    private AudioSource _soundEffect;
    private AudioSource _musicBackground;

    private AudioClip[] _audioClip_win;
    private AudioClip[] _audioClip_lost;

    public void Awake()
    {
        #region Game_Settings
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        #endregion

        // Get
        this._musicBackground = GetComponent<AudioSource>();
        this._musicBackground.playOnAwake = true;
        this._musicBackground.volume = 1f;

        this._soundEffect = GetComponentInChildren<AudioSource>();
        this._soundEffect.playOnAwake = false;
        this._soundEffect.volume = 0.5f;

        this._audioClip_win = new AudioClip[] {
            AssetsController.GetResource<AudioClip>("Sounds/local_exo_won1"),
            AssetsController.GetResource<AudioClip>("Sounds/local_exo_won2"),
            AssetsController.GetResource<AudioClip>("Sounds/local_exo_won3")
        };

        this._audioClip_lost = new AudioClip[] {
            AssetsController.GetResource<AudioClip>("Sounds/local_lose2"),
            AssetsController.GetResource<AudioClip>("Sounds/local_lose3"),
            AssetsController.GetResource<AudioClip>("Sounds/local_lose4")
        };
    }

    public void Start() {
        // Start
        this.status = GameStatus.NO_STATUS;

        float speed = PlayerPrefs.GetFloat("minigameSpeed", 0);
        this.settings.maxTime = this.settings.maxTime - speed;
        if (this.settings.maxTime <= 1f) this.settings.maxTime = 1;

        this.settings.warmup = this.settings.warmup - speed;
        if (this.settings.warmup <= 0.5f) this.settings.warmup = 0.5f;

        if (OnMinigameAwake != null) OnMinigameAwake(this.settings);
        util_timer.UniqueSimple("warmup", this.settings.warmup, () => {
            if (OnMinigameStart != null) OnMinigameStart();


            util_timer.UniqueSimple("gameDone", this.settings.maxTime, () => {
                if (!this.hasWon()) lostGame();
                
                util_timer.UniqueSimple("gameEnd", 1f, () => {
                    PlayerPrefs.SetString("lastMinigame", SceneManager.GetActiveScene().name);
                    SceneManager.LoadScene("minigame_selection");
                });
            });
        });
    }

    public void pauseMusic() {
        if (!this._musicBackground.isPlaying) return;
        this._musicBackground.Pause();
    }

    public void playMusic() {
        if (this._musicBackground.isPlaying) return;
        this._musicBackground.UnPause();
    }

    public void wonGame() {
        if (this.status == GameStatus.LOST_STATUS ||
            this.status == GameStatus.WON_STATUS) return;

        this._soundEffect.PlayOneShot(this._audioClip_win[Random.Range(0, this._audioClip_win.Length)]);
        setGameStatus(GameStatus.WON_STATUS);
    }

    public void lostGame() {
        if (this.status == GameStatus.LOST_STATUS) return;

        this._soundEffect.PlayOneShot(this._audioClip_lost[Random.Range(0, this._audioClip_lost.Length)]);
        setGameStatus(GameStatus.LOST_STATUS);
    }

    private void setGameStatus(GameStatus status) {
        this.status = status;

        PlayerPrefs.SetInt("minigameStatus", (int)status);
        if (OnMinigameStatusUpdate != null) OnMinigameStatusUpdate(status);
    }

    private bool hasWon()  {
        return this.status == GameStatus.WON_STATUS;
    }

    /* ************* 
     * TIMELINE
     ===============*/
    public void FixedUpdate() {
        util_timer.Update();
    }

    public void OnDestroy() {
        util_timer.Clear();
    }
}
