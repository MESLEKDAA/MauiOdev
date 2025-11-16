namespace MauiOdev;

public partial class KrediSayfasi : ContentPage
{
    public KrediSayfasi()
    {
        InitializeComponent();

        // Sayfa yüklendiðinde Picker kontrolünün seçeneklerini (ItemsSource) tanýmlar.
        pickerCreditType.ItemsSource = new List<string> { "Ýhtiyaç Kredisi", "Taþýt Kredisi", "Konut Kredisi" };
        pickerCreditType.SelectedIndex = 0; // Varsayýlan olarak "Ýhtiyaç Kredisi" seçili gelir.
    }

    // Vade (Ay) slider'ý her deðiþtiðinde bu olay (event) tetiklenir.
    private void sliderTerm_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        // Slider'ýn yeni deðerini alýr ve etiketin metnini günceller.
        labelTerm.Text = $"Vade (Ay): {(int)e.NewValue}";
    }

    // 'Hesapla' butonuna týklandýðýnda bu metod çalýþýr.
    private async void btnCalculate_Clicked(object sender, EventArgs e)
    {
        // 1. Girdi Doðrulama (Validasyon):
        // Kredi tutarý veya faiz oraný alanlarýnýn boþ (null, empty veya whitespace) olup olmadýðýný kontrol eder.
        if (string.IsNullOrWhiteSpace(entryAmount.Text) || string.IsNullOrWhiteSpace(entryRate.Text))
        {
            // Eðer boþsa, kullanýcýya asenkron bir uyarý mesajý (DisplayAlert) gösterir.
            await DisplayAlert("Hata", "Kredi tutarý ve faiz oraný boþ býrakýlamaz.", "Tamam");
            return; // Metodun devam etmesini engeller.
        }

        // Girilen deðerlerin geçerli bir 'double' (ondalýklý sayý) olup olmadýðýný kontrol eder.
        if (!double.TryParse(entryAmount.Text, out double principal) || principal <= 0)
        {
            await DisplayAlert("Hata", "Geçerli bir kredi tutarý giriniz.", "Tamam");
            return;
        }

        if (!double.TryParse(entryRate.Text, out double annualRate) || annualRate <= 0)
        {
            await DisplayAlert("Hata", "Geçerli bir yýllýk baz faiz oraný giriniz.", "Tamam");
            return;
        }

        // 2. Deðerleri Alma:
        // Arayüzdeki kontrollerden (Entry, Slider, Picker) verileri alýr.
        int termInMonths = (int)sliderTerm.Value;
        string creditType = pickerCreditType.SelectedItem.ToString();

        // Kullanýcýnýn girdiði yýllýk faizi, aylýk baz faize çevirir (Örn: %12 yýllýk -> 0.12 / 12 = 0.01 aylýk).
        double aylikBazFaiz = (annualRate / 100) / 12;

        // 3. Kredi Türüne Göre Vergileri (KKDF ve BSMV) Belirle:
        // Seçilen kredi türüne göre (switch-case yapýsý) vergi oranlarýný atar.
        double kkdfOrani = 0.0;
        double bsmvOrani = 0.0;

        switch (creditType)
        {
            case "Ýhtiyaç Kredisi":
                kkdfOrani = 0.15; // %15
                bsmvOrani = 0.10; // %10
                break;
            case "Taþýt Kredisi":
                kkdfOrani = 0.15; // %15
                bsmvOrani = 0.05; // %5
                break;
            case "Konut Kredisi":
                kkdfOrani = 0.0;  // %0 (Konut kredisinde vergiler muaftýr)
                bsmvOrani = 0.0;  // %0
                break;
        }

        // 4. Brüt (Vergiler Dahil) Aylýk Faiz Oranýný Hesapla:
        // Aylýk baz faize KKDF ve BSMV'yi ekler. Formül: BazFaiz * (1 + KKDF + BSMV)
        double brutAylikFaiz = aylikBazFaiz * (1 + kkdfOrani + bsmvOrani);

        // 5. Kredi Hesaplama (Anüite Formülü):
        // M = P [ i(1+i)^n ] / [ (1+i)^n – 1 ]
        // M = Aylýk Taksit, P = Anapara (principal), i = Brüt Aylýk Faiz, n = Vade (Ay)

        double monthlyPayment;
        if (brutAylikFaiz == 0) // Eðer faiz 0 ise (örn: faizsiz konut kredisi)
        {
            monthlyPayment = principal / termInMonths; // Direkt anaparayý vadeye böl.
        }
        else
        {
            // Math.Pow(base, exponent) üs alma fonksiyonudur. ( (1+i)^n )
            double factor = Math.Pow(1 + brutAylikFaiz, termInMonths);
            monthlyPayment = principal * (brutAylikFaiz * factor) / (factor - 1);
        }

        double totalPayment = monthlyPayment * termInMonths; // Toplam geri ödeme
        double totalInterest = totalPayment - principal; // Toplam ödenen faiz (vergiler dahil)

        // 6. Sonucu Göster:
        // Hesaplanan deðerleri (aylýk taksit, toplam ödeme) 'labelResult' etiketine formatlayarak yazdýrýr.
        // :C2 -> Para birimi formatý (Currency), 2 ondalýk basamaklý.
        // :F4 -> Sabit (Fixed-point) format, 4 ondalýk basamaklý.
        labelResult.Text = $"Seçilen Kredi: {creditType}\n\n" +
                           $"Girilen Yýllýk Baz Faiz: %{annualRate}\n" +
                           $"Vergilerle Brüt Aylýk Faiz: %{(brutAylikFaiz * 100):F4}\n\n" +
                           $"Aylýk Taksit: {monthlyPayment:C2}\n" +
                           $"Toplam Geri Ödeme: {totalPayment:C2}\n" +
                           $"Toplam Faiz: {totalInterest:C2}";
    }
}