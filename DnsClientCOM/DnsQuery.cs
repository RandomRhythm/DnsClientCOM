using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using DnsClient;
using System.Net;

namespace DnsClientCOM
{
  public class DnsQuery
  {
    [Guid("EA692270-659B-44CC-AE5D-0422A36966A1")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
  
    public interface iNameLookup
    {
      [DispId(1)]
      string DnsNameQuery(string queryName, string nameServer, string type);
    }

    [Guid("289B7834-3CAD-47B8-8EBE-51BA936A9E2C"), ClassInterface(ClassInterfaceType.None)]
    public class ComReverseLookup : iNameLookup
    {
      public string DnsNameQuery(string lookupItem, string nameServer, string recordType)
      {
        string returnDNS = "";
        var client = new LookupClient();


        if (CheckIPValid(nameServer) == true)
        {
          var endpoint = new IPEndPoint(IPAddress.Parse(nameServer), 53);
          client = new LookupClient(endpoint);
        }
        if (CheckIPValid(lookupItem) == false)
        {
          try
          {
            var result = client.Query(lookupItem, RecordQueryType(recordType, QueryType.A));
            if (result.Answers.Count > 0)
            {
              var returnAnswer = result.Answers.ARecords().FirstOrDefault();
              returnDNS = returnAnswer.Address.ToString();
            }
          }
          catch
          {
            return "";
          }
        }
        else
        {
          var result = client.QueryReverse(IPAddress.Parse(lookupItem));
          if (result.Answers.Count > 0)
          {
            var returnAnswer = result.Answers.PtrRecords().FirstOrDefault();
            if (returnAnswer != null)
            {
              returnDNS = returnAnswer.PtrDomainName.ToString();
            }
          }
        }
        return returnDNS;
      }
    }
      private static QueryType RecordQueryType(string recordtype, QueryType typeDefault)
      {
        switch (recordtype.ToLower())
        {
          case "a":
            return QueryType.A;
          case "aaa":
            return QueryType.AAAA;
          case "afsdb":
            return QueryType.AFSDB;
          case "any":
            return QueryType.ANY;
          case "axfr":
            return QueryType.AXFR;
          case "caa":
            return QueryType.CAA;
          case "cnam":
            return QueryType.CNAME;
          case "hinfo":
            return QueryType.HINFO;
          case "mb":
            return QueryType.MB;
          case "mg":
            return QueryType.MG;
          case "minfo":
            return QueryType.MINFO;
          case "mr":
            return QueryType.MR;
          case "mx":
            return QueryType.MX;
          case "ns":
            return QueryType.NS;
          case "ptr":
            return QueryType.PTR;
          case "soa":
            return QueryType.SOA;
          case "srv":
            return QueryType.SRV;
          case "wks":
            return QueryType.WKS;
          case "uri":
            return QueryType.URI;
          case "txt":
            return QueryType.TXT;

        }
        return typeDefault;
      }



      private static bool IPv6Check(string input)
      {
        if (string.IsNullOrWhiteSpace(input))
        {
          return false;
        }

        if (input.IndexOf("::") != input.LastIndexOf("::")) // 'The "::" can only appear once in an address.'
        {
          return false;
        }

        string[] parts = input.Split(new char[] { ':' });
        int partCount = parts.Length;

        if (partCount < 3 || partCount > 8) // From "::" to  "ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff"
        {
          return false;
        }

        UInt16 intValue = UInt16.MaxValue;
        foreach (string part in parts)
        {
          if (part == "") { continue; } // If you dont want addresses like "::" to be a legal address, remove this line
          if (!UInt16.TryParse(part, System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.NumberFormatInfo.InvariantInfo, out intValue)) // This ensures that no part has a numeric value greater than (2^16)-1 or FFFF in hexadecimal
          {
            return false;
          }
        }

        return true;
      }


      private static Boolean CheckIPValid(String strIP) //https://stackoverflow.com/questions/11412956/what-is-the-best-way-of-validating-an-ip-address
      {
        if (strIP.Contains(":"))
        {
          return IPv6Check(strIP.ToLower());
        }
        //  Split string by ".", check that array length is 4
        string[] arrOctets = strIP.Split('.');
        if (arrOctets.Length != 4)
          return false;

        //Check each substring checking that parses to byte
        byte obyte = 0;
        foreach (string strOctet in arrOctets)
          if (!byte.TryParse(strOctet, out obyte))
            return false;

        return true;
      }


  
  }
}
