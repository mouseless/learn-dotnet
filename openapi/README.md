# OpenAPI

OpenAPI provides a standard format for defining the API endpoints in a project
and all their features (such as HTTP methods, parameters, response types, and
error codes). This allows it to be supported by many documentation, reporting,
and tool libraries. OpenAPI is an industry-standard specification and enables
APIs to be defined in a common format like JSON. For example, Swagger UI can
generate automatic documentation based on the OpenAPI specification.

Although OpenAPI is used in many different places, we will focus on its
customized use in `ASP.NET`.

## How is it work?

In projects created with .NET 9, OpenAPI support is enabled by default. However,
if you want to add it manually, you first need to include the
`Microsoft.AspNetCore.OpenApi` package in your project. Then, you need to
register OpenAPI as follows:

```csharp
builder.Services.AddOpenApi();
```
The AddOpenApi method also has overrides that allow you to customize the
generated OpenAPI document. As an example, the code below shows how to change
the default version of the OpenAPI document:

```csharp
builder.Services.AddOpenApi(options =>
{
    options.OpenApiVersion = OpenApiSpecVersion.OpenApi2_0;
});
```

At this point, OpenAPI is enabled, but we haven’t done the "magic" yet😄. To
expose the schema, we need to map the endpoints as shown below:

```csharp
app.MapOpenApi();
```

> :warning:
>
> It’s a good practice to do this only in developer mode. If you don’t want to
> expose the entire schema to everyone, make sure to restrict access in
> production environments.

Now the OpenAPI configuration is complete. After running the application, you
can navigate to `/openapi/v1.json` to see a schema like the one below:

```json
{
  "openapi": "3.0.1",
  "info": {
    "title": "OpenAPI | v1",
    "version": "1.0.0"
  },
  "servers": [
    {
      "url": "http://localhost:5110"
    }
  ],
  "paths": { },
  "components": { },
  "tags": [
    {
      "name": "OpenAPI"
    }
  ]
}
```

In this schema, the paths section contains the endpoints exposed by your
application. Below is an example of an endpoint model that handles a `GET`
request to the root `/` path:

```json
"paths": {
  "/": {
    "get": {
      "tags": [
        "OpenAPI"
      ],
      "responses": {
        "200": {
          "description": "OK",
          "content": {
            "text/plain": {
              "schema": {
                "type": "string"
              }
            }
          }
        }
      }
    }
  }
}
```

The above model specifies that a `GET` request to the root URI will return a
response with string type data.

## UI for better analyze schema

Reading and testing this schema directly might be difficult for users. If you
want a better documented and more UI, there are many libraries available to
achieve this. Below is an example of how to integrate Swagger UI and connect it
to the OpenAPI schema.

First, you need to include the `Swashbuckle.AspNetCore.SwaggerUI` package in
your project. Then, you can configure Swagger UI as shown below by specifying
the path to the OpenAPI schema. This will create a UI available at
`/swagger/index.html`:

```csharp
app.MapOpenApi("/openapi/mouseless/openapi.json");
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/mouseless/openapi.json", "API");
});
```

> :warning:
> One important thing to note here is that `UseSwaggerUI` must be called after
> `MapOpenApi`. Otherwise, Swagger UI will not work correctly.

Once this is set up, you can easily explore and test your API endpoints via the
Swagger UI.

If you want more configurations and a more user-friendly UI, you can check out
how to use Swashbuckle instead of just SwaggerUI by visiting
[`/swashbuckle`](/swashbuckle/README.md).
