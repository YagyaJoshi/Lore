using DotNetOpenAuth.AspNet;
using DotNetOpenAuth.AspNet.Clients;
using DotNetOpenAuth.OAuth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace Loregroup
{
    public class GoogleCustomClient : OAuth2Client
    {
        private const string AuthorizationEndpoint = "https://accounts.google.com/o/oauth2/auth";
    private const string TokenEndpoint = "https://accounts.google.com/o/oauth2/token";
    private readonly string _clientId;
    private readonly string _clientSecret;
    private string absoluteReturnUrl = string.Empty;


    public GoogleCustomClient(string clientId, string clientSecret)
        : base("Google")
    {
        this._clientId = clientId;
        this._clientSecret = clientSecret;
    }
    protected override Uri GetServiceLoginUrl(Uri returnUrl)
    {
        StringBuilder serviceUrl = new StringBuilder();
        serviceUrl.AppendFormat("{0}?", AuthorizationEndpoint);
        serviceUrl.Append("response_type=code");
        serviceUrl.AppendFormat("&client_id={0}", this._clientId);
        serviceUrl.Append("&scope=email");
        //absoluteReturnUrl = Regex.Match(returnUrl.ToString(), "https://hawksight.connekt.in/Account/ExternalLoginCallback").Value;
        absoluteReturnUrl = "http://Loregroup.com/Account/ExternalLoginCallback";
        serviceUrl.AppendFormat("&redirect_uri={0}", Uri.EscapeDataString(absoluteReturnUrl));
        serviceUrl.AppendFormat("&state={0}", Regex.Match(returnUrl.AbsoluteUri, "(?<=__sid__=).*?($|&)", RegexOptions.IgnoreCase).Value);
        return new Uri(serviceUrl.ToString());
    }
    protected override IDictionary<string, string> GetUserData(string accessToken)
    {
        var request = WebRequest.Create("https://www.googleapis.com/oauth2/v1/userinfo?access_token=" + Uri.EscapeDataString(accessToken));

        string responseText = string.Empty;
        using (var response = request.GetResponse())
        {
            using (var responseStream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    responseText = reader.ReadToEnd();
                }
            }
        }
        Dictionary<string, string> userData = new Dictionary<string, string>();
        JavaScriptSerializer deserializer = new JavaScriptSerializer();
        Dictionary<string, string> responseData = deserializer.Deserialize<Dictionary<string, string>>(responseText);
        userData.Add("id", responseData["id"]);
        userData.Add("firstname", responseData["given_name"]);
        userData.Add("lastname", responseData["family_name"]);
        userData.Add("emailAddress", responseData["email"]);
        userData.Add("picture", responseData["picture"]);
        userData.Add("accesstoken", "");
        userData.Add("allData", responseText);
        return userData;
    }
    protected override string QueryAccessToken(Uri returnUrl, string authorizationCode)
    {
        StringBuilder postData = new StringBuilder();
        postData.Append("grant_type=authorization_code");
        postData.AppendFormat("&code={0}", authorizationCode);
        postData.AppendFormat("&redirect_uri={0}", absoluteReturnUrl.EndsWith(returnUrl.ToString(), StringComparison.OrdinalIgnoreCase) ? HttpUtility.UrlEncode(absoluteReturnUrl) : "");
        postData.AppendFormat("&client_id={0}", this._clientId);
        postData.AppendFormat("&client_secret={0}", this._clientSecret);

        string response = "";
        string accessToken = "";

        var webRequest = (HttpWebRequest)WebRequest.Create(TokenEndpoint);

        webRequest.Method = "POST";
        webRequest.ContentType = "application/x-www-form-urlencoded";

        try
        {
            using (Stream s = webRequest.GetRequestStream())
            {
                using (StreamWriter sw = new StreamWriter(s))
                    sw.Write(postData.ToString());
            }
            using (WebResponse webResponse = webRequest.GetResponse())
            {
                using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    response = reader.ReadToEnd();
                }
            }

            JavaScriptSerializer deserializer = new JavaScriptSerializer();
            var userData = deserializer.Deserialize<Dictionary<string, string>>(response);
            accessToken = (string)userData["access_token"];
        }
        catch (Exception)
        {
            return null;
        }

        return accessToken;

    }

    public override AuthenticationResult VerifyAuthentication(HttpContextBase context, Uri returnPageUrl)
    {
        

        string code = context.Request.QueryString["code"];
        if (string.IsNullOrEmpty(code))
        {
            return AuthenticationResult.Failed;
        }

        string accessToken = this.QueryAccessToken(returnPageUrl, code);
        if (accessToken == null)
        {
            return AuthenticationResult.Failed;
        }

        IDictionary<string, string> userData = this.GetUserData(accessToken);
        if (userData == null)
        {
            return AuthenticationResult.Failed;
        }

        string firstname = userData["firstname"];
        string lastname = userData["lastname"];
        string email = userData["emailAddress"];
        string id = userData["id"];
        string image = userData["picture"];
        string alldata = userData["allData"];
        
        
        userData["accesstoken"] = accessToken;

        return new AuthenticationResult(isSuccessful: true, provider: this.ProviderName, providerUserId: id, userName: firstname + " "+lastname, extraData: userData);
    }

    }
}