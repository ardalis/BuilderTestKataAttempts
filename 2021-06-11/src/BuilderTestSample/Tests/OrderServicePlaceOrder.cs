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
                            .WithId(0)
                            .Build();

            Assert.NotNull(order);
            Assert.Equal(CustomerBuilder.TEST_CUSTOMER_ID, order.Customer.Id);
            Assert.Equal(CustomerBuilder.TEST_CUSTOMER_TOTAL_PURCHASES, order.Customer.TotalPurchases);
        }

        [Fact]
        public void ThrowsExceptionGivenOrderWithExistingId()
        {
            var order = _orderBuilder
                            .WithId(123)
                            .Build();

            Assert.Throws<InvalidOrderException>(() => _orderService.PlaceOrder(order));
        }

        [Fact]
        public void ThrowsExceptionGivenOrderWith0TotalAmount()
        {
            var order = _orderBuilder
                            .WithTotalAmount(0)
                            .Build();

            Assert.Throws<InvalidOrderException>(() => _orderService.PlaceOrder(order));
        }

        [Fact]
        public void ThrowsExceptionGivenOrderWithNoAssociatedCustomer()
        {
            var order = _orderBuilder
                            .WithCustomer(null)
                            .Build();

            Assert.Throws<InvalidOrderException>(() => _orderService.PlaceOrder(order));
        }

        [Fact]
        public void ThrowsExceptionGivenOrderWithCustomerWithId0()
        {
            var customer = _customerBuilder
                            .WithId(0)
                            .Build();
            var order = _orderBuilder
                            .WithCustomer(customer)
                            .Build();

            Assert.Throws<InvalidCustomerException>(() => _orderService.PlaceOrder(order));
        }

        [Fact]
        public void ThrowsExceptionGivenOrderWithCustomerWithNullAddress()
        {
            var customer = _customerBuilder
                            .WithAddress(null)
                            .Build();
            var order = _orderBuilder
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
                            .WithFirstName(firstName)
                            .WithLastName(lastName)
                            .Build();
            var order = _orderBuilder
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
                            .WithCreditRating(rating)
                            .Build();
            var order = _orderBuilder
                            .WithCustomer(customer)
                            .Build();

            // Act
            Action action = () => _orderService.PlaceOrder(order);

            // Assert
            var exception = Assert.Throws<InsufficientCreditException>(action);
        }

        [Fact]
        public void ThrowsExceptionGivenOrderWithCustomerWithNoPurchases()
        {
            var customer = _customerBuilder
                            .WithTotalPurchases(0m)
                            .Build();
            var order = _orderBuilder
                            .WithCustomer(customer)
                            .Build();

            Action action = () => _orderService.PlaceOrder(order);

            // Assert
            var exception = Assert.Throws<InvalidCustomerException>(action);
        }

    }
}
