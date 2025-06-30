using System.Collections;
using System.Collections.Generic;

namespace Schedules {

    public class UpdateInvalidSchedule : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return Destination_Must_Exist();
            yield return Port_Must_Exist();
            yield return Schedule_Must_Not_Be_Already_Updated();
        }

        private static object[] Destination_Must_Exist() {
            return new object[] {
                new UpdateTestSchedule {
                    StatusCode = 451,
                    Id = 677,
                    DestinationId = 9999,
                    PortId = 2,
                    Date = "2022-12-04",
                    Time = "08:00",
                    MaxPax = 185
                }
            };
        }

        private static object[] Port_Must_Exist() {
            return new object[] {
                new UpdateTestSchedule {
                    StatusCode = 411,
                    Id = 677,
                    DestinationId = 1,
                    PortId = 999,
                    Date = "2022-12-04",
                    Time = "08:00",
                    MaxPax = 185
                },
            };
        }

        private static object[] Schedule_Must_Not_Be_Already_Updated() {
            return new object[] {
                new UpdateTestSchedule {
                    StatusCode = 415,
                    Id = 677,
                    DestinationId = 1,
                    PortId = 2,
                    Date = "2023-06-23",
                    Time = "09:45",
                    MaxPax = 200,
                    PutAt = "2023-09-07 09:54:22"
                }
            };
        }

    }

}