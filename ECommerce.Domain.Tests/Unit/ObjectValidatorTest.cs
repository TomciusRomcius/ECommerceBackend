using System.Runtime.InteropServices;
using ECommerce.Domain.Entities.User;
using ECommerce.Domain.Services;

namespace ECommerce.Domain.Tests.Unit
{
    public class ObjectValidatorTest
    {
        [Fact]
        public void ObjectValidator_ShouldValidateObjects_WithDataAntonations()
        {
            var validator = new ObjectValidator();
            var invalid = new UserEntity("", "email@gmail.com", "passwordhash", "firstname", "lastname");
            var errors = validator.Validate(invalid);

            Assert.Single(errors);
        }

        [Fact]
        public void ObjectValidator_ShouldReturnMultipleErrors_WhenObjectHasMultipleInvalidFields()
        {
            int expectedErrors = 3;

            var validator = new ObjectValidator();
            var invalid = new UserEntity("", "", "", "firstname", "lastname");
            var errors = validator.Validate(invalid);

            Assert.Equal(expectedErrors, errors.Count());
        }
    }
}