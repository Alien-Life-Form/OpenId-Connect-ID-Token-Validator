# :crown: :trophy: :moneybag: OIDC-JWT-Validator

This code provides the basic steps required to locally verify an ID Token signed using asymmetric encryption (RS256). It uses packages from Microsoft for key parsing and token validation. The code is also testable and comes with a suite of unit tests.

# Authentication vs. Authorization

- Authentication = who you are (eg. username + password)
- Authorization = permissions (eg. what you are allowed to do - read, write, execute)

# Definitions

### What is a `token` and how to get it?

The `token` would be any valid ID Token. Assuming we are using the Authorization Code Flow, to get a valid ID Token:
1. Make a call to the `/auth` endpoint to receive the `authorization code`
2. Make a call to the `/oidc/token` enpoint to receive an ID Token as well as other information

OneLogin has a clear explanation of the Authorization Code Flow:
1. https://developers.onelogin.com/openid-connect/api/authorization-code
2. https://developers.onelogin.com/openid-connect/api/authorization-code-grant

An ID Token usually comes in a form of a JSON Web Token (JWT). [Here](https://jwt.io/introduction/) is a good read on JWTs.

### What is an `issuer` and where to find it?

The `issuer` is the issuing authority - whoever gave you the ID Token, usually this will be the Identity Provider (eg. AzureAD, OneLogin), and it is nothing more than the value of the `iss` claim from the ID Token.

### What is an `audience` and where to find it?

The `audience` is the particular audience - the client, usually this will be the same as your `Client ID` issued by the Identity Provider, and it is nothing more than the value of the `aud` claim from the ID Token.

### What is a `nonce` and where to find it?

### What is a `wellKnownURL` and how to get it?

### Links
- https://developer.okta.com/authentication-guide/tokens/validating-id-tokens
- https://auth0.com/docs/tokens/id-token

# Testable vs Non-Testable Code

# How to Run

# Kudos
- https://stackoverflow.com/questions/47121732/how-to-properly-consume-openid-connect-jwks-uri-metadata-in-c
- https://stackoverrun.com/ru/q/9483098
- https://developer.okta.com/code/dotnet/jwt-validation
