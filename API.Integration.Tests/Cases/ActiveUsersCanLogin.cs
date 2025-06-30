using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace Cases {

    public class ActiveUsersCanLogin : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return Active_Simple_Users_Can_Login();
            yield return Active_Admins_Can_Login();
        }

        private static object[] Active_Simple_Users_Can_Login() {
            return new object[] {
                new Login {
                    Username = "simpleuser",
                    Password = "A#ba439de-446e-4eef-8c4b-833f1b3e18aa"
                }
            };
        }

        private static object[] Active_Admins_Can_Login() {
            return new object[] {
                new Login {
                    Username = "john",
                    Password = "A#ba439de-446e-4eef-8c4b-833f1b3e18aa"
                }
            };
        }

    }

}
