namespace acheesporte_locator_app.Interfaces;

public class IImageInterface
{
    Task<ImageUploadResponseDto> UploadProfileImageAsync(FileResult file);

}
