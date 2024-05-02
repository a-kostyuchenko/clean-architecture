namespace Web.API.Endpoints;

public interface IEndpointGroup
{
    void MapGroup(IEndpointRouteBuilder app);
}