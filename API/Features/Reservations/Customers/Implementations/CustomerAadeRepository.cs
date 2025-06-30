using System.Xml;
using System.Net;
using System.IO;

namespace API.Features.Reservations.Customers {

    public class CustomerAadeRepository : ICustomerAadeRepository {

        public CustomerAadeRepository() { }

        public string GetResponse(CustomerAadeVM vm) {
            const string url = "https://www1.gsis.gr/wsaade/RgWsPublic2/RgWsPublic2";
            const string action = "POST";
            XmlDocument soapEnvelopeXml = CreateSoapEnvelope();
            soapEnvelopeXml = ReplaceFieldsWithVariables(soapEnvelopeXml, vm);
            HttpWebRequest webRequest = CreateWebRequest(url, action);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);
            string response;
            using (WebResponse x = webRequest.GetResponse()) {
                using StreamReader rd = new(x.GetResponseStream());
                response = rd.ReadToEnd();
            }
            return response;
        }

        private static HttpWebRequest CreateWebRequest(string url, string action) {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "application/soap+xml;charset=\"utf-8\"";
            webRequest.Accept = "application/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        private static XmlDocument CreateSoapEnvelope() {
            XmlDocument soapEnvelopeXml = new();
            soapEnvelopeXml.LoadXml(@"<?xml version=""1.0""?>
                <env:Envelope xmlns:ns3=""http://rgwspublic2/RgWsPublic2"" xmlns:ns2=""http://rgwspublic2/RgWsPublic2Service"" xmlns:ns1=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"" xmlns:env=""http://www.w3.org/2003/05/soap-envelope"">
                    <env:Header>
                        <ns1:Security>
                            <ns1:UsernameToken>
                                <ns1:Username></ns1:Username>
                                <ns1:Password></ns1:Password>
                            </ns1:UsernameToken>
                        </ns1:Security>
                    </env:Header>
                    <env:Body>
                        <ns2:rgWsPublic2AfmMethod>
                            <ns2:INPUT_REC>
                                <ns3:afm_called_by/>
                                <ns3:afm_called_for></ns3:afm_called_for>
                            </ns2:INPUT_REC>
                        </ns2:rgWsPublic2AfmMethod>
                    </env:Body>
                </env:Envelope>");
            return soapEnvelopeXml;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest) {
            using Stream stream = webRequest.GetRequestStream();
            soapEnvelopeXml.Save(stream);
        }

        private static XmlDocument ReplaceFieldsWithVariables(XmlDocument soapEnvelopeXml, CustomerAadeVM vm) {
            var x = soapEnvelopeXml.GetElementsByTagName("ns1:Username");
            x[0].InnerText = vm.Username;
            var z = soapEnvelopeXml.GetElementsByTagName("ns1:Password");
            z[0].InnerText = vm.Password;
            var i = soapEnvelopeXml.GetElementsByTagName("ns3:afm_called_for");
            i[0].InnerText = vm.VatNumber;
            return soapEnvelopeXml;
        }

    }

}