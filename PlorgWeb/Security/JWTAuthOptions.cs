using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PlorgWeb.Security {
    public static class JWTAuthOptions {
        public static string ISSUER {
            get {
                if (builder == null)
                    throw new ArgumentNullException("builder is null");
                return builder.Configuration["JWT:AUDIENCE"];
            }
        }

        public static string AUDIENCE {
            get {
                if (builder == null)
                    throw new ArgumentNullException("builder is null");
                return builder.Configuration["JWT:AUDIENCE"];
            }
        }

        public static SymmetricSecurityKey KEY {
            get {
                if (builder == null)
                    throw new ArgumentNullException("builder is null");
                return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:KEY"]));
            }
        }

        public static WebApplicationBuilder? builder;
    }
}
