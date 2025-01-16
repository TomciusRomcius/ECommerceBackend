using Microsoft.AspNetCore.Mvc;

namespace Routes.TestRoute
{
  [ApiController]
  [Route("[controller]")]
  public class TestRouteController : ControllerBase
  {
    [HttpGet()]
    public string testController()
    {
      return "Test";
    }
  }
}