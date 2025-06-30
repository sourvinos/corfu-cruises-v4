using System.Collections;
using System.Collections.Generic;

namespace Prices {

    public class CreateInvalidPrice : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return Customer_Must_Exist();
            yield return Customer_Must_Be_Active();
            yield return Destination_Must_Exist();
            yield return Destination_Must_Be_Active();
            yield return Port_Must_Exist();
            yield return Port_Must_Be_Active();
            yield return AdultsWithTransferMustNotBeNegative();
            yield return AdultsWithoutTransferMustNotBeNegative();
            yield return KidsWithTransferMustNotBeNegative();
            yield return KidsWithoutTransferMustNotBeNegative();
        }

        private static object[] Customer_Must_Exist() {
            return new object[] {
                new TestPrice {
                    StatusCode = 450,
                    CustomerId = 9999,
                    DestinationId = 1,
                    PortId = 1,
                    From = "2023-01-01",
                    To = "2023-01-01",
                    AdultsWithTransfer = 0.01M,
                    AdultsWithoutTransfer = 0.02M,
                    KidsWithTransfer = 0.03M,
                    KidsWithoutTransfer = 0.04M
                }
            };
        }

        private static object[] Customer_Must_Be_Active() {
            return new object[] {
                new TestPrice {
                    StatusCode = 450,
                    CustomerId = 200,
                    DestinationId = 1,
                    PortId = 1,
                    From = "2023-01-01",
                    To = "2023-01-01",
                    AdultsWithTransfer = 0.01M,
                    AdultsWithoutTransfer = 0.02M,
                    KidsWithTransfer = 0.03M,
                    KidsWithoutTransfer = 0.04M
                }
            };
        }

        private static object[] Destination_Must_Exist() {
            return new object[] {
                new TestPrice {
                    StatusCode = 451,
                    CustomerId = 1,
                    DestinationId = 999,
                    PortId = 1,
                    From = "2023-01-01",
                    To = "2023-01-01",
                    AdultsWithTransfer = 0.01M,
                    AdultsWithoutTransfer = 0.02M,
                    KidsWithTransfer = 0.03M,
                    KidsWithoutTransfer = 0.04M
                }
            };
        }

        private static object[] Destination_Must_Be_Active() {
            return new object[] {
                new TestPrice {
                    StatusCode = 451,
                    CustomerId = 1,
                    DestinationId = 7,
                    PortId = 1,
                    From = "2023-01-01",
                    To = "2023-01-01",
                    AdultsWithTransfer = 0.01M,
                    AdultsWithoutTransfer = 0.02M,
                    KidsWithTransfer = 0.03M,
                    KidsWithoutTransfer = 0.04M
                }
            };
        }

        private static object[] Port_Must_Exist() {
            return new object[] {
                new TestPrice {
                    StatusCode = 460,
                    CustomerId = 1,
                    DestinationId = 1,
                    PortId = 999,
                    From = "2023-01-01",
                    To = "2023-01-01",
                    AdultsWithTransfer = 0.01M,
                    AdultsWithoutTransfer = 0.02M,
                    KidsWithTransfer = 0.03M,
                    KidsWithoutTransfer = 0.04M
                }
            };
        }

        private static object[] Port_Must_Be_Active() {
            return new object[] {
                new TestPrice {
                    StatusCode = 460,
                    CustomerId = 1,
                    DestinationId = 1,
                    PortId = 3,
                    From = "2023-01-01",
                    To = "2023-01-01",
                    AdultsWithTransfer = 0.01M,
                    AdultsWithoutTransfer = 0.02M,
                    KidsWithTransfer = 0.03M,
                    KidsWithoutTransfer = 0.04M
                }
            };
        }

        private static object[] AdultsWithTransferMustNotBeNegative() {
            return new object[] {
                new TestPrice {
                    StatusCode = 461,
                    CustomerId = 1,
                    DestinationId = 1,
                    PortId = 1,
                    From = "2023-01-01",
                    To = "2023-12-31",
                    AdultsWithTransfer = -0.01M,
                    AdultsWithoutTransfer = 0.02M,
                    KidsWithTransfer = 0.03M,
                    KidsWithoutTransfer = 0.04M
                }
            };
        }

        private static object[] AdultsWithoutTransferMustNotBeNegative() {
            return new object[] {
                new TestPrice {
                    StatusCode = 461,
                    CustomerId = 1,
                    DestinationId = 1,
                    PortId = 1,
                    From = "2023-01-01",
                    To = "2023-12-31",
                    AdultsWithTransfer = 0.01M,
                    AdultsWithoutTransfer = -0.02M,
                    KidsWithTransfer = 0.03M,
                    KidsWithoutTransfer = 0.04M
                }
            };
        }

        private static object[] KidsWithTransferMustNotBeNegative() {
            return new object[] {
                new TestPrice {
                    StatusCode = 461,
                    CustomerId = 1,
                    DestinationId = 1,
                    PortId = 1,
                    From = "2023-01-01",
                    To = "2023-12-31",
                    AdultsWithTransfer = 0.01M,
                    AdultsWithoutTransfer = 0.02M,
                    KidsWithTransfer = -0.03M,
                    KidsWithoutTransfer = 0.04M
                }
            };
        }

        private static object[] KidsWithoutTransferMustNotBeNegative() {
            return new object[] {
                new TestPrice {
                    StatusCode = 461,
                    CustomerId = 1,
                    DestinationId = 1,
                    PortId = 1,
                    From = "2023-01-01",
                    To = "2023-12-31",
                    AdultsWithTransfer = 0.01M,
                    AdultsWithoutTransfer = 0.02M,
                    KidsWithTransfer = 0.03M,
                    KidsWithoutTransfer = -0.04M
                }
            };
        }

    }

}
