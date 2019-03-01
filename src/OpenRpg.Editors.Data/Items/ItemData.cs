using System.Collections.Generic;

namespace OpenRpg.Data.Items
{
    public class InventoryItemData
    {
        public int Amount { get; set; }
        public int ItemTemplateId { get; set; }
        public List<int> ModificationId { get; set; }
    }
}