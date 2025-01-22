using System.Collections.Generic;
using System.Xml.Serialization;

namespace FinanceManager.Domain
{
    public class Quicken
    {
        public class OFX
        {
            [XmlElement]
            public SIGNONMSGSRSV1 SIGNONMSGSRSV1 { get; set; }

            [XmlElement]
            public BANKMSGSRSV1 BANKMSGSRSV1 { get; set; }

            [XmlElement]
            public CREDITCARDMSGSRSV1 CREDITCARDMSGSRSV1 { get; set; }
        }

        public class SIGNONMSGSRSV1
        {
            [XmlElement]
            public SONRS SONRS { get; set; }
        }

        public class CREDITCARDMSGSRSV1
        {
            [XmlElement]
            public CCSTMTTRNRS CCSTMTTRNRS { get; set; }
        }

        public class CCSTMTTRNRS
        {
            [XmlElement]
            public string TRNUID { get; set; }

            [XmlElement]
            public STATUS STATUS { get; set; }

            [XmlElement]
            public CCSTMTRS CCSTMTRS { get; set; }
        }

        public class CCSTMTRS
        {
            [XmlElement]
            public string CURDEF { get; set; }

            [XmlElement]
            public CCACCTFROM CCACCTFROM { get; set; }

            [XmlElement]
            public BANKTRANLIST BANKTRANLIST { get; set; }

        }

        public class CCACCTFROM
        {
            [XmlElement]
            public string ACCTID { get; set; }

        }

        public class SONRS
        {
            [XmlElement]
            public STATUS STATUS { get; set; }

            [XmlElement]
            public string DTSERVER { get; set; }

            [XmlElement]
            public string USERKEY { get; set; }

            [XmlElement(ElementName = "INTU.BID")]
            public string INTUBID { get; set; }

            [XmlElement]
            public string LANGUAGE { get; set; }
        }

        public class STATUS
        {
            [XmlElement]
            public string CODE { get; set; }

            [XmlElement]
            public string SEVERITY { get; set; }

            [XmlElement]
            public string MESSAGE { get; set; }
        }

        public class BANKMSGSRSV1
        {
            [XmlElement]
            public List<STMTTRNRS> STMTTRNRS { get; set; }
        }

        public class STMTTRNRS
        {
            [XmlElement]
            public string TRNUID { get; set; }

            [XmlElement]
            public STATUS STATUS { get; set; }

            [XmlElement]
            public List<STMTRS> STMTRS { get; set; }
        }

        public class STMTRS
        {
            [XmlElement]
            public string CURDEF { get; set; }

            [XmlElement]
            public BANKACCTFROM BANKACCTFROM { get; set; }

            [XmlElement]
            public BANKTRANLIST BANKTRANLIST { get; set; }

            [XmlElement]
            public LEDGERBAL LEDGERBAL { get; set; }

            [XmlElement]
            public AVAILBAL AVAILBAL { get; set; }
        }

        public class BANKACCTFROM
        {
            [XmlElement]
            public string BANKID { get; set; }

            [XmlElement]
            public string BRANCHID { get; set; }

            [XmlElement]
            public string ACCTID { get; set; }

            [XmlElement]
            public string ACCTTYPE { get; set; }
        }

        public class BANKTRANLIST
        {
            [XmlElement]
            public string DTSTART { get; set; }

            [XmlElement]
            public string DTEND { get; set; }

            [XmlElement]
            public List<STMTTRN> STMTTRN { get; set; }
        }

        public class STMTTRN
        {
            [XmlElement]
            public string TRNTYPE { get; set; }

            [XmlElement]
            public string DTPOSTED { get; set; }

            [XmlElement]
            public string TRNAMT { get; set; }

            [XmlElement]
            public string FITID { get; set; }

            [XmlElement]
            public string NAME { get; set; }

            [XmlElement]
            public string MEMO { get; set; }
        }

        public class LEDGERBAL
        {
            [XmlElement]
            public string BALAMT { get; set; }

            [XmlElement]
            public string DTASOF { get; set; }
        }

        public class AVAILBAL
        {
            [XmlElement]
            public string BALAMT { get; set; }

            [XmlElement]
            public string DTASOF { get; set; }
        }
    }
}
