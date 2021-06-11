using BuilderTestSample.Exceptions;
using BuilderTestSample.Model;
using BuilderTestSample.Services;
using BuilderTestSample.Tests.TestBuilders;
using System;
using Xunit;

namespace BuilderTestSample.Tests
{
    public class OrderServicePlaceOrder
    {
        private readonly OrderService _orderService = new OrderService();
        private readonly OrderBuilder _orderBuilder = new OrderBuilder();
        private readonly CustomerBuilder _customerBuilder = new CustomerBuilder();

        [Fact]
        public void CreatesOrderGivenOrderWithNoId()
        {
            var order = _orderBuilder
                            .WithTestValues()
                            .WithId(0)
                            .Build();

            Assert.NotNull(order);
            Assert.Equal(CustomerBuilder.TEST_CUSTOMER_ID, order.Customer.Id);
        }

        [Fact]
        public void ThrowsExceptionGivenOrderWithExistingId()
        {
            var order = _orderBuilder
                            .WithTestValues()
                            .WithId(123)
                            .Build();

            Assert.Throws<InvalidOrderException>(() => _orderService.PlaceOrder(order));
        }

        [Fact]
        public void ThrowsExceptionGivenOrderWith0TotalAmount()
        {
            var order = _orderBuilder
                            .WithTestValues()
                            .WithTotalAmount(0)
                            .Build();

            Assert.Throws<InvalidOrderException>(() => _orderService.PlaceOrder(order));
        }

        [Fact]
        public void ThrowsExceptionGivenOrderWithNoAssociatedCustomer()
        {
            var order = _orderBuilder
                            .WithTestValues()
                            .WithCustomer(null)
                            .Build();

            Assert.Throws<InvalidOrderException>(() => _orderService.PlaceOrder(order));
        }

        [Fact]
        public void ThrowsExceptionGivenOrderWithCustomerWithId0()
        {
            var customer = _customerBuilder
                            .WithTestValues()
                            .WithId(0)
                            .Build();
            var order = _orderBuilder
                            .WithTestValues()
                            .WithCustomer(customer)
                            .Build();

            Assert.Throws<InvalidCustomerException>(() => _orderService.PlaceOrder(order));
        }

        [Fact]
        public void ThrowsExceptionGivenOrderWithCustomerWithNullAddress()
        {
            var customer = _customerBuilder
                            .WithTestValues()
                            .WithAddress(null)
                            .Build();
            var order = _orderBuilder
                            .WithTestValues()
                            .WithCustomer(customer)
                            .Build();

            Assert.Throws<InvalidCustomerException>(() => _orderService.PlaceOrder(order));
        }

        [Theory]
        [InlineData("steve", "")]
        [InlineData("", "smith")]
        [InlineData("steve", null)]
        [InlineData(null, "smith")]
        [InlineData(null, null)]
        [InlineData("", "")]
        public void ThrowsExceptionGivenOrderWithCustomerWithEmptyFirstOrLastName(string firstName, string lastName)
        {
            var customer = _customerBuilder
                            .WithTestValues()
                            .WithFirstName(firstName)
                            .WithLastName(lastName)
                            .Build();
            var order = _orderBuilder
                            .WithTestValues()
                            .WithCustomer(customer)
                            .Build();

            Assert.Throws<InvalidCustomerException>(() => _orderService.PlaceOrder(order));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        [InlineData(199)]
        public void ThrowsExceptionGivenOrderWithCustomerWithCreditRatingUnder200(int rating)
        {
            // Arrange
            var customer = _customerBuilder
                            .WithTestValues()
                            .WithCreditRating(rating)
                            .Build();
            var order = _orderBuilder
                            .WithTestValues()
                            .WithCustomer(customer)
                            .Build();

            // Act
            Action action = () => _orderService.PlaceOrder(order);

            // Assert
            var exception = Assert.Throws<InsufficientCreditException>(action);
        }

    }
}
