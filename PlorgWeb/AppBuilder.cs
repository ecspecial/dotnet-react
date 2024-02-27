using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PlorgWeb.Security;
using System.Reflection;

namespace PlorgWeb {

  public class AppBuilder {
    public WebApplication Build() {

      var builder = WebApplication.CreateBuilder(new string[] { });

      JWTAuthOptions.builder = builder;

      // Step 1: Add CORS services with an "Allow All" policy
      builder.Services.AddCors(options =>
      {
        options.AddPolicy("AllowAll", policy =>
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader());
      });

      builder.Services.AddControllers();
      builder.Services.AddEndpointsApiExplorer();

      builder.Services.AddSwaggerGen(opt => {
        opt.SwaggerDoc("v1", new OpenApiInfo { Title = "PlorgAPI", Version = "v1" });
        opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
          In = ParameterLocation.Header,
          Description = "Please enter token",
          Name = "Authorization",
          Type = SecuritySchemeType.Http,
          BearerFormat = "JWT",
          Scheme = "bearer"
        });

        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

        opt.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
      });

      builder.Services.AddAuthorization();
      builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
          ValidateIssuer = true,
          ValidIssuer = JWTAuthOptions.ISSUER,
          ValidateAudience = true,
          ValidAudience = JWTAuthOptions.AUDIENCE,
          ValidateLifetime = true,
          IssuerSigningKey = JWTAuthOptions.KEY,
          ValidateIssuerSigningKey = true
        };
      });

      var app = builder.Build();

      if (app.Environment.IsDevelopment()) {
        app.UseSwagger();
        app.UseSwaggerUI();
      }

      // Step 2: Use CORS middleware to apply the "Allow All" policy globally
      app.UseCors("AllowAll");

      //app.UseHttpsRedirection();

      app.UseAuthentication();
      app.UseAuthorization();

      app.MapControllers();

      return app;
    }
  }
}