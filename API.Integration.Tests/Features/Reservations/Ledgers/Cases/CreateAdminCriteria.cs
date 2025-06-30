using System.Collections;
using System.Collections.Generic;

namespace Ledgers {

    public class CreateAdminCriteria : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return Criteria();
        }

        private static object[] Criteria() {
            return new object[] {
                new TestLedgerCriteria {
                    FromDate = "2023-05-01",
                    ToDate = "2023-05-01",
                    CustomerIds = new int[] {12, 40, 48},
                    DestinationIds = new int[] {1,2,3,4,5,6},
                    PortIds = new int[] {1, 2},
                    ShipIds = new int?[] {1}
                }
            };
        }

    }

}
