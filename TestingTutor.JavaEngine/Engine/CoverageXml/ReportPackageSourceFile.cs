using System;

namespace TestingTutor.JavaEngine.Engine.CoverageXml
{
    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class ReportPackageSourceFile
    {
        [System.Xml.Serialization.XmlElementAttribute("line")]
        public ReportPackageSourceFileLine[] Lines { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("counter")]
        public ReportPackageSourceFileCounter[] Counters { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute("name")]
        public string Name { get; set; }
    }
}