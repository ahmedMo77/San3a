using Microsoft.AspNetCore.Mvc;
using San3a.Core.DTOs.Base;
using System.Security.Claims;

namespace San3a.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        #region Protected Methods
        protected string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        protected string GetCurrentUserEmail()
        {
            return User.FindFirstValue(ClaimTypes.Email);
        }

        protected bool IsInRole(string role)
        {
            return User.IsInRole(role);
        }

        protected bool IsAdmin()
        {
            return User.IsInRole("Admin") || User.IsInRole("SuperAdmin");
        }

        protected IActionResult HandleResponse<T>(ApiResponse<T> response)
        {
            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        protected PaginationParams GetPaginationParams()
        {
            var pageNumber = Request.Query.ContainsKey("pageNumber") 
                ? int.Parse(Request.Query["pageNumber"].ToString()) 
                : 1;
            
            var pageSize = Request.Query.ContainsKey("pageSize") 
                ? int.Parse(Request.Query["pageSize"].ToString()) 
                : 10;

            return new PaginationParams 
            { 
                PageNumber = pageNumber, 
                PageSize = pageSize 
            };
        }
        #endregion
    }
}
