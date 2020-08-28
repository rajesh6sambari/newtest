using System;

namespace TestingTutor.JavaEngine.Engine.CoverageXml
{
    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute("report", AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class Report
    {
        [System.Xml.Serialization.XmlElementAttribute("sessioninfo")]
        public ReportSessionInfo ReportSessionInfo { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("package")]
        public ReportPackage Package { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("counter")]
        public ReportCounter[] Counter { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute("name")]
        public string Name { get; set; }
    }
}