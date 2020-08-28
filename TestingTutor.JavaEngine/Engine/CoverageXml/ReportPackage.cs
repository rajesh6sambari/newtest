using System;

namespace TestingTutor.JavaEngine.Engine.CoverageXml
{
    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class ReportPackage
    {
        [System.Xml.Serialization.XmlElementAttribute("class")]
        public ReportPackageClass[] ReportPackageClasses { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("sourcefile")]
        public ReportPackageSourceFile[] SourceFiles { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("counter")]
        public ReportPackageCounter[] Counter { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute("name")]
        public string Name { get; set; }
    }
}