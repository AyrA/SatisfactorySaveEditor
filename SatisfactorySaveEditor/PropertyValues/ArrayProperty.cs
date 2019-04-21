using System;
using System.Collections.Generic;
using System.IO;

namespace SatisfactorySaveEditor.PropertyValues
{
    public class ArrayProperty : IPropertyValue
    {
        public List<IPropertyValue> Properties;

        public ArrayProperty(BinaryReader BR)
        {
            Properties = new List<IPropertyValue>();
            var Bytes = BR.ReadInt64();
            var Pos = BR.BaseStream.Position;
            while (BR.BaseStream.Position < Pos + Bytes)
            {
                Properties.Add(new Property(BR));
            }
        }

        public void Export(BinaryWriter BW)
        {
            throw new NotImplementedException();
        }
    }
}
