namespace PrimaryConstructor;

public class CustomerService : ICustomerService
{
    public IEnumerable<Customer> GetCustomers() =>
        [new("John"), new("Jane"), new("June")];
}
