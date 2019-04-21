using System.IO;

namespace SatisfactorySaveEditor.PropertyValues
{
    class IntProperty : IPropertyValue
    {
        public int UnknownInt { get; private set; }
        public int Value { get; private set; }

        public IntProperty(BinaryReader BR)
        {
            //Always 4
            BR.ReadInt32();
            UnknownInt = BR.ReadInt32();
            BR.ReadByte();
            Value = BR.ReadInt32();
        }

        public void Export(BinaryWriter BW)
        {
            BW.WriteIntString("IntProperty");
            BW.Write(4);
            BW.Write(UnknownInt);
            BW.Write((byte)0);
            BW.Write(Value);
        }
    }
}
