using System.IO;

namespace SatisfactorySaveEditor.PropertyValues
{
    public class InventoryItem
    {
        public int Count { get; set; }
        public string Name { get; set; }
        public string Persistence1 { get; set; }
        public string Persistence2 { get; set; }

        public InventoryItem(BinaryReader BR)
        {
            //name of item
            Name = BR.ReadIntString();
            //"PersistentLevel" type string
            Persistence1 = BR.ReadIntString();
            //"PersistentLevel" type string (Again)
            Persistence2 = BR.ReadIntString();

            //"NumItems"
            BR.ReadIntString();
            //"IntProperty"
            BR.ReadIntString();

            //Discard. 04 00 00 00 00 00 00 00 00
            BR.ReadBytes(BR.ReadInt32());
            BR.ReadByte();

            Count = BR.ReadInt32();
            //"None"
            BR.ReadIntString();
        }
    }
}
