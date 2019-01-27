
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class logic_swipe : MonoBehaviour {

    // Settings
    public float minSwipeMovement;

    private Vector2 swipeStartPos = Vector2.zero;
    private BoxCollider2D _collison;
    private Camera _camera;
    private float tapCD;

    public void Awake()
    {
        this._camera = GameObject.Find("Camera").GetComponent<Camera>();
        this._collison = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    public void FixedUpdate() {
        // If there is no target, ignore
        if (Input.touches.Length <= 0)
            return;
    
        foreach (var touch in Input.touches)
        {  
            TouchPhase phase = touch.phase;
            if (phase == TouchPhase.Began)
            {
                swipeStartPos = touch.position;
                tapCD = Time.time;
            }
            else if (phase == TouchPhase.Moved)
            {
                // Calculate the distance
                float Xdiff = touch.position.x - swipeStartPos.x;
                swipeStartPos = touch.position;

                Vector2 diff = new Vector2(Xdiff, 0);

                if (diff.magnitude < minSwipeMovement)
                    return;

                Vector3 wp = this._camera.ScreenToWorldPoint(touch.position);
                if (!_collison.OverlapPoint(wp))
                    return;

                // We only care about the up and down swipe
                if (diff.y != 0) {
                    gameObject.SendMessage("OnSwipeY", diff.y, SendMessageOptions.DontRequireReceiver);
                }

                if (diff.x != 0)
                {
                    gameObject.SendMessage("OnSwipeX", diff.x, SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }
}