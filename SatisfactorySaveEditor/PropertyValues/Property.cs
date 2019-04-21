using System;
using System.IO;

namespace SatisfactorySaveEditor.PropertyValues
{
    public class Property : IPropertyValue
    {
        public string Name { get; set; }

        public IPropertyValue Value { get; set; }

        public Property(BinaryReader BR)
        {
            var ValueType = BR.ReadIntString();
            switch (ValueType)
            {
                case "ArrayProperty":
                    Value = new ArrayProperty(BR);
                    break;
                case "StructProperty":
                    Value = new StructProperty(BR);
                    break;
                case "None":
                    Value = new NoneProperty(BR);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void Export(BinaryWriter BW)
        {
            throw new NotImplementedException();
        }
    }
}
