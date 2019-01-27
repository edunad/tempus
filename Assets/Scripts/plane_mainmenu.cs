using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class plane_mainmenu : MonoBehaviour {
    [Header("Counter")]
    public TextMeshPro counter;

    [Header("Background")]
    public SpriteRenderer planeSprite;
    public logic_moveTo plane;
    public logic_moveTo cloud;
    public logic_moveTo background;
    public Animator explosionAnim;

    [Header("Hearts")]
    public logic_heart heart_1;
    public logic_heart heart_2;
    public logic_heart heart_3;

    public logic_fader fader;

    public TextMeshPro faster;
    public AudioClip fasterClip;

    private AudioSource _audioSource;
    private AudioSource _bgAudioSource;

    private AudioClip _audioClip_win;
    private AudioClip[] _audioClip_lost;

    private int _lifes;
    private float _speed;

    // Start is called before the first frame update
    public void Awake() {
        this._audioSource = GetComponent<AudioSource>();
        this._audioSource.volume = 0.5f;
        this._audioSource.playOnAwake = false;

        this._bgAudioSource = GameObject.Find("background").GetComponent<AudioSource>();

        this._lifes = PlayerPrefs.GetInt("lifes", 3);
        this._speed = PlayerPrefs.GetFloat("minigameSpeed", 0);

        this.faster.text = "";
        this.explosionAnim.speed = 0f;
        logic_drag.canDrag = true;
        logic_drag.isDragging = 0;

        this._audioClip_win = AssetsController.GetResource<AudioClip>("Sounds/plane");

        this._audioClip_lost = new AudioClip[] {
            AssetsController.GetResource<AudioClip>("Sounds/explosion_1"),
            AssetsController.GetResource<AudioClip>("Sounds/explosion_2"),
            AssetsController.GetResource<AudioClip>("Sounds/explosion_3")
        };
    }

    public void Start() {
        this.counter.text = this.generateCounter(PlayerPrefs.GetInt("minigameRound", 0));
        this.setHearts();

        this.fader.triggerFade(true, new Vector3(0.375f, 0.17f, 1), new Vector3(-4f, 0.17f, 1f), () => {
            this.startScene();
        });
    }

    private void increaseSpeed(){
        this._speed += 0.5f;
        PlayerPrefs.SetFloat("minigameSpeed", this._speed);

        this._bgAudioSource.Stop();
        this._bgAudioSource.PlayOneShot(this.fasterClip);
        this.faster.text = "FASTER!!";
    }

    private void setHearts() {
        if (this._lifes == 2) {
            this.heart_1.setBadHeart();
        } else if (this._lifes == 1) {
            this.heart_1.setBadHeart();
            this.heart_2.setBadHeart();
        }
        else if (this._lifes <= 0)
        {
            this.heart_1.setBadHeart();
            this.heart_2.setBadHeart();
            this.heart_3.setBadHeart();
        }
    }

    private void startScene() {
        GameStatus lastStatus = (GameStatus)PlayerPrefs.GetInt("minigameStatus", (int)GameStatus.NO_STATUS);

        this.plane.start();
        this.cloud.start();
        this.background.start();

        if (lastStatus == GameStatus.LOST_STATUS && this._lifes - 1 < 0)
        {
            util_timer.Simple(1f, () => {
                this.planeSprite.color = new Color(0f, 0f, 0f, 0f);
                this._audioSource.PlayOneShot(this._audioClip_lost[2]);
                this.explosionAnim.speed = 1f;

                util_timer.Simple(1f, () =>
                {
                    SceneManager.LoadScene("mainmenu");
                });
            });
        }
        else
        {
            util_timer.UniqueSimple("effect", 1f, () =>
            {
                int count = PlayerPrefs.GetInt("minigameRound", 0) + 1;

                this.counter.text = this.generateCounter(count);
                PlayerPrefs.SetInt("minigameRound", count); // Save it

                if (lastStatus == GameStatus.LOST_STATUS)
                {
                    this._audioSource.PlayOneShot(this._audioClip_lost[Random.Range(0, this._audioClip_lost.Length - 1)]);
                    this.explosionAnim.speed = 1f;

                    this._lifes -= 1;
                    this.setHearts();
                }
                else if (lastStatus == GameStatus.WON_STATUS)
                {
                    this.explosionAnim.speed = 0f;
                    this._audioSource.PlayOneShot(this._audioClip_win);
                }
                else
                {
                    this._audioSource.PlayOneShot(this._audioClip_win);
                }

                if (count % 4 == 0)
                {
                    this.increaseSpeed(); // FASTER!!!
                }
            });

            util_timer.UniqueSimple("nextMinigame", 4.3f, () =>
            {
                PlayerPrefs.SetInt("lifes", this._lifes);

                List<string> minigames = this.getMinigames();
                SceneManager.LoadScene(minigames[Random.Range(0, minigames.Count)]);
            });
        }
    }

    private List<string> getMinigames(){
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        List<string> minigames = new List<string>(); // Reset
        string lastMinigame = PlayerPrefs.GetString("lastMinigame", "");

        for (int i = 0; i < sceneCount; i++) {
            string scene = SceneUtility.GetScenePathByBuildIndex(i);
            if (scene.IndexOf("minigames") == -1) continue;
            if (lastMinigame != "" && scene.IndexOf(lastMinigame) != -1) continue;

            minigames.Add(scene);
        }

        return minigames;
    }

    /* ************* 
     * TIMELINE
     ===============*/
    public void FixedUpdate()
    {
        util_timer.Update();
    }

    public void OnDestroy()
    {
        util_timer.Clear();
    }

    private string generateCounter(int count)
    {
        string strCount = "000";
        if (count < 10) strCount = "00" + count;
        else if (count >= 10 || count < 100) strCount = "0" + count;
        else strCount = count.ToString();

        return strCount;
    }
}
