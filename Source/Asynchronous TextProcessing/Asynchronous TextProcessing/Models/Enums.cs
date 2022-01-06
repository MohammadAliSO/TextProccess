namespace Asynchronous_TextProcessing.Models;

public enum RequestState
{
    Active = 1,
    InProcess = 2,
    Done = 3,
    Incomplete = 4,
    Failure = 5
}

public enum RequestType
{
    TextProcessing = 1
}

public enum Permissiontype
{
    Request,
    Result,
    GetUser,
    RequestUser,
    AddUser,
    DeleteUser,
    AddPermission,
    DeletePermission,
    GetPermission
}
