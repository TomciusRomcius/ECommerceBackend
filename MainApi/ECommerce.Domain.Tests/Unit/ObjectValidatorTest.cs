using ECommerce.Domain.Entities;
using ECommerce.Domain.Services.Order;
using ECommerce.Domain.Utils;

namespace ECommerce.Domain.Tests.Unit;

public class ObjectValidatorTest
{
    [Fact]
    public void ObjectValidator_ShouldValidateObjects_WithDataAntonations()
    {
        var validator = new ObjectValidator();
        var invalid = new UserEntity("", "email@gmail.com", "passwordhash", "firstname", "lastname");
        IEnumerable<ResultError>? errors = validator.Validate(invalid);

        Assert.Single(errors);
    }

    [Fact]
    public void ObjectValidator_ShouldReturnMultipleErrors_WhenObjectHasMultipleInvalidFields()
    {
        var expectedErrors = 3;

        var validator = new ObjectValidator();
        var invalid = new UserEntity("", "", "", "firstname", "lastname");
        IEnumerable<ResultError> errors = validator.Validate(invalid);

        Assert.Equal(expectedErrors, errors.Count());
    }
}