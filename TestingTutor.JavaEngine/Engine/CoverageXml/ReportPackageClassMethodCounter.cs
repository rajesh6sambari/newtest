using System;

namespace TestingTutor.JavaEngine.Engine.CoverageXml
{
    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class ReportPackageClassMethodCounter
    {
        [System.Xml.Serialization.XmlAttributeAttribute("type")]
        public string Type { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("missed")]
        public int Missed { get; set; }
       
        [System.Xml.Serialization.XmlAttributeAttribute("covered")]
        public int Covered { get; set; }
    }
}