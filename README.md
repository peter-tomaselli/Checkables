# Checkables

## `Maybe`
`Maybe` is implemented here as a `struct` that wraps a reference type. The core API is meant to look exactly like `Nullable`:
* ```bool HasValue { get; }```
* ```T Value { get; } // throws```

## `ICheckable`
The two method signatures above have been pulled out into an `interface` called `ICheckable`. Extension methods on `ICheckable` make it monadic:
* ```static Maybe<U> FlatMap<T, U>(this ICheckable<T> checkable, Func<T, Maybe<U>> func)```
* ```static Maybe<U> Map<T, U>(this ICheckable<T> checkable, Func<T, U> func)```

## `Guarded`
`Guarded` is a variation of `Maybe` that takes an additional 'guard function' during construction. `Guarded` acts just like `Maybe` but with an additional check: in order to yield its value, the guard function must pass.
* See [the class documentation](https://github.com/peter-tomaselli/Checkables/blob/master/Checkables/Checkables/Guarded.cs) for more details on construction.
* `Guarded` also implements `ICheckable`, so it can be used to declaratively implements things like access rules into chains of still-composable data transformations.

### `Guarded` Example
```csharp
class MyClass
{
  bool CheckUserAccess(int userId)
  {
    return _someFictionalService.doesUserHaveAccess(userId);
  }
  
  Guarded<MyEntity> GetSensitiveData(int userId)
  {
    return new Guarded<MyEntity>(
      () => _sensitiveDataService.GetSomeData(), 
      userId => CheckUserAccess(userId)
    );
  }
  
  void ProcessSensitiveData(int userId)
  {
    GetSensitiveData(userId).Map(/* only invoked if the guard function passes */) ...
  }
}
```
