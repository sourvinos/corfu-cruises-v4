using System.Collections;
using System.Collections.Generic;

namespace Users {

    public class CreateValidUser : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return ValidAdmin();
            yield return ValidSimpleUser();
        }

        private static object[] ValidAdmin() {
            return new object[] {
                new TestNewUser {
                    Username = "username",
                    Displayname = "Display Name",
                    Email = "new-email@server.com",
                    IsAdmin = true
                }
            };
        }

        private static object[] ValidSimpleUser() {
            return new object[] {
                new TestNewUser {
                    Username = "username",
                    Displayname = "Display Name",
                    Email = "new-email@server.com",
                    CustomerId = 1
                }
            };
        }

    }

}
