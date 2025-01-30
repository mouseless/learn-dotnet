# Migration Notes

Here we list the operations performed when upgrading projects to .Net 9

> [!NOTE]
>
> The order is not important for now.

- [ ] Upgrade `dotnet` and `language` version in `csproj` or `build.props`
- [ ] Upgrade `dotnet` version to 9 and `checkout`, `setup-dotnet` version to
  `4` in workflows
- [ ] Upgrade libraries to new versions
- [ ] (Optional) If `Base64` encoded information is carried in the url, use
  `Base64Url`.
- [ ] (Optional) If you need to take parameters with an `Array` using `params`
  and then convert to `IEnumerable` type, use `IEnumerable` instead of `Array`.
- [ ] (Optional) `GeneratedRegex` can work with properties. It can be switched
  for a better usage.
- [ ] (Optional) See if `UseExceptionHandler(ExceptionHandlerOptions options)`
  override is available. Especially for the `StatusCodeSelector` property.
- [ ] (Optional) Use the new linQ extensions(`CountBy`, `AggregateBy`,
  `Index<TSource>(IEnumerable<TSource>)`).
- [ ] (Optional) Use new  `TimeSpan.From*` overloads
  - `FromDays`
  - `FromHours`
  - `FromMinutes`
  - `FromSeconds`
  - `FromMilliseconds`
  - `FromMicroseconds`
- [ ] (Optional) Use Keyed Services in Middlewares.
