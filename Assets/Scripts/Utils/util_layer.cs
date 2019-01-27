

using UnityEngine;

[ExecuteInEditMode]
public class util_layer : MonoBehaviour
{
    [Header("Sorting settings")]
    public string sortingLayer;
    public int orderSortingLayer;

    private MeshRenderer _mesh;
    public void Start()
    {
        this._mesh = GetComponent<MeshRenderer>();
        this._mesh.sortingLayerName = this.sortingLayer;
        this._mesh.sortingOrder = orderSortingLayer;
    }
}