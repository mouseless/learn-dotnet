# Swashbuckle

We use `Swashbuckle` to generate Swagger documents and UI. It is easy to setup
and use swashbuckle, but it starts to get complicated when it comes to using
filters and generating multiple swagger documents form one api project.

## Basic Setup

Setting up swashbuckle is straightforward. `AddSwaggerGen`, `UseSwagger` and
`UseSwaggerUI` does the trick;

```csharp
...
builder.Services.AddSwaggerGen();
...
app.UseSwagger();
app.UseSwaggerUI();
...
```

## Filters

Configuration of swagger files can be done through `AddSwaggerGen` but we prefer
`ConfigureSwaggerGen` since it allows you to configure it after adding it.
Either way it suits our needs using the filter system (swashbuckle's name for
conventions) rather than configuring one by one.

There are filters for each level of configuration. `Document`, `Schema`,
`Operation`, `Parameter` etc. Use
`swaggerGenOptions.OperationFilter<YourFilter>([arg1, arg2])` to add a filter.

Every filter has one method to implement `Apply` with two arguments, thing to
configure and its context.

```csharp
swaggerGenOptions.OperationFilter<SampleOperationFilter>()

...

public class SampleOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // filter operation, and return if not targeted
        if (context.ApiDescription.HttpMethod != "POST") { return; }

        // then apply your configurations to operation
    }
}

```

## Action Groups

By default, actions are grouped under their controller name.
`SwaggerGenOptions` has `TagActionsBy` to group actions. To change default
tagging we make use of `GroupName` property of api description and configure
swashbuckle to use group name for tags.

```csharp
swaggerGenOptions.TagActionsBy(api => [api.GroupName]);

...

[ApiExplorerSettings(GroupName = "Customized")]
[HttpGet("/route")]
public void ActionWithCustomGroup() { }
```

> [!TIP]
>
> When you do this, swashbuckle doesn't include endpoints in default document.
> To make it so use `swaggerGenOptions.DocInclusionPredicate((_, _) => true);`
> More on this later.

## Custom Metadata

Assume you require more metadata than `GroupName`, e.g., you may want to mark an
endpoint as internal, through a custom attribute `InternalAttribute`, so that
they don't get documented. To achieve this, write a custom attribute class and
resolve it using `CustomAttributes()` extension of `ApiDescription` class.

```csharp
swaggerGenOptions.DocInclusionPredicate((_, api) =>
    !api.CustomAttributes().OfType<InternalAttribute>().Any()
);

...

[HttpGet("/internal")]
[Internal]
public void InternalAction() { }
```

> [!TIP]
>
> Since `ApiDescription` instance is provided in filters as well, you can make
> use of this custom metadata approach during applying of filters.

## Multi Document

Assume you have two different set of apis from the same api application, where
you want to separate your endpoint documentation into multiple documents.

To achieve this, create a custom metadata `DocumentAttribute` where you accept
name of the document you want your action in. Then add multiple swagger
documents through `.SwaggerDoc()` method of `swaggerGenOptions` instance. Define
your endpoint file using in `swaggerUiOptions` instance. And use
`DocInclusionPredicate` to decide which endpoints go where.

```csharp
swaggerGenOptions.DocInclusionPredicate((document, api) =>
    api.CustomAttributes()
        .OfType<DocumentAttribute>()
        .SingleOrDefault()
        ?.Name == document
);

...

[Document("admin")]
public void AdminAction() { }

[Document("api")]
public void ApiAction() { }
```

> [!TIP]
>
> You may also use `context.DocumentName` in filters to decide if target
> document is ok to apply the filter to.

### Document Based Security

Once you have different documents, you might also support different ways to
authenticate your api. To define security requirements;

- Create a document based document filter and add it to define security
  definition per document
- Create a document based operation filter and add security requirements based
  on their documents

---

See [sample project](./Swashbuckle/) for working examples.
