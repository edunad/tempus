using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class logic_fader : MonoBehaviour{
    private SpriteRenderer _renderer;
    private Vector3 _fadeStart, _fadeEnd;

    private bool _isFading;
    private float _journeyLength;
    private float _startTime;
    private Action _onFadeComplete;

    public float fadeSpeed;

    void Awake(){
        this._renderer = GetComponent<SpriteRenderer>();
        this._renderer.enabled = true; // Just in case
    }

    public void triggerFade(bool fadeIn, Vector3 fadeStart, Vector3 fadeEnd, Action onComplete = null)
    {
        if (this._isFading) return;

        this._fadeStart = fadeStart;
        this._fadeEnd = fadeEnd;

        this._renderer.flipX = fadeIn;

        this.transform.localPosition = fadeStart;

        this._journeyLength = Vector3.Distance(fadeStart, fadeEnd);
        this._startTime = Time.time;

        this._onFadeComplete = onComplete;
        this._isFading = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!this._isFading) return;

        float distCovered = (Time.time - this._startTime) * this.fadeSpeed;
        float fracJourney = distCovered / _journeyLength;

        this.transform.localPosition = Vector3.Lerp(this._fadeStart, this._fadeEnd, fracJourney);

        if (fracJourney >= 1)
        {
            this._isFading = false;
            if (_onFadeComplete != null)
                this._onFadeComplete();
        }

    }
}