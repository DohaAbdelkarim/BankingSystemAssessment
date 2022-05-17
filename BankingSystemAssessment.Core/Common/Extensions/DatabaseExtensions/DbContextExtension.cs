using BankingSystemAssessment.Core.ErrorHandling.Exceptions;
using BankingSystemAssessment.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace BankingSystemAssessment.Core.Extensions.DatabaseExtensions
{
    public static class DbContextExtension
    {
        public static async Task<T> PostAsync<T>(this DbContext context, T entity, ILogger logger) where T : class
        {
            if (entity != null)
            {
                context.Add(entity);
                var result = await context.SaveChangesAsync();
                if (result <= 0)
                {
                    using (logger.BeginScope(JsonSerializer.Serialize(entity)))
                    {
                        logger.LogWarning(LogEvents.InsertItem, $"Saving {entity.GetType().Name} failed");
                        throw new ApiException(HttpStatusCode.InternalServerError, ErrorCodes.DbUpdateException.ToString());
                    }
                }
                return entity;
            }
            else
                throw new ApiException(HttpStatusCode.InternalServerError, ErrorCodes.EntityNullException.ToString());
        }

        public static async Task<T> PutAsync<T>(this DbContext context, T entity, ILogger logger) where T : class
        {
            if (entity != null)
            {
                context.Attach(entity);
                context.Update(entity);
                var result = await context.SaveChangesAsync();
                if (result <= 0)
                {
                    using (logger.BeginScope(JsonSerializer.Serialize(entity)))
                    {
                        logger.LogWarning(LogEvents.UpdateItemFailed, $"Updating {entity.GetType().Name} {entity.GetType().GetProperty("Id").GetValue(entity)} failed");
                        throw new ApiException(HttpStatusCode.InternalServerError, ErrorCodes.DbUpdateException.ToString());
                    }
                }
                return entity;
            }
            else
                throw new ApiException(HttpStatusCode.InternalServerError, ErrorCodes.EntityNullException.ToString());

        }
    }
}