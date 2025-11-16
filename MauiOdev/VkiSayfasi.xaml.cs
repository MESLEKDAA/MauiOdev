namespace MauiOdev;

public partial class VkiSayfasi : ContentPage
{
    public VkiSayfasi()
    {
        InitializeComponent();

        // Sayfa ilk açýldýðýnda, varsayýlan slider deðerleri (70kg, 170cm) 
        // ile ilk VKÝ hesaplamasýný yapar.
        CalculateBmi();
    }

    // Bu metod, her iki slider'dan (Kilo veya Boy) herhangi biri deðiþtiðinde tetiklenir.
    // Bu, "dinamik hesaplama" saðlar (buton gerekmez).
    private void OnBmiSliderChanged(object sender, ValueChangedEventArgs e)
    {
        // Slider'larýn mevcut deðerlerini etiketlere (labelWeight, labelHeight) yazar.
        labelWeight.Text = $"Kilo (kg): {(int)sliderWeight.Value}";
        labelHeight.Text = $"Boy (cm): {(int)sliderHeight.Value}";

        // Deðerler her deðiþtiðinde ana hesaplama metodunu çaðýrýr.
        CalculateBmi();
    }

    // Ana VKÝ hesaplamasýný yapan metod.
    private void CalculateBmi()
    {
        double weight = sliderWeight.Value;
        double heightCm = sliderHeight.Value;

        // Formül: Kilo / (Boy * Boy). Boy metre cinsinden olmalý.
        // Santimetre (cm) cinsinden alýnan boyu 100.0'e bölerek metreye (m) çeviririz.
        double heightM = heightCm / 100.0;
        double bmi = weight / (heightM * heightM);

        // Sonuçlarý etiketlere yaz
        // "F2" formatý: Virgülden sonra 2 basamak (Fixed-point, 2 decimal places) gösterir.
        labelBmiResult.Text = $"{bmi:F2}";

        // Hesaplanan BMI deðerini kategoriye çeviren metodu çaðýrýr ve sonucu etikete basar.
        labelBmiCategory.Text = GetBmiCategory(bmi);
    }

    // VKÝ deðerini alýp, standart kategorilere göre 
    // ilgili metin (string) kategorisini döndüren metod.
    private string GetBmiCategory(double bmi)
    {
        if (bmi < 16) return "Ýleri Düzeyde Zayýf";
        if (bmi < 17) return "Orta Düzeyde Zayýf";   // 16 - 16.99
        if (bmi < 18.5) return "Hafif Düzeyde Zayýf"; // 17 - 18.49
        if (bmi < 25) return "Normal Kilolu";       // 18.5 - 24.9
        if (bmi < 30) return "Hafif Þiþman / Fazla Kilolu"; // 25 - 29.9
        if (bmi < 35) return "1. Derecede Obez";    // 30 - 34.9
        if (bmi < 40) return "2. Derecede Obez";    // 35 - 39.9
        return "3. Derecede Obez / Morbid Obez"; // 40 ve üzeri
    }
}