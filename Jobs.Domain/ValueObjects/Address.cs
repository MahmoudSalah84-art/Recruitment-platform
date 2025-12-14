using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.ValueObjects
{
    using Jobs.Domain.Common.YourProject.Domain.Common;
    using System;
    using System.Collections.Generic;

    namespace YourProject.Domain.ValueObjects
    {
        public sealed class Address : ValueObject
        {
            public string Country { get; }
            public string City { get; }
            public string Street { get; }
            public string BuildingNumber { get; }
            public string PostalCode { get; }

            private Address(string country, string city, string street, string buildingNumber, string postalCode)
            {
                Country = country;
                City = city;
                Street = street;
                BuildingNumber = buildingNumber;
                PostalCode = postalCode;
            }

            // Factory Method
            public static Address Create(string country, string city, string street, string buildingNumber, string postalCode)
            {
                // Business Rules + Guards
                if (string.IsNullOrWhiteSpace(country))
                    throw new ArgumentException("Country is required.");

                if (string.IsNullOrWhiteSpace(city))
                    throw new ArgumentException("City is required.");

                if (string.IsNullOrWhiteSpace(street))
                    throw new ArgumentException("Street is required.");

                if (string.IsNullOrWhiteSpace(buildingNumber))
                    throw new ArgumentException("Building number is required.");

                if (postalCode?.Length > 10)
                    throw new ArgumentException("Postal code cannot exceed 10 characters.");

                return new Address(
                    country.Trim(),
                    city.Trim(),
                    street.Trim(),
                    buildingNumber.Trim(),
                    postalCode?.Trim()
                );
            }

            protected override IEnumerable<object> GetEqualityComponents()
            {
                yield return Country;
                yield return City;
                yield return Street;
                yield return BuildingNumber;
                yield return PostalCode;
            }

            public override string ToString()
            {
                return $"{Street}, {BuildingNumber}, {City}, {Country}, {PostalCode}";
            }
        }
    }

}
