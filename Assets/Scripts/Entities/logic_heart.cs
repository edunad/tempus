using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class logic_heart : MonoBehaviour {
    public Sprite okSprite;
    public Sprite badSprite;

    private SpriteRenderer _render;
    public void Awake() {
        this._render = GetComponent<SpriteRenderer>();
        this._render.sprite = okSprite;
    }

    public void setBadHeart() {
        this._render.sprite = badSprite;
    }
}
