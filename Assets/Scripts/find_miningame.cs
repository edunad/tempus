using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class find_miningame : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> hideSpots = new List<GameObject>();
    public GameObject keys;
    public AudioClip clip;

    private AudioSource _audio;
    private BoxCollider2D _giveArea;
    private BoxCollider2D _keyArea;

    private GameController _game;
    private bool _hasStarted;
    private bool _hasWon;

    public void Awake()
    {
        this._game = GameObject.Find("Core").GetComponent<GameController>();
        this._giveArea = GetComponent<BoxCollider2D>();
        this._audio = GetComponent<AudioSource>();

        this._keyArea = this.keys.GetComponent<BoxCollider2D>();

        GameObject obj = hideSpots[Random.Range(0, hideSpots.Count)];
        keys.transform.position = obj.transform.position;
        
        logic_drag.canDrag = false;
    }

    public void OnEnable() {
        GameController.OnMinigameStart += onMinigameStart;
    }

    public void OnDisable() {
        GameController.OnMinigameStart -= onMinigameStart;
    }

    public void onMinigameStart() {
        this._hasStarted = true;

        logic_drag.isDragging = 0;
        logic_drag.canDrag = true;
    }

    public void Update(){
        if (!this._hasStarted || this._hasWon) return;

        if (this._giveArea.bounds.Intersects(this._keyArea.bounds)) {
            this._hasWon = true;
            this._audio.PlayOneShot(this.clip);
            this._game.wonGame();
        }
    }
}
