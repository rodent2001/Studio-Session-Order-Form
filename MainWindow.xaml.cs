using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;

namespace StudioSessionCalc
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        // PropertyChanged event and invoker
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // Studio properties
        public Studio Studio { get; set; }

        // Selected customer properties
        public Customer? SelectedCustomer
        {
            get;

            set
            {
                if (value != field)
                {
                    field = value;
                    if (value != null) IsSelectedCustomerValid = true;
                    IsAllOrderDataValidatedAfterLastChange = false;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsSelectedCustomerValid
        {
            get;

            set
            {
                if (value != field)
                {
                    field = value;
                    OnPropertyChanged();
                }
                if (!value) IsCustomerWarningBorderActive = true;
                else IsCustomerWarningBorderActive = false;
            }
        } = false;
        public bool IsCustomerWarningBorderActive
        {
            get;

            set
            {
                if (value != field)
                {
                    field = value;
                    OnPropertyChanged();
                }
            }
        } = false;


        // Selected product (session type) properties
        public Product? SelectedProduct
        {
            get;

            set
            {
                if (value != field)
                {
                    field = value;
                    if (value != null) IsSelectedProductValid = true;
                    ProductCostPerHourText = field?.ProductDefaultCostPerHour.ToString();
                    CalculateOrderTotalCost();
                    IsAllOrderDataValidatedAfterLastChange = false;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsSelectedProductValid
        {
            get;

            set
            {
                if (value != field)
                {
                    field = value;
                    OnPropertyChanged();
                }
                if (!value) IsProductWarningBorderActive = true;
                else IsProductWarningBorderActive = false;
            }
        } = false;
        public bool IsProductWarningBorderActive
        {
            get;

            set
            {
                if (value != field)
                {
                    field = value;
                    OnPropertyChanged();
                }
            }
        } = false;


        // Selected session duration properties
        public Duration? SelectedDuration
        {
            get;

            set
            {
                if (value != field)
                {
                    field = value;
                    if (value != null) IsSelectedDurationValid = true;
                    CalculateOrderTotalCost();
                    IsAllOrderDataValidatedAfterLastChange = false;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsSelectedDurationValid
        {
            get;

            set
            {
                if (value != field)
                {
                    field = value;
                    OnPropertyChanged();
                }
                if (!field) IsDurationWarningBorderActive = true;
                else IsDurationWarningBorderActive = false;
            }
        } = false;
        public bool IsDurationWarningBorderActive
        {
            get;

            set
            {
                if (value != field)
                {
                    field = value;
                    OnPropertyChanged();
                }
            }
        } = false;


        // Product cost per hour properties
        public string? ProductCostPerHourText
        {
            get;

            set
            {
                if (value != field)
                {
                    field = value;
                    IsAllOrderDataValidatedAfterLastChange = false;
                    OnPropertyChanged();

                    var separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                    var normalizedValue = value?.Replace(".", separator).Replace(",", separator);

                    if (decimal.TryParse(normalizedValue, NumberStyles.Number, CultureInfo.CurrentCulture, out var result)
                        && result >= 0
                        && ((result.ToString().IndexOf(separator) == -1)
                            || (result.ToString().IndexOf(separator) >= 0
                            && (result.ToString().Length - result.ToString().IndexOf(separator) <= 3))))
                    {
                        IsProductCostPerHourTextValid = true;
                        SelectedProduct?.ProductActualCostPerHour = result;
                        CalculateOrderTotalCost();
                        OnPropertyChanged();
                    }
                    else
                    {
                        SelectedProduct?.ProductActualCostPerHour = 0;
                        IsProductCostPerHourTextValid = false;
                        CalculateOrderTotalCost();
                    }
                }
            }
        } = "";
        public bool IsProductCostPerHourTextValid
        {
            get;

            set
            {
                if (value != field)
                {
                    field = value;
                    OnPropertyChanged();
                }
                if (!field) IsCostPerHourWarningBorderActive = true;
                else IsCostPerHourWarningBorderActive = false;
            }
        } = false;
        public bool IsCostPerHourWarningBorderActive
        {
            get;

            set
            {
                if (value != field)
                {
                    field = value;
                    OnPropertyChanged();
                }
            }
        } = false;


        // Product cost discount properties
        public string ProductCostDiscountText
        {
            get;

            set
            {
                if (value != field)
                {
                    field = value;
                    IsAllOrderDataValidatedAfterLastChange = false;
                    OnPropertyChanged();

                    if (int.TryParse(field, NumberStyles.Number, CultureInfo.CurrentCulture, out var result)
                            && result >= 0 && result < 100)
                    {
                        IsProductCostDiscountTextValid = true;
                        SelectedProduct?.ProductActualCostDiscount = result;
                        CalculateOrderTotalCost();
                    }
                    else if (string.IsNullOrEmpty(field))
                    {
                        IsProductCostDiscountTextValid = true;
                        SelectedProduct?.ProductActualCostDiscount = 0;
                        CalculateOrderTotalCost();
                    }
                    else
                    {
                        IsProductCostDiscountTextValid = false;
                        SelectedProduct?.ProductActualCostDiscount = 0;
                        CalculateOrderTotalCost();
                    }
                }
            }
        } = "";
        public bool IsProductCostDiscountTextValid
        {
            get;

            set
            {
                if (value != field)
                {
                    field = value;
                    OnPropertyChanged();
                }
                if (!field) IsCostDiscountWarningBorderActive = true;
                else IsCostDiscountWarningBorderActive = false;
            }
        } = true;
        public bool IsCostDiscountWarningBorderActive
        {
            get;

            set
            {
                if (value != field)
                {
                    field = value;
                    OnPropertyChanged();
                }
            }
        } = false;


        // Urgent surcharge properties
        public bool IsUrgentSurchargeApplied
        {
            get;

            set
            {
                if (value != field)
                {
                    field = value;
                    CalculateOrderTotalCost();
                    IsAllOrderDataValidatedAfterLastChange = false;
                    OnPropertyChanged();
                }
            }
        } = false;

        // Other properties
        public decimal OrderTotalCost
        {
            get;

            set
            {
                if (value != field)
                {
                    field = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OrderTotalCost)));
                }
            }
        } = 0;
        public bool IsAllOrderDataValidatedAfterLastChange
        {
            get;

            set
            {
                if (value != field)
                {
                    field = value;
                    OnPropertyChanged();
                }
            }
        } = false;

        // Class constructor
        public MainWindow()
        {
            InitializeComponent();
            Studio = new Studio("DataVox");
            SelectedProduct = null;
            SelectedCustomer = null;
            SelectedDuration = null;
            DataContext = this;
        }

        // Class methods

        private bool ValidateAllOrderData()
        {
            bool isAllDataValid = true;

            if (SelectedCustomer == null)
            {
                IsSelectedCustomerValid = false;
                isAllDataValid = false;
            }

            if (SelectedProduct == null)
            {
                IsSelectedProductValid = false;
                isAllDataValid = false;
            }

            if (SelectedDuration == null)
            {
                IsSelectedDurationValid = false;
                isAllDataValid = false;
            }

            if (!IsProductCostPerHourTextValid) isAllDataValid = false;
            if (!IsProductCostDiscountTextValid) isAllDataValid = false;

            return isAllDataValid;
        }

        public void CalculateOrderTotalCost()
        {
            decimal orderTotalCost = 0;

            decimal actualCostWithoutCorrection = (SelectedProduct != null ? SelectedProduct.ProductActualCostPerHour : 0)
                * (SelectedDuration != null ? SelectedDuration.DurationHours : 0);

            decimal costCorrection = CalculateCostCorrection();

            orderTotalCost = actualCostWithoutCorrection * costCorrection;
            OrderTotalCost = orderTotalCost;

            decimal CalculateCostCorrection()
            {
                decimal actualDiscountInPercent = SelectedProduct != null ? (decimal)SelectedProduct.ProductActualCostDiscount / 100 : 0;

                //Debug.WriteLine($"SelectedProduct.ProductActualCostDiscount: {SelectedProduct.ProductActualCostDiscount}");
                //Debug.WriteLine($"actualDiscountInPercent: {actualDiscountInPercent}");

                decimal actualSurchargeInPercent = SelectedProduct != null && IsUrgentSurchargeApplied
                                        ? (decimal)SelectedProduct.ProductDefaultUrgentSurcharge / 100 : 0;
                //Debug.WriteLine($"actualSurchargeInPercent: {actualSurchargeInPercent}");

                decimal correction;

                if (!IsUrgentSurchargeApplied)
                    correction = 1 - actualDiscountInPercent;
                else correction = 1 + actualSurchargeInPercent;
                //Debug.WriteLine($"correction: {correction}");

                return correction;
            }
        }

        // Buttons handlers
        private async void SaveOrderButton_Click(object sender, RoutedEventArgs e)
        {
            int activeWarningsBeforeValidation = CalculateActiveWarnings();
            IsAllOrderDataValidatedAfterLastChange = ValidateAllOrderData();

            if (!IsAllOrderDataValidatedAfterLastChange)
            {
                int activeWarningsAfterValidation = CalculateActiveWarnings();

                if (activeWarningsBeforeValidation == activeWarningsAfterValidation)
                {
                    await BlinkAllWarningsAsync(CreateWarningBordersActionList(), 3, 90);
                }
            }

            int CalculateActiveWarnings()
            {
                int count = 0;

                if (IsCustomerWarningBorderActive) count++;
                if (IsProductWarningBorderActive) count++;
                if (IsDurationWarningBorderActive) count++;
                if (IsCostPerHourWarningBorderActive) count++;
                if (IsCostDiscountWarningBorderActive) count++;

                return count;
            }

            async Task BlinkAllWarningsAsync(IEnumerable<Action<bool>> targets, int count, int delayMs, CancellationToken ct = default)
            {
                for (int i = 0; i < count; i++)
                {
                    SetAllBorderProperties(targets, false);
                    await Task.Delay(delayMs, ct);
                    SetAllBorderProperties(targets, true);
                    await Task.Delay(delayMs, ct);
                }
            }

            List<Action<bool>> CreateWarningBordersActionList()
            {
                var targets = new List<Action<bool>>();

                if (IsCustomerWarningBorderActive) targets.Add(value => IsCustomerWarningBorderActive = value);
                if (IsProductWarningBorderActive) targets.Add(value => IsProductWarningBorderActive = value);
                if (IsDurationWarningBorderActive) targets.Add(value => IsDurationWarningBorderActive = value);
                if (IsCostPerHourWarningBorderActive) targets.Add(value => IsCostPerHourWarningBorderActive = value);
                if (IsCostDiscountWarningBorderActive) targets.Add(value => IsCostDiscountWarningBorderActive = value);

                return targets;
            }

            void SetAllBorderProperties(IEnumerable<Action<bool>> targets, bool value)
            {
                foreach (var target in targets)
                    target(value);
            }
        }

        private void ResetDataButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedCustomer = null;
            IsSelectedCustomerValid = true;
            SelectedProduct = null;
            IsSelectedProductValid = true;
            SelectedDuration = null;
            IsSelectedDurationValid = true;
            ProductCostPerHourText = "";
            IsProductCostPerHourTextValid = true;
            ProductCostDiscountText = "";
            IsProductCostDiscountTextValid = true;
            IsUrgentSurchargeApplied = false;
            //Window_Loaded();
        }

        private void Window_Loaded()
        {
            double currentWindowHeight = this.MainGrid.ActualHeight;
            double currentWindowWidth = this.GridColumn1.ActualWidth;
            MessageBox.Show(currentWindowHeight.ToString());
            MessageBox.Show(currentWindowWidth.ToString());
        }
    }
}