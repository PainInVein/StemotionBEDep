namespace STEMotion.Application.Extensions
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
            app.UseAuthorization();
            app.MapControllers();

        }
    }
}
