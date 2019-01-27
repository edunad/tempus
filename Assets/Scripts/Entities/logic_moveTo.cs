using UnityEngine;

public class logic_moveTo : MonoBehaviour{

    [Header("Settings")]
    public Vector2 offset; 
    public float speed;

    private Vector3 _originalPos;
    private Vector3 _endPos;

    private float _startTime;
    private bool _enabled;

    public void Awake() {
        this._originalPos = this.transform.position;
        this._endPos = this._originalPos + new Vector3(offset.x, offset.y, 0);
    }

    /* ************* 
     * EVENTS + TIME
     ===============*/

    public void start() {
        this.transform.position = this._originalPos;

        this._startTime = 0f;
        this._enabled = true;
    }

    public void Update()
    {
        // Pretty sure lerp could do this, but it doesn't want to talk with me :(
        if (this._enabled && this._startTime < 1f) this._startTime += speed;
        else if (!this._enabled && this._startTime > 0f) this._startTime -= speed;
        else return;

        this._startTime = Mathf.Clamp(this._startTime, 0f, 1f);
        this.transform.position = Vector3.Lerp(this._originalPos, this._endPos, this._startTime);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(this.transform.position, new Vector3(0.1f, 0.1f, 0.1f));

        Gizmos.color = Color.red;
        Vector3 endPoint = this.transform.position + new Vector3(offset.x, offset.y, 0);
        Gizmos.DrawLine(this.transform.position, endPoint);
        Gizmos.DrawCube(endPoint, new Vector3(0.1f, 0.1f, 0.1f));
    }
}