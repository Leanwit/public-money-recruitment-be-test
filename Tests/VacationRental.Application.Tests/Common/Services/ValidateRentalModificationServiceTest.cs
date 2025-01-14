using System;
using System.Collections.Generic;
using FluentAssertions;
using VacationRental.Application.Common.Services.BookingSearchService;
using VacationRental.Application.Common.Services.ReCalculateBookingsService;
using VacationRental.Domain.Bookings;
using VacationRental.Domain.Rentals;
using Xunit;

namespace VacationRental.Application.Tests.Common.Services
{
    [Collection("Unit")]
    public class ValidateRentalModificationServiceTest
    {
        private readonly ValidateRentalModificationService _service;

        public ValidateRentalModificationServiceTest()
        {
            _service = new ValidateRentalModificationService(new BookingSearchService());
        }
        
        
        [Fact]
        public void GivenNoBookingAndRental_WhenValidateRentalModification_ThenGetAValidResponse()
        {
            var rental = new RentalModel() {Id = 1, Units = 2, PreparationTimeInDays = 2};

            var bookings = new List<BookingModel>();

            _service.Validate(rental, bookings).Should().BeTrue();
        }

        [Fact]
        public void GivenOneBookingAndRental_WhenValidateRentalModification_ThenGetAValidResponse()
        {
            var rental = new RentalModel() {Id = 1, Units = 2, PreparationTimeInDays = 2};

            var bookings = new List<BookingModel>()
            {
                new BookingModel() {Id = 1, RentalId = 1, Nights = 1, Start = new DateTime(2002, 01, 01)}
            };

            _service.Validate(rental, bookings).Should().BeTrue();
        }

        [Fact]
        public void GivenTwoBookingAndRental_WhenValidateRentalModification_ThenGetAInvalidResponse()
        {
            var rental = new RentalModel() {Id = 1, Units = 1, PreparationTimeInDays = 2};

            var bookings = new List<BookingModel>()
            {
                new BookingModel() {Id = 1, RentalId = 1, Nights = 1, Start = new DateTime(2002, 01, 01)},
                new BookingModel() {Id = 2, RentalId = 1, Nights = 2, Start = new DateTime(2002, 01, 03)}
            };

            _service.Validate(rental, bookings).Should().BeFalse();
        }

        [Fact]
        public void GivenTwoBookingAndRental_WhenValidateRentalModification_ThenGetAValidResponse()
        {
            var rental = new RentalModel() {Id = 1, Units = 2, PreparationTimeInDays = 2};

            var bookings = new List<BookingModel>()
            {
                new BookingModel() {Id = 1, RentalId = 1, Nights = 1, Start = new DateTime(2002, 01, 01)},
                new BookingModel() {Id = 2, RentalId = 1, Nights = 2, Start = new DateTime(2002, 01, 06)}
            };

            _service.Validate(rental, bookings).Should().BeTrue();
        }

        [Fact]
        public void GivenTwoOverlappingBookingAndRental_WhenValidateRentalModification_ThenGetInvalidResponse()
        {
            var rental = new RentalModel() {Id = 1, Units = 1, PreparationTimeInDays = 2};

            var bookings = new List<BookingModel>()
            {
                new BookingModel() {Id = 1, RentalId = 1, Nights = 2, Start = new DateTime(2002, 01, 01)},
                new BookingModel() {Id = 2, RentalId = 1, Nights = 2, Start = new DateTime(2002, 01, 01)},
            };

            _service.Validate(rental, bookings).Should().BeFalse();
        }

        [Fact]
        public void
            GivenTwoOverlappingBookingAndRentalWithMoreUnits_WhenValidateRentalModification_ThenGetAValidResponse()
        {
            var rental = new RentalModel() {Id = 1, Units = 3, PreparationTimeInDays = 2};

            var bookings = new List<BookingModel>()
            {
                new BookingModel() {Id = 1, RentalId = 1, Nights = 2, Start = new DateTime(2002, 01, 01)},
                new BookingModel() {Id = 2, RentalId = 1, Nights = 2, Start = new DateTime(2002, 01, 01)},
            };

            _service.Validate(rental, bookings).Should().BeTrue();
        }
    }
}
