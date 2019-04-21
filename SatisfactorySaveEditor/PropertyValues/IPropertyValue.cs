using System.IO;

namespace SatisfactorySaveEditor.PropertyValues
{
    public interface IPropertyValue
    {
        void Export(BinaryWriter BW);
    }
}
