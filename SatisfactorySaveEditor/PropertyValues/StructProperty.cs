using System;
using System.Collections.Generic;
using System.IO;

namespace SatisfactorySaveEditor.PropertyValues
{
    public class StructProperty : IPropertyValue
    {
        public Dictionary<string, IPropertyValue> Properties { get; set; }

        public byte[] Sequence { get; set; }

        public StructProperty(BinaryReader BR)
        {
            Properties = new Dictionary<string, IPropertyValue>();
            //Sequence = BR.ReadBytes(5); //This number is dynamic. No Idea how to read
            while (true)
            {
                var Name = BR.ReadIntString();
                //Empty property name means we have them all now.
                if (Name == string.Empty)
                {
                    break;
                }
                Properties.Add(Name, new Property(BR));
            }
        }

        public void Export(BinaryWriter BW)
        {
            throw new NotImplementedException();
        }
    }
}
