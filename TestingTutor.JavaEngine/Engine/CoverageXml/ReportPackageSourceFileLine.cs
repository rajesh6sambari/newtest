using System;

namespace TestingTutor.JavaEngine.Engine.CoverageXml
{
    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class ReportPackageSourceFileLine
    {
        [System.Xml.Serialization.XmlAttributeAttribute("nr")]
        public int LineNumber { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute("mi")]
        public int MissingInstructions { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute("ci")]
        public int CoveredInstructions { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute("mb")]
        public int MissedBranches { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute("cb")]
        public int CoveredBranches { get; set; }
    }
}