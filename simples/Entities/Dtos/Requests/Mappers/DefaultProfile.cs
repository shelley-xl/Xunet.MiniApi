// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Simples.Entities.Dtos.Requests.Mappers;

/// <summary>
/// DefaultProfile
/// </summary>
public class DefaultProfile : Profile
{
    public DefaultProfile()
    {
        CreateMap<RegisterRequest, Accounts>();
    }
}
