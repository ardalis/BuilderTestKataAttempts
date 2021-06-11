using BuilderTestSample.Model;

namespace BuilderTestSample.Tests.TestBuilders
{
    public class CustomerBuilder
    {
        private int _id;

        public CustomerBuilder WithId(int id)
        {
            _id = id;
            return this;
        }
        public CustomerBuilder WithTestValues()
        {
            _id = 0;

            return this;
        }

        public Customer Build()
        {
            return new Customer(_id);
        }


    }
}
