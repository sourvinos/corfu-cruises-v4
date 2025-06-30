using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace PickupPoints {

    public class UpdateInvalidPickupPoint : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return CoachRoute_Must_Exist();
            yield return Port_Must_Exist();
            yield return PickupPoint_Must_Not_Be_Already_Updated();
        }

        private static object[] CoachRoute_Must_Exist() {
            return new object[] {
                new TestPickupPoint {
                    StatusCode = 408,
                    Id = 1,
                    CoachRouteId = 99,
                    PortId = 1,
                    Description = Helpers.CreateRandomString(128),
                    ExactPoint = Helpers.CreateRandomString(128),
                    Time = "08:00",
                    IsActive = true
                }
            };
        }

        private static object[] Port_Must_Exist() {
            return new object[] {
                new TestPickupPoint {
                    StatusCode = 411,
                    Id = 1,
                    CoachRouteId = 1,
                    PortId = 999,
                    Description = Helpers.CreateRandomString(128),
                    ExactPoint = Helpers.CreateRandomString(128),
                    Time = "08:00"
                }
            };
        }


        private static object[] PickupPoint_Must_Not_Be_Already_Updated() {
            return new object[] {
                new TestPickupPoint {
                    StatusCode = 415,
                    Id = 614,
                    CoachRouteId = 1,
                    PortId = 1,
                    Description = Helpers.CreateRandomString(128),
                    ExactPoint = Helpers.CreateRandomString(128),
                    Time = "08:00",
                    PutAt = "2023-09-14 05:17:48"
                }
            };
        }

    }

}
