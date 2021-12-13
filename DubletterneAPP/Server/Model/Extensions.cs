using Core;
namespace Server.Model;

public static class Extensions
{
    public static IActionResult ToActionResult(this Response status) => status switch
    {
        Response.Updated => new NoContentResult(),
        Response.Deleted => new NoContentResult(),
        Response.NotFound => new NotFoundResult(),
        Response.Conflict => new ConflictResult(),
        _ => throw new NotSupportedException($"{status} not supported")
    };

    public static ActionResult<T> ToActionResult<T>(this Option<T> option) where T : class
        => option.IsSome ? option.Value : new NotFoundResult();
}