using BuilderTestSample.Model;

namespace BuilderTestSample.Tests.TestBuilders
{
    public class CustomerBuilder
    {
        public const int TEST_CUSTOMER_ID = 123;

        private int _id;
        private Address _address;
        private string _lastName;
        private string _firstName;

        public CustomerBuilder WithAddress(Address address)
        {
            _address = address;
            return this;
        }
        public CustomerBuilder WithFirstName(string firstName)
        {
            _firstName = firstName;
            return this;
        }
        public CustomerBuilder WithLastName(string lastName)
        {
            _lastName = lastName;
            return this;
        }
        public CustomerBuilder WithId(int id)
        {
            _id = id;
            return this;
        }
        public CustomerBuilder WithTestValues()
        {
            _id = TEST_CUSTOMER_ID;
            _address = new Address();

            return this;
        }

        public Customer Build()
        {
            var address = new Address();
            return new Customer(_id)
            {
                FirstName = _firstName,
                HomeAddress = _address,
                LastName = _lastName
            };
        }
    }
}
