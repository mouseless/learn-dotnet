namespace PrimaryConstructor;

public class Discount(ICustomerService _customerService)
    : Information("Sales Department")
{
    public void Send()
    {
        foreach (var customer in _customerService.GetCustomers())
        {
            SendMessage(
                @for: customer.Name,
                notification: new(Title: "%5 Discount", Content: "You got %5 discount for Electronic product.")
            );
        }
    }
}
