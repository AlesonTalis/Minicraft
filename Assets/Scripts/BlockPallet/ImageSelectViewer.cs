using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImageSelectViewer : MonoBehaviour
{
    public RawImage image;
    public Image selectBorder;
    public int indexX, indexY;
    public FaceDirection faceDirection;

    [Space]
    [SerializeField]
    private TextMeshProUGUI m_FaceDirection;

    private float imageWidth = 1024, imageHeight = 512;

    private void Start()
    {
        Select(false);
        m_FaceDirection.text = faceDirection.ToString();
        SetFaceIndex();
    }

    void Update()
    { }



    void SetFaceIndex()
    {
        Rect rect = new Rect();

        rect.size = new Vector2(16/imageWidth, 16/imageHeight);
        rect.position = new Vector2()
        {
            x = rect.size.x * indexX,
            y = rect.size.y * indexY,
        };

        image.uvRect = rect;
    }



    public void Select()
    {
        EditorBlocoPallet.BlocoFaceSelected(this);
    }

    public void Select(bool select)
    {
        selectBorder.gameObject.SetActive(select);
    }

    public void Select(int indexX, int indexY)
    {
        this.indexX = indexX;
        this.indexY = indexY;

        SetFaceIndex();
    }
}
