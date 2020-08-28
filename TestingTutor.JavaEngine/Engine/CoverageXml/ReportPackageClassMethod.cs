using System;

namespace TestingTutor.JavaEngine.Engine.CoverageXml
{
    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class ReportPackageClassMethod
    {
        [System.Xml.Serialization.XmlElementAttribute("counter")]
        public ReportPackageClassMethodCounter[] Counters { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute("name")]
        public string Name { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute("desc")]
        public string Description { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute("line")]
        public int Line { get; set; }
    }
}