using Assets.Scripts.Scriptables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Gen
{
    [Serializable]
    public struct ItemSettings
    {
        public int itemIndex { get; set; }

        public int itemId { get; set; }
        public string itemName { get; set; }
        public string itemDescription { get; set; }

        public ItemType itemType { get; set; }

        [Obsolete]
        public V2I[] itemImageFaces { get; set; }// 6 items

        public int[] itemFaces { get; set; }// 1,2,3,6
    }

    public enum ItemType
    {
        Item,
        Block
    }

    public struct V2I
    {
        public int x { get; set; }
        public int y { get; set; }
    }
}
