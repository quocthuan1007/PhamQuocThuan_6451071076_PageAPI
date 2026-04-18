using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PageController : ControllerBase
    {
        private readonly FacebookService _facebookService;

        public PageController(FacebookService facebookService)
        {
            _facebookService = facebookService;
        }

        [HttpGet("{pageId}")]
        public async Task<IActionResult> GetPage(string pageId)
        {
            try
            {
                var result = await _facebookService.GetPageAsync(pageId);
                return Content(result, "application/json");
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{pageId}/posts")]
        public async Task<IActionResult> GetPosts(string pageId)
        {
            try
            {
                var result = await _facebookService.GetPagePostsAsync(pageId);
                return Content(result, "application/json");
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("{pageId}/posts")]
        public async Task<IActionResult> CreatePost(string pageId, [FromBody] PostDto dto)
        {
            try
            {
                var result = await _facebookService.CreatePostAsync(pageId, dto.Message);
                return Content(result, "application/json");
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("post/{postId}")]
        public async Task<IActionResult> DeletePost(string postId)
        {
            try
            {
                var result = await _facebookService.DeletePostAsync(postId);
                return Content(result, "application/json");
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("post/{postId}/comments")]
        public async Task<IActionResult> GetComments(string postId)
        {
            try
            {
                var result = await _facebookService.GetPostCommentsAsync(postId);
                return Content(result, "application/json");
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("post/{postId}/likes")]
        public async Task<IActionResult> GetLikes(string postId)
        {
            try
            {
                var result = await _facebookService.GetPostLikesAsync(postId);
                return Content(result, "application/json");
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{pageId}/insights")]
        public async Task<IActionResult> GetInsights(string pageId)
        {
            try
            {
                var result = await _facebookService.GetPageInsightsAsync(pageId);
                return Content(result, "application/json");
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
