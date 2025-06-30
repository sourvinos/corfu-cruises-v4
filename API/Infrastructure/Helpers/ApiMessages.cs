namespace API.Infrastructure.Helpers {

    public enum Icons {
        Success,
        Info,
        Warning,
        Error
    }

    public static class ApiMessages {

        #region Generic Messages

        public static string OK() { return "OK"; }
        public static string RecordInUse() { return "Record is used and can't be deleted"; }
        public static string AuthenticationFailed() { return "Authentication failed."; }
        public static string RecordNotFound() { return "Record not found"; }
        public static string UnknownError() { return "Something bad has happened."; }
        public static string EmailNotSent() { return "Email not sent."; }
        public static string NotUniqueUsernameOrEmail() { return "The username or the email are not unique"; }

        #endregion

        #region  App Specific Messages

        public static string DuplicateRecord() { return "Duplicate records are not allowed."; }
        public static string DuplicateRefNo() { return "RefNo in not unique."; }
        public static string InvalidDateDestinationOrPickupPoint() { return "The reservation is invalid for one of the following reasons: a) Nothing is scheduled for the selected day b) We don't go to the selected destination c) There are no departures from the selected port."; }
        public static string PortHasNoFreeSeats() { return "Overbooking in not allowed."; }
        public static string NotOwnRecord() { return "You are not the owner of this record."; }
        public static string InvalidCustomer() { return "The customer doesn't exist or is inactive."; }
        public static string InvalidDestination() { return "The destination doesn't exist or is inactive."; }
        public static string InvalidDriver() { return "The driver doesn't exist or is inactive."; }
        public static string InvalidShip() { return "The ship doesn't exist or is inactive."; }
        public static string InvalidShipOwner() { return "The shipowner doesn't exist or is inactive."; }
        public static string InvalidNationality() { return "The nationality doesn't exist or is inactive."; }
        public static string InvalidGender() { return "The gender doesn't exist or is inactive."; }
        public static string InvalidTaxOffice() { return "The tax office doesn't exist or is inactive."; }
        public static string InvalidCrewSpecialty() { return "The specialty doesn't exist or is inactive."; }
        public static string InvalidPickupPoint() { return "The pickup point doesn't exist or is inactive."; }
        public static string InvalidCoachRoute() { return "The coach route doesn't exist or is inactive."; }
        public static string InvalidIssueDate() { return "The issue date must be today's date."; }
        public static string InvalidBank() { return "The bank doesn't exist or is inactive."; }
        public static string VatNumberIsDuplicate() { return "OK, but the VAT number already exists."; }
        public static string InvalidPort() { return "The port doesn't exist or is inactive."; }
        public static string InvalidDocumentType() { return "The document type doesn't exist or is inactive."; }
        public static string PriceFieldsMustBeZeroOrGreater() { return "All amounts must be zero or greater."; }
        public static string CustomerPaxFieldMustBeZeroOrGreater() { return "Pax limit must be zero or greater."; }
        public static string InvalidDatePeriod() { return "The date period is not correct."; }
        public static string InvalidPassengerCount() { return "Total persons must be equal or greater than the passenger count."; }
        public static string InvoiceIsAlreadySaved() { return "This invoice already exists."; }
        public static string SimpleUserNightRestrictions() { return "New reservations for the next day with transfer after 22:00 are not allowed"; }
        public static string SimpleUserCanNotAddReservationAfterDepartureTime() { return "Reservations after departure are not allowed"; }
        public static string InvalidPortOrder() { return "The stop order already exists."; }
        public static string InvalidAccountFields() { return "One or more fields are invalid."; }
        public static string CustomerIdDoesNotMatchConnectedSimpleUserCustomerId() { return "Customer Id should match the connected customer id."; }
        public static string ConcurrencyError() { return "Another user has already updated this record."; }
        public static string NewAdminShouldNotHaveCustomerId() { return "When the new user is an admin, customer id must be null."; }
        public static string NewSimpleUserShouldHaveCustomerId() { return "When the new user is not an admin, customer id must exist and be active."; }
        public static string PriceCloningNotCompleted() { return "Cloning not completed. Check your pricelist table."; }
        public static string AadeError() { return "Aade update was not completed"; }
        public static string OxygenOkAadeFault() { return "The provider was unable to contact myData"; }
        public static string CheckInAfterDepartureIsNotAllowed() { return "Check-in after departure is not allowed"; }
        public static string CustomerDataIsInvalid() { return "Customer data is invalid"; }
        public static string TransactionCompositeIndexIsInvalid() { return "Duplicate transactions are not allowed"; }
        public static string TransactionCountMismatch() { return "Transaction count doesn't match invoice number"; }
        public static string CustomerPaxLimitIsExceeded() { return "Customer has exceeded the daily pax limit"; }
        public static string OxygenNotResponding() { return "Oxygen is not responding, try again later"; }

        #endregion

    }

}