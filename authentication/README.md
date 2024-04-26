# Authentication

## Multi Authentication

`MultiAuthentication` is a sample project to demonstrate how to use 
`Authentication` and `Authorization` features implemented by `.net` to achive
a mechanism for providing authentication for single endpoint with multiple 
authentication methods.

### Configuring `ForwardSelector` of `AuthenticationSchemeOptions`

The prefered method is to set `ForwardSelector` property of 
`AuthenticationSchemeOptions`. This way configured selector will forward the
request only to the desired handler.

This setup requiers a default scheme and handler to be set when configuring 
authentication, and authorization policies should not specify any 
authentication schemes when added.

```csharp
source.AddAuthentication(options =>
{
    options.DefaultScheme = "Default";
    options.DefaultAuthenticateScheme = "Default";
    options.AddScheme<DefaultAuthenticationHandler>("Default", default);
    options.AddScheme<AlternativeAuthenticationHandler>("Alternative", default);
    options.AddScheme<AnonymousAuthenticationHandler>("Anonymous", default);
});

source.Configure<AuthenticationSchemeOptions>("MultiAuthentication", options =>
    options.ForwardDefaultSelector = context =>
    {
        if (...)
        {
            return "Default";
        }

        if (...)
        {
            return "Alternative";
        }

        return "Anonymous";
    });

source.AddOptions<AuthenticationSchemeOptions>();
source.AddAuthorization(options =>
{
    ...
    options.AddPolicy("Policy", policy => policy.RequireClaim("Claim"));
});
```

```csharp
//Action in controller

[HttpGet]
[Authorize(Policy = "Policy")]
public void Action()
{
    ...
}
```

### Other Methods

> [!NOTE]
>
> The request made to an endpoint with these two methods will forward requests
> to all named schemes and the result of each handler will be combined in final
> claims result.

#### Using `Authorize` attribute with multiple scheme values

```csharp
//Action in controller

[HttpGet]
[Authorize(AuthenticationSchemes = "Default,Alternative")]
public void Action()
{
    ...
}
```

#### Building `Policy` with multiple authentication schemes

```csharp
source.AddAuthorization(options =>
{
    ...

    var policyBuilder = new AuthorizationPolicyBuilder(
        "Default",
        "Alternative"
    ).RequireAuthenticatedUser();

    options.AddPolicy("PolicyWithMultiSchemes", policyBuilder.Build());
});
```

```csharp
//Action in controller

[HttpGet]
[Authorize(Policy = "PolicyWithMultiSchemes")]
public void Action()
{
    ...
}
```
