using System.Collections.Generic;
using System.IO;

namespace SatisfactorySaveEditor.PropertyValues
{
    public class Inventory
    {
        public List<InventoryItem> Items;
        public Inventory(byte[] Data)
        {
            Items = new List<InventoryItem>();

            using (var MS = new MemoryStream(Data, false))
            {
                using (var BR = new BinaryReader(MS))
                {
                    var Name = BR.ReadIntString();
                    var ValueType = BR.ReadIntString();
                    //Discard unknown bytes for now. Example was 5420, probably byte count
                    BR.ReadBytes(8);
                    //"StructProperty"
                    BR.ReadIntString();
                    //??
                    BR.ReadBytes(5);
                    //"InventoryStacks"
                    BR.ReadIntString();
                    //"StructProperty"
                    BR.ReadIntString();
                    //??
                    BR.ReadBytes(8);
                    //"InventoryStack"
                    BR.ReadIntString();
                    //?? All are Zero
                    BR.ReadBytes(17);

                    //This likely loops

                    //"Item"
                    BR.ReadIntString();
                    //"StructProperty"
                    BR.ReadIntString();
                    //??
                    BR.ReadBytes(8);

                    //"InventoryItem"
                    BR.ReadIntString();
                    //?? All are Zero
                    BR.ReadBytes(21);

                    var Item = new InventoryItem(BR);

                }
            }
        }

        public Inventory()
        {

        }
    }
}
