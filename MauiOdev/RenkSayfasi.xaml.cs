namespace MauiOdev;

public partial class RenkSayfasi : ContentPage
{
    // Rastgele renk üretmek için kullanýlacak Random sýnýfý nesnesi.
    // 'readonly' olarak tanýmlanýr, çünkü sadece bir kez (constructor'da) atanýr.
    private readonly Random _random = new Random();

    public RenkSayfasi()
    {
        InitializeComponent();

        // Sayfa yüklendiðinde, varsayýlan slider deðerleri (128,128,128) 
        // ile ilk renk ayarýný yapar.
        UpdateColor();
    }

    // Bu metod, üç slider'dan (R, G, veya B) herhangi biri deðiþtiðinde tetiklenir.
    private void OnColorSliderChanged(object sender, ValueChangedEventArgs e)
    {
        // Renk güncelleme mantýðýný içeren ana metodu çaðýrýr.
        UpdateColor();
    }

    // Rengi hesaplayan ve arayüzü güncelleyen ana metod.
    private void UpdateColor()
    {
        // 1. Slider'lardan (int) R, G, B deðerlerini (0-255 aralýðýnda) alýr.
        int r = (int)sliderRed.Value;
        int g = (int)sliderGreen.Value;
        int b = (int)sliderBlue.Value;

        // 2. Slider'larýn mevcut deðerlerini "Kýrmýzý: X" formatýnda etiketlere (Label) yazar.
        labelRed.Text = $"Kýrmýzý: {r}";
        labelGreen.Text = $"Yeþil: {g}";
        labelBlue.Text = $"Mavi: {b}";

        // 3. R, G, B deðerlerini kullanarak bir .NET MAUI 'Color' nesnesi oluþturur.
        Color color = Color.FromRgb(r, g, b);

        // 4. Color nesnesini .ToHex() metodu ile #RRGGBB formatýndaki string'e çevirir.
        string hexCode = color.ToHex();
        labelHex.Text = hexCode;

        // 5. Oluþturulan rengi hem sayfanýn ana arka planýna (mainLayout) 
        // hem de üstteki önizleme kutusuna (colorPreview) atar.
        mainLayout.BackgroundColor = color;
        colorPreview.BackgroundColor = color;

        // 6. Okunabilirlik Kontrolü:
        // Arka plan renginin parlaklýk (brightness) deðerini hesaplar.
        // Parlaklýk = (0.299*R + 0.587*G + 0.114*B) / 255 (Luma formülü)
        double brightness = (0.299 * r + 0.587 * g + 0.114 * b) / 255;

        // Arka plan koyuysa (parlaklýk 0.5'ten küçükse) yazý rengini Beyaz,
        // arka plan açýksa yazý rengini Siyah yapar.
        Color textColor = brightness > 0.5 ? Colors.Black : Colors.White;

        // Sayfadaki tüm etiketlerin (Label) yazý rengini (TextColor) ayarlar.
        titleLabel.TextColor = textColor;
        labelRed.TextColor = textColor;
        labelGreen.TextColor = textColor;
        labelBlue.TextColor = textColor;
        labelHex.TextColor = textColor;
    }

    // 'Kodu Kopyala' butonuna týklandýðýnda çalýþýr.
    private async void OnCopyClicked(object sender, EventArgs e)
    {
        // Hex etiketindeki metni (örn: "#FF0000") alýr.
        string renk_kodu = labelHex.Text;
        // Cihazýn panosuna (Clipboard) metni asenkron olarak kopyalar.
        await Clipboard.SetTextAsync(renk_kodu);
        // Kullanýcýya kopyalama iþleminin baþarýlý olduðuna dair bir uyarý gösterir.
        await DisplayAlert("Kopyalandý", $"Renk kodu '{renk_kodu}' panoya kopyalandý.", "Tamam");
    }

    // 'Rastgele Renk' butonuna týklandýðýnda çalýþýr.
    private void OnRandomClicked(object sender, EventArgs e)
    {
        // _random nesnesini kullanarak 0 ile 255 (256 dahil deðil) arasýnda 
        // rastgele int deðerler üretir ve bunlarý slider'larýn 'Value' özelliðine atar.
        sliderRed.Value = _random.Next(256);
        sliderGreen.Value = _random.Next(256);
        sliderBlue.Value = _random.Next(256);

        // ÖNEMLÝ NOT: Slider'larýn 'Value' özelliðini koddan deðiþtirmek,
        // 'OnColorSliderChanged' olayýný otomatik olarak tetikler.
        // Bu sayede UpdateColor() metodunu burada tekrar çaðýrmamýza gerek kalmaz.
    }
}