using Assets.Scripts.Gen;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlockListSelect : MonoBehaviour
{
    [HideInInspector]
    public int blockIndex = 0;
    public string blockName = "Stone";
    public string blockId = "1";
    public int blockIndexX, blockIndexY;

    [Header("Campos")]

    [SerializeField]
    private RawImage m_BlockImage;
    [SerializeField]
    private TextMeshProUGUI m_BlockName;
    [SerializeField]
    private TextMeshProUGUI m_BlockId;

    private void Start()
    {
        ChangeBlockValues();
        ChangeImagePosition();
    }

    public void FillValues(string blockName, int blockId, int blockIndexX, int blockIndexY)
    {
        this.blockName = blockName;
        blockIndex = blockId;
        this.blockId = blockId.ToString();
        this.blockIndexX = blockIndexX;
        this.blockIndexY = blockIndexY;

        ChangeImagePosition();
        ChangeBlockValues();
    }

    public void SelectBlockFromList()
    {
        EditorBlocoPallet.BlocoSelectEdit(blockIndex);
    }

    void ChangeImagePosition()
    {
        Rect rect = new Rect
        {
            size = new Vector2(16f / 1024f, 16f / 512f)
        };

        rect.position = new Vector2()
        {
            x = rect.size.x * blockIndexX,
            y = rect.size.y * blockIndexY,
        };

        Debug.Log(rect);
        m_BlockImage.uvRect = rect;
    }

    void ChangeBlockValues()
    {

        m_BlockName.text = blockName;
        m_BlockId.text = blockId;

    }
}
