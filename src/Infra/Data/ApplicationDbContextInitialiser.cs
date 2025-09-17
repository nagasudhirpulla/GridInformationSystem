﻿using App.Regions.Commands.CreateRegion;
using Core.Constants;
using Infra.Identity;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infra.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMediator mediator)
{
    public async Task InitialiseAsync()
    {
        try
        {
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var administratorRole = new IdentityRole(Roles.Administrator);

        if (roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await roleManager.CreateAsync(administratorRole);
        }

        // Default users
        var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost", DisplayName = "Administrator" };

        if (userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await userManager.CreateAsync(administrator, "Administrator1!");
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await userManager.AddToRolesAsync(administrator, [administratorRole.Name]);
            }
        }

        // Default data
        if (!context.Regions.Any())
        {
            _ = await mediator.Send(new CreateRegionCommand() { Name = "NR" });
            _ = await mediator.Send(new CreateRegionCommand() { Name = "ER" });
            _ = await mediator.Send(new CreateRegionCommand() { Name = "WR" });
            _ = await mediator.Send(new CreateRegionCommand() { Name = "SR" });
            _ = await mediator.Send(new CreateRegionCommand() { Name = "NER" });

            //context.Regions.AddRange([
            //    new Region { Name = "NR" },
            //    new Region { Name = "ER" },
            //    new Region { Name = "WR" },
            //    new Region { Name = "SR" },
            //    new Region { Name = "NER" }
            //    ]);

            //await context.SaveChangesAsync();
        }

    }
}