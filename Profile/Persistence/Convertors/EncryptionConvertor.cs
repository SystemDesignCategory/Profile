using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Profile.Persistence.Helpers;

namespace Profile.Persistence.Convertors;

public class EncryptionConvertor : ValueConverter<string, string>
{
    public EncryptionConvertor() : base(
        (sourceValue) => EncryptionHelper.Encrypt(sourceValue),
        (destinationValue) => EncryptionHelper.Decrypt(destinationValue))
    {
    }
}
