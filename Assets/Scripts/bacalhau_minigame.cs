using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bacalhau_minigame : MonoBehaviour
{
    private GameController _game;
    private GameObject _bacalhau;
    public List<GameObject> bacalhauSalt;
    public List<GameObject> tableSalt;

    private int _totalShakes;
    private bool _hasWon;
    private bool _hasStarted;

    float LowPassKernelWidthInSeconds = 1.0f;
    private float AccelerometerUpdateInterval = 1.0f / 60.0f;
    private float LowPassFilterFactor;

    private Vector3 lowPassValue = Vector3.zero;
    private Vector3 AndroidAcc;
    private Vector3 AndroidDeltaAcc;

    public void Awake()
    {
        this._game = GameObject.Find("Core").GetComponent<GameController>();

        this.bacalhauSalt = new List<GameObject>();
        this.tableSalt = new List<GameObject>();
        GetAllGameObjects();
        HideTableSalt();

        LowPassFilterFactor = AccelerometerUpdateInterval / LowPassKernelWidthInSeconds;

        this._hasStarted = false;
    }

    public void FixedUpdate()
    {
        if (!this._hasStarted || this._hasWon)
            return;

        AndroidAcc = Input.acceleration;
        AndroidDeltaAcc = AndroidAcc - LowPassFilter(AndroidAcc);

        if (Mathf.Abs(AndroidDeltaAcc.x) >= 2
            || Mathf.Abs(AndroidDeltaAcc.y) >= 2
            || Mathf.Abs(AndroidDeltaAcc.z) >= 2)
        {
            this._totalShakes++;

            ToggleBacalhauSaltShacking();

            if (this._totalShakes >= 100)
            {
                this._hasWon = true;
                this._game.wonGame();
            }
        }
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

    public Vector3 LowPassFilter(Vector3 newSample)
    {
        lowPassValue = Vector3.Lerp(lowPassValue, newSample, LowPassFilterFactor);
        return lowPassValue;
    }

    private void GetAllGameObjects()
    {
        this._bacalhau = GameObject.Find("bacalhau");

        tableSalt.Add(GameObject.Find("Table/salt-5"));
        tableSalt.Add(GameObject.Find("Table/salt-4"));
        tableSalt.Add(GameObject.Find("Table/salt-3"));
        tableSalt.Add(GameObject.Find("Table/salt-2"));
        tableSalt.Add(GameObject.Find("Table/salt-1"));

        bacalhauSalt.Add(GameObject.Find("Bacalhau/salt-5"));
        bacalhauSalt.Add(GameObject.Find("Bacalhau/salt-4"));
        bacalhauSalt.Add(GameObject.Find("Bacalhau/salt-3"));
        bacalhauSalt.Add(GameObject.Find("Bacalhau/salt-2"));
        bacalhauSalt.Add(GameObject.Find("Bacalhau/salt-1"));
    }

    private void HideTableSalt()
    {
        for(int x = 0; x < tableSalt.Count; x++)
        {
            tableSalt[x].SetActive(false);
        }
    }

    private void ToggleBacalhauSaltShacking()
    {
        if (this._totalShakes == 20)
        {
            bacalhauSalt[0].SetActive(false);
            tableSalt[0].SetActive(true);
        }
        else if (this._totalShakes == 40)
        {
            bacalhauSalt[1].SetActive(false);
            tableSalt[1].SetActive(true);
        }
        else if (this._totalShakes == 60)
        {
            bacalhauSalt[2].SetActive(false);
            tableSalt[2].SetActive(true);
        }
        else if (this._totalShakes == 80)
        {
            bacalhauSalt[3].SetActive(false);
            tableSalt[3].SetActive(true);
        }
        else if (this._totalShakes == 100)
        {
            bacalhauSalt[4].SetActive(false);
            tableSalt[4].SetActive(true);
        }
    }
}
