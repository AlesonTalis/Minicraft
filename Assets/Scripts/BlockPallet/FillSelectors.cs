using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FillSelectors : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerDownHandler
{
    public int width = 1024;
    public int height = 512;

    public RectTransform box;

    public RectTransform bottomLeft, bottomRight, topLeft, topRight;

    bool inside = false;
    Vector2 mousePos;

    void Start()
    {

    }

    void Update()
    {
        box.gameObject.SetActive(inside);

        if (inside is false) return;

        Vector2 start = bottomLeft.position;
        Vector2 end = topRight.position;
        Vector2 size = end - start;

        Vector2 mouse = mousePos - start;

        Vector2 snap = new Vector2(((size.x / 1024f) * 16), ((size.y / 512) * 16));
        Vector2 mouseSnap = new Vector2()
        {
            x = (mouse.x / snap.x) * snap.x - (mouse.x % snap.x),
            y = (mouse.y / snap.y) * snap.y - (mouse.y % snap.y),
        };

        box.position = start + (mouseSnap);
    }

    Vector2Int GetPositionInsideImageNormalized()
    {
        Vector2 start = bottomLeft.position;
        Vector2 end = topRight.position;
        Vector2 size = end - start;

        Vector2 mouse = mousePos - start;

        Vector2 snap = new Vector2(size.x / 1024f * 16, size.y / 512 * 16);
        Vector2Int mouseSnap = new Vector2Int()
        {
            x = Mathf.FloorToInt((mouse.x / snap.x * snap.x - (mouse.x % snap.x)) / snap.x),
            y = Mathf.FloorToInt((mouse.y / snap.y * snap.y - (mouse.y % snap.y)) / snap.y),
        };

        return mouseSnap;
    }

    public void OnPointerEnter(PointerEventData eventData) => inside = true;

    public void OnPointerExit(PointerEventData eventData) => inside = false;

    public void OnPointerMove(PointerEventData eventData) => mousePos = eventData.position;

    public void OnPointerDown(PointerEventData eventData)
    {
        var pos = GetPositionInsideImageNormalized();

        Debug.Log(pos);

        EditorBlocoPallet.SelectBlockFaceIndex(pos.x, pos.y);
    }
}
