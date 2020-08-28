using System;

namespace TestingTutor.JavaEngine.Engine.CoverageXml
{
    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class ReportPackageClass
    {
        [System.Xml.Serialization.XmlElementAttribute("method")]
        public ReportPackageClassMethod[] Method { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("counter")]
        public ReportPackageClassCounter[] Counter { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute("name")]
        public string Name { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute("sourcefilename")]
        public string SourceFilename { get; set; }
    }
}