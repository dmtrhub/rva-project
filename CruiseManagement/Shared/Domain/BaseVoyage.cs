using System.Xml.Serialization;

namespace Shared.Domain
{
    public abstract class BaseVoyage
    {
        [XmlIgnore]
        public string Code { get; protected set; }

        protected BaseVoyage(string code)
        {
            Code = code;
        }
    }
}