using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace PickupPoints {

    public class UpdateValidPickupPoint : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return ValidRecord();
        }

        private static object[] ValidRecord() {
            return new object[] {
                new TestPickupPoint {
                    Id = 614,
                    CoachRouteId = 1,
                    PortId = 1,
                    Description = Helpers.CreateRandomString(128),
                    ExactPoint = Helpers.CreateRandomString(128),
                    Time = "08:00",
                    IsActive = false,
                    PutAt = "2024-01-01 00:00:00"
                }
            };
        }

    }

}
