using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lebaguette_minigame : MonoBehaviour {
    private GameController _game;

    private int _totalClicks;
    private bool _hasWon;
    private bool _hasStarted;

    public void Awake()
    {
        this._game = GameObject.Find("Core").GetComponent<GameController>();
        this._hasStarted = false;
    }

    public void OnEnable() {
        GameController.OnMinigameStart += onMinigameStart;
    }

    public void OnDisable() {
        GameController.OnMinigameStart -= onMinigameStart;
    }

    public void onMinigameStart() {
        this._hasStarted = true;
    }

    public void OnSwipeX(float xDiff) {
        if(xDiff > 0 || this._hasWon || !this._hasStarted) return;

        transform.localPosition = new Vector3(transform.localPosition.x + xDiff*.01f, 2.7f, 0.0f);

        if(transform.localPosition.x < -69){
            this._hasWon = true;
            this._game.wonGame();
        }
    }
}