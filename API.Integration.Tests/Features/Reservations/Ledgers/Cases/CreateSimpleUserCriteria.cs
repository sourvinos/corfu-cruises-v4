using System.Collections;
using System.Collections.Generic;

namespace Ledgers {

    public class CreateSimpleUserCriteria : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return Criteria();
        }

        private static object[] Criteria() {
            return new object[] {
                new TestLedgerCriteria {
                    FromDate = "2022-04-27",
                    ToDate = "2022-04-27",
                    CustomerIds = new int[] {2},
                    DestinationIds = new int[] {1},
                    PortIds = new int[] {2},
                    ShipIds = new int?[] {1}
                }
            };
        }

    }

}
