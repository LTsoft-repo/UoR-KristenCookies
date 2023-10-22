namespace Logic.Infrastructure;

public enum ResultErrorCode
{
    None,
    IncompleteDozen,
    EmptyOrder,
    StoreError,
    AlreadyExists
}

public class Result<T>
{
    public T? Data { get; init; } = default;

    public bool Success { get; init; }

    public ResultErrorCode Code { get; init; } = ResultErrorCode.None;

    public string Error { get; init; } = string.Empty;
}