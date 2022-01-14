using Microsoft.AspNetCore.Mvc.Filters;
using API.Extensions;
using API.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class LogUserActivity: IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context,
            ActionExecutionDelegate next)
            {
                var resultContext = await next();
                
                if(!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

                var userId = resultContext.HttpContext.User.GetUserId();
                // var repo = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();
                // var user = await repo.GetUserByIdAsync(userId);
                var uow = resultContext.HttpContext.RequestServices.GetService<IUnitOfWork>();                
                var user = await uow.UserRepository.GetUserByIdAsync(userId);
                user.LastActive = DateTime.UtcNow; //DateTime.Now;
                // await repo.SaveAllAsync();
                await uow.Complete();

            }
    }
}