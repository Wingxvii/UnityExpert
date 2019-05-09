using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform origin;
    public float scale;
    public float tokenSize = 10f;
    public Vector2 offset;

    RectTransform rectTrans;

    void Awake()
    {
        rectTrans = this.GetComponent<RectTransform>();
    }

    public Vector2 GetMapCoordinates(Vector3 worldPosition, bool keepInside = false)
    {
        Vector2 mapCoord = worldPosition - origin.position;

        if (keepInside)
        {
            mapCoord = Vector2.Min(mapCoord, rectTrans.rect.max);
            mapCoord = Vector2.Max(mapCoord, rectTrans.rect.min);
        }

        return scale * (mapCoord + offset);
    }
}
