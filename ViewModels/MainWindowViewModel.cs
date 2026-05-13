using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;


namespace StudioSessionOrderForm.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // Current studio

        public Studio CurrentStudio { get; }
        public Studio Studio => CurrentStudio;

        // Selection state

        public Customer? SelectedCustomer
        {
            get;

            set
            {
                if (value != field)
                {
                    field = value;
                    if (field != null) IsSelectedCustomerValid = true;
                    IsAllOrderDataValidatedAfterLastChange = false;
                    OnPropertyChanged();
                }
            }
        }

        public Product? SelectedProduct
        {
            get;

            set
            {
                if (value != field)
                {
                    field = value;
                    if (field != null) IsSelectedProductValid = true;
                    ProductCostPerHourText = field?.ProductDefaultCostPerHour.ToString() ?? "";
                    CalculateOrderTotalCost();
                    IsAllOrderDataValidatedAfterLastChange = false;
                    OnPropertyChanged();
                }
            }
        }

        public Duration? SelectedDuration
        {
            get;

            set
            {
                if (value != field)
                {
                    field = value;
                    if (field != null) IsSelectedDurationValid = true;
                    CalculateOrderTotalCost();
                    IsAllOrderDataValidatedAfterLastChange = false;
                    OnPropertyChanged();
                }
            }
        }

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

        // Text fields

        public string? ProductCostPerHourText
        {
            get;

            set
            {
                if (value != field)
                {
                    field = value;
                    IsAllOrderDataValidatedAfterLastChange = false;

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
                    }
                    else
                    {
                        IsProductCostPerHourTextValid = false;
                        SelectedProduct?.ProductActualCostPerHour = 0;
                        CalculateOrderTotalCost();
                    }
                    OnPropertyChanged();
                }
            }
        } = "";

        public string ProductCostDiscountText
        {
            get;

            set
            {
                if (value != field)
                {
                    field = value;
                    IsAllOrderDataValidatedAfterLastChange = false;

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
                    OnPropertyChanged();
                }
            }
        } = "";

        // Calculation

        public decimal OrderTotalCost
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
        } = 0;

        // Validations

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
                IsCustomerWarningBorderActive = !value;
            }
        } = false;

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
                IsProductWarningBorderActive = !value;

            }
        } = false;

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
                IsDurationWarningBorderActive = !value;

            }
        } = false;

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
                IsCostPerHourWarningBorderActive = !value;

            }
        } = false;

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
                IsCostDiscountWarningBorderActive = !value;

            }
        } = true;

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

        // Visual error flags

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

        // Selection lists

        public ObservableCollection<Customer> Customers => CurrentStudio.StudioCustomers;
        public ObservableCollection<Product> Products => CurrentStudio.StudioProducts;
        public ObservableCollection<Duration> Durations => CurrentStudio.StudioSessionDurations;

        // Class constructor

        public MainWindowViewModel()
        {
            CurrentStudio = new Studio("DataVoxViewModel");
        }

        // Calculation methods

        private void CalculateOrderTotalCost()
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

        // Validation methods

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

        public void ValidateOrderData()
        {
            IsAllOrderDataValidatedAfterLastChange = ValidateAllOrderData();
        }

        // Warnings state properties switch methods

        private async Task BlinkAllWarningsAsync(IEnumerable<Action<bool>> targets, int count, int delayMs, CancellationToken ct = default)
        {
            for (int i = 0; i < count; i++)
            {
                SetAllBorderProperties(targets, false);
                await Task.Delay(delayMs, ct);
                SetAllBorderProperties(targets, true);
                await Task.Delay(delayMs, ct);
            }
        }

        private int CalculateActiveWarnings()
        {
            int count = 0;

            if (IsCustomerWarningBorderActive) count++;
            if (IsProductWarningBorderActive) count++;
            if (IsDurationWarningBorderActive) count++;
            if (IsCostPerHourWarningBorderActive) count++;
            if (IsCostDiscountWarningBorderActive) count++;

            return count;
        }

        private List<Action<bool>> CreateWarningBordersActionList()
        {
            var targets = new List<Action<bool>>();

            if (IsCustomerWarningBorderActive) targets.Add(value => IsCustomerWarningBorderActive = value);
            if (IsProductWarningBorderActive) targets.Add(value => IsProductWarningBorderActive = value);
            if (IsDurationWarningBorderActive) targets.Add(value => IsDurationWarningBorderActive = value);
            if (IsCostPerHourWarningBorderActive) targets.Add(value => IsCostPerHourWarningBorderActive = value);
            if (IsCostDiscountWarningBorderActive) targets.Add(value => IsCostDiscountWarningBorderActive = value);

            return targets;
        }

        private void SetAllBorderProperties(IEnumerable<Action<bool>> targets, bool value)
        {
            foreach (var target in targets)
                target(value);
        }

        public async Task SaveOrderAsync()
        {
            int activeWarningsBeforeValidation = CalculateActiveWarnings();
            ValidateOrderData();

            if (!IsAllOrderDataValidatedAfterLastChange)
            {
                int activeWarningsAfterValidation = CalculateActiveWarnings();

                if (activeWarningsBeforeValidation == activeWarningsAfterValidation)
                {
                    await BlinkAllWarningsAsync(CreateWarningBordersActionList(), 3, 90);
                }
            }
        }

        public void ResetOrderData()
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
        }
    }
}