using JetBrains.Annotations;

namespace Stores.Infrastructure;

public enum StoreErrorCode
{
    None,
    AddException,
    GetException,
    UpdateException,
    RemoveException,
    NotFound
}

[ UsedImplicitly ]
public class StoreResult<TData>
{
    [ UsedImplicitly ]
    public TData? Data { get; init; }

    [ UsedImplicitly ]
    public bool Success { get; init; }

    [ UsedImplicitly ]
    public StoreErrorCode Code { get; init; } = StoreErrorCode.None;

    [ UsedImplicitly ]
    public string Error { get; init; } = string.Empty;

    [ UsedImplicitly ]
    public Exception? Exception { get; init; }
}