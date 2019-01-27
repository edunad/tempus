using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class logic_button : MonoBehaviour {
    [Header("Button Settings")]
    public float tapCooldown = 0.3f;
    public bool isEnabled = true;

    private BoxCollider2D _collision;
    private Camera _uiCamera;
    private float _lastTap;

    public void Awake() {
        this._uiCamera = GameObject.Find("HUDCamera").GetComponent<Camera>();
        this._collision = GetComponent<BoxCollider2D>();
        this._collision.isTrigger = true;
    }

    public void Update() {
        if (!isEnabled) return;
        if (this._uiCamera == null || Input.touches.Length <= 0)
            return;
        
        foreach (var touch in Input.touches)
        {
            Vector2 pos = touch.position;
            Vector2 startPos = this._uiCamera.WorldToScreenPoint(this.transform.position);
            TouchPhase phase = touch.phase;

            if (phase == TouchPhase.Began)
            {
                this._lastTap = Time.time;
            }
            else if (phase == TouchPhase.Ended || phase == TouchPhase.Canceled)
            {
                if (Time.time > this._lastTap)
                {
                    if (this._collision.bounds.Contains(startPos))
                    {
                        this.SendMessage("OnUIClick", SendMessageOptions.DontRequireReceiver);
                        Debug.Log("Clcik");
                    }
                }
            }
        }
    }

    public void OnMouseDown() {
        if (!isEnabled || Time.time < this._lastTap) return;

        this._lastTap = Time.time + this.tapCooldown;
        this.SendMessage("OnUIClick", SendMessageOptions.DontRequireReceiver);
    }
}
