namespace MauiOdev;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Navigasyon (Gezinme) Sistemi için sayfaları kaydetme.
        // Bu, Shell.Current.GoToAsync("SayfaAdi") gibi programatik gezinmelere olanak tanır.
        Routing.RegisterRoute(nameof(AnaSayfa), typeof(AnaSayfa));
        Routing.RegisterRoute(nameof(KrediSayfasi), typeof(KrediSayfasi));
        Routing.RegisterRoute(nameof(VkiSayfasi), typeof(VkiSayfasi));
        Routing.RegisterRoute(nameof(RenkSayfasi), typeof(RenkSayfasi));
    }
}