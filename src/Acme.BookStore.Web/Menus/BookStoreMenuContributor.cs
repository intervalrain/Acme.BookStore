using System.Threading.Tasks;
using Acme.BookStore.Localization;
using Acme.BookStore.Permissions;
using Acme.BookStore.MultiTenancy;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.UI.Navigation;
using Volo.Abp.TenantManagement.Web.Navigation;
using Volo.Abp.Authorization.Permissions;

namespace Acme.BookStore.Web.Menus;

public class BookStoreMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private static async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<BookStoreResource>();

        //Home
        context.Menu.AddItem(
            new ApplicationMenuItem(
                BookStoreMenus.Home,
                l["Menu:Home"],
                "~/",
                icon: "fa fa-home",
                order: 1
            )
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                name: "BooksStore", 
                displayName: l["Menu:BookStore"], 
                icon: "fa fa-book"
            ).AddItem(
                new ApplicationMenuItem(
                    name: "BooksStore.Books", 
                    displayName: l["Menu:Books"], 
                    url: "/Books"
                ).RequirePermissions(BookStorePermissions.Books.Default)
            ).AddItem(
                new ApplicationMenuItem(
                    name: "BooksStore.Authors",
                    displayName: l["Menu:Authors"],
                    url: "/Authors"
                ).RequirePermissions(BookStorePermissions.Authors.Default)
            )
        );

        //Administration
        var administration = context.Menu.GetAdministration();
        administration.Order = 5;

        //Administration->Identity
        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 1);
    
        if (MultiTenancyConsts.IsEnabled)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        }
        // else
        // {
        //     administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        // }
        
        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 3);

        //Administration->Settings
        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 7);
        
        await Task.CompletedTask;
    }
}
