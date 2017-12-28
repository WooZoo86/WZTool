using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace WZToolLib.Internet.Web
{
    public enum PostDataType
    {
        String,
        Byte,
        FilePath
    }

    public enum ResultType
    {
        String,
        Byte
    }

    public enum ResultCookieType
    {
        String,
        CookieCollection
    }

    public class HttpResult
    {
        public string Cookie
        {
            get
            {
                return this._Cookie;
            }
            set
            {
                this._Cookie = value;
            }
        }

        public CookieCollection CookieCollection
        {
            get
            {
                return this._CookieCollection;
            }
            set
            {
                this._CookieCollection = value;
            }
        }

        public string Html
        {
            get
            {
                return this._html;
            }
            set
            {
                this._html = value;
            }
        }

        public byte[] ResultByte
        {
            get
            {
                return this._ResultByte;
            }
            set
            {
                this._ResultByte = value;
            }
        }

        public WebHeaderCollection Header
        {
            get
            {
                return this._Header;
            }
            set
            {
                this._Header = value;
            }
        }

        public string StatusDescription
        {
            get
            {
                return this._StatusDescription;
            }
            set
            {
                this._StatusDescription = value;
            }
        }

        public HttpStatusCode StatusCode
        {
            get
            {
                return this._StatusCode;
            }
            set
            {
                this._StatusCode = value;
            }
        }

        private string _Cookie;

        private CookieCollection _CookieCollection;

        private string _html = string.Empty;

        private byte[] _ResultByte;

        private WebHeaderCollection _Header;

        private string _StatusDescription;

        private HttpStatusCode _StatusCode;
    }

    public class HttpItem
    {
        public string URL
        {
            get
            {
                return this._URL;
            }
            set
            {
                this._URL = value;
            }
        }

        public string Method
        {
            get
            {
                return this._Method;
            }
            set
            {
                this._Method = value;
            }
        }

        public int Timeout
        {
            get
            {
                return this._Timeout;
            }
            set
            {
                this._Timeout = value;
            }
        }

        public int ReadWriteTimeout
        {
            get
            {
                return this._ReadWriteTimeout;
            }
            set
            {
                this._ReadWriteTimeout = value;
            }
        }

        public bool KeepAlive
        {
            get
            {
                return this._KeepAlive;
            }
            set
            {
                this._KeepAlive = value;
            }
        }

        public string Accept
        {
            get
            {
                return this._Accept;
            }
            set
            {
                this._Accept = value;
            }
        }

        public string ContentType
        {
            get
            {
                return this._ContentType;
            }
            set
            {
                this._ContentType = value;
            }
        }

        public string UserAgent
        {
            get
            {
                return this._UserAgent;
            }
            set
            {
                this._UserAgent = value;
            }
        }

        public Encoding Encoding
        {
            get
            {
                return this._Encoding;
            }
            set
            {
                this._Encoding = value;
            }
        }

        public PostDataType PostDataType
        {
            get
            {
                return this._PostDataType;
            }
            set
            {
                this._PostDataType = value;
            }
        }

        public string Postdata
        {
            get
            {
                return this._Postdata;
            }
            set
            {
                this._Postdata = value;
            }
        }

        public byte[] PostdataByte
        {
            get
            {
                return this._PostdataByte;
            }
            set
            {
                this._PostdataByte = value;
            }
        }

        public WebProxy WebProxy
        {
            get
            {
                return this._WebProxy;
            }
            set
            {
                this._WebProxy = value;
            }
        }

        public CookieCollection CookieCollection
        {
            get
            {
                return this.cookiecollection;
            }
            set
            {
                this.cookiecollection = value;
            }
        }

        public string Cookie
        {
            get
            {
                return this._Cookie;
            }
            set
            {
                this._Cookie = value;
            }
        }

        public string Referer
        {
            get
            {
                return this._Referer;
            }
            set
            {
                this._Referer = value;
            }
        }

        public string CerPath
        {
            get
            {
                return this._CerPath;
            }
            set
            {
                this._CerPath = value;
            }
        }

        public bool IsToLower
        {
            get
            {
                return this.isToLower;
            }
            set
            {
                this.isToLower = value;
            }
        }

        public bool Allowautoredirect
        {
            get
            {
                return this.allowautoredirect;
            }
            set
            {
                this.allowautoredirect = value;
            }
        }

        public int Connectionlimit
        {
            get
            {
                return this.connectionlimit;
            }
            set
            {
                this.connectionlimit = value;
            }
        }

        public string ProxyUserName
        {
            get
            {
                return this.proxyusername;
            }
            set
            {
                this.proxyusername = value;
            }
        }

        public string ProxyPwd
        {
            get
            {
                return this.proxypwd;
            }
            set
            {
                this.proxypwd = value;
            }
        }

        public string ProxyIp
        {
            get
            {
                return this.proxyip;
            }
            set
            {
                this.proxyip = value;
            }
        }

        public ResultType ResultType
        {
            get
            {
                return this.resulttype;
            }
            set
            {
                this.resulttype = value;
            }
        }

        public WebHeaderCollection Header
        {
            get
            {
                return this.header;
            }
            set
            {
                this.header = value;
            }
        }

        public Version ProtocolVersion
        {
            get
            {
                return this._ProtocolVersion;
            }
            set
            {
                this._ProtocolVersion = value;
            }
        }

        public bool Expect100Continue
        {
            get
            {
                return this._expect100continue;
            }
            set
            {
                this._expect100continue = value;
            }
        }

        public X509CertificateCollection ClentCertificates
        {
            get
            {
                return this._ClentCertificates;
            }
            set
            {
                this._ClentCertificates = value;
            }
        }

        public Encoding PostEncoding
        {
            get
            {
                return this._PostEncoding;
            }
            set
            {
                this._PostEncoding = value;
            }
        }

        public ResultCookieType ResultCookieType
        {
            get
            {
                return this._ResultCookieType;
            }
            set
            {
                this._ResultCookieType = value;
            }
        }

        public ICredentials ICredentials
        {
            get
            {
                return this._ICredentials;
            }
            set
            {
                this._ICredentials = value;
            }
        }

        public int MaximumAutomaticRedirections
        {
            get
            {
                return this._MaximumAutomaticRedirections;
            }
            set
            {
                this._MaximumAutomaticRedirections = value;
            }
        }

        public DateTime? IfModifiedSince
        {
            get
            {
                return this._IfModifiedSince;
            }
            set
            {
                this._IfModifiedSince = value;
            }
        }

        private string _URL = string.Empty;

        private string _Method = "GET";

        private int _Timeout = 100000;

        private int _ReadWriteTimeout = 30000;

        private bool _KeepAlive = true;

        private string _Accept = "text/html, application/xhtml+xml, */*";

        private string _ContentType = "text/html";

        private string _UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";

        private Encoding _Encoding = null;

        private PostDataType _PostDataType = PostDataType.String;

        private string _Postdata = string.Empty;

        private byte[] _PostdataByte = null;

        private WebProxy _WebProxy;

        private CookieCollection cookiecollection = null;

        private string _Cookie = string.Empty;

        private string _Referer = string.Empty;

        private string _CerPath = string.Empty;

        private bool isToLower = false;

        private bool allowautoredirect = false;

        private int connectionlimit = 1024;

        private string proxyusername = string.Empty;

        private string proxypwd = string.Empty;

        private string proxyip = string.Empty;

        private ResultType resulttype = ResultType.String;

        private WebHeaderCollection header = new WebHeaderCollection();

        private Version _ProtocolVersion;

        private bool _expect100continue = true;

        private X509CertificateCollection _ClentCertificates;

        private Encoding _PostEncoding;

        private ResultCookieType _ResultCookieType = ResultCookieType.String;

        private ICredentials _ICredentials = CredentialCache.DefaultCredentials;

        private int _MaximumAutomaticRedirections;

        private DateTime? _IfModifiedSince = null;
    }
    
    public class HttpHelper
    {
        public HttpResult GetHtml(HttpItem item)
        {
            HttpResult httpResult = new HttpResult();
            try
            {
                this.SetRequest(item);
            }
            catch (Exception ex)
            {
                httpResult.Cookie = string.Empty;
                httpResult.Header = null;
                httpResult.Html = ex.Message;
                httpResult.StatusDescription = "Invalid Arguments：" + ex.Message;
                return httpResult;
            }

            try
            {
                using (this.response = (HttpWebResponse)this.request.GetResponse())
                {
                    this.GetData(item, httpResult);
                }
            }
            catch (WebException ex2)
            {
                if (ex2.Response != null)
                {
                    using (this.response = (HttpWebResponse)ex2.Response)
                    {
                        this.GetData(item, httpResult);
                    }
                }
                else
                {
                    httpResult.Html = ex2.Message;
                }
            }
            catch (Exception ex)
            {
                httpResult.Html = ex.Message;
            }

            if (item.IsToLower)
            {
                httpResult.Html = httpResult.Html.ToLower();
            }

            return httpResult;
        }

        private void GetData(HttpItem item, HttpResult result)
        {
            result.StatusCode = this.response.StatusCode;
            result.StatusDescription = this.response.StatusDescription;
            result.Header = this.response.Headers;
            if (this.response.Cookies != null)
            {
                result.CookieCollection = this.response.Cookies;
            }

            if (this.response.Headers["set-cookie"] != null)
            {
                result.Cookie = this.response.Headers["set-cookie"];
            }

            byte[] @byte = this.GetByte();
            if (@byte != null & @byte.Length > 0)
            {
                this.SetEncoding(item, result, @byte);
                result.Html = this.encoding.GetString(@byte);
            }
            else
            {
                result.Html = string.Empty;
            }
        }

        private void SetEncoding(HttpItem item, HttpResult result, byte[] ResponseByte)
        {
            if (item.ResultType == ResultType.Byte)
            {
                result.ResultByte = ResponseByte;
            }

            if (this.encoding == null)
            {
                Match match = Regex.Match(Encoding.Default.GetString(ResponseByte), "<meta[^<]*charset=([^<]*)[\"']", RegexOptions.IgnoreCase);
                string text = string.Empty;
                if (match != null && match.Groups.Count > 0)
                {
                    text = match.Groups[1].Value.ToLower().Trim();
                }

                if (text.Length > 2)
                {
                    try
                    {
                        this.encoding = Encoding.GetEncoding(text.Replace("\"", string.Empty).Replace("'", "").Replace(";", "").Replace("iso-8859-1", "gbk").Trim());
                    }
                    catch
                    {
                        if (string.IsNullOrEmpty(this.response.CharacterSet))
                        {
                            this.encoding = Encoding.UTF8;
                        }
                        else
                        {
                            this.encoding = Encoding.GetEncoding(this.response.CharacterSet);
                        }
                    }
                }
                else if (string.IsNullOrEmpty(this.response.CharacterSet))
                {
                    this.encoding = Encoding.UTF8;
                }
                else
                {
                    this.encoding = Encoding.GetEncoding(this.response.CharacterSet);
                }
            }
        }

        private byte[] GetByte()
        {
            MemoryStream memoryStream = new MemoryStream();
            if (this.response.ContentEncoding != null && this.response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
            {
                memoryStream = this.GetMemoryStream(new GZipStream(this.response.GetResponseStream(), CompressionMode.Decompress));
            }
            else
            {
                memoryStream = this.GetMemoryStream(this.response.GetResponseStream());
            }

            byte[] result = memoryStream.ToArray();
            memoryStream.Close();

            return result;
        }

        private MemoryStream GetMemoryStream(Stream streamResponse)
        {
            MemoryStream memoryStream = new MemoryStream();
            int num = 256;
            byte[] buffer = new byte[num];
            for (int i = streamResponse.Read(buffer, 0, num); i > 0; i = streamResponse.Read(buffer, 0, num))
            {
                memoryStream.Write(buffer, 0, i);
            }

            return memoryStream;
        }

        private void SetRequest(HttpItem item)
        {
            this.SetCer(item);
            if (item.Header != null && item.Header.Count > 0)
            {
                foreach (string name in item.Header.AllKeys)
                {
                    this.request.Headers.Add(name, item.Header[name]);
                }
            }

            this.SetProxy(item);
            if (item.ProtocolVersion != null)
            {
                this.request.ProtocolVersion = item.ProtocolVersion;
            }

            this.request.ServicePoint.Expect100Continue = item.Expect100Continue;
            this.request.Method = item.Method;
            this.request.Timeout = item.Timeout;
            this.request.KeepAlive = item.KeepAlive;
            this.request.ReadWriteTimeout = item.ReadWriteTimeout;

            if (item.IfModifiedSince != null)
            {
                this.request.IfModifiedSince = Convert.ToDateTime(item.IfModifiedSince);
            }

            this.request.Accept = item.Accept;
            this.request.ContentType = item.ContentType;
            this.request.UserAgent = item.UserAgent;
            this.encoding = item.Encoding;
            this.request.Credentials = item.ICredentials;
            this.SetCookie(item);
            this.request.Referer = item.Referer;
            this.request.AllowAutoRedirect = item.Allowautoredirect;

            if (item.MaximumAutomaticRedirections > 0)
            {
                this.request.MaximumAutomaticRedirections = item.MaximumAutomaticRedirections;
            }

            this.SetPostData(item);
            if (item.Connectionlimit > 0)
            {
                this.request.ServicePoint.ConnectionLimit = item.Connectionlimit;
            }
        }

        private void SetCer(HttpItem item)
        {
            if (!string.IsNullOrEmpty(item.CerPath))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.CheckValidationResult);
                this.request = (HttpWebRequest)WebRequest.Create(item.URL);
                this.SetCerList(item);
                this.request.ClientCertificates.Add(new X509Certificate(item.CerPath));
            }
            else
            {
                this.request = (HttpWebRequest)WebRequest.Create(item.URL);
                this.SetCerList(item);
            }
        }

        private void SetCerList(HttpItem item)
        {
            if (item.ClentCertificates != null && item.ClentCertificates.Count > 0)
            {
                foreach (X509Certificate value in item.ClentCertificates)
                {
                    this.request.ClientCertificates.Add(value);
                }
            }
        }

        private void SetCookie(HttpItem item)
        {
            if (!string.IsNullOrEmpty(item.Cookie))
            {
                this.request.Headers[HttpRequestHeader.Cookie] = item.Cookie;
            }

            if (item.ResultCookieType == ResultCookieType.CookieCollection)
            {
                this.request.CookieContainer = new CookieContainer();
                if (item.CookieCollection != null && item.CookieCollection.Count > 0)
                {
                    this.request.CookieContainer.Add(item.CookieCollection);
                }
            }
        }

        private void SetPostData(HttpItem item)
        {
            if (!this.request.Method.Trim().ToLower().Contains("get"))
            {
                if (item.PostEncoding != null)
                {
                    this.postencoding = item.PostEncoding;
                }

                byte[] array = null;
                if (item.PostDataType == PostDataType.Byte && item.PostdataByte != null && item.PostdataByte.Length > 0)
                {
                    array = item.PostdataByte;
                }
                else if (item.PostDataType == PostDataType.FilePath && !string.IsNullOrEmpty(item.Postdata))
                {
                    StreamReader streamReader = new StreamReader(item.Postdata, this.postencoding);
                    array = this.postencoding.GetBytes(streamReader.ReadToEnd());
                    streamReader.Close();
                }
                else if (!string.IsNullOrEmpty(item.Postdata))
                {
                    array = this.postencoding.GetBytes(item.Postdata);
                }

                if (array != null)
                {
                    this.request.ContentLength = (long)array.Length;
                    this.request.GetRequestStream().Write(array, 0, array.Length);
                }
            }
        }

        private void SetProxy(HttpItem item)
        {
            bool flag = false;
            if (!string.IsNullOrEmpty(item.ProxyIp))
            {
                flag = item.ProxyIp.ToLower().Contains("ieproxy");
            }

            if (!string.IsNullOrEmpty(item.ProxyIp) && !flag)
            {
                if (item.ProxyIp.Contains(":"))
                {
                    string[] array = item.ProxyIp.Split(new char[]
                    {
                        ':'
                    });

                    WebProxy webProxy = new WebProxy(array[0].Trim(), Convert.ToInt32(array[1].Trim()));
                    webProxy.Credentials = new NetworkCredential(item.ProxyUserName, item.ProxyPwd);
                    this.request.Proxy = webProxy;
                }
                else
                {
                    WebProxy webProxy = new WebProxy(item.ProxyIp, false);
                    webProxy.Credentials = new NetworkCredential(item.ProxyUserName, item.ProxyPwd);
                    this.request.Proxy = webProxy;
                }
            }
            else if (!flag)
            {
                this.request.Proxy = item.WebProxy;
            }
        }

        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        private Encoding encoding = Encoding.Default;

        private Encoding postencoding = Encoding.Default;

        private HttpWebRequest request = null;

        private HttpWebResponse response = null;
    }
}
