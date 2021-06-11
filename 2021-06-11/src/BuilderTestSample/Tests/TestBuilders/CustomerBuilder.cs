using BuilderTestSample.Model;

namespace BuilderTestSample.Tests.TestBuilders
{
    public class CustomerBuilder
    {
        public const int TEST_CUSTOMER_ID = 123;

        private int _id;
        private Address _address;

        public CustomerBuilder WithAddress(Address address)
        {
            _address = address;
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

            return this;
        }

        public Customer Build()
        {
            return new Customer(_id) { HomeAddress = _address };
        }


    }
}
