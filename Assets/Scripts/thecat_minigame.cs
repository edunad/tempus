using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thecat_minigame : MonoBehaviour {
    public AudioClip clip;

    private GameController _game;
    private Animator _animator;
    private AudioSource _catScream;

    private int _totalClicks;
    private bool _hasWon;
    private bool _hasStarted;

    public void Awake()
    {
        this._game = GameObject.Find("Core").GetComponent<GameController>();
        this._animator = GetComponent<Animator>();
        this._animator.SetInteger("status", 0);

        this._catScream = GetComponent<AudioSource>();
        this._catScream.playOnAwake = false;
        this._catScream.volume = 0.5f;

        this._hasStarted = false;
    }

    public void OnEnable() {
        GameController.OnMinigameStart += onMinigameStart;
    }

    public void OnDisable() {
        GameController.OnMinigameStart -= onMinigameStart;
    }

    public void onMinigameStart() {
        // ANGRY
        this._animator.SetInteger("status", 1);
        this._hasStarted = true;
        this._catScream.Play();

        this._game.pauseMusic();
    }

    public void OnUIClick() {
        if (!this._hasStarted || this._hasWon) return;
        this._totalClicks++;

        this.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
        this._catScream.PlayOneShot(clip);

        if (this._totalClicks >= 10) {
            this._animator.SetInteger("status", 2);

            this._hasWon = true;
            this._game.wonGame();

            this._catScream.Stop();
            this._game.playMusic();
        }
    }
}
