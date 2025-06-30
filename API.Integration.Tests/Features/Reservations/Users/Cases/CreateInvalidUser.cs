using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace Users {

    public class CreateInvalidUser : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return When_User_Is_Admin_Customer_Id_Must_Be_Null();
            yield return When_User_Is_SimpleUser_Customer_Id_Must_Exist();
            yield return When_User_Is_SimpleUser_Customer_Id_Must_Be_Active();
            yield return EmailAlreadyExists();
            yield return UsernameAlreadyExists();
        }

        private static object[] When_User_Is_Admin_Customer_Id_Must_Be_Null() {
            return new object[] {
                new TestNewUser {
                    StatusCode = 416,
                    CustomerId = 1,
                    Username = Helpers.CreateRandomString(128),
                    Displayname = Helpers.CreateRandomString(128),
                    Email = "email@server.com",
                    IsAdmin = true
                }
            };
        }

        private static object[] When_User_Is_SimpleUser_Customer_Id_Must_Exist() {
            return new object[] {
                new TestNewUser {
                    StatusCode = 417,
                    CustomerId = null,
                    Username = Helpers.CreateRandomString(128),
                    Displayname = Helpers.CreateRandomString(128),
                    Email = "email@server.com",
                    IsAdmin = false
                }
            };
        }

        private static object[] When_User_Is_SimpleUser_Customer_Id_Must_Be_Active() {
            return new object[] {
                new TestNewUser {
                    StatusCode = 418,
                    CustomerId = 200,
                    Username = Helpers.CreateRandomString(128),
                    Displayname = Helpers.CreateRandomString(128),
                    Email = "email@server.com",
                    IsAdmin = false
                }
            };
        }

        private static object[] EmailAlreadyExists() {
            return new object[] {
                new TestNewUser {
                    StatusCode = 492,
                    Username = "newuser",
                    Displayname = "New User",
                    CustomerId = 2,
                    Email = "operations.corfucruises@gmail.com",
                    IsAdmin = false,
                    IsActive = true
                }
            };
        }

        private static object[] UsernameAlreadyExists() {
            return new object[] {
                new TestNewUser {
                    StatusCode = 492,
                    Username = "foteini",
                    Displayname = "FOTEINI",
                    CustomerId = 2,
                    Email = "newemail@server.com",
                    IsAdmin = false,
                    IsActive = true
                }
            };
        }

    }

}
