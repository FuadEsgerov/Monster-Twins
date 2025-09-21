using HalloweenFx.MinApi.Contracts;

namespace HalloweenFx.MinApi.Providers;

public interface IImageEditProvider
{
    Task<byte[]> EditAsync(ImageEditRequest req);
}