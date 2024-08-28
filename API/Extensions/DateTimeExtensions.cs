using System;

namespace API.Extensions;

public static class DateTimeExtensions
{
    public static int CalculateAge(this DateOnly datebirth){
        var today = DateOnly.FromDateTime(DateTime.Now);
        var age  = today.Year - datebirth.Year;
        if (datebirth > today.AddYears(-age)) age--;
        return age;
    }
}
