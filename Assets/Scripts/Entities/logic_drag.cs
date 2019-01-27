
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class logic_drag : MonoBehaviour {

    private BoxCollider2D _collider;
    private Camera _camera;

    public static int isDragging;
    public static bool canDrag;
    private float originalZ;

    public void Awake()
    {
        this._camera = GameObject.Find("Camera").GetComponent<Camera>();
        this._collider = GetComponent<BoxCollider2D>();

        this.originalZ = this.transform.position.z;
    }

    public void FixedUpdate() {
        if (!logic_drag.canDrag) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            Vector2 TouchPos = touch.position;
            Vector3 wp = this._camera.ScreenToWorldPoint(TouchPos);

            if (!_collider.OverlapPoint(wp) && this.GetInstanceID() != logic_drag.isDragging)
            {
                return;
            }
                
            if(logic_drag.isDragging != 0)
            {
                if (this.GetInstanceID() != logic_drag.isDragging) return;
            }
            
            if (touch.phase == TouchPhase.Began)
            {
                logic_drag.isDragging = GetInstanceID();
            }
            else if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {
                Vector3 touchedPos = this._camera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));
                touchedPos.z = this.originalZ;

                transform.position = touchedPos;
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                logic_drag.isDragging = 0;
            }
        }
    }
}