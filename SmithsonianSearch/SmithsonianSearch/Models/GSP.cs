/*
* Generated from XML returned by GSA via VisualStudio (Copy XML, then: EDIT -> Paste Special -> Classes from XML)
*/
namespace SmithsonianSearch.Models
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class GSP
    {

        private decimal tmField;

        private string qField;

        private GSPPARAM[] pARAMField;

        private GSPRES rESField;

        private decimal vERField;

        private GSPSuggestion[] spellingField;

        private GSPOneSynonym[] synonymsField;

        /// <remarks/>
        public decimal TM
        {
            get
            {
                return this.tmField;
            }
            set
            {
                this.tmField = value;
            }
        }

        /// <remarks/>
        public string Q
        {
            get
            {
                return this.qField;
            }
            set
            {
                this.qField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PARAM")]
        public GSPPARAM[] PARAM
        {
            get
            {
                return this.pARAMField;
            }
            set
            {
                this.pARAMField = value;
            }
        }

        /// <remarks/>
        public GSPRES RES
        {
            get
            {
                return this.rESField;
            }
            set
            {
                this.rESField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal VER
        {
            get
            {
                return this.vERField;
            }
            set
            {
                this.vERField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Suggestion", IsNullable = false)]
        public GSPSuggestion[] Spelling
        {
            get
            {
                return this.spellingField;
            }
            set
            {
                this.spellingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("OneSynonym", IsNullable = false)]
        public GSPOneSynonym[] Synonyms
        {
            get
            {
                return this.synonymsField;
            }
            set
            {
                this.synonymsField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class GSPPARAM
    {

        private string nameField;

        private string valueField;

        private string original_valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string original_value
        {
            get
            {
                return this.original_valueField;
            }
            set
            {
                this.original_valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class GSPRES
    {

        private uint mField;

        private object fiField;

        private GSPRESNB nbField;

        private GSPRESR[] rField;

        private GSPRESPARM pARMField;

        private int snField;

        private int enField;

        /// <remarks/>
        public uint M
        {
            get
            {
                return this.mField;
            }
            set
            {
                this.mField = value;
            }
        }

        /// <remarks/>
        public object FI
        {
            get
            {
                return this.fiField;
            }
            set
            {
                this.fiField = value;
            }
        }

        /// <remarks/>
        public GSPRESNB NB
        {
            get
            {
                return this.nbField;
            }
            set
            {
                this.nbField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("R")]
        public GSPRESR[] R
        {
            get
            {
                return this.rField;
            }
            set
            {
                this.rField = value;
            }
        }

        /// <remarks/>
        public GSPRESPARM PARM
        {
            get
            {
                return this.pARMField;
            }
            set
            {
                this.pARMField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int SN
        {
            get
            {
                return this.snField;
            }
            set
            {
                this.snField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int EN
        {
            get
            {
                return this.enField;
            }
            set
            {
                this.enField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class GSPRESNB
    {

        private string puField;

        private string nuField;

        /// <remarks/>
        public string PU
        {
            get
            {
                return this.puField;
            }
            set
            {
                this.puField = value;
            }
        }

        /// <remarks/>
        public string NU
        {
            get
            {
                return this.nuField;
            }
            set
            {
                this.nuField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class GSPRESR
    {

        private string uField;

        private string ueField;

        private string tField;

        private int rkField;

        private string eNT_SOURCEField;

        private GSPRESRFS fsField;

        private GSPRESRMT[] mtField;

        private string sField;

        private string lANGField;

        private GSPRESRHAS hASField;

        private int nField;

        /// <remarks/>
        public string U
        {
            get
            {
                return this.uField;
            }
            set
            {
                this.uField = value;
            }
        }

        /// <remarks/>
        public string UE
        {
            get
            {
                return this.ueField;
            }
            set
            {
                this.ueField = value;
            }
        }

        /// <remarks/>
        public string T
        {
            get
            {
                return this.tField;
            }
            set
            {
                this.tField = value;
            }
        }

        /// <remarks/>
        public int RK
        {
            get
            {
                return this.rkField;
            }
            set
            {
                this.rkField = value;
            }
        }

        /// <remarks/>
        public string ENT_SOURCE
        {
            get
            {
                return this.eNT_SOURCEField;
            }
            set
            {
                this.eNT_SOURCEField = value;
            }
        }

        /// <remarks/>
        public GSPRESRFS FS
        {
            get
            {
                return this.fsField;
            }
            set
            {
                this.fsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("MT")]
        public GSPRESRMT[] MT
        {
            get
            {
                return this.mtField;
            }
            set
            {
                this.mtField = value;
            }
        }

        /// <remarks/>
        public string S
        {
            get
            {
                return this.sField;
            }
            set
            {
                this.sField = value;
            }
        }

        /// <remarks/>
        public string LANG
        {
            get
            {
                return this.lANGField;
            }
            set
            {
                this.lANGField = value;
            }
        }

        /// <remarks/>
        public GSPRESRHAS HAS
        {
            get
            {
                return this.hASField;
            }
            set
            {
                this.hASField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int N
        {
            get
            {
                return this.nField;
            }
            set
            {
                this.nField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class GSPRESRFS
    {

        private string nAMEField;

        private string vALUEField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string NAME
        {
            get
            {
                return this.nAMEField;
            }
            set
            {
                this.nAMEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string VALUE
        {
            get
            {
                return this.vALUEField;
            }
            set
            {
                this.vALUEField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class GSPRESRMT
    {

        private string nField;

        private string vField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string N
        {
            get
            {
                return this.nField;
            }
            set
            {
                this.nField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string V
        {
            get
            {
                return this.vField;
            }
            set
            {
                this.vField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class GSPRESRHAS
    {

        private object lField;

        private GSPRESRHASC cField;

        /// <remarks/>
        public object L
        {
            get
            {
                return this.lField;
            }
            set
            {
                this.lField = value;
            }
        }

        /// <remarks/>
        public GSPRESRHASC C
        {
            get
            {
                return this.cField;
            }
            set
            {
                this.cField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class GSPRESRHASC
    {

        private string szField;

        private string cIDField;

        private string eNCField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string SZ
        {
            get
            {
                return this.szField;
            }
            set
            {
                this.szField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CID
        {
            get
            {
                return this.cIDField;
            }
            set
            {
                this.cIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ENC
        {
            get
            {
                return this.eNCField;
            }
            set
            {
                this.eNCField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class GSPRESPARM
    {

        private int pcField;

        private GSPRESPARMPMT[] pMTField;

        /// <remarks/>
        public int PC
        {
            get
            {
                return this.pcField;
            }
            set
            {
                this.pcField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PMT")]
        public GSPRESPARMPMT[] PMT
        {
            get
            {
                return this.pMTField;
            }
            set
            {
                this.pMTField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class GSPRESPARMPMT
    {

        private GSPRESPARMPMTPV[] pvField;

        private string nmField;

        private string dnField;

        private int irField;

        private int tField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PV")]
        public GSPRESPARMPMTPV[] PV
        {
            get
            {
                return this.pvField;
            }
            set
            {
                this.pvField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string NM
        {
            get
            {
                return this.nmField;
            }
            set
            {
                this.nmField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string DN
        {
            get
            {
                return this.dnField;
            }
            set
            {
                this.dnField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int IR
        {
            get
            {
                return this.irField;
            }
            set
            {
                this.irField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int T
        {
            get
            {
                return this.tField;
            }
            set
            {
                this.tField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class GSPRESPARMPMTPV
    {

        private string vField;

        private string lField;

        private string hField;

        private ushort cField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string V
        {
            get
            {
                return this.vField;
            }
            set
            {
                this.vField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string L
        {
            get
            {
                return this.lField;
            }
            set
            {
                this.lField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string H
        {
            get
            {
                return this.hField;
            }
            set
            {
                this.hField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort C
        {
            get
            {
                return this.cField;
            }
            set
            {
                this.cField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class GSPSuggestion
    {

        private string qField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string q
        {
            get
            {
                return this.qField;
            }
            set
            {
                this.qField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class GSPOneSynonym
    {

        private string qField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string q
        {
            get
            {
                return this.qField;
            }
            set
            {
                this.qField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }
}