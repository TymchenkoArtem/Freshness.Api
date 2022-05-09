using Freshness.Common.CustomExceptions;
using Freshness.Common.ResponseMessages;
using System;
using System.Linq;
using System.Security.Claims;

namespace Freshness.Common.Extensions
{
    public static class ClaimsExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal userClaims)
        {
            try
            {
                var claim = userClaims.Claims.FirstOrDefault(w => w.Type == ClaimTypes.NameIdentifier)?.Value;

                var id = new Guid(claim);

                if (id == Guid.Empty)
                {
                    throw new CustomException(ResponseMessage.UserDoesNotExist);
                }

                return id;
            }
            catch
            {
                throw new CustomException(ResponseMessage.UserDoesNotExist);
            }
        }
    }
}
