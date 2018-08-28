using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

[TestClass]
public class Test_OpenIdConnectTokenValidator
{
    private Mock<IOpenIdConnectConfiguration> _discoveryDocument;

    private string token;
    private string issuer;
    private string audience;
    private string nonce;

    private string e;
    private string n;
    private string kid;

    private static byte[] Base64UrlDecoder(string base64Url)
    {
        string padded = base64Url.Length % 4 == 0
            ? base64Url : base64Url + "====".Substring(base64Url.Length % 4);
        string base64 = padded.Replace("_", "/")
                .Replace("-", "+");
        return Convert.FromBase64String(base64);
    }

    [TestInitialize]
    public void Initilize()
    {
        _discoveryDocument = new Mock<IOpenIdConnectConfiguration>(MockBehavior.Strict);

        token = "";
        issuer = "";
        audience = "";
        nonce = "";

        e = "";
        n = 
        kid = "";
    }

    [TestMethod]
    public async Task InvalidTokenThrowsArgumentException()
    {
        token = "7.7.7";

        var keys = new List<SecurityKey>();
        byte[] exponent = Base64UrlDecoder(e);
        byte[] modulus = Base64UrlDecoder(n);
        var rsaParameters = new RSAParameters { Exponent = Base64UrlDecoder(e), Modulus = Base64UrlDecoder(n) };
        var rsaSecurityKey = new RsaSecurityKey(rsaParameters) { KeyId = kid };
        keys.Add(rsaSecurityKey);
        _discoveryDocument.SetupGet(d => d.SigningKeys).Returns(keys);

        var validator = new OpenIdConnectTokenValidator();

        try
        {
            bool result = validator.ValidateOpenIdConnectJSONWebToken(token, issuer, audience, nonce, _discoveryDocument.Object, false);
        }
        catch (Exception exception)
        {
            Assert.IsTrue(exception is ArgumentException);
        }
    }

    [TestMethod]
    public async Task InvalidAudienceThrowsSecurityTokenInvalidAudienceException()
    {
        audience = "777";

        var keys = new List<SecurityKey>();
        byte[] exponent = Base64UrlDecoder(e);
        byte[] modulus = Base64UrlDecoder(n);
        var rsaParameters = new RSAParameters { Exponent = Base64UrlDecoder(e), Modulus = Base64UrlDecoder(n) };
        var rsaSecurityKey = new RsaSecurityKey(rsaParameters) { KeyId = kid };
        keys.Add(rsaSecurityKey);
        _discoveryDocument.SetupGet(d => d.SigningKeys).Returns(keys);

        var validator = new OpenIdConnectTokenValidator();

        try
        {
            bool result = validator.ValidateOpenIdConnectJSONWebToken(token, issuer, audience, nonce, _discoveryDocument.Object, false);
        }
        catch (Exception exception)
        {
            Assert.IsTrue(exception is SecurityTokenInvalidAudienceException);
        }
    }

    [TestMethod]
    public async Task InvalidIssuerThrowsSecurityTokenInvalidIssuerException()
    {
        issuer = "777";

        var keys = new List<SecurityKey>();
        byte[] exponent = Base64UrlDecoder(e);
        byte[] modulus = Base64UrlDecoder(n);
        var rsaParameters = new RSAParameters { Exponent = Base64UrlDecoder(e), Modulus = Base64UrlDecoder(n) };
        var rsaSecurityKey = new RsaSecurityKey(rsaParameters) { KeyId = kid };
        keys.Add(rsaSecurityKey);
        _discoveryDocument.SetupGet(d => d.SigningKeys).Returns(keys);

        var validator = new OpenIdConnectTokenValidator();

        try
        {
            bool result = validator.ValidateOpenIdConnectJSONWebToken(token, issuer, audience, nonce, _discoveryDocument.Object, false);
        }
        catch (Exception exception)
        {
            Assert.IsTrue(exception is SecurityTokenInvalidIssuerException);
        }
    }

    [TestMethod]
    public async Task ExpiredTokenThrowsSecurityTokenExpiredException()
    {

        var keys = new List<SecurityKey>();
        byte[] exponent = Base64UrlDecoder(e);
        byte[] modulus = Base64UrlDecoder(n);
        var rsaParameters = new RSAParameters { Exponent = Base64UrlDecoder(e), Modulus = Base64UrlDecoder(n) };
        var rsaSecurityKey = new RsaSecurityKey(rsaParameters) { KeyId = kid };
        keys.Add(rsaSecurityKey);
        _discoveryDocument.SetupGet(d => d.SigningKeys).Returns(keys);

        var validator = new OpenIdConnectTokenValidator();

        try
        {
            bool result = validator.ValidateOpenIdConnectJSONWebToken(token, issuer, audience, nonce, _discoveryDocument.Object, true);
        }
        catch (Exception exception)
        {
            Assert.IsTrue(exception is SecurityTokenExpiredException);
        }
    }

    [TestMethod]
    public async Task InvalidNonceThrowsSecurityTokenValidationException()
    {
        nonce = "777";

        var keys = new List<SecurityKey>();
        byte[] exponent = Base64UrlDecoder(e);
        byte[] modulus = Base64UrlDecoder(n);
        var rsaParameters = new RSAParameters { Exponent = Base64UrlDecoder(e), Modulus = Base64UrlDecoder(n) };
        var rsaSecurityKey = new RsaSecurityKey(rsaParameters) { KeyId = kid };
        keys.Add(rsaSecurityKey);
        _discoveryDocument.SetupGet(d => d.SigningKeys).Returns(keys);

        var validator = new OpenIdConnectTokenValidator();

        bool result = validator.ValidateOpenIdConnectJSONWebToken(token, issuer, audience, nonce, _discoveryDocument.Object, false);
        Assert.AreEqual(false, result);
    }
    
    [TestMethod]
    public async Task NullValidateLifetimeThrowsArgumentNullException()
    {
        var keys = new List<SecurityKey>();
        _discoveryDocument.SetupGet(d => d.SigningKeys).Returns(keys);

        var validator = new OpenIdConnectTokenValidator();

        try
        {
            bool result = validator.ValidateOpenIdConnectJSONWebToken(token, issuer, audience, nonce, _discoveryDocument.Object, null);
        }
        catch (Exception exception)
        {
            Assert.IsTrue(exception is ArgumentNullException);
        }
    }
    
    [TestMethod]
    public async Task NullDiscoveryDocumentThrowsArgumentNullException()
    {
        var keys = new List<SecurityKey>();
        _discoveryDocument.SetupGet(d => d.SigningKeys).Returns(keys);

        var validator = new OpenIdConnectTokenValidator();

        try
        {
            bool result = validator.ValidateOpenIdConnectJSONWebToken(token, issuer, audience, nonce, null, false);
        }
        catch (Exception exception)
        {
            Assert.IsTrue(exception is ArgumentNullException);
        }
    }

    [TestMethod]
    public async Task EmptyAudienceThrowsArgumentNullException()
    {
        issuer = "";

        var keys = new List<SecurityKey>();
        _discoveryDocument.SetupGet(d => d.SigningKeys).Returns(keys);

        var validator = new OpenIdConnectTokenValidator();

        try
        {
            bool result = validator.ValidateOpenIdConnectJSONWebToken(token, issuer, audience, nonce, _discoveryDocument.Object, false);
        }
        catch (Exception exception)
        {
            Assert.IsTrue(exception is ArgumentNullException);
        }
    }

    [TestMethod]
    public async Task EmptyNonceThrowsArgumentNullException()
    {
        nonce = "";

        var keys = new List<SecurityKey>();
        _discoveryDocument.SetupGet(d => d.SigningKeys).Returns(keys);

        var validator = new OpenIdConnectTokenValidator();

        try
        {
            bool result = validator.ValidateOpenIdConnectJSONWebToken(token, issuer, audience, nonce, _discoveryDocument.Object, false);
        }
        catch (Exception exception)
        {
            Assert.IsTrue(exception is ArgumentNullException);
        }
    }


    [TestMethod]
    public async Task EmptyIssuerThrowsArgumentNullException()
    {
        string audience = "";

        var keys = new List<SecurityKey>();
        _discoveryDocument.SetupGet(d => d.SigningKeys).Returns(keys);

        var validator = new OpenIdConnectTokenValidator();

        try
        {
            bool result = validator.ValidateOpenIdConnectJSONWebToken(token, issuer, audience, nonce, _discoveryDocument.Object, false);
        }
        catch (Exception exception)
        {
            Assert.IsTrue(exception is ArgumentNullException);
        }
    }

    [TestMethod]
    public async Task EmptyTokenThrowsArgumentNullException()
    {
        token = "";

        var keys = new List<SecurityKey>();
        _discoveryDocument.SetupGet(d => d.SigningKeys).Returns(keys);

        var validator = new OpenIdConnectTokenValidator();

        try
        {
            bool result = validator.ValidateOpenIdConnectJSONWebToken(token, issuer, audience, nonce, _discoveryDocument.Object, false);
        }
        catch (Exception exception)
        {
            Assert.IsTrue(exception is ArgumentNullException);
        }
    }

    [TestMethod]
    public async Task ValidKeyReturnsTrue()
    {
        var keys = new List<SecurityKey>();
        byte[] exponent = Base64UrlDecoder(e);
        byte[] modulus = Base64UrlDecoder(n);
        var rsaParameters = new RSAParameters { Exponent = Base64UrlDecoder(e), Modulus = Base64UrlDecoder(n) };
        var rsaSecurityKey = new RsaSecurityKey(rsaParameters) { KeyId = kid };
        keys.Add(rsaSecurityKey);
        _discoveryDocument.SetupGet(d => d.SigningKeys).Returns(keys);

        var validator = new OpenIdConnectTokenValidator();
        bool result = validator.ValidateOpenIdConnectJSONWebToken(token, issuer, audience, nonce, _discoveryDocument.Object, false);

        Assert.AreEqual(true, result);
    }

    [TestMethod]
    public async Task InvalidExponentReturnsFalse()
    {
        n = "777";

        var keys = new List<SecurityKey>();
        byte[] exponent = Base64UrlDecoder(e);
        byte[] modulus = Base64UrlDecoder(n);
        var rsaParameters = new RSAParameters { Exponent = Base64UrlDecoder(e), Modulus = Base64UrlDecoder(n) };
        var rsaSecurityKey = new RsaSecurityKey(rsaParameters) { KeyId = kid };
        keys.Add(rsaSecurityKey);
        _discoveryDocument.SetupGet(d => d.SigningKeys).Returns(keys);

        var validator = new OpenIdConnectTokenValidator();
        bool result = validator.ValidateOpenIdConnectJSONWebToken(token, issuer, audience, nonce, _discoveryDocument.Object, false);

        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public async Task InvalidModulusReturnsFalse()
    {
        e = "777";

        var keys = new List<SecurityKey>();
        byte[] exponent = Base64UrlDecoder(e);
        byte[] modulus = Base64UrlDecoder(n);
        var rsaParameters = new RSAParameters { Exponent = Base64UrlDecoder(e), Modulus = Base64UrlDecoder(n) };
        var rsaSecurityKey = new RsaSecurityKey(rsaParameters) { KeyId = kid };
        keys.Add(rsaSecurityKey);
        _discoveryDocument.SetupGet(d => d.SigningKeys).Returns(keys);

        var validator = new OpenIdConnectTokenValidator();
        bool result = validator.ValidateOpenIdConnectJSONWebToken(token, issuer, audience, nonce, _discoveryDocument.Object, false);

        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public async Task NoKeysReturnsFalse()
    {
        var keys = new List<SecurityKey>();
        _discoveryDocument.SetupGet(d => d.SigningKeys).Returns(keys);

        var validator = new OpenIdConnectTokenValidator();
        bool result = validator.ValidateOpenIdConnectJSONWebToken(token, issuer, audience, nonce, _discoveryDocument.Object, false);

        Assert.AreEqual(false, result);
    }
}
