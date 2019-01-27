
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {
    private Camera _camera;

    public void Awake() {
        this._camera = GetComponent<Camera>();
    }

    /* ************* 
     * CAMERA CONTROLS
     ===============*/
    public void Update() {
        if (this._camera == null) return;
    }
}