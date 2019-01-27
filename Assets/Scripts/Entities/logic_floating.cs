
using UnityEngine;

public class logic_floating : MonoBehaviour
{
    [Header("Time")]
    public float timeOffset = 0f;

    [Header("Timing Y")]
    public float ySpeed = 2f;
    public float yOffset = 0.0005f;

    [Header("Timing X")]
    public float xSpeed = 2f;
    public float xOffset = 0f;
    
    private Vector3 _originalPos;
    public void Update() {
        Vector3 pos = this.transform.localPosition;

        float y = Mathf.Sin((Time.time + timeOffset) * this.ySpeed) * this.yOffset;
        float x = Mathf.Cos((Time.time + timeOffset) * this.xSpeed) * this.xOffset;

        this.transform.localPosition = new Vector3(pos.x + x, pos.y + y, pos.z);
    }
}