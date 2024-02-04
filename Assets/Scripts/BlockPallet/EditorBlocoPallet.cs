using Assets.Scripts.Gen;
using Assets.Scripts.Utils;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class EditorBlocoPallet : MonoBehaviour
{
    public static EditorBlocoPallet instance;


    [SerializeField]
    private GameObject m_BlockListViewPrefab;
    [SerializeField]
    private RectTransform m_BlockListTarget;

    [Header("Mostrar Campos")]
    [SerializeField]
    private TMP_InputField m_BlockName;
    [SerializeField]
    private TMP_InputField m_BlockId;
    [SerializeField]
    private ImageSelectViewer[] m_ImageSelectViewer;

    [Space]
    [TextArea]
    [SerializeField]
    private string m_SavedResult;


    ImageSelectViewer faceSelected;

    ItemSettings itemEditing;

    List<ItemSettings> itemsSaved;



    private void Awake()
    {
        instance = this;
    }

    public void Save()
    {
        var edit = false;

        for (int i = 0; i < itemsSaved.Count; i++)
        {
            if (itemsSaved[i].itemId == itemEditing.itemId)
            {
                edit = true;

                itemsSaved[i] = itemEditing;

                break;
            }
        }

        if (edit is false)
            itemsSaved.Add(JsonConvert.DeserializeObject<ItemSettings>(JsonConvert.SerializeObject(itemEditing)));// dumb...

        CreateViewList();
    }

    public static void BlocoSelectEdit(int blockIndex)
    {
        instance.itemEditing = instance.itemsSaved.FirstOrDefault(f => f.itemId == blockIndex);

        instance.ShowSelectedValues();
    }

    public static void BlocoFaceSelected(ImageSelectViewer image)
    {
        instance.faceSelected?.Select(false);

        instance.faceSelected = image;
        image.Select(true);
    }

    public static void BlocoFaceSetIndex(FaceDirection face, int indexX, int indexY)
    {
        instance.SetBlockFace(face, indexX, indexY);
    }

    public static void SelectBlockFaceIndex(int indexX, int indexY)
    {
        var face = instance.faceSelected?.faceDirection ?? FaceDirection.Top;

        instance.faceSelected.Select(indexX, indexY);

        BlocoFaceSetIndex(face, indexX, indexY);
    }






    private void Start()
    {
        itemEditing = new ItemSettings()
        {
            itemImageFaces = new V2I[6]
        };

        itemsSaved ??= new List<ItemSettings>();

        LoadBlocks();

        if (itemsSaved.Count > 0)
        {
            itemEditing = itemsSaved[0];

            ShowSelectedValues();
        }
    }

    void ClearBlockListView()
    {
        foreach (Transform child in m_BlockListTarget.transform)
        {
            Destroy(child.gameObject);
        }
    }

    void ShowSelectedValues()
    {
        m_BlockId.text = itemEditing.itemId.ToString();
        m_BlockName.text = itemEditing.itemName;

        for (int i = 0; i < m_ImageSelectViewer.Length; i++)
        {
            m_ImageSelectViewer[i].Select(itemEditing.itemImageFaces[i].x, itemEditing.itemImageFaces[i].y);
        }
    }

    void LoadBlocks()
    {
        itemsSaved = JsonConvert.DeserializeObject<List<ItemSettings>>(m_SavedResult);
        CreateViewList();
    }

    private void CreateViewList()
    {
        ClearBlockListView();

        for (int i = 0; i < itemsSaved.Count; i++)
        {
            var go = Instantiate(m_BlockListViewPrefab, m_BlockListTarget);

            go.GetComponent<BlockListSelect>().FillValues(
                itemsSaved[i].itemName,
                itemsSaved[i].itemId,
                itemsSaved[i].itemImageFaces[0].x,
                itemsSaved[i].itemImageFaces[0].y
            );
        }

        m_SavedResult = JsonConvert.SerializeObject(itemsSaved);
    }



    public void SetBlockName(string name)
    {
        itemEditing.itemName = name;
    }

    public void SetBlockId(string id)
    {
        itemEditing.itemId = id.ToInt();
    }

    public void SetBlockFace(FaceDirection face, int indexX, int indexY)
    {
        if (itemEditing.itemImageFaces?.Length != 6) itemEditing.itemImageFaces = new V2I[6];

        itemEditing.itemImageFaces[(int)face] = new V2I
        {
            x = indexX,
            y = indexY
        };
    }

    public void SetBlockFace(int indexX, int indexY)
    {

    }
}


[System.Serializable]
public enum FaceDirection
{
    Top,
    Bottom,
    Left,
    Right,
    Front,
    Back
}