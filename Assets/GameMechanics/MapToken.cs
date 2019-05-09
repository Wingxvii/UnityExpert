using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapToken : MonoBehaviour
{
    public bool stayOnMap = false;
    public Sprite icon;

    MiniMap map;
    RectTransform tokenRect;
    bool isActive;

    void Awake()
    {
        map = FindObjectOfType<MiniMap>();

        GenerateToken();
    }

    void LateUpdate()
    {
        if (isActive)
            tokenRect.localPosition = map.GetMapCoordinates(this.transform.position, stayOnMap);
    }

    private void GenerateToken()
    {
        GameObject obj = new GameObject();
        obj.transform.SetParent(map.gameObject.transform, false);
        Image sr = obj.AddComponent<Image>();
        sr.sprite = icon;

        tokenRect = obj.GetComponent<RectTransform>();
        if (tokenRect == null)
        {
            tokenRect = obj.AddComponent<RectTransform>();
        }

        tokenRect.sizeDelta = map.tokenSize * Vector2.one;
        SetTokenActive(false);
    }

    public void SetTokenActive(bool enabled)
    {
        isActive = enabled;
        tokenRect.gameObject.SetActive(enabled);
    }
}