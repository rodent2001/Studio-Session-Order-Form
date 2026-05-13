using System.ComponentModel;

namespace StudioSessionOrderForm;

public class Product : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public string ProductName { get; set; } = string.Empty;

    public decimal ProductDefaultCostPerHour { get; set; }
    public decimal ProductActualCostPerHour
    {
        get;

        set
        {
            if (value != field)
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProductActualCostPerHour)));
            }
        }
    }

    public int ProductDefaultCostDiscount { get; set; }
    public int ProductActualCostDiscount
    {
        get;

        set
        {
            if (value != field)
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProductActualCostDiscount)));
            }
        }
    }

    public int ProductDefaultUrgentSurcharge { get; set; }


    public Product(string productName, decimal costPerHour, int discount, int urgentSurcharge)
    {
        ProductName = productName;
        ProductDefaultCostPerHour = costPerHour;
        ProductActualCostPerHour = ProductDefaultCostPerHour;
        ProductDefaultCostDiscount = discount;
        ProductActualCostDiscount = ProductDefaultCostDiscount;
        ProductDefaultUrgentSurcharge = urgentSurcharge;
    }
}