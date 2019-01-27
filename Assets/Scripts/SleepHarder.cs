using System;
using System.Diagnostics.Tracing;
using UnityEngine;

public class SleepHarder : MonoBehaviour
{
    private GameController _game;
    private int score = 0;

    private float previousX = 0;
    private float previousY = 0;

    private GameObject sheepy;
    private Animator sheepAnim;
    private GameObject sleep_peaceful;
    private GameObject sleep_waking;

    private bool _hasStarted;
    private bool _hasWon;

    public void Awake()
    {
        this._game = GameObject.Find("Core").GetComponent<GameController>();

        sheepy = GameObject.Find("sheepy");
        sleep_peaceful = GameObject.Find("sleep");
        sleep_waking = GameObject.Find("sleep_waking");
        sheepAnim = sheepy.GetComponent<Animator>();
        sheepAnim.speed = 0.3f;

        this._hasStarted = false;
        this._hasWon = false;
    }

    public void OnEnable()
    {
        GameController.OnMinigameStart += onMinigameStart;
    }

    public void OnDisable()
    {
        GameController.OnMinigameStart -= onMinigameStart;
    }

    public void onMinigameStart()
    {
        this._hasStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (this._hasWon || !this._hasStarted) return;

        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            var roundX = RoundOff(touch.position.x);
            var roundY = RoundOff(touch.position.y);

            if (previousX != roundX && previousY != roundY)
            {
                previousX = roundX;
                previousY = roundY;
                score++;

                sheepAnim = sheepy.GetComponent<Animator>();
                sheepAnim.speed = 0.3f * (score / 5);
            }
        }

        if (score > 100) {
            this._game.wonGame();
            this._hasWon = true;
        }
    }

    private int RoundOff(float i)
    {
        return ((int)Math.Round(i / 10.0)) * 10;
    }
}
