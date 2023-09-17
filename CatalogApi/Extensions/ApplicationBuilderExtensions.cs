public static class ApplicationBuilderExtensions
{
    public static void UseCustomMiddlewares(this IApplicationBuilder app)
    {
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCors(opt => opt.AllowAnyOrigin());
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

    public static void ConfigureSwagger(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}
