namespace StudioSessionCalc;

public static class CollectionsMaker
{
    public static StudioProducts MakeStudioProducts()
    {
        var products = new StudioProducts();
        products.Add(new Product("Rehearsal", 7.5m, 0, 10));
        products.Add(new Product("Voice recording", 20m, 0, 15));
        products.Add(new Product("Drums recording", 32m, 0, 30));
        products.Add(new Product("Mixing", 50m, 0, 50));

        return products;
    }

    public static StudioCustomers MakeStudioCustomers()
    {
        var customers = new StudioCustomers();
        customers.Add(new Customer() { CustomerName = "Michael Jackson" });
        customers.Add(new Customer() { CustomerName = "Paul McCartney" });
        customers.Add(new Customer() { CustomerName = "Elvis Presley" });
        customers.Add(new Customer() { CustomerName = "Mariah Carey" });

        return customers;
    }

    public static StudioSessionDurations MakeStudioSessionDurations()
    {
        var durations = new StudioSessionDurations();

        for (int i = 0; i < 6; i++)
        {
            durations.Add(new Duration(i + 1));
        }

        return durations;
    }
}