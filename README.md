> ⚠️ **Caution**
> This project is under active development, and the architecture may change significantly.  
> Use at your own risk (or better yet, please don’t use it for now).

> 🇯🇵 **日本語:**  
> 注意！このプロジェクトは現在開発中であり、アーキテクチャが大幅に変更される可能性があります。  
> ご利用は自己責任でお願いします（あるいは、安定するまで使用しないでください）。

## Current Direction of Renovation

While retaining the ability to check for exceptions using attributes, we aim to make it possible to assert exception *contents* in a natural and expressive way.

### Current Usage

You can verify that a specific exception type was thrown, but cannot easily assert on exception details:

```csharp
[ExceptionFact(typeof(InvalidOperationException), "RecipeBook")]
public static void MyTest()
{
    SomeMethodExpectedToThrowTheException();
}
```

### Planned Usage (TO BE)
You will be able to record the exception and assert its properties more freely:
```csharp
[ExceptionFact(typeof(InvalidOperationException), "already")]
public async Task MyTest(IExceptionRecorder recorder)
{
    var ex = await recorder.RecordAsync(() => SomeAsyncMethod());

    // Additional assertions
    Assert.Equal("somewhere", ex.Source);
}
```

Please wait patiently (and without expectations). 🙂
