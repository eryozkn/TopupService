using Microsoft.AspNetCore.Mvc;

namespace TopupService.UnitTests
{
    public static class TestUtils
    {
        public static T? GetObjectResultContent<T>(ActionResult<T> result)
        {
            return (T?)(result.Result as ObjectResult)?.Value;
        }
    }
}
