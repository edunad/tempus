
using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour {
    private GameObject _clockPointer;
    private logic_timer _clock;
    private SpriteRenderer _clockSprite;

    private GameObject _uiTitle;
    private TextMeshPro _uiTitleMesh;

    private game_settings _settings;

    public void Awake() {
        this._clockPointer = GameObject.Find("ui_bomb_timer");
        this._clock = this._clockPointer.GetComponent<logic_timer>();
        this._clockSprite = this._clockPointer.GetComponent<SpriteRenderer>();

        this._uiTitle = GameObject.Find("ui_title");
        this._uiTitleMesh = this._uiTitle.GetComponent<TextMeshPro>();
    }

    public void OnEnable() {
        GameController.OnMinigameStart += onMinigameStart;
        GameController.OnMinigameAwake += onMinigameAwake;
        GameController.OnMinigameStatusUpdate += onMinigameStatusUpdate;
    }

    public void OnDisable() {
        GameController.OnMinigameStart -= onMinigameStart;
        GameController.OnMinigameAwake -= onMinigameAwake;
        GameController.OnMinigameStatusUpdate -= onMinigameStatusUpdate;
    }

    public void onMinigameStatusUpdate(GameStatus status) {
        switch(status)
        {
            case GameStatus.LOST_STATUS:
                this._clockSprite.color = new Color(0.90f, 0.29f, 0.23f);
                this._uiTitleMesh.text = "FAILURE";
                this._uiTitleMesh.color = new Color(0.90f, 0.29f, 0.23f);

                break;

            case GameStatus.WON_STATUS:
                this._clockSprite.color = new Color(0.15f, 0.68f, 0.37f);
                this._uiTitleMesh.text = "VICTORY";
                this._uiTitleMesh.color = new Color(0.15f, 0.68f, 0.37f);

                this._clock.hideCountdown();
                break;
        }
    }

    public void onMinigameStart() {
        this._clockSprite.color = new Color(1f, 1f, 1f);
        this._clock.startTimer(this._settings.maxTime, true);
    }

    public void onMinigameAwake(game_settings settings) {
        this._settings = settings;

        // Setup
        this._uiTitleMesh.text = settings.title;

        this._clockSprite.color = new Color(0.94f, 0.76f, 0.05f);
        this._clock.startTimer(settings.warmup);
    }
}