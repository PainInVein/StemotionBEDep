namespace STEMotion.Presentation.Extensions
{
    public static class ApplicationExtensions
    {
        public static void UseInfrastructure(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            /* 
              app.UseRouting();
            */
            // app.UseHttpsRedirection(); // for production only
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

        }
    }
}
