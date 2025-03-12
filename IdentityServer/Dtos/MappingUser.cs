using IdentityServer.Domain;
using IdentityServer.Models;

namespace IdentityServer.Dtos;

public static class MappingUser
{
    public static DtoUser MapToDto(this User user) =>
         new DtoUser
         {
             UserId = user.Id,
             EMail = user.EMail,
             Password = user.Password,
             GivenName = user.GivenName,
             Surname = user.Surname,
             Age = user.Age,
             Country = user.Country,
             City = user.City,
             Street = user.Street,
             HouseNumber = user.HouseNumber,
             PostalCode = user.PostalCode,
         };


    public static User MapToUser(this DtoUser userDto) =>

        new User
        {
            Id = userDto.UserId,
            EMail = userDto.EMail,
            Password = userDto.Password,
            GivenName = userDto.GivenName,
            Surname = userDto.Surname,
            Age = userDto.Age,
            Country = userDto.Country,
            City = userDto.City,
            Street = userDto.Street,
            HouseNumber = userDto.HouseNumber,
            PostalCode = userDto.PostalCode,
        };

    public static UserModel MapToModel(this User user)
        => new UserModel
        {
            UserId = user.Id,
            EMail = user.EMail,
            Password = user.Password,
            GivenName = user.GivenName,
            Surname = user.Surname,
            Age = user.Age,
            Country = user.Country,
            City = user.City,
            Street = user.Street,
            HouseNumber = user.HouseNumber,
            PostalCode = user.PostalCode,
        };

    public static IReadOnlyCollection<UserModel> MapToModelList(this IReadOnlyCollection<User> userList)
        => userList.Select(o => o.MapToModel()).ToList();

    public static List<DtoUser> MapToDtoList(this List<User> userList) =>
        userList.Select(o => o.MapToDto()).ToList();
}
