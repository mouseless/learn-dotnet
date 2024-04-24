# Authentication

## Multi Authentication

`MultiAuthentication` is a sample project to demonstrate how to use 
`Authentication` and `Authorization` features implemented by `.net` to achive
a mechanism for providing authentication for single endpoint from  multiple 
schemes.

### Using `Authorize` attribute with multiple scheme values

First option is to set _AuthenticationSchemes_ value of `Authorize` attribute
and register multiple schemes with `AuthenticationBuilder`.

```csharp
//Program.cs

builder.Services.
    .AddAuthentication()
    .AddScheme<AuthenticationSchemeOptions, HandlerOne>("SchemeOne", options => { })
    .AddScheme<AuthenticationSchemeOptions, HandlerTwo>("SchemeTwo", options => { });
```

```csharp
//Action in controller

[HttpGet]
[Authorize(AuthenticationSchemes = "ShemeOne,SchemeTwo")]
public void Action()
{
    ...
}
```

> [!NOTE]
>
> The a request is made to the endpoint, will be handled by all two named schemes 
> and the result of each handler will be combined in final result.

> [!NOTE]
>
> Authorize attribute with no specific scheme will forward request to default
> scheme. When multiple schemes are added, default authentication scheme cannot 
> be determined, so default scheme should be set explicitly

### Building `Policy` with multiple authentication schemes

Another options is to set _Policy_ value of `Authorize` attribute and add an 
`AuthorizationPolicy` with multiple schemes.

```csharp
source.AddAuthorization(options =>
{
    ...

    var policyBuilder = new AuthorizationPolicyBuilder(
        "SchemeOne",
        "SchemeTwo"
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

> [!NOTE]
>
> The a request is made to the endpoint, will be handled by all two named schemes 
> and the result of each handler will be combined in final result.

> [!NOTE]
>
> If the policy is not added as default policy, when `Authorize` attribute
> is used without a specific policy, an exception will be thrown which will
> state a default policy should be set.

### Configuring `ForwardSelector` of `AuthenticationSchemeOptions`

Third option which is sort of a work around is to set `ForwardSelector` 
property of `AuthenticationSchemeOptions` for all handlers, and selector will 
forward the request accordingly to each handlers.

This option requiers a default scheme and handler to be set when configuring 
authentication and options to be configured for default handler.

```csharp
source.AddAuthentication(options =>
{
    options.DefaultScheme = "Default";
    options.DefaultAuthenticateScheme = "Default";
    options.AddScheme<ApiKeyAuthenticationHandler>("Default", "Default");
    //Add other schemes
});

source.Configure<AuthenticationSchemeOptions>("Default", options =>
    options.ForwardDefaultSelector = context =>
    {
        if (...)
        {
            return "SchemeOne";
        }

        if (...)
        {
            return "SchemeTwo";
        }

        return "Default";
    });

source.AddOptions<AuthenticationSchemeOptions>();
```