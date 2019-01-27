

using TMPro;
using UnityEngine;

public class logic_timer : MonoBehaviour {

    private AudioSource _source;
    private AudioClip[] _audioClips;
    private TextMeshPro _countdownText;

    private float _targetTime;
    private float _target;
    private bool _enabled;
    private bool _displayCountdown;
    private int _lastAnnounce;

    public void Awake()
    {
        this._countdownText = GameObject.Find("ui_countdown").GetComponent<TextMeshPro>();
        this._source = GetComponent<AudioSource>();
        this._source.playOnAwake = false;

        this._audioClips = new AudioClip[] {
            AssetsController.GetResource<AudioClip>("Sounds/countdown_dos_sec1"),
            AssetsController.GetResource<AudioClip>("Sounds/countdown_dos_sec2"),
            AssetsController.GetResource<AudioClip>("Sounds/countdown_dos_sec3"),
            AssetsController.GetResource<AudioClip>("Sounds/countdown_dos_sec4"),
            AssetsController.GetResource<AudioClip>("Sounds/countdown_dos_sec5")
        };
    }

    public void startTimer(float time, bool displayCountdown = false) {
        this._targetTime = time + Time.time;
        this._target = time;

        this._displayCountdown = displayCountdown;
        this._lastAnnounce = 0;

        this._countdownText.text = "";
        this._enabled = true;
    }

    public void hideCountdown() {
        if (!this._displayCountdown) return;
        this._displayCountdown = false;
        this._countdownText.text = "";
    }

    public void stopTimer(){
        this._enabled = true;
        this.transform.eulerAngles = Vector3.zero;
        this.hideCountdown();
    }

    public void FixedUpdate() {
        if (!this._enabled) return;
        if (Time.time >= this._targetTime) {
            this.stopTimer();
            return;
        }

        float remaining = this._targetTime - Time.time;

        this.announceTime((int)remaining);
        this.transform.eulerAngles = new Vector3(0, 0, (remaining * 360 ) / this._target);
    }

    private void announceTime(int seconds) {
        if (!this._displayCountdown || seconds > 5) return;
        if (this._lastAnnounce == seconds) return;

        this._lastAnnounce = seconds;
        this._countdownText.text = seconds > 0 ? seconds.ToString(): "";

        if (seconds > 0) {
            this._source.PlayOneShot(this._audioClips[seconds - 1]);
        }
    }
}
