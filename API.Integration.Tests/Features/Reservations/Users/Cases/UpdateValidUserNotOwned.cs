using System.Collections;
using System.Collections.Generic;

namespace Users {

    public class UpdateValidUserNotOwnRecord : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return AccountIsOwnedByAnotherUser();
        }

        private static object[] AccountIsOwnedByAnotherUser() {
            return new object[] {
                new TestUpdateUser {
                    StatusCode = 490,
                    Id = "00a6a6d1-12ee-4f66-aea3-f79802a0ce39",
                    CustomerId = 70,
                    Username = "litourgis",
                    Displayname = "LITOURGIS TRAVEL",
                    Email = "litourgistravel@yahoo.gr",
                    IsAdmin = false,
                    IsActive = true
                }
            };
        }

    }

}
