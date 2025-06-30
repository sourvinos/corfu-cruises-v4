using System.Collections;
using System.Collections.Generic;

namespace Users {

    public class UpdateInvalidUser : IEnumerable<object[]> {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator() {
            yield return When_User_Is_Admin_Customer_Id_Must_Be_Null();
            yield return When_User_Is_SimpleUser_Customer_Id_Must_Exist();
            yield return UsernameAlreadyExists();
            yield return EmailAlreadyExists();
        }

        private static object[] When_User_Is_Admin_Customer_Id_Must_Be_Null() {
            return new object[] {
                new TestUpdateUser {
                    StatusCode = 416,
                    Id = "eae03de1-6742-4015-9d52-102dba5d7365",
                    CustomerId = 2,
                    Username = "eva",
                    Displayname = "Eva",
                    Email = "invoice.corfucruises@gmail.com",
                    IsAdmin = true,
                    IsActive = true
                }
            };
        }

        private static object[] When_User_Is_SimpleUser_Customer_Id_Must_Exist() {
            return new object[] {
                new TestUpdateUser {
                    StatusCode = 418,
                    Id = "eae03de1-6742-4015-9d52-102dba5d7365",
                    CustomerId = 999,
                    Username = "simpleuser",
                    Displayname = "Simple User",
                    Email = "email@server.com",
                    IsAdmin = false,
                    IsActive = true
                }
            };
        }

        private static object[] UsernameAlreadyExists() {
            return new object[] {
                new TestUpdateUser {
                    StatusCode = 492,
                    Id = "c0e4af2d-be81-41a3-8e6c-67aea53ee486",
                    CustomerId = 2,
                    Username = "mpotsis",
                    Displayname = "WOW",
                    Email = "candebeleted@server.com",
                    IsAdmin = false,
                    IsActive = true
                }
            };
        }

        private static object[] EmailAlreadyExists() {
            return new object[] {
                new TestUpdateUser {
                    StatusCode = 492,
                    Id = "c0e4af2d-be81-41a3-8e6c-67aea53ee486",
                    Username = "wow",
                    Displayname = "Wow",
                    CustomerId = 2,
                    Email = "operations.corfucruises@gmail.com",
                    IsAdmin = false,
                    IsActive = true
                }
            };
        }

    }

}
