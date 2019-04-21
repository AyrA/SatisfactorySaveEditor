using System.IO;

namespace SatisfactorySaveEditor.PropertyValues
{
    public class NoneProperty : IPropertyValue
    {
        public NoneProperty(BinaryReader BR)
        {
            //This property has no values
        }

        public void Export(BinaryWriter BW)
        {
            BW.WriteIntString("None");
        }
    }
}
